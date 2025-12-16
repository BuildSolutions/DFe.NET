using System.Globalization;

namespace DFe.Utils
{
    public static class DecimalExtension
    {
        public static string ToMysqlString(this decimal value)
        {
            return value.ToString("0.##############").Replace(".", "").Replace(",", ".");
        }

        public static string ToFormattedString(this decimal? valor, int casasDecimais = 2, int qtdMinimaZerosAposVirgula = 0)
        {
            if (valor == null || valor == 0)
            {
                return null;
            }

            return ToFormattedString(valor.GetValueOrDefault(), casasDecimais, qtdMinimaZerosAposVirgula);
        }

        public static string ToFormattedString(this decimal valor, int casasDecimais = 2, int qtdMinimaZerosAposVirgula = 0)
        {
            string mascara = "##,##0";

            for (int i = 0; i < casasDecimais; i++)
            {
                if (i == 0)
                {
                    mascara += ".";
                }

                mascara += qtdMinimaZerosAposVirgula > i ? "0" : "#";
            }

            return valor.ToString(mascara);
        }

        public static string ToUsdFormattedString(this decimal value)
        {
            CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            customCulture.NumberFormat.CurrencySymbol = "USD ";
            customCulture.NumberFormat.CurrencyDecimalDigits = 2;
            customCulture.NumberFormat.CurrencyGroupSeparator = "";
            customCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            return value.ToString("N", customCulture);
        }
    }
}
