using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades
{
    public class TotalizadorIBSMunicipio : TotalizadorCBSIBSBase
    {
        public TotalizadorIBSMunicipio(decimal diferimentoValorTotal,
            decimal devolucaoTributosValorTotal,
            decimal valorTotal) : base(diferimentoValorTotal, devolucaoTributosValorTotal, valorTotal) { }

        public TotalizadorIBSMunicipio(gIBSMunTotal total) : base(diferimentoValorTotal: total?.vDif ?? 0,
            devolucaoTributosValorTotal: total?.vDevTrib ?? 0,
            valorTotal: total?.vIBSMun ?? 0) { }
    }
}
