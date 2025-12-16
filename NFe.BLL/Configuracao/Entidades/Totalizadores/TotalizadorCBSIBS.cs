using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades.Totalizadores
{
    public class TotalizadorCBSIBS
    {
        public TotalizadorCBSIBS(decimal cbsibsBaseCalculo,
            TotalizadorIBS totalizadorIBS,
            TotalizadorCBS totalizadorCBS,
            TotalizadorCBSIBSMonofasico totalizadorCBSIBSMonofasico,
            TotalizadorCBSIBSEstornoCredito totalizadorCBSIBSEstornoCredito)
        {
            CBSIBSBaseCalculo = cbsibsBaseCalculo;
            TotalizadorIBS = totalizadorIBS;
            TotalizadorCBS = totalizadorCBS;
            TotalizadorCBSIBSMonofasico = totalizadorCBSIBSMonofasico;
            TotalizadorCBSIBSEstornoCredito = totalizadorCBSIBSEstornoCredito;
        }

        public TotalizadorCBSIBS(IBSCBSTot total)
        {
            CBSIBSBaseCalculo = total?.vBCIBSCBS ?? 0;

            if (total?.gIBS?.vIBS > 0 || total?.gIBS?.vCredPres > 0 || total?.gIBS?.vCredPresCondSus > 0)
            {
                TotalizadorIBS = new TotalizadorIBS(total?.gIBS);
            }

            if (total?.gCBS?.vCBS > 0 || total?.gCBS?.vCredPres > 0 || total?.gCBS?.vCredPresCondSus > 0)
            {
                TotalizadorCBS = new TotalizadorCBS(total?.gCBS);
            }

            if (total?.gMono?.vCBSMono > 0 || total?.gMono?.vCBSMonoRet > 0 || total?.gMono?.vCBSMonoReten > 0
                || total?.gMono?.vIBSMono > 0 || total?.gMono?.vIBSMonoRet > 0 || total?.gMono?.vIBSMonoReten > 0)
            {
                TotalizadorCBSIBSMonofasico = new TotalizadorCBSIBSMonofasico(total?.gMono);
            }

            if (total?.gEstornoCred?.vCBSEstCred > 0 || total?.gEstornoCred?.vIBSEstCred > 0)
            {
                TotalizadorCBSIBSEstornoCredito = new TotalizadorCBSIBSEstornoCredito(total.gEstornoCred);
            }
        }

        public decimal ISTotal { get; }

        public decimal CBSIBSBaseCalculo { get; }

        public TotalizadorIBS TotalizadorIBS { get; }

        public TotalizadorCBS TotalizadorCBS { get; }

        public TotalizadorCBSIBSMonofasico TotalizadorCBSIBSMonofasico { get; }

        public TotalizadorCBSIBSEstornoCredito TotalizadorCBSIBSEstornoCredito { get; }

    }
}
