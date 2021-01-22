using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Bahia
{
    internal enum ETipoCampoExtraBA
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("86")]
        ChaveAcessoNFe = 86,
    }
}
