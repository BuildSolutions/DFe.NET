using System;
using System.Linq;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// Os valores de ENotaFiscalCreditoTipo e ENotaFiscalDebitoTipo são os códigos
    /// oficiais de TTpNFCredito e TTpNFDebito (leiauteNFe_v4.00.xsd), persistidos no
    /// cadastro de regra fiscal. Renumerar um membro reinterpreta o dado já gravado, e
    /// valor repetido torna membro inalcançável em combo alimentado por Enum.GetValues.
    /// </summary>
    public class TiposNotaCreditoDebitoTestes
    {
        [Fact]
        public void NaoExisteValorRepetido()
        {
            foreach (var tipo in new[] { typeof(ENotaFiscalCreditoTipo), typeof(ENotaFiscalDebitoTipo) })
            {
                var nomes = Enum.GetNames(tipo);
                var valores = nomes.Select(n => (uint)Enum.Parse(tipo, n)).ToList();

                Assert.Equal(nomes.Length, valores.Distinct().Count());
                Assert.Equal(nomes.Length, Enum.GetValues(tipo).Length);
            }
        }

        [Theory]
        [InlineData(ENotaFiscalCreditoTipo.MultaEJuros, TipoNFeCredito.tcMultaEJuros)]
        [InlineData(ENotaFiscalCreditoTipo.ApropriacaoDeCreditoPresumidoIBSSobreSaldoDevedorNaZFM, TipoNFeCredito.tcApropiacaoCreditoPresumido)]
        [InlineData(ENotaFiscalCreditoTipo.RetornoPorRecusaTotalNaEntregaOuPorNaoLocalizacaoDoDestinatarioNaTentativaDeEntrega, TipoNFeCredito.tcRetornoRecusaNaoLocalizacaoEntrega)]
        [InlineData(ENotaFiscalCreditoTipo.ReducaoDeValores, TipoNFeCredito.tcReducaoValores)]
        [InlineData(ENotaFiscalCreditoTipo.TransferenciaDeCreditoNaSucessao, TipoNFeCredito.tcTransferenciaCreditoSucessao)]
        [InlineData(ENotaFiscalCreditoTipo.RetornoPorRecusaParcialNaEntrega, TipoNFeCredito.tcRetornoRecusaParcialEntrega)]
        public void CreditoUsaOCodigoDoLeiaute(ENotaFiscalCreditoTipo cadastro, TipoNFeCredito leiaute)
        {
            Assert.Equal((int)leiaute, (int)cadastro);
        }

        [Theory]
        [InlineData(ENotaFiscalDebitoTipo.TransferenciaDeCreditosParaCooperativas, TipoNFeDebito.tdTransferenciaCreditoCooperativa)]
        [InlineData(ENotaFiscalDebitoTipo.AnulacaoDeCreditoPorSaidasImunesOuIsentas, TipoNFeDebito.tdAnulacao)]
        [InlineData(ENotaFiscalDebitoTipo.DebitosDeNotasFiscaisNaoProcessadasNaApuracao, TipoNFeDebito.tdDebitosNaoProcessadas)]
        [InlineData(ENotaFiscalDebitoTipo.MultaEJuros, TipoNFeDebito.tdMultaJuros)]
        [InlineData(ENotaFiscalDebitoTipo.TransferenciaDeCreditoNaSucessao, TipoNFeDebito.tdTransferenciaCreditoSucessao)]
        [InlineData(ENotaFiscalDebitoTipo.PagamentoAntecipado, TipoNFeDebito.tdPagamentoAntecipado)]
        [InlineData(ENotaFiscalDebitoTipo.PerdaEmEstoque, TipoNFeDebito.tdPerdaEmEstoque)]
        [InlineData(ENotaFiscalDebitoTipo.DesenquadramentoDoSN, TipoNFeDebito.tdDesenquadramentoSN)]
        public void DebitoUsaOCodigoDoLeiaute(ENotaFiscalDebitoTipo cadastro, TipoNFeDebito leiaute)
        {
            Assert.Equal((int)leiaute, (int)cadastro);
        }
    }
}
