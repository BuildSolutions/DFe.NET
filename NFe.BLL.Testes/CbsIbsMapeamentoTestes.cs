using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.CreditoPresumido;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Total;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// Guarda os mapeamentos de IBS/CBS da NFe.BLL que já saíram trocados uma vez.
    /// Cada Assert aqui falha se o campo voltar a ser ligado no vizinho errado.
    /// </summary>
    public class CbsIbsMapeamentoTestes
    {
        [Fact]
        public void GrupoMonofasicoPadrao_LeAdRemDeCadaImposto()
        {
            var padrao = new gMonoPadrao { qBCMono = 10m, adRemIBS = 2m, adRemCBS = 3m, vIBSMono = 20m, vCBSMono = 30m };

            var grupo = new CBSIBSGrupoMonofasicoTributado(padrao);

            Assert.Equal(10m, grupo.Quantidade);
            Assert.Equal(3m, grupo.AliquotaAdRemCBS);
            Assert.Equal(2m, grupo.AliquotaAdRemIBS);
            Assert.Equal(30m, grupo.ValorMonofasicoCBS);
            Assert.Equal(20m, grupo.ValorMonofasicoIBS);
        }

        [Fact]
        public void GrupoMonofasicoRetido_LeAdRemDeCadaImposto()
        {
            var retido = new gMonoRet { qBCMonoRet = 11m, adRemIBSRet = 4m, adRemCBSRet = 5m, vIBSMonoRet = 40m, vCBSMonoRet = 50m };

            var grupo = new CBSIBSGrupoMonofasicoRetido(retido);

            Assert.Equal(11m, grupo.Quantidade);
            Assert.Equal(5m, grupo.AliquotaAdRemCBS);
            Assert.Equal(4m, grupo.AliquotaAdRemIBS);
        }

        [Fact]
        public void GrupoMonofasicoSujeitoRetencao_LeAdRemDeCadaImposto()
        {
            var reten = new gMonoReten { qBCMonoReten = 12m, adRemIBSReten = 6m, adRemCBSReten = 7m, vIBSMonoReten = 60m, vCBSMonoReten = 70m };

            var grupo = new CBSIBSGrupoMonofasicoSujeitoRetencao(reten);

            Assert.Equal(12m, grupo.Quantidade);
            Assert.Equal(7m, grupo.AliquotaAdRemCBS);
            Assert.Equal(6m, grupo.AliquotaAdRemIBS);
        }

        [Fact]
        public void TotalizadorIbs_SeparaUfDeMunicipio()
        {
            var total = new gIBSTotal
            {
                vIBS = 100m,
                gIBSUF = new gIBSUFTotal { vIBSUF = 60m, vDif = 1m, vDevTrib = 2m },
                gIBSMun = new gIBSMunTotal { vIBSMun = 40m, vDif = 3m, vDevTrib = 4m }
            };

            var totalizador = new TotalizadorIBS(total);

            Assert.Equal(60m, totalizador.TotalizadorIBSUF.ValorTotal);
            Assert.Equal(1m, totalizador.TotalizadorIBSUF.DiferimentoValorTotal);
            Assert.Equal(40m, totalizador.TotalizadorIBSMunicipio.ValorTotal);
            Assert.Equal(3m, totalizador.TotalizadorIBSMunicipio.DiferimentoValorTotal);
            Assert.Equal(4m, totalizador.TotalizadorIBSMunicipio.DevolucaoTributosValorTotal);
        }

        [Fact]
        public void TotalizadorCbs_GuardaCreditoPresumido()
        {
            var totalizador = new TotalizadorCBS(diferimentoValorTotal: 1m,
                devolucaoTributosValorTotal: 2m,
                valorTotal: 3m,
                creditoPresumidoValorTotal: 4m,
                creditoPresumidoEmCondicaoSuspensivaValorTotal: 5m);

            Assert.Equal(4m, totalizador.CreditoPresumidoValorTotal);
            Assert.Equal(5m, totalizador.CreditoPresumidoEmCondicaoSuspensivaValorTotal);
        }

        [Fact]
        public void CbsGrupo_LidoDoXml_ETipadoComoCbs()
        {
            var gIbsCbs = new gIBSCBS { vBC = 100m, gCBS = new gCBS { pCBS = 8.8m, vCBS = 8.8m } };

            var grupo = new CBSGrupo(gIbsCbs);

            Assert.Equal(EImpostoReformaTributariaTipo.CBS, grupo.EImpostoReformaTributariaTipo);
            Assert.Equal(8.8m, grupo.AliquotaPercentual);
        }

        [Fact]
        public void CreditoPresumido_GuardaCodigoBaseEGrupoIbs()
        {
            var credito = new CBSIBSGrupoCreditoPresumido(
                codigoClassificacaoCreditoPresumido: TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculoCreditoPresumido: 1000m,
                cbsPercentualCreditoPresumido: null,
                cbsValorCreditoPresumido: null,
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                ibsPercentualCreditoPresumido: 5m,
                ibsValorCreditoPresumido: 50m,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: 0m,
                zfmDataCompetenciaApuracao: null,
                zfmTipoCreditoPresumido: tpCredPresIBSZFM.tpCredPresIbsZfm0,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: 0m);

            Assert.Equal(TipocCredPres.AquisicaoResiduosReciclagem, credito.CodigoClassificacaoCreditoPresumido);
            Assert.Equal(1000m, credito.BaseCalculoCreditoPresumido);
            // o grupo do IBS depende do percentual do IBS, nao do da CBS
            Assert.NotNull(credito.IBSGrupoCreditoPresumido);
            Assert.Equal(50m, credito.IBSGrupoCreditoPresumido.ValorCredito);
            Assert.Null(credito.CBSGrupoCreditoPresumido);
            // sem competencia de apuracao o grupo ZFM nao pode existir
            Assert.Null(credito.IBSGrupoCreditoPresumidoZFM);
        }

        [Fact]
        public void CbsIbsGrupo_GuardaGrupoDeCreditoPresumido()
        {
            var credito = new CBSIBSGrupoCreditoPresumido(
                codigoClassificacaoCreditoPresumido: TipocCredPres.AquisicaoProdutorRuralNaoContribuinte,
                baseCalculoCreditoPresumido: 500m,
                cbsPercentualCreditoPresumido: 1m,
                cbsValorCreditoPresumido: 5m,
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: 0m,
                ibsPercentualCreditoPresumido: null,
                ibsValorCreditoPresumido: null,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                zfmDataCompetenciaApuracao: null,
                zfmTipoCreditoPresumido: tpCredPresIBSZFM.tpCredPresIbsZfm0,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: 0m);

            var grupo = new CBSIBSGrupo(baseCalculo: 100m,
                ibsGrupo: null,
                cbsGrupo: null,
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: credito);

            Assert.Same(credito, grupo.GrupoCreditoPresumido);
        }

        [Fact]
        public void Imposto_GuardaEReleIndicadorDeDoacao()
        {
            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: true,
                cbsIbs: null);

            Assert.True(imposto.IsDoacao);
            Assert.Equal("000001", imposto.ClassificacaoTributaria);

            var lido = new CBSIBSImposto(new IBSCBS { CST = CSTIBSCBS.cst000, cClassTrib = "000001", indDoacao = "1" });

            Assert.True(lido.IsDoacao);
        }

        [Fact]
        public void EnderecoDeEntrega_PreservaZeroAEsquerdaDoCep()
        {
            var entrega = new entrega
            {
                xLgr = "RUA UM",
                nro = "1",
                xBairro = "CENTRO",
                cMun = 3509502,
                xMun = "CAMPINAS",
                UF = "SP",
                CEP = 1001000,
                cPais = 1058,
                xPais = "BRASIL"
            };

            var endereco = new Endereco(entrega);

            Assert.Equal("01001000", endereco.CEP);
        }
    }
}
