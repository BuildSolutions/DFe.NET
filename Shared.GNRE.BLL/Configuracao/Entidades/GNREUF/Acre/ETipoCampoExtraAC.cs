using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Acre
{
    internal enum ETipoCampoExtraAC
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("76")]
        ChaveAcessoNFe = 76,
    }
}
