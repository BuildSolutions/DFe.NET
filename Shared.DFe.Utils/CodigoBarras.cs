namespace DFe.Utils
{
    public static class CodigoBarras
    {
        public static bool IsGtinValido(string vGTIN)
        {
            if(string.IsNullOrEmpty(vGTIN))
            {
                return false;
            }

            var resultado = ObterDigitoVerificador(vGTIN);
            if (resultado != vGTIN[vGTIN.Length - 1].ToString())
            {
                return false;
            }

            return true;
        }

        public static string ObterDigitoVerificador(string vGTIN)
        {
            if (string.IsNullOrEmpty(vGTIN)
                || vGTIN.Length != 12)
            {
                return string.Empty;
            }

            int[] array = new int[]
            {
                int.Parse(vGTIN[0].ToString()),
                int.Parse(vGTIN[1].ToString()) * 3,
                int.Parse(vGTIN[2].ToString()),
                int.Parse(vGTIN[3].ToString()) * 3,
                int.Parse(vGTIN[4].ToString()),
                int.Parse(vGTIN[5].ToString()) * 3,
                int.Parse(vGTIN[6].ToString()),
                int.Parse(vGTIN[7].ToString()) * 3,
                int.Parse(vGTIN[8].ToString()),
                int.Parse(vGTIN[9].ToString())* 3,
                int.Parse(vGTIN[10].ToString()),
                int.Parse(vGTIN[11].ToString()) * 3
            };

            int sum = (array[0] + array[1] + array[2] + array[3] + array[4] + array[5] + array[6] + array[7] + array[8] + array[9] + array[10] + array[11]);

            int resultado = (((sum / 10) + 1) * 10) - sum;

            if (resultado % 10 == 0)
            {
                resultado = 0;
            }

            return resultado.ToString();
        }
    }
}
