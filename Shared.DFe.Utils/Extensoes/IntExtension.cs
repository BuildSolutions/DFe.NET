namespace DFe.Utils.Extensoes
{
    public static class IntExtension
    {
        public static int? NuloSeZero(this int? valor)
        {
            if (valor == null || valor == 0)
            {
                return null;
            }

            return valor;
        }

        public static int? NuloSeZero(this int valor)
        {
            if (valor == 0)
            {
                return null;
            }

            return valor;
        }
    }
}
