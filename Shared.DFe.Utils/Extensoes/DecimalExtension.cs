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
    }
}
