using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.RioGrandeDoNorte
{
    internal enum ETipoCampoExtraRN
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("97")]
        ChaveAcessoNFe = 97,
    }
}
