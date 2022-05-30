using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ETipoDocumento
    {
        [Description("Nota Fiscal Avulsa")]
        [XmlEnum("1")]
        NotaFiscalAvulsa = 1,

        [Description("Conhecimento de Transporte Rodoviário")]
        [XmlEnum("7")]
        ConhecimentoTransporteRodoviario = 7,

        [Description("Conhecimento de Transporte Aéreo")]
        [XmlEnum("8")]
        ConhecimentoTransporteAereo = 8,

        [Description("Nota Fiscal Eletrônica")]
        [XmlEnum("10")]
        NotaFiscal = 10,

        [Description("Auto de Lançamento")]
        [XmlEnum("13")]
        AutoLancamento = 13,

        [Description("Chave de Acesso Da NFe")]
        [XmlEnum("22")]
        ChaveNFe = 22,

        [Description("Chave de Acesso do DFe")]
        [XmlEnum("24")]
        ChaveDFe = 24,

        [Description("DUIMP - Documento Único de Importação")]
        [XmlEnum("25")]
        DUIMP = 25,
    }
}
