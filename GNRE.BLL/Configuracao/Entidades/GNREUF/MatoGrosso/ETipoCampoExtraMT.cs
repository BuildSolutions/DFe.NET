using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.MatoGrosso
{
    internal enum ETipoCampoExtraMT
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("17")]
        ChaveAcessoNFe = 17,

        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("108")]
        ChaveAcessoNFeCTe = 108,
    }
}
