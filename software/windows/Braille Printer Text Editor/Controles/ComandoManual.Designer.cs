namespace Braille_Printer_Editor.Controles
{
    partial class ComandoManual
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.esquerdaButton = new System.Windows.Forms.Button();
            this.baixarButton = new System.Windows.Forms.Button();
            this.subirButton = new System.Windows.Forms.Button();
            this.direitaBbutton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.baixarButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.subirButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.direitaBbutton, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.32584F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.67416F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(250, 250);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // esquerdaButton
            // 
            this.esquerdaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.esquerdaButton.FlatAppearance.BorderSize = 0;
            this.esquerdaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.esquerdaButton.Location = new System.Drawing.Point(3, 73);
            this.esquerdaButton.Name = "esquerdaButton";
            this.esquerdaButton.Size = new System.Drawing.Size(64, 102);
            this.esquerdaButton.TabIndex = 0;
            this.esquerdaButton.UseVisualStyleBackColor = true;
            this.esquerdaButton.Click += new System.EventHandler(this.esquerdaButton_Click);
            // 
            // baixarButton
            // 
            this.baixarButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baixarButton.FlatAppearance.BorderSize = 0;
            this.baixarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baixarButton.Location = new System.Drawing.Point(73, 181);
            this.baixarButton.Name = "baixarButton";
            this.baixarButton.Size = new System.Drawing.Size(102, 66);
            this.baixarButton.TabIndex = 1;
            this.baixarButton.UseVisualStyleBackColor = true;
            this.baixarButton.Click += new System.EventHandler(this.baixarButton_Click);
            // 
            // subirButton
            // 
            this.subirButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subirButton.FlatAppearance.BorderSize = 0;
            this.subirButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.subirButton.Location = new System.Drawing.Point(73, 3);
            this.subirButton.Name = "subirButton";
            this.subirButton.Size = new System.Drawing.Size(102, 64);
            this.subirButton.TabIndex = 2;
            this.subirButton.UseVisualStyleBackColor = true;
            this.subirButton.Click += new System.EventHandler(this.subirButton_Click);
            // 
            // direitaBbutton
            // 
            this.direitaBbutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.direitaBbutton.FlatAppearance.BorderSize = 0;
            this.direitaBbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.direitaBbutton.Location = new System.Drawing.Point(181, 73);
            this.direitaBbutton.Name = "direitaBbutton";
            this.direitaBbutton.Size = new System.Drawing.Size(66, 102);
            this.direitaBbutton.TabIndex = 3;
            this.direitaBbutton.UseVisualStyleBackColor = true;
            this.direitaBbutton.Click += new System.EventHandler(this.direitaBbutton_Click);
            // 
            // ComandoManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ComandoManual";
            this.Size = new System.Drawing.Size(250, 250);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button esquerdaButton;
        private System.Windows.Forms.Button baixarButton;
        private System.Windows.Forms.Button subirButton;
        private System.Windows.Forms.Button direitaBbutton;
    }
}
