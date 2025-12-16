using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gTribCompraGov — Grupo de informações da composição do valor do IBS e da CBS em compras governamentais(UB82a)
    /// Informar somente para compras governamentais
    /// </summary>
    public class CBSIBSGrupoTributacaoCompraGoverno : ICBSIBSGrupo
    {
        private readonly decimal _baseCalculo;

        public CBSIBSGrupoTributacaoCompraGoverno(decimal aliquotaPercentualCBS,
            decimal aliquotaPercentualIBSUF,
            decimal aliquotaPercentualIBSMunicipio,
            decimal valorTributoCBS,
            decimal valorTributoIBSUF,
            decimal valorTributoIBSMunicipio)
        {
            AliquotaPercentualCBS = aliquotaPercentualCBS;
            AliquotaPercentualIBSUF = aliquotaPercentualIBSUF;
            AliquotaPercentualIBSMunicipio = aliquotaPercentualIBSMunicipio;

            ValorTributoCBS = valorTributoCBS;
            ValorTributoIBSUF = valorTributoIBSUF;
            ValorTributoIBSMunicipio = valorTributoIBSMunicipio;
        }

        public CBSIBSGrupoTributacaoCompraGoverno(gTribCompraGov compraGoverno)
        {
            AliquotaPercentualCBS = compraGoverno?.pAliqCBS ?? 0;
            AliquotaPercentualIBSUF = compraGoverno?.pAliqIBSUF ?? 0;
            AliquotaPercentualIBSMunicipio = compraGoverno?.pAliqIBSMun ?? 0;

            ValorTributoCBS = compraGoverno?.vTribCBS ?? 0;
            ValorTributoIBSUF = compraGoverno?.vTribIBSUF ?? 0;
            ValorTributoIBSMunicipio = compraGoverno?.vTribIBSMun ?? 0;
        }

        /// <summary>pAliqIBSUF — Alíquota do IBS de competência do Estado (UB82b)</summary>
        public decimal AliquotaPercentualIBSUF { get; }

        /// <summary>vTribIBSUF — Valor do Tributo do IBS da UF calculado (UB82c)
        /// Valor que seria devido a UF, sem aplicação do Art. 473. da LC 214/2025.</summary>
        public decimal ValorTributoIBSUF { get; private set; }

        /// <summary>pAliqIBSMun — Alíquota do IBS de competência do Município (UB72d)</summary>
        public decimal AliquotaPercentualIBSMunicipio { get; }

        /// <summary>vTribIBSMun — Valor do Tributo do IBS do Município calculado (UB82e)
        /// Valor que seria devido ao município, sem aplicação do Art. 473. da LC 214/2025.</summary>
        public decimal ValorTributoIBSMunicipio { get; private set; }

        /// <summary>pAliqCBS — Alíquota da CBS (UB82f)</summary>
        public decimal AliquotaPercentualCBS { get; }

        /// <summary>vTribCBS — Valor do Tributo da CBS calculado (UB82g)
        /// Valor que seria devido a CBS, sem aplicação do Art. 473. da LC 214/2025.</summary>
        public decimal ValorTributoCBS { get; private set; }
    }
}
