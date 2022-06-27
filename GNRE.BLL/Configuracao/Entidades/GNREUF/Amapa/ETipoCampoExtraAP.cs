using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Amapa
{
    internal enum ETipoCampoExtraAP
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("47")]
        ChaveAcessoNFe = 47,
    }
}
