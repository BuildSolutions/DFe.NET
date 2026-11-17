using System.Collections.Generic;
using NFe.BLL.Configuracao.Entidades;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// B25.1 e B25.2 são campos separados no XML, mas a entidade guarda UM código: o lado
    /// vem do finNFe (5 crédito, 6 débito). Na leitura do XML os dois voltam para a mesma
    /// propriedade, e é essa fusão que estes testes cobrem — sem ela a nota lida de arquivo
    /// perderia o tipo, que é o que a rejeição 1202 cobra.
    /// </summary>
    public class TipoNotaCreditoDebitoNoIdeTestes
    {
        private static NotaFiscal Ler(ide ide)
        {
            return new NotaFiscal(new NFe.Classes.nfeProc
            {
                NFe = new NFe.Classes.NFe
                {
                    infNFe = new NFe.Classes.Informacoes.infNFe
                    {
                        ide = ide,
                        emit = new NFe.Classes.Informacoes.Emitente.emit(),
                        total = new NFe.Classes.Informacoes.Total.total(),
                        det = new List<NFe.Classes.Informacoes.Detalhe.det>()
                    }
                }
            });
        }

        [Fact]
        public void NotaDeCreditoVoltaComOTipoDoCredito()
        {
            var lido = Ler(new ide
            {
                finNFe = FinalidadeNFe.fnNotaCredito,
                tpNFCredito = TipoNFeCredito.tcTransferenciaCreditoSucessao
            });

            Assert.Equal(FinalidadeNFe.fnNotaCredito, lido.EFinalidadeNFe);
            Assert.Equal((int)TipoNFeCredito.tcTransferenciaCreditoSucessao, lido.TipoNotaCreditoDebito);
        }

        [Fact]
        public void NotaDeDebitoVoltaComOTipoDoDebito()
        {
            var lido = Ler(new ide
            {
                finNFe = FinalidadeNFe.fnNotaDebito,
                tpNFDebito = TipoNFeDebito.tdAnulacao
            });

            Assert.Equal(FinalidadeNFe.fnNotaDebito, lido.EFinalidadeNFe);
            Assert.Equal((int)TipoNFeDebito.tdAnulacao, lido.TipoNotaCreditoDebito);
        }

        /// <summary>
        /// Os códigos se repetem com significados diferentes entre os dois enums — 02 é
        /// "Apropriação de crédito presumido de IBS na ZFM" no crédito e "Anulação de Crédito
        /// por Saídas Imunes/Isentas" no débito. O número só se lê com a finalidade ao lado.
        /// </summary>
        [Fact]
        public void OMesmoCodigoEmFinalidadesDiferentesVoltaIgual()
        {
            var credito = Ler(new ide
            {
                finNFe = FinalidadeNFe.fnNotaCredito,
                tpNFCredito = TipoNFeCredito.tcApropiacaoCreditoPresumido
            });

            var debito = Ler(new ide
            {
                finNFe = FinalidadeNFe.fnNotaDebito,
                tpNFDebito = TipoNFeDebito.tdAnulacao
            });

            Assert.Equal(2, credito.TipoNotaCreditoDebito);
            Assert.Equal(2, debito.TipoNotaCreditoDebito);
            Assert.NotEqual(credito.EFinalidadeNFe, debito.EFinalidadeNFe);
        }

        /// <summary>
        /// B25-80, rejeição 1001: nota de crédito e de débito não levam os grupos de imposto do
        /// regime antigo — ICMS, ICMSUFDest, IPI, II, PIS, COFINS, <c>impostoDevol</c> e os
        /// totais correspondentes no <c>ICMSTot</c> —, nem zerados. As outras quatro finalidades
        /// levam, e o tipo não as afeta.
        /// </summary>
        [Theory]
        [InlineData(FinalidadeNFe.fnNormal, null)]
        [InlineData(FinalidadeNFe.fnComplementar, null)]
        [InlineData(FinalidadeNFe.fnAjuste, null)]
        [InlineData(FinalidadeNFe.fnDevolucao, null)]
        public void FinalidadeSemCreditoNemDebitoLevaImpostoDoRegimeAntigo(FinalidadeNFe finNFe, int? tipo)
        {
            Assert.True(DadosNFe.FinalidadeLevaImpostoDoRegimeAntigo(finNFe, tipo));
        }

        /// <summary>
        /// Os quatro tipos em que a mercadoria CIRCULA são exceção da B25-80 e continuam levando
        /// imposto: crédito 03 e 06 (retorno por recusa total e parcial), crédito 04 (redução de
        /// valores) e débito 07 (perda em estoque).
        /// </summary>
        [Theory]
        [InlineData(FinalidadeNFe.fnNotaCredito, TipoNFeCredito.tcMultaEJuros, false)]
        [InlineData(FinalidadeNFe.fnNotaCredito, TipoNFeCredito.tcApropiacaoCreditoPresumido, false)]
        [InlineData(FinalidadeNFe.fnNotaCredito, TipoNFeCredito.tcRetornoRecusaNaoLocalizacaoEntrega, true)]
        [InlineData(FinalidadeNFe.fnNotaCredito, TipoNFeCredito.tcReducaoValores, true)]
        [InlineData(FinalidadeNFe.fnNotaCredito, TipoNFeCredito.tcTransferenciaCreditoSucessao, false)]
        [InlineData(FinalidadeNFe.fnNotaCredito, TipoNFeCredito.tcRetornoRecusaParcialEntrega, true)]
        public void CreditoLevaImpostoSoNosTiposDeExcecao(FinalidadeNFe finNFe, TipoNFeCredito tipo, bool leva)
        {
            Assert.Equal(leva, DadosNFe.FinalidadeLevaImpostoDoRegimeAntigo(finNFe, (int)tipo));
        }

        [Theory]
        [InlineData(TipoNFeDebito.tdTransferenciaCreditoCooperativa, false)]
        [InlineData(TipoNFeDebito.tdAnulacao, false)]
        [InlineData(TipoNFeDebito.tdDebitosNaoProcessadas, false)]
        [InlineData(TipoNFeDebito.tdMultaJuros, false)]
        [InlineData(TipoNFeDebito.tdTransferenciaCreditoSucessao, false)]
        [InlineData(TipoNFeDebito.tdPagamentoAntecipado, false)]
        [InlineData(TipoNFeDebito.tdPerdaEmEstoque, true)]
        [InlineData(TipoNFeDebito.tdDesenquadramentoSN, false)]
        public void DebitoLevaImpostoSoNaPerdaEmEstoque(TipoNFeDebito tipo, bool leva)
        {
            Assert.Equal(leva, DadosNFe.FinalidadeLevaImpostoDoRegimeAntigo(FinalidadeNFe.fnNotaDebito, (int)tipo));
        }

        /// <summary>
        /// Três decisões dependem de "é crédito ou débito?" e têm de concordar: os grupos de
        /// imposto do item e os totais, o <c>pag</c> (sai com <c>fpSemPagamento</c> e
        /// <c>vPag = 0</c>) e o <c>cobr</c> (não sai). Ajuste e devolução ficam fora: o
        /// <c>vNF</c> deles não é recalculado, então a fatura fecha.
        /// </summary>
        [Theory]
        [InlineData(FinalidadeNFe.fnNormal, false)]
        [InlineData(FinalidadeNFe.fnComplementar, false)]
        [InlineData(FinalidadeNFe.fnAjuste, false)]
        [InlineData(FinalidadeNFe.fnDevolucao, false)]
        [InlineData(FinalidadeNFe.fnNotaCredito, true)]
        [InlineData(FinalidadeNFe.fnNotaDebito, true)]
        public void CreditoEDebitoSaoAsDuasFinalidadesSemImpostoPagamentoNemCobranca(FinalidadeNFe finNFe, bool ehCreditoOuDebito)
        {
            Assert.Equal(ehCreditoOuDebito, DadosNFe.EhNotaDeCreditoOuDebito(finNFe));
        }

        /// <summary>
        /// A W16-10 refaz a conta do <c>vNF</c> com o que está no XML: produto, frete, seguro e
        /// despesas, menos desconto, mais os impostos que <b>saem</b>. Com os impostos do regime
        /// antigo zerados (B25-80), o <c>vNF</c> que vem do total do banco — que os soma — deixa de
        /// fechar e a nota toma rejeição <b>610</b>. Esta é a conta que o <c>GetIcmsTotal</c> refaz.
        /// </summary>
        [Theory]
        [InlineData(100, 0, 0, 0, 0, 100)]
        [InlineData(100, 10, 5, 2, 1, 98)]
        [InlineData(250.75, 0.75, 10, 0, 0, 260)]
        public void TotalDaNotaSemImpostoAntigoEhSoOValorDoDocumento(decimal vProd, decimal vDesc,
            decimal vFrete, decimal vSeg, decimal vOutro, decimal vNFEsperado)
        {
            Assert.Equal(vNFEsperado,
                DadosNFe.TotalDaNotaSemImpostoDoRegimeAntigo(vProd, vDesc, vFrete, vSeg, vOutro));
        }

        /// <summary>
        /// Crédito ou débito <b>sem tipo</b> não é emissível — o validador recusa antes —, mas a
        /// decisão não pode cair na exceção por tabela: sem tipo, nada de imposto antigo.
        /// </summary>
        [Theory]
        [InlineData(FinalidadeNFe.fnNotaCredito)]
        [InlineData(FinalidadeNFe.fnNotaDebito)]
        public void CreditoEDebitoSemTipoNaoLevamImposto(FinalidadeNFe finNFe)
        {
            Assert.False(DadosNFe.FinalidadeLevaImpostoDoRegimeAntigo(finNFe, null));
        }

        [Fact]
        public void FinalidadeSemTipoNaoInventaCodigo()
        {
            var lido = Ler(new ide { finNFe = FinalidadeNFe.fnNormal });

            Assert.Null(lido.TipoNotaCreditoDebito);
        }
    }
}
