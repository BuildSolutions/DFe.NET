using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Alagoas
{
    internal enum ETipoCampoExtraAL
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("90")]
        ChaveAcessoNFe = 90,
    }
}
