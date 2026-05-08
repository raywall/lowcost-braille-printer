using Braille_Printer_Editor.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Braille_Printer_Editor
{
    public partial class ControleForm : Form
    {
        private SerialPort currentPort = null;
        private delegate void CommandDelegate();


        public ControleForm()
        {
            InitializeComponent();
        }

        private void ControleForm_Load(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void EnviarComando(string comando)
        {
            try
            {
                if (currentPort.IsOpen)
                {
                    var byteArray = comando.ToByteArrayMessage();

                    if (byteArray != null)
                        currentPort.Write(byteArray, 0, byteArray.Length);
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
                Invoke(new CommandDelegate(() => {
                    logListBox.Items.Add(data.TrimEnd().TrimStart());
                }));
        }

        private void marcarButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100000111111111"); //C1
        }

        private void esquerdaButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100001011111111"); //C2
        }

        private void direitaBbutton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100001111111111"); //C3
        }

        private void avancarFolhaButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100010011111111"); //C4
        }

        private void retrocedeFolhaButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100010111111111"); //C5
        }

        private void subirButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100011011111111"); //C6
        }

        private void baixarButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100011111111111"); //C7
        }

        private void inicioPaginaButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100100011111111"); //C8
        }

        private void localizarPaginaButton_Click(object sender, EventArgs e)
        {
            EnviarComando("111000001100100111111111"); //C9
        }

        private void ControleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentPort != null)
                currentPort.Close();
        }        
    }
}
