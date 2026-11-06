using System.Collections.Generic;
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// O construtor que lê um nfeProc concentrava quatro desreferenciações sem guarda:
    /// protNFe, cobr/dup, infIntermed e o totalizador.
    /// </summary>
    public class LeituraDeXmlTestes
    {
        private static nfeProc ObterNfeProcMinimo()
        {
            return new nfeProc
            {
                NFe = new Classes.NFe
                {
                    infNFe = new infNFe
                    {
                        versao = "4.00",
                        ide = new ide
                        {
                            mod = ModeloDocumento.NFe,
                            serie = 1,
                            nNF = 123,
                            dhEmi = new System.DateTimeOffset(2026, 1, 15, 10, 0, 0, System.TimeSpan.FromHours(-3)),
                            tpNF = TipoNFe.tnSaida,
                            finNFe = FinalidadeNFe.fnNormal,
                            natOp = "VENDA"
                        },
                        emit = new emit
                        {
                            CNPJ = "12345678000195",
                            xNome = "EMITENTE TESTE",
                            IE = "123456789",
                            CRT = CRT.RegimeNormal,
                            enderEmit = new enderEmit
                            {
                                xLgr = "RUA UM",
                                nro = "1",
                                xBairro = "CENTRO",
                                cMun = 3509502,
                                xMun = "CAMPINAS",
                                UF = Estado.SP,
                                CEP = "13000000",
                                cPais = 1058,
                                xPais = "BRASIL"
                            }
                        },
                        det = new List<det>()
                    }
                }
            };
        }

        [Fact]
        public void NotaSemProtocolo_NaoEstoura_ESaiSemProtocolo()
        {
            var nfeProcessada = ObterNfeProcMinimo();
            Assert.Null(nfeProcessada.protNFe);

            var notaFiscal = new NotaFiscal(nfeProcessada);

            Assert.Null(notaFiscal.Protocolo);
            Assert.Equal(8, notaFiscal.CNf.Length);
        }

        [Fact]
        public void NotaComCobrancaSemDuplicatas_NaoEstoura()
        {
            var nfeProcessada = ObterNfeProcMinimo();
            nfeProcessada.NFe.infNFe.cobr = new Classes.Informacoes.Cobranca.cobr();
            Assert.Null(nfeProcessada.NFe.infNFe.cobr.dup);

            var notaFiscal = new NotaFiscal(nfeProcessada);

            Assert.Empty(notaFiscal.Duplicatas);
        }

        [Fact]
        public void NotaSemIntermediador_NaoCriaIntermediadorVazio()
        {
            var nfeProcessada = ObterNfeProcMinimo();
            // retirada preenchida e infIntermed ausente: a condição testava a tag errada
            nfeProcessada.NFe.infNFe.retirada = new retirada
            {
                xLgr = "RUA DOIS",
                nro = "2",
                xBairro = "CENTRO",
                cMun = 3509502,
                xMun = "CAMPINAS",
                UF = "SP",
                CEP = 13000000,
                cPais = 1058,
                xPais = "BRASIL"
            };

            var notaFiscal = new NotaFiscal(nfeProcessada);

            Assert.Null(notaFiscal.DadosIntermediador);
            Assert.NotNull(notaFiscal.EnderecoRetirada);
        }

        [Fact]
        public void DuasNotasSeguidas_RecebemCNfDiferente()
        {
            var primeira = new NotaFiscal(ObterNfeProcMinimo());
            var segunda = new NotaFiscal(ObterNfeProcMinimo());

            Assert.NotEqual(primeira.CNf, segunda.CNf);
        }

        [Fact]
        public void EnderecoComUfInvalida_NaoEstoura()
        {
            var enderecoDestinatario = new enderDest
            {
                xLgr = "RUA TRES",
                nro = "3",
                xBairro = "CENTRO",
                cMun = 3509502,
                xMun = "CAMPINAS",
                UF = "ZZ",
                CEP = "13000000",
                cPais = 1058,
                xPais = "BRASIL"
            };

            var endereco = new Endereco(enderecoDestinatario);

            Assert.Null(endereco.MunicipioEstadoSigla);
            Assert.Equal("CAMPINAS", endereco.MunicipioNome);
        }
    }
}
