using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Tocantins
{
    internal enum ETipoCampoExtraTO
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("80")]
        ChaveAcessoNFe = 80,
    }
}
