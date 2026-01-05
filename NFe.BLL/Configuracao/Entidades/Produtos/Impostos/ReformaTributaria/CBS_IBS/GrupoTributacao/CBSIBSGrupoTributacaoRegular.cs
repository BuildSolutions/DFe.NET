using DFe.Utils;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gTribRegular — Grupo de informações da Tributação Regular (UB68)
    /// Grupo de informações da Tributação Regular. Informar como seria a tributação caso não cumprida a condição resolutória/suspensiva.
    /// Exemplo 1: Art. 445, §4 da LC 214/2025. Operações com ZFM e ALC.
    /// Exemplo 2: Operações com suspensão do tributo.
    /// </summary>
    public class CBSIBSGrupoTributacaoRegular : IIBSGrupo, ICBSGrupo
    {
        private readonly decimal _baseCalculo;

        public CBSIBSGrupoTributacaoRegular(CSTIBSCBS cstRegular,
            int cClassTribRegular,
            decimal aliquotaPercentualRegularCBS,
            decimal aliquotaPercentualRegularIBSUF,
            decimal aliquotaPercentualRegularIBSMunicipio,
            decimal valorTributoRegularCBS,
            decimal valorTributoRegularIBSUF,
            decimal valorTributoRegularIBSMunicipio)
        {
            CSTRegular = cstRegular;
            CClassTribRegular = cClassTribRegular.ToString("D6");

            AliquotaPercentualRegularCBS = aliquotaPercentualRegularCBS;
            AliquotaPercentualRegularIBSUF = aliquotaPercentualRegularIBSUF;
            AliquotaPercentualRegularIBSMunicipio = aliquotaPercentualRegularIBSMunicipio;

            ValorTributoRegularCBS = valorTributoRegularCBS;
            ValorTributoRegularIBSUF = valorTributoRegularIBSUF;
            ValorTributoRegularIBSMunicipio = valorTributoRegularIBSMunicipio;
        }

        public CBSIBSGrupoTributacaoRegular(gTribRegular tributacaoRegular)
        {
            CSTRegular = tributacaoRegular.CSTReg;
            CClassTribRegular = tributacaoRegular.cClassTribReg;

            AliquotaPercentualRegularCBS = tributacaoRegular?.pAliqEfetRegCBS ?? 0;
            AliquotaPercentualRegularIBSUF = tributacaoRegular?.pAliqEfetRegIBSUF ?? 0;
            AliquotaPercentualRegularIBSMunicipio = tributacaoRegular?.pAliqEfetRegIBSMun ?? 0;

            ValorTributoRegularCBS = tributacaoRegular?.vTribRegCBS ?? 0;
            ValorTributoRegularIBSUF = tributacaoRegular?.vTribRegIBSUF ?? 0;
            ValorTributoRegularIBSMunicipio = tributacaoRegular?.vTribRegIBSMun ?? 0;

        }

        /// <summary>CSTReg — Código de Situação Tributária do IBS e CBS (UB69)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.
        /// Utilizar tabela CÓDIGO DE CLASSIFICAÇÃO TRIBUTÁRIA DO IBS E DA CBS</summary>
        public CSTIBSCBS CSTRegular { get; }

        /// <summary>cClassTribReg — Código de Classificação Tributária do IBS e CBS (UB70)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.
        /// Utilizar tabela CÓDIGO DE CLASSIFICAÇÃO TRIBUTÁRIA DO IBS E DA CBS</summary>
        public string CClassTribRegular { get; }

        /// <summary>pAliqEfetRegIBSUF — Valor da alíquota do IBS da UF (UB71)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.</summary>
        public decimal AliquotaPercentualRegularIBSUF { get; }

        /// <summary>pAliqEfetRegIBSMun — Valor da alíquota do IBS do Municipio (UB72a)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.</summary>
        public decimal AliquotaPercentualRegularIBSMunicipio { get; }

        /// <summary>pAliqEfetRegCBS — Valor da alíquota da CBS (UB72c)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.</summary>
        public decimal AliquotaPercentualRegularCBS { get; }

        /// <summary>vTribRegIBSUF — Valor do Tributo do IBS da UF (UB72)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.</summary>
        public decimal ValorTributoRegularIBSUF { get; private set; }

        /// <summary>vTribRegIBSMun — Valor do Tributo do IBS do município (UB72b)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.</summary>
        public decimal ValorTributoRegularIBSMunicipio { get; private set; }

        /// <summary>vTribRegCBS — Valor do Tributo da CBS (UB72d)
        /// Informado como seria caso não cumprida a condição resolutória/suspensiva.</summary>
        public decimal ValorTributoRegularCBS { get; private set; }
    }
}
