using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum ECampoExtra
    {
        [Description("NOME DO REMETENTE")]
        [XmlEnum("24")]
        NomeRemetente = 24,

        [Description("CNPJ REMETENTE")]
        [XmlEnum("25")]
        CNPJRemetente = 25,

        [Description("NRO DA NOTA FISCAL")]
        [XmlEnum("26")]
        NumeroNotaFiscal = 26,

        [Description("Obs 1")]
        [XmlEnum("68")]
        Obs1 = 68,

        [Description("Obs 2")]
        [XmlEnum("69")]
        Obs2 = 69,

        [Description("Chave de Acesso da NFe")]
        [XmlEnum("76")]
        ChaveAcessoNFe = 76,
    }
}
