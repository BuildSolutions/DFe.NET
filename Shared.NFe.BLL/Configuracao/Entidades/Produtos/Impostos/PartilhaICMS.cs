namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class PartilhaICMS
    {
        public PartilhaICMS(decimal baseCalculoICMSDestino, decimal aliquotaInternaUFDestino, decimal valorICMSUFDestino, decimal aliquotaInterestadual, decimal aliquotaICMSPartilha, decimal valorICMSUFOrigem, decimal valorFCP, decimal aliquotaFCP)
        {
            BaseCalculoICMSDestino = baseCalculoICMSDestino;
            AliquotaInternaUFDestino = aliquotaInternaUFDestino;
            ValorICMSUFDestino = valorICMSUFDestino;
            AliquotaInterestadual = aliquotaInterestadual;
            AliquotaICMSPartilha = aliquotaICMSPartilha;
            ValorICMSUFOrigem = valorICMSUFOrigem;
            BaseCalculoFCP = 0;
            ValorFCP = valorFCP;
            AliquotaFCP = aliquotaFCP;
        }

        public decimal BaseCalculoICMSDestino { get; }

        public decimal AliquotaInternaUFDestino { get; }

        public decimal ValorICMSUFDestino { get; }

        public decimal AliquotaInterestadual { get; }

        public decimal AliquotaICMSPartilha { get; }

        public decimal ValorICMSUFOrigem { get; }

        public decimal BaseCalculoFCP { get; }

        public decimal ValorFCP { get; }

        public decimal AliquotaFCP { get; }
    }
}
