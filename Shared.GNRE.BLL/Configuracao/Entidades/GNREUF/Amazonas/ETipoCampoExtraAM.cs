using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Amazonas
{
    internal enum ETipoCampoExtraAM
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("12")]
        ChaveAcessoNFe = 12,

        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("96")]
        ChaveAcessoNFeCTe = 96,
    }
}
