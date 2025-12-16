using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades
{
    public class TotalizadorCBSIBSEstornoCredito
    {
        public TotalizadorCBSIBSEstornoCredito(decimal ibsValorEstornado,
            decimal cbsValorEstornado)
        {
            IBSValorEstornado = ibsValorEstornado;
            CBSValorEstornado = cbsValorEstornado;
        }

        public TotalizadorCBSIBSEstornoCredito(gEstornoCredTotal total)
        {
            IBSValorEstornado = total?.vIBSEstCred ?? 0;
            CBSValorEstornado = total?.vCBSEstCred ?? 0;
        }

        public decimal IBSValorEstornado { get; }

        public decimal CBSValorEstornado { get; }
    }
}
