using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico
{
    /// <summary>
    /// Operações com diferimento, aplicado aos biocombustíveis. Exemplo: operação do produtor de biocombustível (usina).
    /// vMono = Quantidade × Alíquota ad-rem.
    /// </summary>
    public class CBSIBSGrupoMonofasicoDiferimento
    {
        public CBSIBSGrupoMonofasicoDiferimento(decimal percentualMonofasicoCBSDiferido,
            decimal percentualMonofasicoIBSDiferido,
            decimal valorMonofasicoCBSDiferimento,
            decimal valorMonofasicoIBSDiferimento)
        {
            PercentualMonofasicoCBSDiferimento = percentualMonofasicoCBSDiferido;
            PercentualMonofasicoIBSDiferimento = percentualMonofasicoIBSDiferido;

            ValorMonofasicoCBSDiferimento = valorMonofasicoCBSDiferimento;
            ValorMonofasicoIBSDiferimento = valorMonofasicoIBSDiferimento;
        }

        public CBSIBSGrupoMonofasicoDiferimento(gMonoDif monofasicoDiferimento)
        {
            PercentualMonofasicoCBSDiferimento = monofasicoDiferimento?.pDifCBS ?? 0;
            PercentualMonofasicoIBSDiferimento = monofasicoDiferimento?.pDifIBS ?? 0;

            ValorMonofasicoCBSDiferimento = monofasicoDiferimento?.vCBSMonoDif ?? 0;
            ValorMonofasicoIBSDiferimento = monofasicoDiferimento?.vIBSMonoDif ?? 0;
        }

        /// <summary>pDifCBS — Percentual do diferimento do imposto monofásico CBS.
        /// A ser aplicado em vCBSMono.</summary>
        public decimal PercentualMonofasicoCBSDiferimento { get; set; }

        /// <summary>pDifIBS — Percentual do diferimento do imposto monofásico IBS.
        /// A ser aplicado em vIBSMono.</summary>
        public decimal PercentualMonofasicoIBSDiferimento { get; set; }

        /// <summary>vCBSMonoDif — Valor da CBS Mono diferido..
        /// Valor da CBS com retenção, a ser somado ao valor de CBS a ser recolhido.</summary>
        public decimal ValorMonofasicoCBSDiferimento { get; private set; }

        /// <summary>vIBSMonoDif — Valor da IBS Mono diferido..
        /// Valor da IBS com retenção, a ser somado ao valor de IBS a ser recolhido.</summary>
        public decimal ValorMonofasicoIBSDiferimento { get; private set; }
    }
}
