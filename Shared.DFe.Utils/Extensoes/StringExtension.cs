using System.Text;
using System.Text.RegularExpressions;

namespace DFe.Utils.Extensoes
{
    public static class StringExtension
    {
        public static string RetornaNumeros(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            string retorno = string.Empty;

            foreach (char item in text)
            {
                if (char.IsDigit(item))
                {
                    retorno += item;
                }
            }

            return retorno;
        }

        public static string RetornaAlfanumericos(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(text, "").Trim();
        }

        public static string SanitizeString(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            // \xEF\xBF\xBD == \xFFFD
            // https://stackoverflow.com/a/11162470
            text = text.Replace("\xFFFD", "");

            StringBuilder buffer = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (c == '\xFFFD')
                {

                }
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString().Trim();
        }

        public static string SubstringMaxLength(this string text, int maxLength)
        {
            if(string.IsNullOrEmpty(text)
                || maxLength == 0
                || text.Length <= maxLength)
            {
                return text?.Trim();
            }

            return text.Substring(0, maxLength).Trim();
        }

        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character != 0x9 /* == '\t' == 9   */          &&
                 character != 0xA /* == '\n' == 10  */          &&
                 character != 0xD /* == '\r' == 13  */          &&
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }
    }
}
