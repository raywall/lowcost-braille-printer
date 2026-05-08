using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Braille_Printer_Editor.Extensions;

namespace Braille_Printer_Editor.Controles
{
    public partial class ComandoManual : UserControl
    {
        private void EnviarComando(string comando)
        {
            SerialPort currentPort = null;

            try
            {
                currentPort = new SerialPort(this.DetectArduinoPort(), 115200, Parity.None, 8, StopBits.One);
                currentPort.Open();

                if (currentPort.IsOpen)
                {
                    currentPort.DataReceived += new SerialDataReceivedEventHandler(CurrentPort_DataReceived);
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
            finally
            {
                if (currentPort != null)
                    currentPort.Close();
            }
        }

        private void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;
            var data = serialPort.ReadExisting();

            if (!string.IsNullOrEmpty(data))
                Console.WriteLine(data);
        }

        public ComandoManual()
        {
            InitializeComponent(); 
        }

        private void esquerdaButton_Click(object sender, EventArgs e)
        {
            EnviarComando("11110000110000101100000011111111"); //C2
        }

        private void direitaBbutton_Click(object sender, EventArgs e)
        {
            EnviarComando("11110000110000111100000011111111"); //C3
        }

        private void subirButton_Click(object sender, EventArgs e)
        {
            EnviarComando("11110000110001101100000011111111"); //C6
        }

        private void baixarButton_Click(object sender, EventArgs e)
        {
            EnviarComando("11110000110001111100000011111111"); //C7
        }
    }
}
