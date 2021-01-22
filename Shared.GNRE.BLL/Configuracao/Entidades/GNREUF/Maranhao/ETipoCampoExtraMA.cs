using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Maranhao
{
    internal enum ETipoCampoExtraMA
    {
        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("94")]
        ChaveAcessoNFeCTe = 94,
    }
}
