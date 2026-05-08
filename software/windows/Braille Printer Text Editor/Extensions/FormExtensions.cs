using Braille_Printer_Editor.Mapping;
using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Braille_Printer_Editor.Extensions
{
    public static class FormExtensions
    {
        #region Funcoes
        /// <summary>
        /// Gera uma lista de portas USB disponíveis no computador
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static List<PortInfo> Portas()
        {
            var portList = new List<PortInfo>();

            ManagementScope managementScope = PrepararEscopo(Environment.MachineName, PrepararOpcoes(), @"\root\CIMV2");
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
            ManagementObjectSearcher portSearcher = new ManagementObjectSearcher(managementScope, objectQuery);

            using (portSearcher)
            {
                string caption = null;
                foreach (ManagementObject currentObject in portSearcher.Get())
                {
                    if (currentObject != null)
                    {
                        object currentObjectCaption = currentObject["Caption"];
                        if (currentObjectCaption != null)
                        {
                            caption = currentObjectCaption.ToString();
                            if (caption.Contains("(COM"))
                            {
                                portList.Add(new PortInfo()
                                {
                                    Nome = caption.Substring(caption.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty),
                                    Descricao = caption,
                                    Fabricante = currentObject["Manufacturer"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return portList;
        }

        /// <summary>
        /// Identifica se um arduino está conectado as portas USB
        /// </summary>
        /// <returns></returns>
        public static string DetectArduinoPort(this Form value)
        {
            try
            {
                foreach (Mapping.PortInfo porta in Portas())
                    if (porta.Descricao.Contains("Arduino") || porta.Fabricante.Contains("wch.cn"))
                        return porta.Nome;
            }
            catch (ManagementException e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        /// <summary>
        /// Identifica se um arduino está conectado as portas USB
        /// </summary>
        /// <returns></returns>
        public static string DetectArduinoPort(this UserControl value)
        {
            try
            {
                foreach (Mapping.PortInfo porta in Portas())
                    if (porta.Descricao.Contains("Arduino") || porta.Fabricante.Contains("wch.cn"))
                        return porta.Nome;
            }
            catch (ManagementException e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        /// <summary>
        /// Transforma codigo binario string em array de bytes
        /// </summary>
        /// <returns></returns>
        public static byte[] ToByteArrayMessage(this string value)
        {
            var input = value;
            var byteArray = new byte[input.Length / 8];

            for (int i = 0; i < (input.Length / 8); ++i)
                byteArray[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);

            return byteArray;
        }
        #endregion

        /// <summary>
        /// Abre um arquivo paraedição
        /// </summary>
        /// <param name="form"></param>
        /// <param name="braille"></param>
        /// <returns></returns>
        public static Mapping.EditorFile AbrirArquivo(this Form form, bool braille = false)
        {
            var editorFile = new Mapping.EditorFile();
            editorFile.BrailleContent = braille;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = (braille ? "Musibraille|*.brm" : "Arquivos de texto|*.rtf;*.txt");
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(ofd.FileName))
                    {
                        if (!braille)
                            editorFile.Content = File.ReadAllText(ofd.FileName);
                        else
                            foreach (string line in File.ReadAllLines(ofd.FileName))
                            {
                                if (line.Contains("config"))
                                    break;

                                if (!line.TrimStart().StartsWith(".") && line.Trim().Length > 0)
                                    editorFile.Content += line + System.Environment.NewLine;
                            }
                    }
                }
                else
                    return null;
            }

            return editorFile;
        }

        /// <summary>
        /// Salva um arquivo editado
        /// </summary>
        /// <param name="form"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool SalvarArquivo(this Form form, string content, bool salvarComo = true)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivos de texto|*.rtf;*.txt";
                    sfd.DefaultExt = "rtf";
                    sfd.AddExtension = true;

                    if (sfd.ShowDialog() == DialogResult.OK)
                        if (!string.IsNullOrEmpty(sfd.FileName))
                            File.WriteAllText(sfd.FileName, content);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Seta as configurações de porta serial para comunicação do arduino caso esteja conectado
        /// </summary>
        /// <returns></returns>
        //public static SerialPort  PortSet(this SerialPort serialPort)
        //{
        //    bool PortFound = false;

        //    try
        //    {
        //        var porta = DetectArduinoPort();
        //        PortFound = (!string.IsNullOrEmpty(porta) ? true : false);

        //        if (PortFound)
        //        {
        //            serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
        //            /*serialPort.PortName = "COM3";
        //            serialPort.Parity = Parity.None;
        //            serialPort.DataBits = 8;
        //            serialPort.StopBits = (StopBits)1;*/
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message, "Error Reading Port");
        //    }

        //    return serialPort;
        //}

        /// <summary>
        /// Opções de conexão das portas
        /// </summary>
        /// <returns></returns>
        private static ConnectionOptions PrepararOpcoes()
        {
            ConnectionOptions options = new ConnectionOptions()
            {
                Impersonation = ImpersonationLevel.Impersonate,
                Authentication = AuthenticationLevel.Default,
                EnablePrivileges = true
            };

            return options;
        }

        /// <summary>
        /// Escopo de busca das portas
        /// </summary>
        /// <param name="nomeMaquina"></param>
        /// <param name="opcoes"></param>
        /// <param name="caminho"></param>
        /// <returns></returns>
        private static ManagementScope PrepararEscopo(string nomeMaquina, ConnectionOptions opcoes, string caminho)
        {
            ManagementScope scope = new ManagementScope()
            {
                Path = new ManagementPath(string.Concat(@"\\", nomeMaquina, caminho)),
                Options = opcoes
            };

            scope.Connect();

            return scope;
        }
    }
}
