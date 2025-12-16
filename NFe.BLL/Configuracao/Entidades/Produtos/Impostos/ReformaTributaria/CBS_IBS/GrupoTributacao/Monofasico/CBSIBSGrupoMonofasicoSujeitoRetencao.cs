using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico
{
    /// <summary>
    /// Uso em operações com combustíveis derivados de petróleo (Gasolina A) [ou *Óleo Diesel A*] para retenção do imposto sobre o biocombustível a ser misturado. Art. 178 da LC 214/2025.
    /// vMono = Quantidade × Alíquota ad-rem.
    /// </summary>
    public class CBSIBSGrupoMonofasicoSujeitoRetencao : CBSIBSMonofasicoGrupoBase
    {
        public CBSIBSGrupoMonofasicoSujeitoRetencao(decimal quantidadeRetencao,
            decimal aliquotaAdRemCBSRetencao,
            decimal aliquotaAdRemIBSRetencao,
            decimal valorMonofasicoCBSRetencao,
            decimal valorMonofasicoIBSRetencao) : base(quantidadeRetencao,
                                                       aliquotaAdRemCBSRetencao,
                                                       aliquotaAdRemIBSRetencao,
                                                       valorMonofasicoCBSRetencao,
                                                       valorMonofasicoIBSRetencao) { }

        public CBSIBSGrupoMonofasicoSujeitoRetencao(gMonoReten monofasicoSujeitoRetencao) : base(quantidade: monofasicoSujeitoRetencao?.qBCMonoReten ?? 0,
                                                                                                 aliquotaAdRemCBS: monofasicoSujeitoRetencao?.qBCMonoReten ?? 0,
                                                                                                 aliquotaAdRemIBS: monofasicoSujeitoRetencao?.adRemCBSReten ?? 0,
                                                                                                 valorMonofasicoCBS: monofasicoSujeitoRetencao?.vCBSMonoReten ?? 0,
                                                                                                 valorMonofasicoIBS: monofasicoSujeitoRetencao?.vIBSMonoReten ?? 0) { }
    }
}
