using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Braille_Printer_Editor.Extensions;
using Braille_Printer_Editor.Mapping;
using Braille_Translator;

namespace Braille_Printer_Editor
{
    public partial class EditorForm : Form
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);


        #region Variáveis
        private SpeechSynthesizer assistente = new SpeechSynthesizer();
        private Timer posicaoTextoTimer = new Timer();
        private SerialPort currentPort = null;

        private int limiteLinhas = 30;
        private bool newFile = true;
        #endregion

        #region Propriedades
        public string PortName { get; set; }
        public bool BrailleContent { get; set; }
        public bool PortFound { get; set; }
        #endregion

        #region Funções complementares do RichTextBox
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int GetCaretPos(ref Point lpPoint);

        private static int GetCorrection(RichTextBox e, int index)
        {
            Point pt1 = Point.Empty;
            GetCaretPos(ref pt1);
            Point pt2 = e.GetPositionFromCharIndex(index);

            if (pt1 != pt2)
                return 1;
            else
                return 0;
        }

        public static int Line(RichTextBox e, int index)
        {
            int correction = GetCorrection(e, index);
            return e.GetLineFromCharIndex(index) - correction + 1;
        }

        public static int Column(RichTextBox e, int index1)
        {
            int correction = GetCorrection(e, index1);
            Point p = e.GetPositionFromCharIndex(index1 - correction);

            if (p.X == 1)
                return 1;

            p.X = 0;
            int index2 = e.GetCharIndexFromPosition(p);

            int col = index1 - index2 + 1;

            return col;
        }
        #endregion

        #region Funções
        /// <summary>
        /// Impressão de texto em impressora convencional
        /// </summary>
        private void Imprimir()
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();

            printDocument.DocumentName = "BraillePrint";

            printDialog.Document = printDocument;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                StringReader reader = new StringReader(contentRichTextBox.Text);
                printDocument.PrintPage += new PrintPageEventHandler(DocumentPrintPage);
                printDocument.Print();
            }
        }

        /// <summary>
        /// Impressão de texto em impressora braille de baixo custo
        /// </summary>
        private void BraillePrint()
        {
            try
            {
                if (currentPort != null && currentPort.IsOpen)
                    currentPort.Close();

                currentPort = new SerialPort(this.DetectArduinoPort(), 115200, Parity.None, 8, StopBits.One)
                {
                    ReadTimeout = (int)500,
                    WriteTimeout = (int)500,
                    DtrEnable = true,
                    RtsEnable = true,
                    Encoding = System.Text.Encoding.ASCII,
                    Handshake = Handshake.None
                };

                currentPort.DataReceived += new SerialDataReceivedEventHandler(CurrentPort_DataReceived);
                currentPort.Open();

                if (currentPort.IsOpen)
                {
                    var braillePrinterMessage = new BraillePrinterMessage();
                    braillePrinterMessage.MessageType = BraillePrinterMessage.BrailleType.Texto;
                    braillePrinterMessage.Message = contentRichTextBox.Text;

                    var byteArray = braillePrinterMessage.PrinterByteArrayMessage;
                    var printThread = new System.Threading.Thread(() =>
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                        if (byteArray != null)
                            currentPort.Write(byteArray, 0, byteArray.Length);
                    });

                    printThread.IsBackground = true;
                    printThread.Start();
                }
                else
                    MessageBox.Show("Não foi possível conectar a impressora Braille", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;
            var data = serialPort.ReadExisting();

            if (!string.IsNullOrEmpty(data))
                if (currentPort != null && data.Contains("END"))
                    currentPort.Close();
        }

        /// <summary>
        /// Configuração padráo de pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentPrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringReader reader = new StringReader(contentRichTextBox.Text);
            float LinesPerPage = 0;
            float YPosition = 0;

            int Count = 0;

            float LeftMargin = e.MarginBounds.Left;
            float TopMargin = e.MarginBounds.Top;

            string Line = null;

            Font PrintFont = contentRichTextBox.Font;
            SolidBrush PrintBrush = new SolidBrush(Color.Black);

            LinesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);

            while (Count < LinesPerPage && ((Line = reader.ReadLine()) != null))
            {
                YPosition = TopMargin + (Count * PrintFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(Line, PrintFont, PrintBrush, LeftMargin, YPosition, new StringFormat());
                Count++;
            }

            if (Line != null)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;

            PrintBrush.Dispose();
        }

        /// <summary>
        /// Gera um ícone de cor para adicionar ao controle de seleção de cores
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Bitmap GerarIcone(Color cor)
        {
            Bitmap solidColorBitmap = new Bitmap(16, 16);

            using (Graphics gfx = Graphics.FromImage(solidColorBitmap))
            using (SolidBrush brush = new SolidBrush(cor))
                gfx.FillRectangle(brush, 0, 0, 16, 16);

            return solidColorBitmap;
        }
        #endregion

        public EditorForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();

            Stream fontStream = this.GetType().Assembly.GetManifestResourceStream("Braille_Printer_Editor.Fonts.Braille.ttf");
            byte[] fontdata = new byte[fontStream.Length];

            fontStream.Read(fontdata, 0, (int)fontStream.Length);
            fontStream.Close();

            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    privateFontCollection.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
                }
            }

            brailleTextBox.Font = new Font(privateFontCollection.Families[0], 14, FontStyle.Regular);

            for (int i = 0; i < 10; i++)
            {
                switch (i)
                {
                    case 0:
                        corTextoToolStripButton.DropDownItems.Add("Preto", GerarIcone(Color.Black));
                        corTextoToolStripButton.Image = GerarIcone(Color.Black);
                        break;

                    case 1:
                        corTextoToolStripButton.DropDownItems.Add("Vermelho", GerarIcone(Color.Red));
                        break;

                    case 2:
                        corTextoToolStripButton.DropDownItems.Add("Amarelo", GerarIcone(Color.Yellow));
                        break;

                    case 3:
                        corTextoToolStripButton.DropDownItems.Add("Azul", GerarIcone(Color.Blue));
                        break;

                    case 4:
                        corTextoToolStripButton.DropDownItems.Add("Verde", GerarIcone(Color.Green));
                        break;

                    case 5:
                        corTextoToolStripButton.DropDownItems.Add("Branco", GerarIcone(Color.White));
                        break;

                    case 6:
                        corTextoToolStripButton.DropDownItems.Add("Rosa", GerarIcone(Color.Pink));
                        break;

                    case 7:
                        corTextoToolStripButton.DropDownItems.Add("Laranja", GerarIcone(Color.Orange));
                        break;

                    case 8:
                        corTextoToolStripButton.DropDownItems.Add("Marrom", GerarIcone(Color.Brown));
                        break;

                    case 9:
                        corTextoToolStripButton.DropDownItems.Add("Cinza", GerarIcone(Color.Cyan));
                        break;
                }
            }

            contentRichTextBox.Select();
        }

        private void contentRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (contentRichTextBox.SelectionLength > 1 && (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back))
                contentRichTextBox.SelectedText = string.Empty;

            if (e.KeyCode != Keys.Delete && e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.Back && e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && 
                e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
                try
                {
                    int index = contentRichTextBox.SelectionStart;
                    int line = contentRichTextBox.GetLineFromCharIndex(index);

                    var regex = new Regex("[A-Z]");
                    var maiusculas = regex.Matches(contentRichTextBox.Lines[line].ToString()).Count;
                    var tamanho = contentRichTextBox.Lines[line].ToString().Length + maiusculas;

                    if (tamanho >= limiteLinhas)
                    {
                        line += 1;

                        if (contentRichTextBox.Lines.Count() <= line)
                            contentRichTextBox.Text += Environment.NewLine;

                        index = contentRichTextBox.GetFirstCharIndexFromLine(line++);
                        contentRichTextBox.Select(index, 0);
                    }
                }
                catch { }
        }

        private void contentRichTextBox_TextChanged(object sender, EventArgs e)
        {
            brailleTextBox.Text = contentRichTextBox.Text;
            brailleTextBox.Rtf = brailleTextBox.Rtf.Replace(@"pard\", @"pard\sl480\slmult1\");

            try
            {
                int index = contentRichTextBox.SelectionStart;
                int line = contentRichTextBox.GetLineFromCharIndex(index) + 1;

                lineToolStripStatusLabel.Text = string.Format("Linha: {0}, Coluna: {1}", line, Column(contentRichTextBox, index));
            }
            catch { }
        }

        private void novoArquivvoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Text = string.Empty;
            contentRichTextBox.Select();
            newFile = true;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editorFile = this.AbrirArquivo();

            if (editorFile != null)
            {
                newFile = false;

                if (editorFile.BrailleContent)
                    contentRichTextBox.Text = editorFile.Content;
                else
                    contentRichTextBox.Text = editorFile.Content;  //rtf
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SalvarArquivo(contentRichTextBox.Text, newFile);
        }

        private void salvarcomoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SalvarArquivo(contentRichTextBox.Text, true);
        }

        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void textoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BraillePrint();
        }

        private void importarDoMusibrailleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editorFile = this.AbrirArquivo(true);

            if (editorFile != null)
            {
                if (editorFile.BrailleContent)
                    contentRichTextBox.Text = editorFile.Content;
                else
                    contentRichTextBox.Text = editorFile.Content; //rtf
            }
        }

        private void comandoManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPort != null && currentPort.IsOpen)
                currentPort.Close();

            var controle = new ControleForm();
            controle.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (contentRichTextBox.Text.Trim().Length > 0)
                switch (MessageBox.Show(new Form() { TopMost = true }, "Deseja salvar o texto digitado em um arquivo?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        this.SalvarArquivo(contentRichTextBox.Text, true);
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
                    
        }

        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var splashScreen = new SplashScreen(false);
            splashScreen.ShowDialog();
        }

        #region Atalhos
        private void novoToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Text = string.Empty;
            contentRichTextBox.Select();
            newFile = true;
        }

        private void abrirToolStripButton_Click(object sender, EventArgs e)
        {
            var editorFile = this.AbrirArquivo();

            if (editorFile != null)
            {
                newFile = false;

                if (editorFile.BrailleContent)
                    contentRichTextBox.Text = editorFile.Content;
                else
                    contentRichTextBox.Text = editorFile.Content;  //rtf
            }
        }

        private void salvarToolStripButton_Click(object sender, EventArgs e)
        {
            this.SalvarArquivo(contentRichTextBox.Text, newFile);
        }

        private void imprimirToolStripButton_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void imprimirBrailleToolStripButton_Click(object sender, EventArgs e)
        {
            BraillePrint();
        }

        private void voltarToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Undo();
        }

        private void avancarToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Redo();
        }

        private void negritoToolStripButton_Click(object sender, EventArgs e)
        {
            if (!contentRichTextBox.SelectionFont.Bold)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Bold | contentRichTextBox.SelectionFont.Style);

            else if (contentRichTextBox.SelectionFont.Bold && contentRichTextBox.SelectionFont.Italic && contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Italic | FontStyle.Underline);

            else if (contentRichTextBox.SelectionFont.Bold && contentRichTextBox.SelectionFont.Italic && !contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Italic);

            else if (contentRichTextBox.SelectionFont.Bold && !contentRichTextBox.SelectionFont.Italic && contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Underline);

            else
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular);
        }

        private void italicoToolStripButton_Click(object sender, EventArgs e)
        {
            if (!contentRichTextBox.SelectionFont.Italic)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Italic | contentRichTextBox.SelectionFont.Style);

            else if (contentRichTextBox.SelectionFont.Italic && contentRichTextBox.SelectionFont.Bold && contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Bold | FontStyle.Underline);

            else if (contentRichTextBox.SelectionFont.Italic && contentRichTextBox.SelectionFont.Bold && !contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Bold);

            else if (contentRichTextBox.SelectionFont.Italic && !contentRichTextBox.SelectionFont.Bold && contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Underline);

            else
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular);
        }

        private void sublinhadoToolStripButton_Click(object sender, EventArgs e)
        {
            if (!contentRichTextBox.SelectionFont.Underline)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Underline | contentRichTextBox.SelectionFont.Style);

            else if (contentRichTextBox.SelectionFont.Underline && contentRichTextBox.SelectionFont.Bold && contentRichTextBox.SelectionFont.Italic)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Bold | FontStyle.Italic);

            else if (contentRichTextBox.SelectionFont.Underline && contentRichTextBox.SelectionFont.Bold && !contentRichTextBox.SelectionFont.Italic)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Bold);

            else if (contentRichTextBox.SelectionFont.Underline && !contentRichTextBox.SelectionFont.Bold && contentRichTextBox.SelectionFont.Italic)
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular | FontStyle.Italic);

            else
                contentRichTextBox.SelectionFont = new Font(contentRichTextBox.SelectionFont, FontStyle.Regular);
        }

        private void aumentarFonteToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Font = new Font(contentRichTextBox.Font.FontFamily, contentRichTextBox.Font.Size + 1);
        }

        private void diminuirFonteToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Font = new Font(contentRichTextBox.Font.FontFamily, contentRichTextBox.Font.Size - 1);
        }

        private void corTextoToolStripButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Bitmap solidColorBitmap = (Bitmap)e.ClickedItem.Image;
            Color pixelColor = solidColorBitmap.GetPixel(8, 8);

            corTextoToolStripButton.Image = GerarIcone(pixelColor);
            contentRichTextBox.SelectionColor = pixelColor;
        }

        private void alinharEsquerdaToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void alinharCentroToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void alinharDiireitaToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void copiarToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Copy();
        }

        private void colarToolStripButton_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Paste();
        }
        #endregion
    }
}

