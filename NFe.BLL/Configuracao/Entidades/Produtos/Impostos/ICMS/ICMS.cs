using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public abstract class ICMS
    {
        public Csticms? CST { get; protected set; }

        public Csosnicms? CSOSN { get; protected set; }

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

        public decimal? AliquotaReducaoBaseCalculoST { get; protected set; }

        public decimal? ValorICMSDesonerado { get; protected set; }

        public decimal? BaseCalculoICMSRetido { get; protected set; }

        public decimal? ValorICMSRetido { get; protected set; }

        public decimal? AliquotaAdRemICMSRetido { get; protected set; }

        public decimal? AliquotaAdRemICMS { get; protected set; }

        public static ICMS ObterIcms(ICMSBasico TipoICMS)
        {
            switch (TipoICMS.GetType().Name)
            {
                case nameof(ICMS00):
                    return new ICMS00((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS00)TipoICMS);
                case nameof(ICMS02):
                    return new ICMS02((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS02)TipoICMS);
                case nameof(ICMS10):
                    return new ICMS10((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS10)TipoICMS);
                case nameof(ICMSPart):
                    return new ICMSPart((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSPart)TipoICMS);
                case nameof(ICMS20):
                    return new ICMS20((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS20)TipoICMS);
                case nameof(ICMS30):
                    return new ICMS30((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS30)TipoICMS);
                case nameof(ICMS40):
                    return new ICMS40((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS40)TipoICMS);
                case nameof(ICMS51):
                    return new ICMS51((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS51)TipoICMS);
                case nameof(ICMS53):
                    return new ICMS53((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS53)TipoICMS);
                case nameof(ICMS60):
                    return new ICMS60((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS60)TipoICMS);
                case nameof(ICMS61):
                    return new ICMS61((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS61)TipoICMS);
                case nameof(ICMSST):
                    return new ICMSST((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSST)TipoICMS);
                case nameof(ICMS70):
                    return new ICMS70((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS70)TipoICMS);
                case nameof(ICMS90):
                    return new ICMS90((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS90)TipoICMS);

                case nameof(ICMSSN101):
                    return new ICMSSN101((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN101)TipoICMS);
                case nameof(ICMSSN102):
                    return new ICMSSN102((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN102)TipoICMS);
                case nameof(ICMSSN201):
                    return new ICMSSN201((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN201)TipoICMS);
                case nameof(ICMSSN202):
                    return new ICMSSN202((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN202)TipoICMS);
                case nameof(ICMSSN500):
                    return new ICMSSN500((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN500)TipoICMS);
                case nameof(ICMSSN900):
                    return new ICMSSN900((NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN900)TipoICMS);
                default:
                    return null;
            }
        }
    }
}
