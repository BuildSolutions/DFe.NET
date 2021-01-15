using GNRE.Classes.Servicos.Recepcao.Retorno;

namespace GNRE.Servicos.Retorno
{
    public class RetornoRecepcaoGNRE : RetornoBasico
    {
        public RetornoRecepcaoGNRE(string envioStr, string retornoStr, string retornoCompletaStr, TRetLote_GNRE retorno) : base(envioStr, retornoStr, retornoCompletaStr, retorno)
        {
            Retorno = retorno;
        }

        public new TRetLote_GNRE Retorno { get; }
    }
}
