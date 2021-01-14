using GNRE.Classes.Servicos.Consulta.Lote.Retorno;

namespace GNRE.Servicos.Retorno
{
    public class RetornoProcessamentoLote : RetornoBasico
    {
        public RetornoProcessamentoLote(string envioStr, string retornoStr, string retornoCompletaStr, TresultLote_GNRE retorno) : base(envioStr, retornoStr, retornoCompletaStr, retorno)
        {
            Retorno = retorno;
        }

        public new TresultLote_GNRE Retorno { get; }
    }
}
