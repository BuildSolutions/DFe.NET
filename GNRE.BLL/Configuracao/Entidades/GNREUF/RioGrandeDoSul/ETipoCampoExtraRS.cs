using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.RioGrandeDoSul
{
    internal enum ETipoCampoExtraRS
    {
        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("74")]
        ChaveAcessoNFeCTe = 74,
    }
}
