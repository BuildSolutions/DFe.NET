using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ECourier
    {
        /// <summary>
        /// Sim
        /// </summary>
        [Description("Sim")]
        [XmlEnum("S")]
        Sim = 1,

        /// <summary>
        /// Não
        /// </summary>
        [Description("Não")]
        [XmlEnum("N")]
        Nao = 2
    }
}
