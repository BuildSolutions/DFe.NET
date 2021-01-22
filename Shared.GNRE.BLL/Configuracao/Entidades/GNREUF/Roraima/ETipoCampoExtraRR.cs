using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Roraima
{
    internal enum ETipoCampoExtraRR
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("36")]
        ChaveAcessoNFe = 36,
    }
}
