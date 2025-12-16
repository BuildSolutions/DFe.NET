using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gRed — Redução da alíquota do IBS/CBS.
    /// pAliqEfet = Aliquota original - Percentual de redução da alíquota.
    /// </summary>
    public class CBSIBSGrupoReducao : ICBSIBSGrupo
    {
        private readonly decimal _aliquotaOriginal;
        private readonly decimal _percentualReducaoGrupoGoverno;

        public CBSIBSGrupoReducao(decimal percentualReducao, decimal aliquotaEfetiva)
        {
            PercentualReducao = percentualReducao;
            AliquotaEfetiva = aliquotaEfetiva;
        }

        public CBSIBSGrupoReducao(gRed reducaoAliquota)
        {
            PercentualReducao = reducaoAliquota.pRedAliq;
            AliquotaEfetiva = reducaoAliquota.pAliqEfet;
        }

        /// <summary>pRed — Percentual de redução da alíquota.</summary>
        public decimal PercentualReducao { get; }

        /// <summary>vRed — Valor reduzido.</summary>
        public decimal AliquotaEfetiva { get; private set; }
    }

}
