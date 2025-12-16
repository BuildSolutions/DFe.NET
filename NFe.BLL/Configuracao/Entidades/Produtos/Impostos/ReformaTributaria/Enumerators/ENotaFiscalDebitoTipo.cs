using System.ComponentModel;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators
{
    public enum ENotaFiscalDebitoTipo : uint
    {
        [Description("NÃO SE APLICA")]
        NaoSeAplica = 0,

        [Description("TRANSFERÊNCIA DE CRÉDITOS PARA COOPERATIVAS")]
        TransferenciaDeCreditosParaCooperativas = 1,

        [Description("ANULAÇÃO DE CRÉDITO POR SAÍDAS IMUNES/ISENTAS")]
        AnulacaoDeCreditoPorSaidasImunesOuIsentas = 2,

        [Description("DÉBITOS DE NOTAS FISCAIS NÃO PROCESSADAS NA APURAÇÃO")]
        DebitosDeNotasFiscaisNaoProcessadasNaApuracao = 3,

        [Description("MULTA E JUROS")]
        MultaEJuros = 4,

        [Description("TRANSFERÊNCIA DE CRÉDITO NA SUCESSÃO")]
        TransferenciaDeCreditoNaSucessao = 5,

        [Description("PAGAMENTO ANTECIPADO")]
        PagamentoAntecipado = 5,

        [Description("PERDA EM ESTOQUE")]
        PerdaEmEstoque = 5,

        [Description("DESENQUADRAMENTO DO SN")]
        DesenquadramentoDoSN = 5,
    }
}
