using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Paraiba
{
    internal enum ETipoCampoExtraPB
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("30")]
        ChaveAcessoNFe = 30,

        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("99")]
        ChaveAcessoNFeCTe = 99,
    }
}
