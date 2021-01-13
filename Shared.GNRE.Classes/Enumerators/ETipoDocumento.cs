using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ETipoDocumento
    {
        [Description("NOTA FISCAL AVULSA")]
        [XmlEnum("1")]
        NotaFiscalAvulsa = 1,

        [Description("CONHECIMENTO DE TRANSPORTE RODOVIÁRIO")]
        [XmlEnum("7")]
        ConhecimentoTransporteRodoviario = 7,

        [Description("CONHECIMENTO DE TRANSPORTE AÉREO")]
        [XmlEnum("8")]
        ConhecimentoTransporteAereo = 8,

        [Description("NOTA FISCAL")]
        [XmlEnum("10")]
        NotaFiscal = 10,

        [Description("AUTO DE LANÇAMENTO")]
        [XmlEnum("13")]
        AutoLancamento = 13,

        [Description("CHAVE DA NFe")]
        [XmlEnum("22")]
        ChaveNFe = 22,

        [Description("CHAVE DO DFe")]
        [XmlEnum("24")]
        ChaveDFe = 24,

        [Description("DUIMP - DOCUMENTO ÚNICO DE IMPORTAÇÃO")]
        [XmlEnum("25")]
        DUIMP = 25,
    }
}
