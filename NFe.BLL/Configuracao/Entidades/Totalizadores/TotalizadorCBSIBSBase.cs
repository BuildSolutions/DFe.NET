namespace NFe.BLL.Configuracao.Entidades
{
    public abstract class TotalizadorCBSIBSBase
    {
        public TotalizadorCBSIBSBase(decimal diferimentoValorTotal,
            decimal devolucaoTributosValorTotal,
            decimal valorTotal)
        {
            DiferimentoValorTotal = diferimentoValorTotal;
            DevolucaoTributosValorTotal = devolucaoTributosValorTotal;
            ValorTotal = valorTotal;
        }

        public decimal DiferimentoValorTotal { get; }

        public decimal DevolucaoTributosValorTotal { get; }

        public decimal ValorTotal { get; }
    }
}
