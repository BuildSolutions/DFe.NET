using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades
{
    public class TotalizadorIBSUF : TotalizadorCBSIBSBase
    {
        public TotalizadorIBSUF(decimal diferimentoValorTotal,
            decimal devolucaoTributosValorTotal,
            decimal valorTotal) : base(diferimentoValorTotal, devolucaoTributosValorTotal, valorTotal) { }

        public TotalizadorIBSUF(gIBSUFTotal total) : base(diferimentoValorTotal: total?.vDif ?? 0,
            devolucaoTributosValorTotal: total?.vDevTrib ?? 0,
            valorTotal: total?.vIBSUF ?? 0) { }
    }
}
