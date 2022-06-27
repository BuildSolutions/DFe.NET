using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.RioDeJaneiro
{
    internal enum ETipoCampoExtraRJ
    {
        [Description("Data de Emissão")]
        [XmlEnum("117")]
        DataEmissao = 117,
    }
}
