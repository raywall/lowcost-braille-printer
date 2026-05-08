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

namespace PrinterSerialTester
{
    public partial class Form1 : Form
    {
        private delegate void FormDelegate();
        private SerialPort currentPort = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string value = textBox1.Text;

            var thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    var portas = this.BraillePrinterPort();

                    if (portas.Count > 0)
                    {
                        currentPort = new SerialPort(portas[0].Nome, 115200, Parity.None, 8, StopBits.One)
                        {
                            ReadTimeout = (int)500,
                            WriteTimeout = (int)500,
                            //DtrEnable = true,
                            //RtsEnable = true,
                            Encoding = System.Text.Encoding.ASCII,
                            Handshake = Handshake.None
                        };

                        currentPort.Open();

                        if (currentPort.IsOpen)
                        {
                            var byteArray = Encoding.ASCII.GetBytes(value);
                            currentPort.Write(byteArray, 0, byteArray.Length);
                            currentPort.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("ERROR: {0}", ex.ToString()));

                    Invoke(new FormDelegate(() => {
                        MessageBox.Show("Erro ocorrido ao conectar a porta serial da impressora!");
                    }));
                }
            }));

            thread.IsBackground = true;
            thread.Start();

            textBox1.Text = string.Empty;
            textBox1.Select();
        }
    }
}
