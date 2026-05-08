namespace Braille_Printer_Editor
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.brailleStyleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this.brailleTextBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarcomoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.imprimirEmBrailleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.importarDoMusibrailleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comandoManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.falarTextoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contentRichTextBox = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lineToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.novoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.abrirToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.salvarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.imprimirToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.imprimirBrailleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.voltarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.avancarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.negritoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.italicoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sublinhadoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.aumentarFonteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.diminuirFonteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.corTextoToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.alinharEsquerdaToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.alinharCentroToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.alinharDiireitaToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copiarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.colarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.novoArquivvoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imprimirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // brailleStyleManager
            // 
            this.brailleStyleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Silver;
            // 
            // brailleTextBox
            // 
            this.brailleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brailleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brailleTextBox.Font = new System.Drawing.Font("Braile font", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brailleTextBox.Location = new System.Drawing.Point(656, 3);
            this.brailleTextBox.Name = "brailleTextBox";
            this.brailleTextBox.ReadOnly = true;
            this.brailleTextBox.Size = new System.Drawing.Size(648, 665);
            this.brailleTextBox.TabIndex = 5;
            this.brailleTextBox.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.sobreToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1307, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novoArquivvoToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.salvarToolStripMenuItem,
            this.salvarcomoToolStripMenuItem,
            this.toolStripMenuItem1,
            this.imprimirToolStripMenuItem,
            this.imprimirEmBrailleToolStripMenuItem,
            this.toolStripMenuItem2,
            this.importarDoMusibrailleToolStripMenuItem,
            this.comandoManualToolStripMenuItem,
            this.toolStripMenuItem3,
            this.falarTextoToolStripMenuItem,
            this.sairToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "&Arquivo";
            // 
            // salvarcomoToolStripMenuItem
            // 
            this.salvarcomoToolStripMenuItem.Name = "salvarcomoToolStripMenuItem";
            this.salvarcomoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.salvarcomoToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.salvarcomoToolStripMenuItem.Text = "Salvar &como";
            this.salvarcomoToolStripMenuItem.Click += new System.EventHandler(this.salvarcomoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(232, 6);
            // 
            // imprimirEmBrailleToolStripMenuItem
            // 
            this.imprimirEmBrailleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imagemToolStripMenuItem,
            this.textoToolStripMenuItem});
            this.imprimirEmBrailleToolStripMenuItem.Name = "imprimirEmBrailleToolStripMenuItem";
            this.imprimirEmBrailleToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.imprimirEmBrailleToolStripMenuItem.Text = "Imprimir em &Braille";
            // 
            // imagemToolStripMenuItem
            // 
            this.imagemToolStripMenuItem.Enabled = false;
            this.imagemToolStripMenuItem.Name = "imagemToolStripMenuItem";
            this.imagemToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.imagemToolStripMenuItem.Text = "&Imagem";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(232, 6);
            // 
            // importarDoMusibrailleToolStripMenuItem
            // 
            this.importarDoMusibrailleToolStripMenuItem.Name = "importarDoMusibrailleToolStripMenuItem";
            this.importarDoMusibrailleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importarDoMusibrailleToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.importarDoMusibrailleToolStripMenuItem.Text = "Importar do &Musibraille";
            this.importarDoMusibrailleToolStripMenuItem.Click += new System.EventHandler(this.importarDoMusibrailleToolStripMenuItem_Click);
            // 
            // comandoManualToolStripMenuItem
            // 
            this.comandoManualToolStripMenuItem.Name = "comandoManualToolStripMenuItem";
            this.comandoManualToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.comandoManualToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.comandoManualToolStripMenuItem.Text = "Comand&o manual";
            this.comandoManualToolStripMenuItem.Click += new System.EventHandler(this.comandoManualToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(232, 6);
            // 
            // falarTextoToolStripMenuItem
            // 
            this.falarTextoToolStripMenuItem.CheckOnClick = true;
            this.falarTextoToolStripMenuItem.Enabled = false;
            this.falarTextoToolStripMenuItem.Name = "falarTextoToolStripMenuItem";
            this.falarTextoToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.falarTextoToolStripMenuItem.Text = "&Falar texto digitado";
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.sobreToolStripMenuItem.Text = "&Sobre";
            this.sobreToolStripMenuItem.Click += new System.EventHandler(this.sobreToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.contentRichTextBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.brailleTextBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1307, 671);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // contentRichTextBox
            // 
            this.contentRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contentRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.contentRichTextBox.Name = "contentRichTextBox";
            this.contentRichTextBox.Size = new System.Drawing.Size(647, 665);
            this.contentRichTextBox.TabIndex = 6;
            this.contentRichTextBox.Text = "";
            this.contentRichTextBox.TextChanged += new System.EventHandler(this.contentRichTextBox_TextChanged);
            this.contentRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contentRichTextBox_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 720);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1307, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lineToolStripStatusLabel
            // 
            this.lineToolStripStatusLabel.Name = "lineToolStripStatusLabel";
            this.lineToolStripStatusLabel.Size = new System.Drawing.Size(98, 17);
            this.lineToolStripStatusLabel.Text = "Linha 0; Coluna 0";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novoToolStripButton,
            this.abrirToolStripButton,
            this.salvarToolStripButton,
            this.toolStripSeparator1,
            this.imprimirToolStripButton,
            this.imprimirBrailleToolStripButton,
            this.toolStripSeparator6,
            this.voltarToolStripButton,
            this.avancarToolStripButton,
            this.toolStripSeparator2,
            this.negritoToolStripButton,
            this.italicoToolStripButton,
            this.sublinhadoToolStripButton,
            this.toolStripSeparator7,
            this.aumentarFonteToolStripButton,
            this.diminuirFonteToolStripButton,
            this.toolStripSeparator3,
            this.corTextoToolStripButton,
            this.toolStripSeparator4,
            this.alinharEsquerdaToolStripButton,
            this.alinharCentroToolStripButton,
            this.alinharDiireitaToolStripButton,
            this.toolStripSeparator5,
            this.copiarToolStripButton,
            this.colarToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1307, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // novoToolStripButton
            // 
            this.novoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.novoToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources._new;
            this.novoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.novoToolStripButton.Name = "novoToolStripButton";
            this.novoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.novoToolStripButton.Text = "Novo arquivo";
            this.novoToolStripButton.Click += new System.EventHandler(this.novoToolStripButton_Click);
            // 
            // abrirToolStripButton
            // 
            this.abrirToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.abrirToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.open_file;
            this.abrirToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.abrirToolStripButton.Name = "abrirToolStripButton";
            this.abrirToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.abrirToolStripButton.Text = "Abrir arquivo";
            this.abrirToolStripButton.Click += new System.EventHandler(this.abrirToolStripButton_Click);
            // 
            // salvarToolStripButton
            // 
            this.salvarToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.salvarToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.save;
            this.salvarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.salvarToolStripButton.Name = "salvarToolStripButton";
            this.salvarToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.salvarToolStripButton.Text = "Salvar arquivo";
            this.salvarToolStripButton.Click += new System.EventHandler(this.salvarToolStripButton_Click);
            // 
            // imprimirToolStripButton
            // 
            this.imprimirToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imprimirToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.Print;
            this.imprimirToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imprimirToolStripButton.Name = "imprimirToolStripButton";
            this.imprimirToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.imprimirToolStripButton.Text = "Imprimir arquivvo";
            this.imprimirToolStripButton.Click += new System.EventHandler(this.imprimirToolStripButton_Click);
            // 
            // imprimirBrailleToolStripButton
            // 
            this.imprimirBrailleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imprimirBrailleToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.Braille;
            this.imprimirBrailleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imprimirBrailleToolStripButton.Name = "imprimirBrailleToolStripButton";
            this.imprimirBrailleToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.imprimirBrailleToolStripButton.Text = "Imprimir arquivo em braille";
            this.imprimirBrailleToolStripButton.Click += new System.EventHandler(this.imprimirBrailleToolStripButton_Click);
            // 
            // voltarToolStripButton
            // 
            this.voltarToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.voltarToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.undo;
            this.voltarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.voltarToolStripButton.Name = "voltarToolStripButton";
            this.voltarToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.voltarToolStripButton.Text = "Desfazer";
            this.voltarToolStripButton.Click += new System.EventHandler(this.voltarToolStripButton_Click);
            // 
            // avancarToolStripButton
            // 
            this.avancarToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.avancarToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.redo;
            this.avancarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.avancarToolStripButton.Name = "avancarToolStripButton";
            this.avancarToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.avancarToolStripButton.Text = "Refazer";
            this.avancarToolStripButton.Click += new System.EventHandler(this.avancarToolStripButton_Click);
            // 
            // negritoToolStripButton
            // 
            this.negritoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.negritoToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.negrito;
            this.negritoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.negritoToolStripButton.Name = "negritoToolStripButton";
            this.negritoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.negritoToolStripButton.Text = "Negrito";
            this.negritoToolStripButton.Click += new System.EventHandler(this.negritoToolStripButton_Click);
            // 
            // italicoToolStripButton
            // 
            this.italicoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.italicoToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.Italico;
            this.italicoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.italicoToolStripButton.Name = "italicoToolStripButton";
            this.italicoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.italicoToolStripButton.Text = "Italico";
            this.italicoToolStripButton.Click += new System.EventHandler(this.italicoToolStripButton_Click);
            // 
            // sublinhadoToolStripButton
            // 
            this.sublinhadoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sublinhadoToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.Sublinhado;
            this.sublinhadoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sublinhadoToolStripButton.Name = "sublinhadoToolStripButton";
            this.sublinhadoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.sublinhadoToolStripButton.Text = "Sublinhado";
            this.sublinhadoToolStripButton.Click += new System.EventHandler(this.sublinhadoToolStripButton_Click);
            // 
            // aumentarFonteToolStripButton
            // 
            this.aumentarFonteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aumentarFonteToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.AumentarTexto;
            this.aumentarFonteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aumentarFonteToolStripButton.Name = "aumentarFonteToolStripButton";
            this.aumentarFonteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.aumentarFonteToolStripButton.Text = "Aumentar tamanho da fonte";
            this.aumentarFonteToolStripButton.Click += new System.EventHandler(this.aumentarFonteToolStripButton_Click);
            // 
            // diminuirFonteToolStripButton
            // 
            this.diminuirFonteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.diminuirFonteToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.DiminuirTexto;
            this.diminuirFonteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.diminuirFonteToolStripButton.Name = "diminuirFonteToolStripButton";
            this.diminuirFonteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.diminuirFonteToolStripButton.Text = "Diminuir tamanho da fonte";
            this.diminuirFonteToolStripButton.Click += new System.EventHandler(this.diminuirFonteToolStripButton_Click);
            // 
            // corTextoToolStripButton
            // 
            this.corTextoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.corTextoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("corTextoToolStripButton.Image")));
            this.corTextoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.corTextoToolStripButton.Name = "corTextoToolStripButton";
            this.corTextoToolStripButton.Size = new System.Drawing.Size(29, 22);
            this.corTextoToolStripButton.Text = "Selecionar cor do texto";
            this.corTextoToolStripButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.corTextoToolStripButton_DropDownItemClicked);
            // 
            // alinharEsquerdaToolStripButton
            // 
            this.alinharEsquerdaToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.alinharEsquerdaToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.text_align_left;
            this.alinharEsquerdaToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.alinharEsquerdaToolStripButton.Name = "alinharEsquerdaToolStripButton";
            this.alinharEsquerdaToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.alinharEsquerdaToolStripButton.Text = "Alinhar texto â esquerda";
            this.alinharEsquerdaToolStripButton.Click += new System.EventHandler(this.alinharEsquerdaToolStripButton_Click);
            // 
            // alinharCentroToolStripButton
            // 
            this.alinharCentroToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.alinharCentroToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.text_align_center;
            this.alinharCentroToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.alinharCentroToolStripButton.Name = "alinharCentroToolStripButton";
            this.alinharCentroToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.alinharCentroToolStripButton.Text = "Alinhar texto ao centro";
            this.alinharCentroToolStripButton.Click += new System.EventHandler(this.alinharCentroToolStripButton_Click);
            // 
            // alinharDiireitaToolStripButton
            // 
            this.alinharDiireitaToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.alinharDiireitaToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.text_align_right;
            this.alinharDiireitaToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.alinharDiireitaToolStripButton.Name = "alinharDiireitaToolStripButton";
            this.alinharDiireitaToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.alinharDiireitaToolStripButton.Text = "Alinhar texto â direita";
            this.alinharDiireitaToolStripButton.Click += new System.EventHandler(this.alinharDiireitaToolStripButton_Click);
            // 
            // copiarToolStripButton
            // 
            this.copiarToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copiarToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.copiar;
            this.copiarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copiarToolStripButton.Name = "copiarToolStripButton";
            this.copiarToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copiarToolStripButton.Text = "Copiar texto";
            this.copiarToolStripButton.Click += new System.EventHandler(this.copiarToolStripButton_Click);
            // 
            // colarToolStripButton
            // 
            this.colarToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.colarToolStripButton.Image = global::Braille_Printer_Editor.Properties.Resources.colar;
            this.colarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.colarToolStripButton.Name = "colarToolStripButton";
            this.colarToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.colarToolStripButton.Text = "Colar";
            this.colarToolStripButton.Click += new System.EventHandler(this.colarToolStripButton_Click);
            // 
            // novoArquivvoToolStripMenuItem
            // 
            this.novoArquivvoToolStripMenuItem.Image = global::Braille_Printer_Editor.Properties.Resources._new;
            this.novoArquivvoToolStripMenuItem.Name = "novoArquivvoToolStripMenuItem";
            this.novoArquivvoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.novoArquivvoToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.novoArquivvoToolStripMenuItem.Text = "&Novo";
            this.novoArquivvoToolStripMenuItem.Click += new System.EventHandler(this.novoArquivvoToolStripMenuItem_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Image = global::Braille_Printer_Editor.Properties.Resources.open_file;
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.abrirToolStripMenuItem.Text = "&Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // salvarToolStripMenuItem
            // 
            this.salvarToolStripMenuItem.Image = global::Braille_Printer_Editor.Properties.Resources.save;
            this.salvarToolStripMenuItem.Name = "salvarToolStripMenuItem";
            this.salvarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.salvarToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.salvarToolStripMenuItem.Text = "&Salvar";
            this.salvarToolStripMenuItem.Click += new System.EventHandler(this.salvarToolStripMenuItem_Click);
            // 
            // imprimirToolStripMenuItem
            // 
            this.imprimirToolStripMenuItem.Image = global::Braille_Printer_Editor.Properties.Resources.Print;
            this.imprimirToolStripMenuItem.Name = "imprimirToolStripMenuItem";
            this.imprimirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.imprimirToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.imprimirToolStripMenuItem.Text = "&Imprimir";
            this.imprimirToolStripMenuItem.Click += new System.EventHandler(this.imprimirToolStripMenuItem_Click);
            // 
            // textoToolStripMenuItem
            // 
            this.textoToolStripMenuItem.Image = global::Braille_Printer_Editor.Properties.Resources.Braille;
            this.textoToolStripMenuItem.Name = "textoToolStripMenuItem";
            this.textoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.textoToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.textoToolStripMenuItem.Text = "&Texto";
            this.textoToolStripMenuItem.Click += new System.EventHandler(this.textoToolStripMenuItem_Click);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Image = global::Braille_Printer_Editor.Properties.Resources.Exit16;
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.sairToolStripMenuItem.Text = "Sai&r";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1307, 742);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Impressora braille de baixo custo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditorForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager brailleStyleManager;
        private System.Windows.Forms.RichTextBox brailleTextBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novoArquivvoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salvarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salvarcomoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem imprimirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imprimirEmBrailleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imagemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem importarDoMusibrailleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comandoManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem falarTextoToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lineToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.RichTextBox contentRichTextBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton novoToolStripButton;
        private System.Windows.Forms.ToolStripButton abrirToolStripButton;
        private System.Windows.Forms.ToolStripButton salvarToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton imprimirToolStripButton;
        private System.Windows.Forms.ToolStripButton imprimirBrailleToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton voltarToolStripButton;
        private System.Windows.Forms.ToolStripButton avancarToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton negritoToolStripButton;
        private System.Windows.Forms.ToolStripButton italicoToolStripButton;
        private System.Windows.Forms.ToolStripButton sublinhadoToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton corTextoToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton alinharEsquerdaToolStripButton;
        private System.Windows.Forms.ToolStripButton alinharCentroToolStripButton;
        private System.Windows.Forms.ToolStripButton alinharDiireitaToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton copiarToolStripButton;
        private System.Windows.Forms.ToolStripButton colarToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton aumentarFonteToolStripButton;
        private System.Windows.Forms.ToolStripButton diminuirFonteToolStripButton;
    }
}

