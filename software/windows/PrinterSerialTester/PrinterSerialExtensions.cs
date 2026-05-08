using FTD2XX_NET;
using PrinterSerialTester.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterSerialTester
{
    public static class PrinterSerialExtensions
    {
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
                var caption = string.Empty;
                var objects = portSearcher.Get();

                foreach (ManagementObject currentObject in objects)
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
        /// Detecta uma impressora Braille conectada
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static List<PortInfo> BraillePrinterPort(this Form form)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            FTDI Ftdi1 = new FTDI();

            string comstr;
            UInt32 ftdiDeviceCount = 0;
            var portList = new List<PortInfo>();

            Ftdi1.ResetPort();

            // Determine the number of FTDI devices connected to the machine
            ftStatus = Ftdi1.GetNumberOfDevices(ref ftdiDeviceCount);

            // Check status
            if (ftStatus != FTDI.FT_STATUS.FT_OK || ftdiDeviceCount == 0)
            {
                Ftdi1.Close();
                return portList;
            }

            // Allocate storage for device info list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = Ftdi1.GetDeviceList(ftdiDeviceList);

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                for (UInt32 i = 0; i < ftdiDeviceCount; i++)
                {
                    if (ftdiDeviceList[i].Description.ToString().Contains("MICBR"))
                    {
                        Console.WriteLine("Device Index: " + i.ToString());
                        Console.WriteLine("Flags: " + String.Format("{0:x}", ftdiDeviceList[i].Flags));
                        Console.WriteLine("Type: " + ftdiDeviceList[i].Type.ToString());
                        Console.WriteLine("ID: " + String.Format("{0:x}", ftdiDeviceList[i].ID));
                        Console.WriteLine("Location ID: " + String.Format("{0:x}", ftdiDeviceList[i].LocId));
                        Console.WriteLine("Serial Number: " + ftdiDeviceList[i].SerialNumber.ToString());
                        Console.WriteLine("Description: " + ftdiDeviceList[i].Description.ToString());

                        // Open first device in our list by serial number
                        ftStatus = Ftdi1.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            Ftdi1.Close();
                            return portList;
                        }

                        Ftdi1.GetCOMPort(out comstr);

                        portList.Add(new PortInfo()
                        {
                            Nome = comstr,
                            Descricao = ftdiDeviceList[i].Description.ToString(),
                            Fabricante = "MIC Brasil"
                        });
                    }
                }
            }

            Ftdi1.Close();
            return portList;
        }

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
