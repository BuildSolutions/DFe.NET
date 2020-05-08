using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public abstract class ICMS
    {
        public Csticms CST { get; protected set; }

        public Csosnicms CSOSN { get; protected set; }

        public OrigemMercadoria Origem { get; protected set; }

        public DeterminacaoBaseIcms? ModalidadeCalculo { get; protected set; }

        public decimal BaseCalculo { get; protected set; }

        public decimal ValorTotal { get; protected set; }

        public decimal Aliquota { get; protected set; }

        public decimal? ValorICMSOperacao { get; protected set; }

        public decimal? PercentualDiferimento { get; protected set; }

        public decimal? ValorDiferimento { get; protected set; }

        public decimal? RepasseCreditoAliquota { get; protected set; }

        public decimal? RepasseCreditoValor { get; protected set; }

        public DeterminacaoBaseIcmsSt? ModalidadeCalculoST { get; protected set; }

        public decimal? AliquotaMVAST { get; protected set; }

        public decimal BaseCalculoST { get; protected set; }

        public decimal AliquotaST { get; protected set; }

        public decimal ValorTotalST { get; protected set; }

        public decimal? BaseCaluloFCP { get; protected set; }

        public decimal? AliquotaFCP { get; protected set; }

        public decimal? ValorTotalFCP { get; protected set; }

        public MotivoDesoneracaoIcms? MotivoDesoneracao { get; protected set; }

        public decimal? AliquotaReducaoBaseCalculo { get; protected set; }

        public decimal? ValorICMSDesonerado { get; protected set; }

        public decimal? BaseCalculoICMSRetido { get; protected set; }

        public decimal? ValorICMSRetido { get; protected set; }
    }
}
