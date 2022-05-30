namespace DFe.Utils.Extensoes
{
    public static class LongExtension
    {
        public static string NuloSeZero(this long? valor)
        {
            if (valor == null || valor == 0)
            {
                return null;
            }

            return valor.ToString();
        }

        public static string NuloSeZero(this long valor)
        {
            if (valor == 0)
            {
                return null;
            }

            return valor.ToString();
        }
    }
}
