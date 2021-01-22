namespace DFe.Utils.Extensoes
{
    public static class DecimalExtension
    {
        public static decimal? NuloSeZero(this decimal? valor)
        {
            if(valor == null || valor == 0)
            {
                return null;
            }

            return valor;
        }

        public static decimal? NuloSeZero(this decimal valor)
        {
            if (valor == 0)
            {
                return null;
            }

            return valor;
        }

        public static string ParaDecimalString(this decimal? valor, int casasDecimais = 2)
        {
            if (valor == null || valor == 0)
            {
                return null;
            }

            string mascara = "####0.";

            for (int i = 0; i < casasDecimais; i++)
                mascara += "0";

            return valor.Value.ToString(mascara).Replace(".", string.Empty).Replace(",", ".");
        }

        public static string ParaDecimalString(this decimal valor, int casasDecimais = 2)
        {
            if (valor == 0)
            {
                return null;
            }

            string mascara = "####0.";

            for (int i = 0; i < casasDecimais; i++)
                mascara += "0";

            return valor.ToString(mascara).Replace(".", string.Empty).Replace(",", ".");
        }

    }
}
