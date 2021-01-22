using NFe.Classes.Servicos.Suframa;

namespace NFe.Servicos.Retorno
{
    public class RetornoLoteSuframa : RetornoBasico
    {
        public RetornoLoteSuframa(string envioStr, string retornoStr, string retornoCompletaStr, lote retorno)
            : base(envioStr, retornoStr, retornoCompletaStr, retorno)
        {
            Retorno = retorno;
        }

        public new lote Retorno { get; set; }
    }
}
