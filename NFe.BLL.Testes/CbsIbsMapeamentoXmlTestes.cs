using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.CreditoPresumido;
using Monofasico = NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// Testa o mapeamento de IBS/CBS que vai para o XML, e não só o construtor da entidade:
    /// chama DadosNFe.GetProdutoCBSIBS, serializa e valida contra o PL_010f.
    /// <para>TCIBSCBS tem dois xs:choice. O primeiro entre gIBSCBS, gIBSCBSMono,
    /// gTransfCred e gAjusteCompet; o segundo entre gCredPresOper e gCredPresIBSZFM.</para>
    /// </summary>
    public class CbsIbsMapeamentoXmlTestes
    {
        private const string NamespaceNfe = "http://www.portalfiscal.inf.br/nfe";

        private const string SchemaEnvelope =
            "<xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"" + NamespaceNfe + "\" targetNamespace=\"" + NamespaceNfe + "\" elementFormDefault=\"qualified\">" +
            "<xs:include schemaLocation=\"DFeTiposBasicos_v1.00.xsd\"/>" +
            "<xs:element name=\"IBSCBS\" type=\"TTribNFe\"/>" +
            "</xs:schema>";

        private static List<string> ValidarContraPl010f(IBSCBS ibsCbs)
        {
            var serializador = new XmlSerializer(typeof(IBSCBS), NamespaceNfe);
            string xml;
            using (var sw = new StringWriter())
            {
                serializador.Serialize(sw, ibsCbs);
                xml = sw.ToString();
            }

            var erros = new List<string>();
            var schemas = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            using (var reader = XmlReader.Create(new StringReader(SchemaEnvelope), new XmlReaderSettings(), Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "envelope.xsd")))
                schemas.Add(NamespaceNfe, reader);

            var cfg = new XmlReaderSettings { ValidationType = ValidationType.Schema, Schemas = schemas };
            cfg.ValidationEventHandler += (sender, e) => erros.Add(e.Message);

            using (var reader = XmlReader.Create(new StringReader(xml), cfg))
                while (reader.Read()) { }

            return erros;
        }

        private static CBSIBSImposto ObterImposto(CBSIBSGrupoCreditoPresumido creditoPresumido)
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: new CBSGrupo(aliquota: 8.8m, valor: 88m),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: creditoPresumido);

            return new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);
        }

        private static CBSIBSGrupoCreditoPresumido ObterCreditoPresumido(TipocCredPres? codigo,
            decimal baseCalculo,
            decimal? cbsPercentual,
            string zfmCompetencia)
        {
            return new CBSIBSGrupoCreditoPresumido(codigoClassificacaoCreditoPresumido: codigo,
                baseCalculoCreditoPresumido: baseCalculo,
                cbsPercentualCreditoPresumido: cbsPercentual,
                cbsValorCreditoPresumido: cbsPercentual.HasValue ? 10m : (decimal?)null,
                // null, e nao zero: vCredPres e vCredPresCondSus sao um xs:choice, e quem
                // diz qual dos dois saiu e a PRESENCA do valor.
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                ibsPercentualCreditoPresumido: null,
                ibsValorCreditoPresumido: null,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                zfmDataCompetenciaApuracao: zfmCompetencia,
                // sem competencia nao ha grupo de ZFM, logo tipo e valor ficam zerados
                zfmTipoCreditoPresumido: string.IsNullOrEmpty(zfmCompetencia)
                    ? tpCredPresIBSZFM.tpCredPresIbsZfm0
                    : tpCredPresIBSZFM.tpCredPresIbsZfm1,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: string.IsNullOrEmpty(zfmCompetencia) ? 0m : 55m);
        }

        [Fact]
        public void OperacaoSemNenhumSubgrupo_ERecusada()
        {
            // O schema deixa gIBSCredPres e gCBSCredPres como minOccurs=0, mas os 13
            // codigos da tabela cCredPres (Anexo IV da NT 2025.002) tem ind_gCBSCredPres
            // ou ind_gIBSCredPres igual a 1 - nenhum aceita o grupo da operacao sem
            // nenhum dos dois. Opcional no XSD nao significa dispensavel na regra.
            var erro = Assert.Throws<ArgumentException>(() => ObterCreditoPresumido(TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculo: 1000m,
                cbsPercentual: null,
                zfmCompetencia: null));

            Assert.Contains("pelo menos um dos dois", erro.Message);
        }

        private static CBSIBSGrupo GrupoDeAjuste(string competencia)
        {
            return new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: null,
                cbsGrupo: null,
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null,
                grupoAjusteCompetencia: new CBSIBSGrupoAjusteCompetencia(anoMesCompetenciaApuracao: competencia,
                    valorAjusteIBS: 12m,
                    valorAjusteCBS: 3m));
        }

        [Fact]
        public void AjusteDeCompetencia_GeraSomenteGAjusteCompet()
        {
            // CST 811 (AJUSTES) tem ind_gIBSCBS = N: o grupo normal nao pode sair, e o
            // ajuste ocupa a mesma opcao do xs:choice de TTribNFe.
            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst811,
                classificacaoTributaria: 811001,
                isDoacao: false,
                cbsIbs: GrupoDeAjuste("2026-11"));

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gIBSCBS);
            Assert.Null(ibsCbs.gIBSCBSMono);
            Assert.Null(ibsCbs.gTransfCred);
            Assert.Equal("2026-11", ibsCbs.gAjusteCompet.competApur);
            Assert.Equal(12m, ibsCbs.gAjusteCompet.vIBS);
            Assert.Equal(3m, ibsCbs.gAjusteCompet.vCBS);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Theory]
        [InlineData("11/2026")]
        [InlineData("2026-13")]
        [InlineData("2024-12")]
        public void CompetenciaDoAjusteForaDoLeiaute_ERecusada(string competencia)
        {
            var erro = Assert.Throws<ArgumentException>(() => GrupoDeAjuste(competencia));

            Assert.Contains("AAAA-MM", erro.Message);
        }

        [Fact]
        public void AjusteDeCompetenciaSemPeriodoDeApuracao_ERecusado()
        {
            // competApur nao e opcional dentro de TAjusteCompet: grupo sem ela tem de
            // acusar no cadastro, nao sair do XML em silencio.
            var erro = Assert.Throws<ArgumentException>(() => GrupoDeAjuste(string.Empty));

            Assert.Contains("competApur", erro.Message);
        }

        [Fact]
        public void AjusteDeCompetenciaComTributacaoNormal_ERecusado()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: new CBSGrupo(aliquota: 0.9m, valor: 9m),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null,
                grupoAjusteCompetencia: new CBSIBSGrupoAjusteCompetencia(anoMesCompetenciaApuracao: "2026-11",
                    valorAjusteIBS: 1m,
                    valorAjusteCBS: 1m));

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst811,
                classificacaoTributaria: 811001,
                isDoacao: false,
                cbsIbs: grupo);

            var erro = Assert.Throws<ArgumentException>(() => DadosNFe.GetProdutoCBSIBS(imposto));

            Assert.Contains("somente um desses grupos", erro.Message);
        }

        [Fact]
        public void EstornoDeCredito_ConviveComOGrupoNormal()
        {
            // gEstornoCred e irmao opcional do xs:choice em TTribNFe, nao uma das opcoes:
            // sai junto com gIBSCBS sem ferir o schema.
            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 410030,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(baseCalculo: 1000m,
                    ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                        ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                        valorTotal: 1.1m),
                    cbsGrupo: new CBSGrupo(aliquota: 0.9m, valor: 9m),
                    grupoTributacaoRegular: null,
                    grupoTributacaoCompraGoverno: null,
                    grupoTributacaoMonofasico: null,
                    grupoTransferenciaCredito: null,
                    grupoCreditoPresumido: null),
                grupoEstornoCredito: new CBSIBSGrupoEstornoCredito(valorEstornoIBS: 1.1m, valorEstornoCBS: 9m));

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.NotNull(ibsCbs.gIBSCBS);
            Assert.Equal(1.1m, ibsCbs.gEstornoCred.vIBSEstCred);
            Assert.Equal(9m, ibsCbs.gEstornoCred.vCBSEstCred);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void EstornoDeCreditoZerado_NaoVaiParaOXml()
        {
            // vIBSEstCred e vCBSEstCred sao obrigatorios dentro do grupo: grupo zerado nao
            // informa nada e so ocupa espaco no XML.
            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 410030,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(baseCalculo: 1000m,
                    ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                        ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                        valorTotal: 1.1m),
                    cbsGrupo: new CBSGrupo(aliquota: 0.9m, valor: 9m),
                    grupoTributacaoRegular: null,
                    grupoTributacaoCompraGoverno: null,
                    grupoTributacaoMonofasico: null,
                    grupoTransferenciaCredito: null,
                    grupoCreditoPresumido: null),
                grupoEstornoCredito: new CBSIBSGrupoEstornoCredito(valorEstornoIBS: 0m, valorEstornoCBS: 0m));

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gEstornoCred);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        /// <summary>
        /// "00 - Sem Crédito Presumido" representa a ausência do crédito: o grupo
        /// gCredPresOper não deve sair. O enum publica o valor, então a recusa tem de dizer
        /// o que fazer em vez de reclamar de percentual.
        /// </summary>
        [Fact]
        public void CreditoPresumidoComCodigoZero_ERecusado()
        {
            var erro = Assert.Throws<ArgumentException>(() => ObterCreditoPresumido(TipocCredPres.SemCreditoPresumido,
                baseCalculo: 500m,
                cbsPercentual: 2m,
                zfmCompetencia: null));

            Assert.Contains("não informe o grupo", erro.Message);
        }

        /// <summary>
        /// Crédito aproveitado com valor e sem condição suspensiva: o caminho comum, e o que
        /// a emissão a partir do banco produz. Guarda contra regressão da mudança para
        /// <c>decimal?</c>: informar os dois passou a ser recusado, e quem monta a partir de
        /// coluna não anulável tem de mandar null no lado ausente.
        /// </summary>
        [Fact]
        public void CreditoAproveitadoSemSuspensivo_VaiNoXml()
        {
            var credito = new CBSIBSGrupoCreditoPresumido(codigoClassificacaoCreditoPresumido: TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculoCreditoPresumido: 500m,
                cbsPercentualCreditoPresumido: 2m,
                cbsValorCreditoPresumido: 10m,
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                ibsPercentualCreditoPresumido: null,
                ibsValorCreditoPresumido: null,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                zfmDataCompetenciaApuracao: null,
                zfmTipoCreditoPresumido: tpCredPresIBSZFM.tpCredPresIbsZfm0,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: 0m);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(ObterImposto(credito));

            Assert.Equal(10m, ibsCbs.gCredPresOper.gCBSCredPres.vCredPres);
            Assert.Null(ibsCbs.gCredPresOper.gCBSCredPres.vCredPresCondSus);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        /// <summary>
        /// Crédito suspensivo de valor zero continua no braço suspensivo do xs:choice. Até
        /// 11/11/2026 a entidade colapsava ausência e zero no mesmo <c>decimal</c>, e um
        /// <c>vCredPresCondSus</c> zero voltava como <c>vCredPres</c> zero na reescrita —
        /// mesmo número, outra semântica fiscal.
        /// </summary>
        [Fact]
        public void CreditoSuspensivoZero_NaoViraCreditoAproveitado()
        {
            var original = new gCredPresOper
            {
                cCredPres = TipocCredPres.AquisicaoResiduosReciclagem,
                vBCCredPres = 500m,
                gCBSCredPres = new gIBSCredPres { pCredPres = 2m, vCredPresCondSus = 0m }
            };

            var lido = new CBSIBSGrupoCreditoPresumido(creditoPresumidoOperacao: original, creditoPresumidoZFM: null);

            Assert.Null(lido.CBSGrupoCreditoPresumido.ValorCredito);
            Assert.Equal(0m, lido.CBSGrupoCreditoPresumido.ValorCreditoEmCondicaoSuspensiva);

            var reescrito = DadosNFe.GetProdutoCBSIBS(new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(baseCalculo: 1000m,
                    ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                        ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                        valorTotal: 1.1m),
                    cbsGrupo: new CBSGrupo(aliquota: 8.8m, valor: 88m),
                    grupoTributacaoRegular: null,
                    grupoTributacaoCompraGoverno: null,
                    grupoTributacaoMonofasico: null,
                    grupoTransferenciaCredito: null,
                    grupoCreditoPresumido: lido)));

            Assert.Equal(0m, reescrito.gCredPresOper.gCBSCredPres.vCredPresCondSus);
            Assert.True(reescrito.gCredPresOper.gCBSCredPres.ShouldSerializevCredPresCondSus()
                || reescrito.gCredPresOper.gCBSCredPres.vCredPresCondSus.HasValue);
            Assert.Empty(ValidarContraPl010f(reescrito));
        }

        /// <summary>
        /// gALCZFMCBS pelo caminho normal da BLL: a entidade entra no CBSGrupo e sai no XML
        /// sem ninguém mexer no DTO depois. Até 11/11/2026 o grupo só existia na camada XML,
        /// e estes testes injetavam a classe crua no objeto já montado - testavam a
        /// serialização e não o que a biblioteca entrega.
        /// </summary>
        [Fact]
        public void GrupoDeAreaIncentivadaComProcessoSuframa_VaiNoXml()
        {
            var ibsCbs = DadosNFe.GetProdutoCBSIBS(ObterImpostoComAreaIncentivada(
                new CBSGrupoAreaIncentivada(tipoAplicacao: tpALCZFMCBS.tpAlcZfmCbs2,
                    aliquotaEfetivaRegular: 0.9m,
                    valorTributoRegular: 4.5m,
                    numeroProcessoSuframa: "123456789")));

            Assert.Equal(tpALCZFMCBS.tpAlcZfmCbs2, ibsCbs.gIBSCBS.gCBS.gALCZFMCBS.tpALCZFMCBS);
            Assert.Equal("123456789", ibsCbs.gIBSCBS.gCBS.gALCZFMCBS.nProcSuframa);
            Assert.Equal(0.9m, ibsCbs.gIBSCBS.gCBS.gALCZFMCBS.pAliqEfetRegCBS);
            Assert.Equal(4.5m, ibsCbs.gIBSCBS.gCBS.gALCZFMCBS.vTribRegCBS);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void GrupoDeAreaIncentivadaSemProcessoSuframa_OmiteATag()
        {
            var ibsCbs = DadosNFe.GetProdutoCBSIBS(ObterImpostoComAreaIncentivada(
                new CBSGrupoAreaIncentivada(tipoAplicacao: tpALCZFMCBS.tpAlcZfmCbs1,
                    aliquotaEfetivaRegular: 0.9m,
                    valorTributoRegular: 4.5m)));

            // nProcSuframa tem minLength 8: string vazia reprova no schema, por isso a tag
            // sai do XML quando nao ha processo.
            Assert.False(ibsCbs.gIBSCBS.gCBS.gALCZFMCBS.ShouldSerializenProcSuframa());
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        /// <summary>
        /// UB66c-10: o tipo 2 exige o número do processo na Suframa (rejeição 1192).
        /// </summary>
        [Fact]
        public void AreaIncentivadaTipo2SemProcessoSuframa_ERecusada()
        {
            var erro = Assert.Throws<ArgumentException>(() => new CBSGrupoAreaIncentivada(
                tipoAplicacao: tpALCZFMCBS.tpAlcZfmCbs2,
                aliquotaEfetivaRegular: 0.9m,
                valorTributoRegular: 4.5m));

            Assert.Contains("nProcSuframa", erro.Message);
        }

        [Fact]
        public void AreaIncentivadaComTipoForaDoDominio_ERecusada()
        {
            var erro = Assert.Throws<ArgumentException>(() => new CBSGrupoAreaIncentivada(
                tipoAplicacao: (tpALCZFMCBS)3,
                aliquotaEfetivaRegular: 0.9m,
                valorTributoRegular: 4.5m,
                numeroProcessoSuframa: "123456789"));

            Assert.Contains("1 ou 2", erro.Message);
        }

        [Fact]
        public void AreaIncentivadaComProcessoSuframaCurto_ERecusada()
        {
            var erro = Assert.Throws<ArgumentException>(() => new CBSGrupoAreaIncentivada(
                tipoAplicacao: tpALCZFMCBS.tpAlcZfmCbs1,
                aliquotaEfetivaRegular: 0.9m,
                valorTributoRegular: 4.5m,
                numeroProcessoSuframa: "1234567"));

            Assert.Contains("8 a 12", erro.Message);
        }

        /// <summary>
        /// Ida e volta: o grupo lido de um XML volta igual na reescrita.
        /// </summary>
        [Fact]
        public void AreaIncentivada_IdaEVolta()
        {
            var ibsCbs = DadosNFe.GetProdutoCBSIBS(ObterImpostoComAreaIncentivada(
                new CBSGrupoAreaIncentivada(tipoAplicacao: tpALCZFMCBS.tpAlcZfmCbs2,
                    aliquotaEfetivaRegular: 0.9m,
                    valorTributoRegular: 4.5m,
                    numeroProcessoSuframa: "123456789")));

            var lido = new CBSIBSGrupo(ibsCbs).CBSGrupo.GrupoAreaIncentivada;

            Assert.Equal(tpALCZFMCBS.tpAlcZfmCbs2, lido.TipoAplicacao);
            Assert.Equal("123456789", lido.NumeroProcessoSuframa);
            Assert.Equal(0.9m, lido.AliquotaEfetivaRegular);
            Assert.Equal(4.5m, lido.ValorTributoRegular);
        }

        private static CBSIBSImposto ObterImpostoComAreaIncentivada(CBSGrupoAreaIncentivada areaIncentivada)
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: new CBSGrupo(aliquota: 0m, valor: 0m, grupoAreaIncentivada: areaIncentivada),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            return new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);
        }

        [Fact]
        public void OperacaoComSubgrupoDaCbs_VaiNoXml()
        {
            var imposto = ObterImposto(ObterCreditoPresumido(TipocCredPres.AquisicaoProdutorRuralNaoContribuinte,
                baseCalculo: 500m,
                cbsPercentual: 2m,
                zfmCompetencia: null));

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.NotNull(ibsCbs.gCredPresOper.gCBSCredPres);
            Assert.Equal(10m, ibsCbs.gCredPresOper.gCBSCredPres.vCredPres);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void SomenteZfm_NaoGeraGrupoDaOperacao()
        {
            var imposto = ObterImposto(ObterCreditoPresumido(codigo: null,
                baseCalculo: 0m,
                cbsPercentual: null,
                zfmCompetencia: "2026-01"));

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gCredPresOper);
            Assert.NotNull(ibsCbs.gCredPresIBSZFM);
            Assert.Equal(55m, ibsCbs.gCredPresIBSZFM.vCredPresIBSZFM);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void CreditoComValorEmCondicaoSuspensiva_NaoEmiteOsDoisValores()
        {
            // TCredPres tem xs:choice entre vCredPres e vCredPresCondSus.
            var credito = new CBSIBSGrupoCreditoPresumido(codigoClassificacaoCreditoPresumido: TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculoCreditoPresumido: 500m,
                cbsPercentualCreditoPresumido: 2m,
                cbsValorCreditoPresumido: null,
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: 10m,
                ibsPercentualCreditoPresumido: null,
                ibsValorCreditoPresumido: null,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                zfmDataCompetenciaApuracao: null,
                zfmTipoCreditoPresumido: tpCredPresIBSZFM.tpCredPresIbsZfm0,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: 0m);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(ObterImposto(credito));

            Assert.Equal(10m, ibsCbs.gCredPresOper.gCBSCredPres.vCredPresCondSus);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void CreditoComValorEValorSuspensivo_ERecusado()
        {
            var credito = new CBSIBSGrupoCreditoPresumido(codigoClassificacaoCreditoPresumido: TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculoCreditoPresumido: 500m,
                cbsPercentualCreditoPresumido: 2m,
                cbsValorCreditoPresumido: 10m,
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: 5m,
                ibsPercentualCreditoPresumido: null,
                ibsValorCreditoPresumido: null,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: null,
                zfmDataCompetenciaApuracao: null,
                zfmTipoCreditoPresumido: tpCredPresIBSZFM.tpCredPresIbsZfm0,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: 0m);

            var erro = Assert.Throws<ArgumentException>(() => DadosNFe.GetProdutoCBSIBS(ObterImposto(credito)));

            Assert.Contains("somente um dos dois", erro.Message);
        }

        /// <summary>
        /// vCredPres/vCredPresCondSus só existem dentro de gCBSCredPres/gIBSCredPres, e o
        /// subgrupo só é montado com pCredPres > 0. Valor sem percentual desaparecia do item
        /// em silêncio, enquanto os totalizadores da nota continuavam somando o mesmo valor.
        /// </summary>
        [Theory]
        [InlineData(10d, 0d, 0d, 0d, "CBS")]
        [InlineData(0d, 10d, 0d, 0d, "CBS")]
        [InlineData(0d, 0d, 10d, 0d, "IBS")]
        [InlineData(0d, 0d, 0d, 10d, "IBS")]
        public void CreditoComValorESemPercentual_ERecusado(double cbsValor,
            double cbsValorSuspensivo,
            double ibsValor,
            double ibsValorSuspensivo,
            string tributo)
        {
            var erro = Assert.Throws<ArgumentException>(() => new CBSIBSGrupoCreditoPresumido(
                codigoClassificacaoCreditoPresumido: TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculoCreditoPresumido: 500m,
                cbsPercentualCreditoPresumido: null,
                cbsValorCreditoPresumido: cbsValor == 0d ? (decimal?)null : (decimal)cbsValor,
                cbsValorCreditoPresumidoEmCondicaoSuspensiva: cbsValorSuspensivo == 0d ? (decimal?)null : (decimal)cbsValorSuspensivo,
                ibsPercentualCreditoPresumido: null,
                ibsValorCreditoPresumido: ibsValor == 0d ? (decimal?)null : (decimal)ibsValor,
                ibsValorCreditoPresumidoEmCondicaoSuspensiva: ibsValorSuspensivo == 0d ? (decimal?)null : (decimal)ibsValorSuspensivo,
                zfmDataCompetenciaApuracao: null,
                zfmTipoCreditoPresumido: tpCredPresIBSZFM.tpCredPresIbsZfm0,
                zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado: 0m));

            Assert.Contains($"Crédito presumido de {tributo} com valor informado e sem percentual", erro.Message);
        }

        [Fact]
        public void OperacaoEZfmJuntos_SaoRecusados()
        {
            var erro = Assert.Throws<ArgumentException>(() => ObterCreditoPresumido(TipocCredPres.AquisicaoResiduosReciclagem,
                baseCalculo: 1000m,
                cbsPercentual: 2m,
                zfmCompetencia: "2026-01"));

            Assert.Contains("somente um dos dois grupos", erro.Message);
        }

        [Fact]
        public void SomenteTransferenciaDeCredito_GeraSomenteGTransfCred()
        {
            // gIBSCBS e gTransfCred sao opcoes do mesmo xs:choice: item de transferencia
            // chega com o grupo normal vazio e so o gTransfCred pode ir ao XML.
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: null,
                cbsGrupo: null,
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: new CBSIBSGrupoTransferenciaCredito(valorASerTransferidoCBS: 10m,
                    valorASerTransferidoIBS: 5m),
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst800,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gIBSCBS);
            Assert.NotNull(ibsCbs.gTransfCred);
            Assert.Equal(10m, ibsCbs.gTransfCred.vCBS);
            Assert.Equal(5m, ibsCbs.gTransfCred.vIBS);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void SomenteMonofasico_GeraSomenteGIbsCbsMono()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: null,
                cbsGrupo: null,
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: new Monofasico.CBSIBSGrupoMonofasico(
                    grupoMonofasicoTributado: new Monofasico.CBSIBSGrupoMonofasicoTributado(quantidadeTributada: 10m,
                        aliquotaAdRemCBS: 1m,
                        aliquotaAdRemIBS: 2m,
                        valorMonofasicoCBS: 10m,
                        valorMonofasicoIBS: 20m),
                    grupoMonofasicoRetido: null,
                    grupoMonofasicoSujeitoRetencao: null,
                    grupoMonofasicoDiferido: null,
                    valorTotalMonofasicoCBS: 10m,
                    valorTotalMonofasicoIBS: 20m),
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst620,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gIBSCBS);
            Assert.NotNull(ibsCbs.gIBSCBSMono);

            // TMonofasia exige os invólucros: nada de gMonoPadrao direto sob gIBSCBSMono.
            Assert.Null(ibsCbs.gIBSCBSMono.gMonoPadrao);
            Assert.Null(ibsCbs.gIBSCBSMono.gMonoReten);
            Assert.Null(ibsCbs.gIBSCBSMono.gMonoRet);

            Assert.Equal(2m, ibsCbs.gIBSCBSMono.gIBSMonoAdRem.gMonoPadrao.adRemIBS);
            Assert.Equal(10m, ibsCbs.gIBSCBSMono.gIBSMonoAdRem.gMonoPadrao.qBCMono);
            Assert.Equal(20m, ibsCbs.gIBSCBSMono.gIBSMonoAdRem.gMonoPadrao.vIBSMono);

            Assert.Equal(1m, ibsCbs.gIBSCBSMono.gCBSMonoAdRem.gMonoPadrao.adRemCBS);
            Assert.Equal(10m, ibsCbs.gIBSCBSMono.gCBSMonoAdRem.gMonoPadrao.qBCMono);
            Assert.Equal(10m, ibsCbs.gIBSCBSMono.gCBSMonoAdRem.gMonoPadrao.vCBSMono);

            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Theory]
        [InlineData("01/2026")]
        [InlineData("2026-13")]
        [InlineData("2024-12")]
        public void CompetenciaDeApuracaoZfmFuraDoLeiaute_ERecusada(string competencia)
        {
            // TCompetApur é xs:gYearMonth com mínimo 2025-01: AAAA-MM, não MM/AAAA.
            var erro = Assert.Throws<ArgumentException>(() => ObterCreditoPresumido(codigo: null,
                baseCalculo: 0m,
                cbsPercentual: null,
                zfmCompetencia: competencia));

            Assert.Contains("AAAA-MM", erro.Message);
        }

        [Fact]
        public void TributacaoNormalIncompleta_ERecusada()
        {
            // TCIBS_NFe exige gIBSUF, gIBSMun e gCBS juntos. Item com IBS e sem CBS
            // (base zerada e so aliquota de IBS informada) gerava gIBSCBS sem gCBS.
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: null,
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var erro = Assert.Throws<ArgumentException>(() => DadosNFe.GetProdutoCBSIBS(imposto));

            Assert.Contains("incompleta", erro.Message);
        }

        [Fact]
        public void MonofasicoComDiferimento_NaoVaiMaisParaOXml()
        {
            // O grupo de diferimento da monofasia saiu do leiaute. A entidade continua
            // lendo o grupo de documento já autorizado, separando valor de percentual
            // (vCBSMonoDif já recebeu o percentual no lugar do valor), mas a emissão não
            // pode mais mandar nada disso.
            var lido = new Monofasico.CBSIBSGrupoMonofasico(new gIBSCBSMono()
            {
                gMonoDif = new gMonoDif()
                {
                    pDifCBS = 50m,
                    pDifIBS = 40m,
                    vCBSMonoDif = 5m,
                    vIBSMonoDif = 4m
                },
                vTotCBSMonoItem = 5m,
                vTotIBSMonoItem = 4m
            });

            Assert.Equal(50m, lido.GrupoMonofasicoDiferido.PercentualMonofasicoCBSDiferimento);
            Assert.Equal(40m, lido.GrupoMonofasicoDiferido.PercentualMonofasicoIBSDiferimento);
            Assert.Equal(5m, lido.GrupoMonofasicoDiferido.ValorMonofasicoCBSDiferimento);
            Assert.Equal(4m, lido.GrupoMonofasicoDiferido.ValorMonofasicoIBSDiferimento);

            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: null,
                cbsGrupo: null,
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: lido,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst620,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gIBSCBSMono.gMonoDif);
            Assert.Null(ibsCbs.gIBSCBSMono.gIBSMonoAdRem);
            Assert.Null(ibsCbs.gIBSCBSMono.gCBSMonoAdRem);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        [Fact]
        public void GrupoNormalComTransferenciaDeCredito_ERecusado()
        {
            // Aqui os dois grupos tem dados proprios, e isso e conflito de verdade.
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: new CBSGrupo(aliquota: 8.8m, valor: 88m),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: new CBSIBSGrupoTransferenciaCredito(valorASerTransferidoCBS: 10m,
                    valorASerTransferidoIBS: 5m),
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst800,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var erro = Assert.Throws<ArgumentException>(() => DadosNFe.GetProdutoCBSIBS(imposto));

            Assert.Contains("somente um desses grupos", erro.Message);
        }

        [Fact]
        public void CreditoComPercentualSemCodigo_ERecusado()
        {
            var erro = Assert.Throws<ArgumentException>(() => ObterCreditoPresumido(codigo: null,
                baseCalculo: 500m,
                cbsPercentual: 2m,
                zfmCompetencia: null));

            Assert.Contains("sem código de classificação", erro.Message);
        }

        /// <summary>
        /// Compra governamental: o pRedutor do Art. 473 reduz a alíquota efetiva sem passar
        /// pelo pRedAliq, então o gRed tem que sair com pRedAliq zero. A UB26-20 (rejeição
        /// 1033), a UB45-20 (1074) e a UB64-20 (1079) exigem o grupo nos três sempre que
        /// gCompraGov é informado. Sem ele o pAliqEfet não sai e o valor passa a ser
        /// conferido contra a alíquota nominal.
        /// <para>O gIBSMun é o caso que derruba qualquer heurística baseada na alíquota
        /// efetiva: em 2026 o pIBSMun é zero, logo a efetiva também é.</para>
        /// </summary>
        [Fact]
        public void CompraGovernamentalSemReducaoDoCst_GeraGRedComPRedAliqZero()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 221.25m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m,
                        valor: 0.06m,
                        percentualReducaoAliquota: 0m,
                        aliquotaEfetiva: 0.07m,
                        exigeGrupoReducao: true),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0m,
                        valor: 0m,
                        percentualReducaoAliquota: 0m,
                        aliquotaEfetiva: 0m,
                        exigeGrupoReducao: true),
                    valorTotal: 0.06m),
                cbsGrupo: new CBSGrupo(aliquota: 0.9m,
                    valor: 0.56m,
                    percentualReducaoAliquota: 0m,
                    aliquotaEfetiva: 0.63m,
                    exigeGrupoReducao: true),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: new CBSIBSGrupoTributacaoCompraGoverno(aliquotaPercentualCBS: 0.9m,
                    aliquotaPercentualIBSUF: 0.1m,
                    aliquotaPercentualIBSMunicipio: 0m,
                    valorTributoCBS: 0.8m,
                    valorTributoIBSUF: 0.09m,
                    valorTributoIBSMunicipio: 0m),
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.NotNull(ibsCbs.gIBSCBS.gIBSUF.gRed);
            Assert.NotNull(ibsCbs.gIBSCBS.gIBSMun.gRed);
            Assert.NotNull(ibsCbs.gIBSCBS.gCBS.gRed);
            Assert.Equal(0m, ibsCbs.gIBSCBS.gCBS.gRed.pRedAliq);
            Assert.Equal(0.63m, ibsCbs.gIBSCBS.gCBS.gRed.pAliqEfet);
            Assert.Equal(0.07m, ibsCbs.gIBSCBS.gIBSUF.gRed.pAliqEfet);
            Assert.Equal(0m, ibsCbs.gIBSCBS.gIBSMun.gRed.pAliqEfet);
            Assert.NotNull(ibsCbs.gIBSCBS.gTribCompraGov);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        /// <summary>
        /// Ida e volta do monofásico: o que a escrita produz tem de voltar pela leitura.
        /// Até 11/11/2026 a escrita já usava os invólucros gIBSMonoAdRem e gCBSMonoAdRem da
        /// NT 2025.002 v1.50, e a leitura só enxergava o leiaute anterior — o XML emitido
        /// por esta mesma versão voltava só com os totais, e a reemissão saía com o
        /// gIBSCBSMono sem tributação nenhuma.
        /// <para>Os testes de escrita e de leitura existiam separados, cada um com o seu
        /// leiaute, e por isso não acusavam.</para>
        /// </summary>
        [Fact]
        public void Monofasico_IdaEVoltaPeloLeiauteNovo()
        {
            var original = new Monofasico.CBSIBSGrupoMonofasico(
                grupoMonofasicoTributado: new Monofasico.CBSIBSGrupoMonofasicoTributado(quantidadeTributada: 100m,
                    aliquotaAdRemCBS: 0.3m,
                    aliquotaAdRemIBS: 0.2m,
                    valorMonofasicoCBS: 30m,
                    valorMonofasicoIBS: 20m),
                grupoMonofasicoRetido: new Monofasico.CBSIBSGrupoMonofasicoRetido(quantidadeRetida: 0m,
                    aliquotaAdRemCBSRetido: 0m,
                    aliquotaAdRemIBSRetido: 0m,
                    valorMonofasicoCBSRetido: 9m,
                    valorMonofasicoIBSRetido: 6m),
                grupoMonofasicoSujeitoRetencao: new Monofasico.CBSIBSGrupoMonofasicoSujeitoRetencao(quantidadeRetencao: 50m,
                    aliquotaAdRemCBSRetencao: 0.15m,
                    aliquotaAdRemIBSRetencao: 0.1m,
                    valorMonofasicoCBSRetencao: 7.5m,
                    valorMonofasicoIBSRetencao: 5m),
                grupoMonofasicoDiferido: null,
                valorTotalMonofasicoCBS: 46.5m,
                valorTotalMonofasicoIBS: 31m);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst620,
                classificacaoTributaria: 620001,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(baseCalculo: 1000m,
                    ibsGrupo: null,
                    cbsGrupo: null,
                    grupoTributacaoRegular: null,
                    grupoTributacaoCompraGoverno: null,
                    grupoTributacaoMonofasico: original,
                    grupoTransferenciaCredito: null,
                    grupoCreditoPresumido: null));

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.NotNull(ibsCbs.gIBSCBSMono.gIBSMonoAdRem);
            Assert.NotNull(ibsCbs.gIBSCBSMono.gCBSMonoAdRem);
            Assert.Empty(ValidarContraPl010f(ibsCbs));

            var lido = new CBSIBSGrupo(ibsCbs).GrupoTributacaoMonofasico;

            Assert.Equal(100m, lido.GrupoMonofasicoTributado.Quantidade);
            Assert.Equal(0.3m, lido.GrupoMonofasicoTributado.AliquotaAdRemCBS);
            Assert.Equal(0.2m, lido.GrupoMonofasicoTributado.AliquotaAdRemIBS);
            Assert.Equal(30m, lido.GrupoMonofasicoTributado.ValorMonofasicoCBS);
            Assert.Equal(20m, lido.GrupoMonofasicoTributado.ValorMonofasicoIBS);

            Assert.Equal(50m, lido.GrupoMonofasicoSujeitoRetencao.Quantidade);
            Assert.Equal(0.15m, lido.GrupoMonofasicoSujeitoRetencao.AliquotaAdRemCBS);
            Assert.Equal(7.5m, lido.GrupoMonofasicoSujeitoRetencao.ValorMonofasicoCBS);

            // gMonoRet perdeu quantidade e alíquota no leiaute novo: só o valor volta.
            Assert.Equal(9m, lido.GrupoMonofasicoRetido.ValorMonofasicoCBS);
            Assert.Equal(6m, lido.GrupoMonofasicoRetido.ValorMonofasicoIBS);

            Assert.Equal(46.5m, lido.ValorTotalMonofasicoCBS);
            Assert.Equal(31m, lido.ValorTotalMonofasicoIBS);
        }

        /// <summary>
        /// XML que traz só o IBS tem de voltar só com o IBS. A entidade guarda os dois
        /// tributos no mesmo objeto, e com a guarda de escrita apenas na quantidade — que é
        /// compartilhada — a reescrita fabricava um gCBSMonoAdRem zerado que não estava no
        /// documento original.
        /// </summary>
        [Fact]
        public void Monofasico_SomenteIbs_NaoFabricaCbs()
        {
            var ibsCbs = new IBSCBS
            {
                CST = CSTIBSCBS.cst620,
                cClassTrib = "620001",
                gIBSCBSMono = new gIBSCBSMono
                {
                    gIBSMonoAdRem = new gIBSMonoAdRem
                    {
                        gMonoPadrao = new gMonoPadraoIBSAdRem { qBCMono = 100m, adRemIBS = 0.2m, vIBSMono = 20m }
                    },
                    vTotIBSMonoItem = 20m
                }
            };

            var reescrito = DadosNFe.GetProdutoCBSIBS(new CBSIBSImposto(cst: CSTIBSCBS.cst620,
                classificacaoTributaria: 620001,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(ibsCbs)));

            Assert.NotNull(reescrito.gIBSCBSMono.gIBSMonoAdRem);
            Assert.Null(reescrito.gIBSCBSMono.gCBSMonoAdRem);
            Assert.Equal(100m, reescrito.gIBSCBSMono.gIBSMonoAdRem.gMonoPadrao.qBCMono);
            Assert.Equal(20m, reescrito.gIBSCBSMono.gIBSMonoAdRem.gMonoPadrao.vIBSMono);
            Assert.Empty(ValidarContraPl010f(reescrito));
        }

        [Fact]
        public void Monofasico_SomenteCbs_NaoFabricaIbs()
        {
            var ibsCbs = new IBSCBS
            {
                CST = CSTIBSCBS.cst620,
                cClassTrib = "620001",
                gIBSCBSMono = new gIBSCBSMono
                {
                    gCBSMonoAdRem = new gCBSMonoAdRem
                    {
                        gMonoPadrao = new gMonoPadraoCBSAdRem { qBCMono = 100m, adRemCBS = 0.3m, vCBSMono = 30m }
                    },
                    vTotCBSMonoItem = 30m
                }
            };

            var reescrito = DadosNFe.GetProdutoCBSIBS(new CBSIBSImposto(cst: CSTIBSCBS.cst620,
                classificacaoTributaria: 620001,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(ibsCbs)));

            Assert.NotNull(reescrito.gIBSCBSMono.gCBSMonoAdRem);
            Assert.Null(reescrito.gIBSCBSMono.gIBSMonoAdRem);
            Assert.Equal(30m, reescrito.gIBSCBSMono.gCBSMonoAdRem.gMonoPadrao.vCBSMono);
            Assert.Empty(ValidarContraPl010f(reescrito));
        }

        /// <summary>
        /// Retenção também é independente por tributo.
        /// </summary>
        [Fact]
        public void MonofasicoComRetencaoSomenteCbs_NaoFabricaRetencaoDeIbs()
        {
            var ibsCbs = new IBSCBS
            {
                CST = CSTIBSCBS.cst620,
                cClassTrib = "620001",
                gIBSCBSMono = new gIBSCBSMono
                {
                    gCBSMonoAdRem = new gCBSMonoAdRem
                    {
                        gMonoReten = new gMonoRetenCBSAdRem { qBCMonoReten = 50m, adRemCBSReten = 0.15m, vCBSMonoReten = 7.5m }
                    },
                    vTotCBSMonoItem = 7.5m
                }
            };

            var reescrito = DadosNFe.GetProdutoCBSIBS(new CBSIBSImposto(cst: CSTIBSCBS.cst620,
                classificacaoTributaria: 620001,
                isDoacao: false,
                cbsIbs: new CBSIBSGrupo(ibsCbs)));

            Assert.Null(reescrito.gIBSCBSMono.gIBSMonoAdRem);
            Assert.Equal(7.5m, reescrito.gIBSCBSMono.gCBSMonoAdRem.gMonoReten.vCBSMonoReten);
            Assert.Empty(ValidarContraPl010f(reescrito));
        }

        /// <summary>
        /// Documento autorizado antes da NT 2025.002 v1.50 traz os subgrupos direto no
        /// gIBSCBSMono. A leitura continua entendendo, senão reemitir documento antigo
        /// perderia a monofasia.
        /// </summary>
        [Fact]
        public void Monofasico_LeiauteAnterior_ContinuaSendoLido()
        {
            var ibsCbs = new IBSCBS
            {
                CST = CSTIBSCBS.cst620,
                cClassTrib = "620001",
                gIBSCBSMono = new gIBSCBSMono
                {
                    gMonoPadrao = new gMonoPadrao { qBCMono = 100m, adRemCBS = 0.3m, adRemIBS = 0.2m, vCBSMono = 30m, vIBSMono = 20m },
                    vTotCBSMonoItem = 30m,
                    vTotIBSMonoItem = 20m
                }
            };

            var lido = new CBSIBSGrupo(ibsCbs).GrupoTributacaoMonofasico;

            Assert.Equal(100m, lido.GrupoMonofasicoTributado.Quantidade);
            Assert.Equal(30m, lido.GrupoMonofasicoTributado.ValorMonofasicoCBS);
            Assert.Equal(20m, lido.GrupoMonofasicoTributado.ValorMonofasicoIBS);
        }

        /// <summary>
        /// Geometria exata que a SEFAZ aceitou em compra governamental, fixada por cinco
        /// transmissões de homologação em 11/11/2026 (NF-e 10, base 221,25, pRedutor 30%,
        /// cClassTrib 200030 com redução de 60%):
        /// <list type="bullet">
        /// <item>gIBSUF leva o pRedAliq da tabela — zero tomou rejeição 1034;</item>
        /// <item>gIBSMun e gCBS levam zero — 60 tomou 1046 e 1063;</item>
        /// <item>gTribCompraGov repete os valores do item — divergir tomou 1142.</item>
        /// </list>
        /// Quem mexer nisto sem reler PwFiscal/ReformaTributaria/CLAUDE.md desfaz o que a
        /// SEFAZ já recusou.
        /// </summary>
        [Fact]
        public void CompraGovernamentalComReducaoDoCst_FormaAceitaPelaSefaz()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 221.25m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m,
                        valor: 0.06m,
                        percentualReducaoAliquota: 60m,
                        aliquotaEfetiva: 0.028m,
                        exigeGrupoReducao: true),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0m,
                        valor: 0m,
                        percentualReducaoAliquota: 0m,
                        aliquotaEfetiva: 0m,
                        exigeGrupoReducao: true),
                    valorTotal: 0.06m),
                cbsGrupo: new CBSGrupo(aliquota: 0.9m,
                    valor: 1.39m,
                    percentualReducaoAliquota: 0m,
                    aliquotaEfetiva: 0.63m,
                    exigeGrupoReducao: true),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: new CBSIBSGrupoTributacaoCompraGoverno(aliquotaPercentualCBS: 0.63m,
                    aliquotaPercentualIBSUF: 0.028m,
                    aliquotaPercentualIBSMunicipio: 0m,
                    valorTributoCBS: 1.39m,
                    valorTributoIBSUF: 0.06m,
                    valorTributoIBSMunicipio: 0m),
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst200,
                classificacaoTributaria: 200030,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Equal(60m, ibsCbs.gIBSCBS.gIBSUF.gRed.pRedAliq);
            Assert.Equal(0m, ibsCbs.gIBSCBS.gIBSMun.gRed.pRedAliq);
            Assert.Equal(0m, ibsCbs.gIBSCBS.gCBS.gRed.pRedAliq);

            // UB82a-20, tolerância 0,04: as duas somas têm que ser iguais.
            var somaItem = ibsCbs.gIBSCBS.gIBSUF.vIBSUF + ibsCbs.gIBSCBS.gIBSMun.vIBSMun + ibsCbs.gIBSCBS.gCBS.vCBS;
            var somaGrupo = ibsCbs.gIBSCBS.gTribCompraGov.vTribIBSUF
                + ibsCbs.gIBSCBS.gTribCompraGov.vTribIBSMun
                + ibsCbs.gIBSCBS.gTribCompraGov.vTribCBS;

            Assert.Equal(somaItem, somaGrupo);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        /// <summary>
        /// Fora de compra governamental, com pRedAliq zero, o gRed não pode sair: a UB26-10
        /// (rejeição 1032) recusa o grupo em CST que não permite redução.
        /// </summary>
        [Fact]
        public void SemCompraGovernamentalEComPRedAliqZero_NaoGeraGRed()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0m, valor: 0m),
                    valorTotal: 1m),
                cbsGrupo: new CBSGrupo(aliquota: 0.9m, valor: 9m),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gIBSCBS.gIBSUF.gRed);
            Assert.Null(ibsCbs.gIBSCBS.gIBSMun.gRed);
            Assert.Null(ibsCbs.gIBSCBS.gCBS.gRed);
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }

        /// <summary>
        /// gDevTrib — devolução do tributo ("cashback" na própria nota, LC 214/25 art. 118).
        /// O <c>pDevTrib</c> é opcional no <c>TDevTrib</c>: a CBS da NF-e o exige e os demais
        /// grupos não o levam, então ele é anulável e só é serializado quando informado.
        /// </summary>
        [Fact]
        public void DevolucaoDeTributoComPercentual_SaiNoXmlEVoltaNaLeitura()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: new CBSGrupo(aliquota: 8.8m,
                    valor: 88m,
                    valorDevolucaoTributos: 8.80m,
                    percentualDevolucaoTributos: 10m),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Equal(10m, ibsCbs.gIBSCBS.gCBS.gDevTrib.pDevTrib);
            Assert.Equal(8.80m, ibsCbs.gIBSCBS.gCBS.gDevTrib.vDevTrib);
            Assert.Empty(ValidarContraPl010f(ibsCbs));

            // Leitura de volta: o braco do percentual nao pode se perder no round-trip.
            var relido = new CBSGrupo(ibsCbs.gIBSCBS);

            Assert.Equal(10m, relido.GrupoDevolucaoTributo.PercentualDevolucao);
            Assert.Equal(8.80m, relido.GrupoDevolucaoTributo.ValorDevolvido);
        }

        /// <summary>
        /// Sem percentual o elemento não sai: <c>ShouldSerializepDevTrib</c> depende do
        /// <c>HasValue</c>. Zero não é ausência — zero seria "devolveu 0%".
        /// </summary>
        [Fact]
        public void DevolucaoDeTributoSemPercentual_NaoSerializaOElemento()
        {
            var grupo = new CBSIBSGrupo(baseCalculo: 1000m,
                ibsGrupo: new IBSGrupo(ibsUFGrupo: new IBSUFGrupo(aliquota: 0.1m, valor: 1m),
                    ibsMunicipioGrupo: new IBSMunicipioGrupo(aliquota: 0.01m, valor: 0.1m),
                    valorTotal: 1.1m),
                cbsGrupo: new CBSGrupo(aliquota: 8.8m, valor: 88m, valorDevolucaoTributos: 8.80m),
                grupoTributacaoRegular: null,
                grupoTributacaoCompraGoverno: null,
                grupoTributacaoMonofasico: null,
                grupoTransferenciaCredito: null,
                grupoCreditoPresumido: null);

            var imposto = new CBSIBSImposto(cst: CSTIBSCBS.cst000,
                classificacaoTributaria: 1,
                isDoacao: false,
                cbsIbs: grupo);

            var ibsCbs = DadosNFe.GetProdutoCBSIBS(imposto);

            Assert.Null(ibsCbs.gIBSCBS.gCBS.gDevTrib.pDevTrib);
            Assert.False(ibsCbs.gIBSCBS.gCBS.gDevTrib.ShouldSerializepDevTrib());
            Assert.Empty(ValidarContraPl010f(ibsCbs));
        }
    }
}
