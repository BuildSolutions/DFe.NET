using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Ceara
{
    internal enum ETipoCampoExtraCE
    {
        [Description("Data da saida da Mercadoria")]
        [XmlEnum("50")]
        DataSaidaMercadoria = 50,
    }
}
