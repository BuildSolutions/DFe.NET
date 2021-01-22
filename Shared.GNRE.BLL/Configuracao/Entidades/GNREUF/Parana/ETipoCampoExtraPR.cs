using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Parana
{
    internal enum ETipoCampoExtraPR
    {
        [Description("Chave de Acesso da NFe ou do CTe/CTE-OS")]
        [XmlEnum("107")]
        ChaveAcessoNFeCTe = 107,
    }
}
