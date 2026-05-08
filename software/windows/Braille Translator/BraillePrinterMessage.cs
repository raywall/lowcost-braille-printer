using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braille_Translator
{
    public class BraillePrinterMessage
    {
        public enum BrailleType : long
        {
            Comando = 1110,
            Texto = 1111,
            Musica = 0
        }

        public BrailleType MessageType { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Retorna a representação binária da mensagem em formato string
        /// </summary>
        public string PrinterMessage
        {
            get
            {
                if (Message != null)
                {
                    var content = string.Empty;

                    foreach (var l in Message.ToCharArray())
                        content += l.Binary();

                    return string.Format("{0}0000{1}11111111", Convert.ToString((long)MessageType).PadLeft(4, '0'), content);
                }
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Retorna a representação binária da mensagem em bytes
        /// </summary>
        public byte[] PrinterByteArrayMessage
        {
            get
            {
                var input = PrinterMessage;
                var byteArray = new byte[input.Length / 8];

                for (int i = 0; i < (input.Length / 8); ++i)
                    byteArray[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);

                return byteArray;
            }
        }
    }
}
