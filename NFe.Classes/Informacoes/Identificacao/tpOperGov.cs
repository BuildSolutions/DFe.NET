using System.ComponentModel;
using System.Xml.Serialization;

//O arquivo mora em Identificacao/ por historico, mas o tipo segue no namespace
//Identificacao.Tipos, que e onde ele sempre esteve: mudar de namespace quebraria
//compilacao de quem ja usa a biblioteca.
namespace NFe.Classes.Informacoes.Identificacao.Tipos
{
    /// <summary>
    /// BB04 - Tipo de operação com o ente governamental (TOperCompraGov).
    /// <para>refDFeAnt é obrigatório nos tipos 2 e 3 e vedado nos tipos 1 e 4. O tipo 2
    /// aceita uma única chave; o tipo 3 aceita várias.</para>
    /// </summary>
    public enum TipoOperGov
    {
        [Description("Fornecimento com pagamento posterior")]
        [XmlEnum("1")]
        FornecimentoPagamentoPosterior = 1,

        [Description("Recebimento do pagamento com fornecimento já realizado")]
        [XmlEnum("2")]
        RecebimentoPagamentoFornecimentoRealizado = 2,

        [Description("Fornecimento com pagamento já realizado")]
        [XmlEnum("3")]
        FornecimentoPagamentoRealizado = 3,

        [Description("Recebimento do pagamento com fornecimento posterior")]
        [XmlEnum("4")]
        RecebimentoPagamentoFornecimentoPosterior = 4
    }
}
