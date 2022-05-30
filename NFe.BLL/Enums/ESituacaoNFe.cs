using System.ComponentModel;
using System.Xml.Serialization;

namespace NFe.BLL.Enums
{
    public enum ESituacaoNFe : uint
    {
        [Description("NFe Autorizada")]
        [XmlEnum("1")]
        Autorizada = 1,

        [Description("NFe Denegada")]
        [XmlEnum("2")]
        Denegada = 2,

        [Description("NFe Cancelada")]
        [XmlEnum("3")]
        Cancelada = 3,
    }
}
