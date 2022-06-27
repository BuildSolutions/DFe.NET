using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Goias
{
    internal enum ETipoCampoExtraGO
    {
        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("102")]
        ChaveAcessoNFeCTe = 102,
    }
}
