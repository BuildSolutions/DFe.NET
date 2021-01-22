using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ETipoValor
    {
        [Description("Principal ICMS")]
        [XmlEnum("11")]
        PrincipalICMS = 11,

        [Description("Principal FECP")]
        [XmlEnum("12")]
        PrincipalFECP = 12,

        [Description("Valor Total ICMS")]
        [XmlEnum("21")]
        TotalICMS = 21,

        [Description("Valor Total FECP")]
        [XmlEnum("22")]
        TotalFECP = 22,

        [Description("Valor Multa ICMS")]
        [XmlEnum("31")]
        ValorMultaICMS = 31,

        [Description("Valor Multa FECP")]
        [XmlEnum("32")]
        ValorMultaFECP = 32,

        [Description("Valor Juros ICMS")]
        [XmlEnum("41")]
        ValorJurosICMS = 41,

        [Description("Valor Juros FECP")]
        [XmlEnum("42")]
        ValorJurosFECP = 42,

        [Description("Valor Atualizacao Monetária ICMS")]
        [XmlEnum("51")]
        ValorAtualizacaoMonetariaICMS = 51,

        [Description("Valor Atualizacao Monetária FECP")]
        [XmlEnum("52")]
        ValorAtualizacaoMonetariaFECP = 52,
    }
}
