using System.ComponentModel;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators
{
    public enum ENotaFiscalCreditoTipo : uint
    {
        [Description("NÃO SE APLICA")]
        NaoSeAplica = 0,

        [Description("MULTA E JUROS")]
        MultaEJuros = 1,

        [Description("APROPRIAÇÃO DE CRÉDITO PRESUMIDO DE IBS SOBRE O SALDO DEVEDOR NA ZFM")]
        ApropriacaoDeCreditoPresumidoIBSSobreSaldoDevedorNaZFM = 2,

        [Description("RETORNO POR RECUSA TOTAL NA ENTREGA OU POR NÃO LOCALIZAÇÃO DO DESTINATÁRIO NA TENTATIVA DE ENTREGA")]
        RetornoPorRecusaTotalNaEntregaOuPorNaoLocalizacaoDoDestinatarioNaTentativaDeEntrega = 3,

        [Description("REDUÇÃO DE VALORES")]
        ReducaoDeValores = 4,

        [Description("TRANSFERÊNCIA DE CRÉDITO NA SUCESSÃO")]
        TransferenciaDeCreditoNaSucessao = 4,
    }
}
