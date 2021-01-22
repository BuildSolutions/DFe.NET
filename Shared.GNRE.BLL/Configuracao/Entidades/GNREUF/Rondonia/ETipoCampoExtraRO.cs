using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Rondonia
{
    internal enum ETipoCampoExtraRO
    {
        [Description("Chave de Acesso da NFe")]
        [XmlEnum("83")]
        ChaveAcessoNFe = 83,
    }
}
