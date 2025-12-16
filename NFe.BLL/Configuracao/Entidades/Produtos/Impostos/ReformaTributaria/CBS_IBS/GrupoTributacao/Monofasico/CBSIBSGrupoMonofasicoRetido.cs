using System;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico
{
    /// <summary>
    /// Tributação monofásica própria sobre combustíveis cobrada anteriormente
    /// vCBSMonoRet = Quantidade × Alíquota ad-rem.
    /// vIBSMonoRet = Quantidade × Alíquota ad-rem.
    /// </summary>
    public class CBSIBSGrupoMonofasicoRetido : CBSIBSMonofasicoGrupoBase
    {
        public CBSIBSGrupoMonofasicoRetido(decimal quantidadeRetida,
            decimal aliquotaAdRemCBSRetido,
            decimal aliquotaAdRemIBSRetido,
            decimal valorMonofasicoCBSRetido,
            decimal valorMonofasicoIBSRetido) : base(quantidadeRetida,
                                                     aliquotaAdRemCBSRetido,
                                                     aliquotaAdRemIBSRetido,
                                                     valorMonofasicoCBSRetido,
                                                     valorMonofasicoIBSRetido) { }

        public CBSIBSGrupoMonofasicoRetido(gMonoRet monofasicoRetido) : base(quantidade: monofasicoRetido?.qBCMonoRet ?? 0,
                                                                             aliquotaAdRemCBS: monofasicoRetido?.qBCMonoRet ?? 0,
                                                                             aliquotaAdRemIBS: monofasicoRetido?.adRemCBSRet ?? 0,
                                                                             valorMonofasicoCBS: monofasicoRetido?.vCBSMonoRet ?? 0,
                                                                             valorMonofasicoIBS: monofasicoRetido?.vIBSMonoRet ?? 0) { }
    }
}
