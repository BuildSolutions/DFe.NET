using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Para
{
    internal enum ETipoCampoExtraPA
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("101")]
        ChaveAcessoNFe = 101,
    }
}
