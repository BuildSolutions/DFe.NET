using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Sergipe
{
    internal enum ETipoCampoExtraSE
    {
        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("77")]
        ChaveAcessoNFeCTe = 77,
    }
}
