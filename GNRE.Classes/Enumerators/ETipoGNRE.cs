using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ETipoGNRE
    {
        /// <summary>
        /// 0 – Guia com Multiplos Documentos Origem
        /// </summary>
        [Description("Guia Simples")]
        [XmlEnum("0")]
        GuiaSimples = 0,

        /// <summary>
        /// 1 – Guia com Multiplos Documentos Origem
        /// </summary>
        [Description("Guia com Multiplos Documentos Origem")]
        [XmlEnum("1")]
        GuiaMultiplosDocumentosOrigem = 1,

        /// <summary>
        /// 2 – Guia com Multiplas Receitas
        /// </summary>
        [Description("Guia com Multiplas Receitas")]
        [XmlEnum("2")]
        GuiaMultiplasReceitas = 2,
    }
}
