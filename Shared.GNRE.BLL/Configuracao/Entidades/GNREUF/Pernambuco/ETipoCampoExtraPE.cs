using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Pernambuco
{
    internal enum ETipoCampoExtraPE
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("9")]
        ChaveAcessoNFe = 9,

        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("92")]
        ChaveAcessoNFeCTe = 92,
    }
}
