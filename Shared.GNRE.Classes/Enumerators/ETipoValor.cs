using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ETipoValor
    {
        [Description("Principal ICMS")]
        [XmlEnum("11")]
        PrincipalICMS = 11,

        [Description("Principal ICMS")]
        [XmlEnum("12")]
        PrincipalFECP = 12,
    }
}
