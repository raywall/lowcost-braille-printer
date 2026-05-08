using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Braille_Translator
{
    public static class BrailleTranslator
    {
        public static string Binary(this char value)
        {
            var dic = new BrailleSystem().Dicionario;

            if (dic.ContainsKey(value))
                return dic[value];

            return string.Empty;
        }
    }
}
