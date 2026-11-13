using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gRed — Redução da alíquota do IBS/CBS (UB26, UB45 e UB64).
    /// Só carrega valores já calculados até o XML: quem deriva o pAliqEfet é o motor
    /// (PwFiscal, projeto simplesolution), com pAliqEfet = alíquota × (1 - pRedAliq/100)
    /// × (1 - pRedutor/100).
    /// </summary>
    public class CBSIBSGrupoReducao : ICBSIBSGrupo
    {

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

        /// <summary>pRedAliq — Percentual de redução da alíquota.</summary>
        public decimal PercentualReducao { get; }

        /// <summary>pAliqEfet — Alíquota efetiva, já com as reduções aplicadas.</summary>
        public decimal AliquotaEfetiva { get; private set; }
    }

}
