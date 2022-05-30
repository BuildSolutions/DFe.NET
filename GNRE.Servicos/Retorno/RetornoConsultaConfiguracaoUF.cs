using GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno;

namespace GNRE.Servicos.Retorno
{
    public class RetornoConsultaConfiguracaoUF : RetornoBasico
    {
        public RetornoConsultaConfiguracaoUF(string envioStr, string retornoStr, string retornoCompletaStr, TConfigUf retorno)
            : base(envioStr, retornoStr, retornoCompletaStr, retorno)
        {
            Retorno = retorno;
        }

        public new TConfigUf Retorno { get; }
    }
}
