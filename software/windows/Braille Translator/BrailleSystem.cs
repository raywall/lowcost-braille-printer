using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braille_Translator
{
    public class BrailleSystem
    {
        public Dictionary<char, string> Dicionario = new Dictionary<char, string>();

        public BrailleSystem()
        {
            Dicionario.Add(' ', "00000000");

            #region Letras minusculas
            Dicionario.Add('a', "00100000"); // 00xxxxxx - letras minusculas
            Dicionario.Add('á', "00111011");
            Dicionario.Add('à', "00110101");
            Dicionario.Add('â', "00100001");
            Dicionario.Add('ã', "00001110");
            Dicionario.Add('b', "00110000");
            Dicionario.Add('c', "00100100");
            Dicionario.Add('ç', "00111101");
            Dicionario.Add('d', "00100110");
            Dicionario.Add('e', "00100010");
            Dicionario.Add('é', "00111111");
            Dicionario.Add('ê', "00110001");
            Dicionario.Add('f', "00110100");
            Dicionario.Add('g', "00110110");
            Dicionario.Add('h', "00110010");
            Dicionario.Add('i', "00010100");
            Dicionario.Add('í', "00001100");
            Dicionario.Add('j', "00010110");
            Dicionario.Add('k', "00101000");
            Dicionario.Add('l', "00111000");
            Dicionario.Add('m', "00101100");
            Dicionario.Add('n', "00101110");
            Dicionario.Add('o', "00101010");
            Dicionario.Add('ó', "00001101");
            Dicionario.Add('ô', "00100111");
            Dicionario.Add('õ', "00010101");
            Dicionario.Add('p', "00111100");
            Dicionario.Add('q', "00111110");
            Dicionario.Add('r', "00111010");
            Dicionario.Add('s', "00011100");
            Dicionario.Add('t', "00011110");
            Dicionario.Add('u', "00101001");
            Dicionario.Add('ú', "00011111");
            Dicionario.Add('ü', "00110011");
            Dicionario.Add('v', "00111001");
            Dicionario.Add('x', "00101101");
            Dicionario.Add('y', "00101111");
            Dicionario.Add('w', "00010111");
            Dicionario.Add('z', "00101011");
            #endregion

            #region Letras maiusculas
            Dicionario.Add('A', "0100010101100000"); // 01xxxxxx - letras maiusculas
            Dicionario.Add('Á', "0100010101111011");
            Dicionario.Add('À', "0100010101110101");
            Dicionario.Add('Â', "0100010101100001");
            Dicionario.Add('Ã', "0100010101001110");
            Dicionario.Add('B', "0100010101110000");
            Dicionario.Add('C', "0100010101100100");
            Dicionario.Add('Ç', "0100010101111101");
            Dicionario.Add('D', "0100010101100110");
            Dicionario.Add('E', "0100010101100010");
            Dicionario.Add('É', "0100010101111111");
            Dicionario.Add('Ê', "0100010101110001");
            Dicionario.Add('F', "0100010101110100");
            Dicionario.Add('G', "0100010101110110");
            Dicionario.Add('H', "0100010101110010");
            Dicionario.Add('I', "0100010101010100");
            Dicionario.Add('Í', "0100010101001100");
            Dicionario.Add('J', "0100010101010110");
            Dicionario.Add('K', "0100010101101000");
            Dicionario.Add('L', "0100010101111000");
            Dicionario.Add('M', "0100010101101100");
            Dicionario.Add('N', "0100010101101110");
            Dicionario.Add('O', "0100010101101010");
            Dicionario.Add('Ó', "0100010101001101");
            Dicionario.Add('Ô', "0100010101100111");
            Dicionario.Add('Õ', "0100010101010101");
            Dicionario.Add('P', "0100010101111100");
            Dicionario.Add('Q', "0100010101111110");
            Dicionario.Add('R', "0100010101111010");
            Dicionario.Add('S', "0100010101011100");
            Dicionario.Add('T', "0100010101011110");
            Dicionario.Add('U', "0100010101101001");
            Dicionario.Add('Ú', "0100010100011111");
            Dicionario.Add('Ü', "0100010100110011");
            Dicionario.Add('V', "0100010101111001");
            Dicionario.Add('X', "0100010101101101");
            Dicionario.Add('Y', "0100010101101111");
            Dicionario.Add('W', "0100010101010111");
            Dicionario.Add('Z', "0100010101101011");
            #endregion

            #region Numeros
            Dicionario.Add('0', "1000111110010110"); // 10xxxxxx - números
            Dicionario.Add('1', "1000111110100000");
            Dicionario.Add('2', "1000111110110000");
            Dicionario.Add('3', "1000111110100100");
            Dicionario.Add('4', "1000111110100110");
            Dicionario.Add('5', "1000111110100010");
            Dicionario.Add('6', "1000111110110100");
            Dicionario.Add('7', "1000111110110110");
            Dicionario.Add('8', "1000111110110010");
            Dicionario.Add('9', "1000111110010100");
            #endregion

            #region Caracteres especiais
            Dicionario.Add('\n', "11000000"); // 11000000 - indica uma quebra de linha
            Dicionario.Add(',',  "11010000");
            Dicionario.Add(';',  "11011000");
            Dicionario.Add(':',  "11010010");
            Dicionario.Add('?',  "11010001");
            Dicionario.Add('!',  "11011010");
            Dicionario.Add('.',  "11001000");
            Dicionario.Add('-',  "11001001");
            Dicionario.Add('_',  "1100100111001001");
            Dicionario.Add('(',  "11110001");
            Dicionario.Add(')',  "11001110");
            Dicionario.Add('[',  "11111011");
            Dicionario.Add(']',  "11011111");
            Dicionario.Add('"',  "11011001");
            Dicionario.Add('*',  "11001010");
            Dicionario.Add('&',  "11111101");
            Dicionario.Add('/',  "1100000111010000");
            Dicionario.Add('|',  "11000111");
            Dicionario.Add('$',  "11000011");
            Dicionario.Add('%',  "1100011111001011");
            Dicionario.Add('§',  "1101110011011100");
            Dicionario.Add('+',  "11011010");
            Dicionario.Add('>',  "11101010");
            Dicionario.Add('<',  "11010101");
            Dicionario.Add('º',  "11001011");
            #endregion
        }
    }
}
