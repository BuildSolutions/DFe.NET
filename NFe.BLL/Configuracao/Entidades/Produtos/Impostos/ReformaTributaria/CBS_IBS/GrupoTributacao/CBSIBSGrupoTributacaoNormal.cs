using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gTrib — Tributação normal do IBS/CBS.
    /// Contém CST, Classificação Tributária e a alíquota.
    /// vIBS/vCBS = Base × Alíquota.
    /// </summary>
    public class CBSIBSGrupoTributacaoNormal : ICBSIBSGrupo
    {
        public CBSIBSGrupoTributacaoNormal(decimal aliquota, decimal valor)
        {
            AliquotaPercentual = aliquota;
            ValorTributo = valor;
        }

        ///// <summary>CST — Código de Situação Tributária.</summary>
        //public string CodigoSituacaoTributaria { get; }

        ///// <summary>cClassTrib — Código de Classificação Tributária.</summary>
        //public string ClassificacaoTributaria { get; }

        /// <summary>pIBS/pCBS — Alíquota em percentual (ex.: 1.5 = 1,5%).</summary>
        public decimal AliquotaPercentual { get; }

        /// <summary>vIBS/vCBS — Valor calculado do tributo.</summary>
        public decimal ValorTributo { get; private set; }
    }

}
