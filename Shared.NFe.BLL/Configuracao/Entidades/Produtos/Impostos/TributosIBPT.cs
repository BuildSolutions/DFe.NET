namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class TributosIBPT
    {
        public TributosIBPT(decimal aliquotaEstadual, decimal valorEstadual, decimal aliquotaNacional, decimal valorNacional)
        {
            AliquotaEstadual = aliquotaEstadual;
            ValorEstadual = valorEstadual;
            AliquotaNacional = aliquotaNacional;
            ValorNacional = valorNacional;
        }

        public decimal AliquotaEstadual { get; }

        public decimal ValorEstadual { get; }

        public decimal AliquotaNacional { get; }

        public decimal ValorNacional { get; }
    }
}
