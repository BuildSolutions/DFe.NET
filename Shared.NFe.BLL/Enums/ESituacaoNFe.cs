using System.ComponentModel;
using System.Xml.Serialization;

namespace NFe.BLL.Enums
{
    public enum ESituacaoNFe
    {
        [Description("NFe Autorizada")]
        [XmlEnum("1")]
        Autorizada = 1,

        [Description("NFe Denegada")]
        [XmlEnum("3")]
        Denegada = 2
    }
}
