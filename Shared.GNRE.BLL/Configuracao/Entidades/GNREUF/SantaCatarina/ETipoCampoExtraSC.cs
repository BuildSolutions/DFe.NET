using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.SantaCatarina
{
    internal enum ETipoCampoExtraSC
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("84")]
        ChaveAcessoNFe = 84,
    }
}
