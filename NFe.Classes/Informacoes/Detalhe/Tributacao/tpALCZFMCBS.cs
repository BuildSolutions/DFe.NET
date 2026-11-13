using System.ComponentModel;
using System.Xml.Serialization;

namespace NFe.Classes.Informacoes.Detalhe.Tributacao
{
    /// <summary>
    /// UB66b - Tipo de aplicação da alíquota zero da CBS em área incentivada (ALC/ZFM).
    /// </summary>
    public enum tpALCZFMCBS
    {
        [Description("Fornecedor e destinatário em área incentivada, alíquota zero da CBS, sem processo aprovado na Suframa para o item")]
        [XmlEnum("1")]
        tpAlcZfmCbs1 = 1,

        [Description("Fornecedor e destinatário em área incentivada, alíquota zero da CBS, operação industrial com processo aprovado na Suframa para o item")]
        [XmlEnum("2")]
        tpAlcZfmCbs2 = 2
    }
}
