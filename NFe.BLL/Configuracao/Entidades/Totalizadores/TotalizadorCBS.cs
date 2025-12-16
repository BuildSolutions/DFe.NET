using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades
{
    public class TotalizadorCBS : TotalizadorCBSIBSBase
    {
        public TotalizadorCBS(decimal diferimentoValorTotal,
            decimal devolucaoTributosValorTotal,
            decimal valorTotal,
            decimal creditoPresumidoValorTotal,
            decimal creditoPresumidoEmCondicaoSuspensivaValorTotal) : base(diferimentoValorTotal, devolucaoTributosValorTotal, valorTotal)
        {
            creditoPresumidoValorTotal = CreditoPresumidoValorTotal;
            creditoPresumidoEmCondicaoSuspensivaValorTotal = CreditoPresumidoEmCondicaoSuspensivaValorTotal;
        }

        public TotalizadorCBS(gCBSTotal total) : base(diferimentoValorTotal: total?.vDif ?? 0,
            devolucaoTributosValorTotal: total?.vDevTrib ?? 0,
            valorTotal: total?.vCBS ?? 0)
        {
            CreditoPresumidoValorTotal = total?.vCredPres ?? 0;
            CreditoPresumidoEmCondicaoSuspensivaValorTotal = total?.vCredPresCondSus ?? 0;
        }

        public decimal CreditoPresumidoValorTotal { get; }

        public decimal CreditoPresumidoEmCondicaoSuspensivaValorTotal { get; }
    }
}
