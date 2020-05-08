namespace NFe.BLL.Configuracao.Entidades
{
    public class ConfiguracaoCsc
    {
        public ConfiguracaoCsc(int cIdToken, string csc)
        {
            CIdToken = cIdToken;
            Csc = csc;
        }

        /// <summary>
        /// Identificador do CSC – Código de Segurança do Contribuinte no Banco de Dados da SEFAZ
        /// </summary>
        public int CIdToken { get; set; }

        /// <summary>
        /// Código de Segurança do Contribuinte(antigo Token)
        /// </summary>
        public string Csc { get; set; }
    }
}