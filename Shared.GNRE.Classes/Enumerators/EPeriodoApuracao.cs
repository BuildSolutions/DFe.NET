using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum EPeriodoApuracao
    {
        [Description("Mensal")]
        [XmlEnum("0")]
        Mensal = 0,

        [Description("1ª Quinzena")]
        [XmlEnum("1")]
        Quinzena1 = 1,

        [Description("2ª Quinzena")]
        [XmlEnum("2")]
        Quinzena2 = 2,

        [Description("1° Decêndio")]
        [XmlEnum("3")]
        Decendio1 = 3,

        [Description("2° Decêndio")]
        [XmlEnum("4")]
        Decendio2 = 4,

        [Description("3° Decêndio")]
        [XmlEnum("5")]
        Decendio3 = 5,
    }
}
