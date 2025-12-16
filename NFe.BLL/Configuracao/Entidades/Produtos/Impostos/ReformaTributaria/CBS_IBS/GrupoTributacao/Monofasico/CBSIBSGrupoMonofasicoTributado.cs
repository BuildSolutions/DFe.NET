using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico
{
    /// <summary>
    /// gMonoTrib — Tributação Monofásica (ad-rem) - Monofasia dos Combustíveis.
    /// vCBSMono = Quantidade × Alíquota ad-rem.
    /// vIBSMono = Quantidade × Alíquota ad-rem.
    /// </summary>
    public class CBSIBSGrupoMonofasicoTributado : CBSIBSMonofasicoGrupoBase
    {
        public CBSIBSGrupoMonofasicoTributado(decimal quantidadeTributada,
            decimal aliquotaAdRemCBS,
            decimal aliquotaAdRemIBS,
            decimal valorMonofasicoCBS,
            decimal valorMonofasicoIBS) : base (quantidadeTributada,
                                                aliquotaAdRemCBS,
                                                aliquotaAdRemIBS,
                                                valorMonofasicoCBS,
                                                valorMonofasicoIBS) { }

        public CBSIBSGrupoMonofasicoTributado(gMonoPadrao monofasicoPadrao) : base(quantidade: monofasicoPadrao?.qBCMono ?? 0,
                                                                                   aliquotaAdRemCBS: monofasicoPadrao?.qBCMono ?? 0,
                                                                                   aliquotaAdRemIBS: monofasicoPadrao?.adRemCBS ?? 0,
                                                                                   valorMonofasicoCBS: monofasicoPadrao?.vCBSMono ?? 0,
                                                                                   valorMonofasicoIBS: monofasicoPadrao?.vIBSMono ?? 0) { }
    }
}
