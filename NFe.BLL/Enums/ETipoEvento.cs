using System.ComponentModel;
using System.Xml.Serialization;

namespace NFe.BLL.Enums
{
    public enum ETipoEvento : uint
    {
        /// <summary>
        /// 110110 - Carta de Correção
        /// </summary>
        [Description("Carta de Correção")]
        [XmlEnum("110110")]
        TeNfeCartaCorrecao = 110110,

        /// <summary>
        /// 110140 - EPEC
        /// </summary>
        [Description("EPEC")]
        [XmlEnum("110140")]
        TeNfceEpec = 110140,

        /// <summary>
        /// 110111 - Cancelamento
        /// </summary>
        [Description("Cancelamento")]
        [XmlEnum("110111")]
        TeNfeCancelamento = 110111,

        /// <summary>
        /// 110112 - Cancelamento por substituição
        /// </summary>
        [Description("Cancelamento por substituicao")]
        [XmlEnum("110112")]
        TeNfeCancelamentoSubstituicao = 110112,

        /// <summary>
        /// 210200 – Confirmação da Operação
        /// </summary>
        [Description("Confirmacao da Operacao")]
        [XmlEnum("210200")]
        TeMdConfirmacaoDaOperacao = 210200,

        /// <summary>
        /// 210210 – Ciência da Operação
        /// </summary>
        [Description("Ciencia da Operacao")]
        [XmlEnum("210210")]
        TeMdCienciaDaOperacao = 210210,

        /// <summary>
        /// 210220 – Desconhecimento da Operação
        /// </summary>
        [Description("Desconhecimento da Operacao")]
        [XmlEnum("210220")]
        TeMdDesconhecimentoDaOperacao = 210220,

        /// <summary>
        /// 210240 – Operação não Realizada
        /// </summary>
        [Description("Operacao nao Realizada")]
        [XmlEnum("210240")]
        TeMdOperacaoNaoRealizada = 210240,

        /// <summary>
        /// 790700 – Averbação para Exportação
        /// </summary>
        [Description("Averbação para Exportação")]
        [XmlEnum("790700")]
        TeMdAverbacaoparaExportacao = 790700
    }
}
