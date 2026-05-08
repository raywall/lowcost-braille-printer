namespace Braille_Printer_Editor
{
    partial class ControleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControleForm));
            this.logListBox = new System.Windows.Forms.ListBox();
            this.inicioPaginaButton = new System.Windows.Forms.Button();
            this.localizarPaginaButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.subirButton = new System.Windows.Forms.Button();
            this.baixarButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.esquerdaButton = new System.Windows.Forms.Button();
            this.retrocederFolhaButton = new System.Windows.Forms.Button();
            this.avancarFolhaButton = new System.Windows.Forms.Button();
            this.direitaBbutton = new System.Windows.Forms.Button();
            this.marcarButton = new System.Windows.Forms.Button();
            this.commandToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logListBox
            // 
            this.logListBox.FormattingEnabled = true;
            this.logListBox.Location = new System.Drawing.Point(12, 12);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(405, 160);
            this.logListBox.TabIndex = 3;
            // 
            // inicioPaginaButton
            // 
            this.inicioPaginaButton.Location = new System.Drawing.Point(279, 332);
            this.inicioPaginaButton.Name = "inicioPaginaButton";
            this.inicioPaginaButton.Size = new System.Drawing.Size(138, 50);
            this.inicioPaginaButton.TabIndex = 4;
            this.inicioPaginaButton.Text = "Início da página";
            this.inicioPaginaButton.UseVisualStyleBackColor = true;
            this.inicioPaginaButton.Click += new System.EventHandler(this.inicioPaginaButton_Click);
            // 
            // localizarPaginaButton
            // 
            this.localizarPaginaButton.Location = new System.Drawing.Point(279, 388);
            this.localizarPaginaButton.Name = "localizarPaginaButton";
            this.localizarPaginaButton.Size = new System.Drawing.Size(138, 50);
            this.localizarPaginaButton.TabIndex = 5;
            this.localizarPaginaButton.Text = "Localizar página";
            this.localizarPaginaButton.UseVisualStyleBackColor = true;
            this.localizarPaginaButton.Click += new System.EventHandler(this.localizarPaginaButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackgroundImage = global::Braille_Printer_Editor.Properties.Resources.updown1;
            this.tableLayoutPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.subirButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.baixarButton, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(279, 188);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(138, 127);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // subirButton
            // 
            this.subirButton.BackColor = System.Drawing.Color.Transparent;
            this.subirButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.subirButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subirButton.FlatAppearance.BorderSize = 0;
            this.subirButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.subirButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.subirButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.subirButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.subirButton.Location = new System.Drawing.Point(3, 3);
            this.subirButton.Name = "subirButton";
            this.subirButton.Size = new System.Drawing.Size(132, 57);
            this.subirButton.TabIndex = 0;
            this.commandToolTip.SetToolTip(this.subirButton, "Levantar eixo Z");
            this.subirButton.UseVisualStyleBackColor = false;
            this.subirButton.Click += new System.EventHandler(this.subirButton_Click);
            // 
            // baixarButton
            // 
            this.baixarButton.BackColor = System.Drawing.Color.Transparent;
            this.baixarButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.baixarButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baixarButton.FlatAppearance.BorderSize = 0;
            this.baixarButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.baixarButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.baixarButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.baixarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baixarButton.Location = new System.Drawing.Point(3, 66);
            this.baixarButton.Name = "baixarButton";
            this.baixarButton.Size = new System.Drawing.Size(132, 58);
            this.baixarButton.TabIndex = 1;
            this.commandToolTip.SetToolTip(this.baixarButton, "Baixar eixo Z");
            this.baixarButton.UseVisualStyleBackColor = false;
            this.baixarButton.Click += new System.EventHandler(this.baixarButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BackgroundImage = global::Braille_Printer_Editor.Properties.Resources.controles;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.54802F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.45198F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.Controls.Add(this.esquerdaButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.retrocederFolhaButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.avancarFolhaButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.direitaBbutton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.marcarButton, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 188);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.32584F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.67416F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(250, 250);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // esquerdaButton
            // 
            this.esquerdaButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.esquerdaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.esquerdaButton.FlatAppearance.BorderSize = 0;
            this.esquerdaButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.esquerdaButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.esquerdaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.esquerdaButton.Location = new System.Drawing.Point(3, 73);
            this.esquerdaButton.Name = "esquerdaButton";
            this.esquerdaButton.Size = new System.Drawing.Size(64, 102);
            this.esquerdaButton.TabIndex = 0;
            this.commandToolTip.SetToolTip(this.esquerdaButton, "Correr eixo X para a esquerda");
            this.esquerdaButton.UseVisualStyleBackColor = false;
            this.esquerdaButton.Click += new System.EventHandler(this.esquerdaButton_Click);
            // 
            // retrocederFolhaButton
            // 
            this.retrocederFolhaButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.retrocederFolhaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.retrocederFolhaButton.FlatAppearance.BorderSize = 0;
            this.retrocederFolhaButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.retrocederFolhaButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.retrocederFolhaButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.retrocederFolhaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.retrocederFolhaButton.Location = new System.Drawing.Point(73, 181);
            this.retrocederFolhaButton.Name = "retrocederFolhaButton";
            this.retrocederFolhaButton.Size = new System.Drawing.Size(102, 66);
            this.retrocederFolhaButton.TabIndex = 1;
            this.commandToolTip.SetToolTip(this.retrocederFolhaButton, "Retroceder folha");
            this.retrocederFolhaButton.UseVisualStyleBackColor = false;
            this.retrocederFolhaButton.Click += new System.EventHandler(this.retrocedeFolhaButton_Click);
            // 
            // avancarFolhaButton
            // 
            this.avancarFolhaButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.avancarFolhaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avancarFolhaButton.FlatAppearance.BorderSize = 0;
            this.avancarFolhaButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.avancarFolhaButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.avancarFolhaButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.avancarFolhaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.avancarFolhaButton.Location = new System.Drawing.Point(73, 3);
            this.avancarFolhaButton.Name = "avancarFolhaButton";
            this.avancarFolhaButton.Size = new System.Drawing.Size(102, 64);
            this.avancarFolhaButton.TabIndex = 2;
            this.commandToolTip.SetToolTip(this.avancarFolhaButton, "Avancar folha");
            this.avancarFolhaButton.UseVisualStyleBackColor = false;
            this.avancarFolhaButton.Click += new System.EventHandler(this.avancarFolhaButton_Click);
            // 
            // direitaBbutton
            // 
            this.direitaBbutton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.direitaBbutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.direitaBbutton.FlatAppearance.BorderSize = 0;
            this.direitaBbutton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.direitaBbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.direitaBbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.direitaBbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.direitaBbutton.Location = new System.Drawing.Point(181, 73);
            this.direitaBbutton.Name = "direitaBbutton";
            this.direitaBbutton.Size = new System.Drawing.Size(66, 102);
            this.direitaBbutton.TabIndex = 3;
            this.commandToolTip.SetToolTip(this.direitaBbutton, "Correr eixo X para a direita");
            this.direitaBbutton.UseVisualStyleBackColor = false;
            this.direitaBbutton.Click += new System.EventHandler(this.direitaBbutton_Click);
            // 
            // marcarButton
            // 
            this.marcarButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.marcarButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marcarButton.FlatAppearance.BorderSize = 0;
            this.marcarButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.marcarButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.marcarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.marcarButton.Location = new System.Drawing.Point(73, 73);
            this.marcarButton.Name = "marcarButton";
            this.marcarButton.Size = new System.Drawing.Size(102, 102);
            this.marcarButton.TabIndex = 4;
            this.commandToolTip.SetToolTip(this.marcarButton, "Pressionar para marcar folha");
            this.marcarButton.UseVisualStyleBackColor = true;
            this.marcarButton.Click += new System.EventHandler(this.marcarButton_Click);
            // 
            // ControleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 450);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.localizarPaginaButton);
            this.Controls.Add(this.inicioPaginaButton);
            this.Controls.Add(this.logListBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controle manual dos motores";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControleForm_FormClosing);
            this.Load += new System.EventHandler(this.ControleForm_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button esquerdaButton;
        private System.Windows.Forms.Button retrocederFolhaButton;
        private System.Windows.Forms.Button avancarFolhaButton;
        private System.Windows.Forms.Button direitaBbutton;
        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.Button inicioPaginaButton;
        private System.Windows.Forms.Button localizarPaginaButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button subirButton;
        private System.Windows.Forms.Button baixarButton;
        private System.Windows.Forms.ToolTip commandToolTip;
        private System.Windows.Forms.Button marcarButton;
    }
}