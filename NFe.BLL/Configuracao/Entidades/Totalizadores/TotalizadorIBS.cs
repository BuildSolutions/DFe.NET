using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades
{
    public class TotalizadorIBS
    {
        public TotalizadorIBS(TotalizadorIBSMunicipio totalizadorIBSMunicipio,
            TotalizadorIBSUF totalizadorIBSUF,
            decimal creditoPresumidoValorTotal,
            decimal creditoPresumidoEmCondicaoSuspensivaValorTotal,
            decimal valorTotal)
        {
            TotalizadorIBSMunicipio = totalizadorIBSMunicipio;
            TotalizadorIBSUF = totalizadorIBSUF;
            CreditoPresumidoValorTotal = creditoPresumidoValorTotal;
            CreditoPresumidoEmCondicaoSuspensivaValorTotal = creditoPresumidoEmCondicaoSuspensivaValorTotal;
            ValorTotal = valorTotal;
        }

        public TotalizadorIBS(gIBSTotal total)
        {
            if (total?.gIBSUF?.vIBSUF > 0 || total?.gIBSUF?.vDif > 0 || total?.gIBSUF?.vDevTrib > 0)
            {
                TotalizadorIBSUF = new TotalizadorIBSUF(diferimentoValorTotal: total?.gIBSUF?.vDif ?? 0,
                devolucaoTributosValorTotal: total?.gIBSUF?.vDevTrib ?? 0,
                valorTotal: total?.gIBSUF?.vIBSUF ?? 0);
            }
            if (total?.gIBSMun?.vIBSMun > 0 || total?.gIBSMun?.vDif > 0 || total?.gIBSMun?.vDevTrib > 0)
            {
                TotalizadorIBSMunicipio = new TotalizadorIBSMunicipio(diferimentoValorTotal: total?.gIBSUF?.vDif ?? 0,
                devolucaoTributosValorTotal: total?.gIBSUF?.vDevTrib ?? 0,
                valorTotal: total?.gIBSUF?.vIBSUF ?? 0);
            }

            ValorTotal = total?.vIBS ?? 0;
            CreditoPresumidoValorTotal = total?.vCredPres ?? 0;
            CreditoPresumidoEmCondicaoSuspensivaValorTotal = total?.vCredPresCondSus ?? 0;
        }

        public TotalizadorIBSMunicipio TotalizadorIBSMunicipio { get; }

        public TotalizadorIBSUF TotalizadorIBSUF { get; }

        public decimal ValorTotal { get; }

        public decimal CreditoPresumidoValorTotal { get; }

        public decimal CreditoPresumidoEmCondicaoSuspensivaValorTotal { get; }
    }
}
