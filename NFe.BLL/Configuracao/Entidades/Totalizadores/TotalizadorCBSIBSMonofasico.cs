using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades
{
    public class TotalizadorCBSIBSMonofasico
    {
        public TotalizadorCBSIBSMonofasico(decimal ibsMonofasicoValorTotal,
            decimal cbsMonofasicoValorTotal,
            decimal ibsMonofasicoSujeitoRetencaoValorTotal,
            decimal cbsMonofasicoSujeitoRetencaoValorTotal,
            decimal ibsMonofasicoRetidoValorTotal,
            decimal cbsMonofasicoRetidoValorTotal)
        {
            IBSMonofasicoValorTotal = ibsMonofasicoValorTotal;
            CBSMonofasicoValorTotal = cbsMonofasicoValorTotal;
            IBSMonofasicoSujeitoRetencaoValorTotal = ibsMonofasicoSujeitoRetencaoValorTotal;
            CBSMonofasicoSujeitoRetencaoValorTotal = cbsMonofasicoSujeitoRetencaoValorTotal;
            IBSMonofasicoRetidoValorTotal = ibsMonofasicoRetidoValorTotal;
            CBSMonofasicoRetidoValorTotal = cbsMonofasicoRetidoValorTotal;
        }

        public TotalizadorCBSIBSMonofasico(gMonoTotal total)
        {
            IBSMonofasicoValorTotal = total?.vIBSMono ?? 0;
            CBSMonofasicoValorTotal = total?.vCBSMono ?? 0;
            IBSMonofasicoSujeitoRetencaoValorTotal = total?.vIBSMonoReten ?? 0;
            CBSMonofasicoSujeitoRetencaoValorTotal = total?.vCBSMonoReten ?? 0;
            IBSMonofasicoRetidoValorTotal = total?.vIBSMonoRet ?? 0;
            CBSMonofasicoRetidoValorTotal = total?.vCBSMonoRet ?? 0;
        }

        public decimal IBSMonofasicoValorTotal { get; }

        public decimal CBSMonofasicoValorTotal { get; }

        public decimal IBSMonofasicoSujeitoRetencaoValorTotal { get; }

        public decimal CBSMonofasicoSujeitoRetencaoValorTotal { get; }

        public decimal IBSMonofasicoRetidoValorTotal { get; }

        public decimal CBSMonofasicoRetidoValorTotal { get; }
    }
}
