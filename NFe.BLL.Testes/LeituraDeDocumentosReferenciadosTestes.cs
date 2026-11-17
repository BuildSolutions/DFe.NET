using System.Collections.Generic;
using System.Linq;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Identificacao;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// Ida e volta do grupo BA: o que GetNFRef emite tem de voltar igual pela leitura do
    /// XML. O construtor de leitura não preenchia DocumentosReferenciados, então a
    /// referência entrava por um caminho e não voltava pelo outro.
    /// </summary>
    public class LeituraDeDocumentosReferenciadosTestes
    {
        private const string ChaveNFe = "35260146410064000242650010000003511876888666";
        private const string ChaveCTe = "35260146410064000242570010000003511876888660";

        /// <summary>Modelo 65 com o cNF zerado, que é o que refNFeSig exige.</summary>
        private const string ChaveSigilosa = "35260146410064000242650010000003511000000007";

        private static List<DocumentoReferenciado> IdaEVolta(List<DocumentoReferenciado> original)
        {
            var nFref = DadosNFe.GetNFRef(original);

            // Mesmo caminho que NotaFiscal(nfeProc) usa para reconstruir as entidades.
            var lido = new NotaFiscal(new NFe.Classes.nfeProc
            {
                NFe = new NFe.Classes.NFe
                {
                    infNFe = new NFe.Classes.Informacoes.infNFe
                    {
                        ide = new ide { NFref = nFref },
                        emit = new NFe.Classes.Informacoes.Emitente.emit(),
                        total = new NFe.Classes.Informacoes.Total.total(),
                        det = new List<NFe.Classes.Informacoes.Detalhe.det>()
                    }
                }
            });

            return lido.DocumentosReferenciados?.ToList() ?? new List<DocumentoReferenciado>();
        }

        [Fact]
        public void ChaveDeNFeVoltaComOMesmoTipo()
        {
            var lido = IdaEVolta(new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFe(ChaveNFe)
            });

            var documento = Assert.Single(lido);
            Assert.Equal(EDocumentoReferenciado.NFe, documento.TipoDocumento);
            Assert.Equal(ChaveNFe, ((DocumentoReferenciadoNFe)documento).ChaveAcesso);
        }

        [Fact]
        public void TodasAsTresFormasPorChaveVoltamDistintas()
        {
            var lido = IdaEVolta(new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFe(ChaveNFe),
                new DocumentoReferenciadoNFe(ChaveSigilosa, EDocumentoReferenciado.NFeSigilosa),
                new DocumentoReferenciadoNFe(ChaveCTe, EDocumentoReferenciado.CTe)
            });

            Assert.Equal(3, lido.Count);
            Assert.Equal(EDocumentoReferenciado.NFe, lido[0].TipoDocumento);
            Assert.Equal(EDocumentoReferenciado.NFeSigilosa, lido[1].TipoDocumento);
            Assert.Equal(EDocumentoReferenciado.CTe, lido[2].TipoDocumento);
        }

        [Fact]
        public void SemNFrefNaoVoltaNada()
        {
            Assert.Empty(IdaEVolta(new List<DocumentoReferenciado>()));
        }

        /// <summary>
        /// XML de terceiro com referência torta não pode derrubar a leitura:
        /// NFeEntradaOperacoes usa este construtor na importação de NF-e de entrada, na
        /// devolução, na transferência de estoque e na complementar por XML. A referência
        /// ruim é descartada e o resto da nota continua sendo lido.
        /// </summary>
        [Fact]
        public void XmlComChaveReferenciadaInvalida_DescartaAReferenciaESegueLendo()
        {
            // Primeira com DV errado, segunda valida.
            var lido = new NotaFiscal(new NFe.Classes.nfeProc
            {
                NFe = new NFe.Classes.NFe
                {
                    infNFe = new NFe.Classes.Informacoes.infNFe
                    {
                        ide = new ide
                        {
                            NFref = new List<NFref>
                            {
                                new NFref { refNFe = "35260146410064000242650010000093511876888666" },
                                new NFref { refNFe = ChaveNFe }
                            }
                        },
                        emit = new NFe.Classes.Informacoes.Emitente.emit(),
                        total = new NFe.Classes.Informacoes.Total.total(),
                        det = new List<NFe.Classes.Informacoes.Detalhe.det>()
                    }
                }
            });

            var documento = Assert.Single(lido.DocumentosReferenciados);
            Assert.Equal(ChaveNFe, ((DocumentoReferenciadoNFe)documento).ChaveAcesso);
        }

        /// <summary>
        /// refNFP (BA10) é uma das seis formas do xs:choice de NFref. Até 11/11/2026 a
        /// leitura pulava a referência de produtor rural, então um XML autorizado que a
        /// trouxesse voltava sem ela — e reemitir mudava o documento em silêncio.
        /// </summary>
        [Fact]
        public void NotaDeProdutorRuralComCnpj_VoltaIgual()
        {
            var original = new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFP(dataEmissao: new System.DateTime(2026, 11, 1),
                    emitenteUF: DFe.Classes.Entidades.Estado.SP,
                    emitenteInscricaoEstadual: "110042490114",
                    modelo: "04",
                    serie: 1,
                    numeroNotaFiscal: 351,
                    emitenteCNPJ: "46410064000242")
            };

            var documento = Assert.IsType<DocumentoReferenciadoNFP>(Assert.Single(IdaEVolta(original)));

            Assert.Equal(EDocumentoReferenciado.NFProdutorRural, documento.TipoDocumento);
            Assert.Equal("2611", documento.DataEmissao);
            Assert.Equal(DFe.Classes.Entidades.Estado.SP, documento.EmitenteCodigoUF);
            Assert.Equal("46410064000242", documento.EmitenteCNPJ);
            Assert.Null(documento.EmitenteCPF);
            Assert.Equal("110042490114", documento.EmitenteInscricaoEstadual);
            Assert.Equal("04", documento.Modelo);
            Assert.Equal(1, documento.Serie);
            Assert.Equal(351, documento.Numero);
        }

        [Fact]
        public void NotaDeProdutorRuralComCpf_VoltaIgual()
        {
            var original = new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFP(dataEmissao: new System.DateTime(2026, 11, 1),
                    emitenteUF: DFe.Classes.Entidades.Estado.MG,
                    emitenteInscricaoEstadual: "ISENTO",
                    modelo: "01",
                    serie: 0,
                    numeroNotaFiscal: 7,
                    emitenteCPF: "07783008119")
            };

            var documento = Assert.IsType<DocumentoReferenciadoNFP>(Assert.Single(IdaEVolta(original)));

            Assert.Equal("07783008119", documento.EmitenteCPF);
            Assert.Null(documento.EmitenteCNPJ);
        }

        /// <summary>
        /// CNPJ e CPF são um xs:choice: os dois juntos, ou nenhum, é rejeição de schema sem
        /// apontar o item.
        /// </summary>
        [Fact]
        public void NotaDeProdutorRuralComCnpjECpf_ERecusada()
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: "110042490114",
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "46410064000242",
                emitenteCPF: "07783008119"));

            Assert.Contains("somente um dos dois", erro.Message);
        }

        [Theory]
        [InlineData("02")]
        [InlineData("55")]
        [InlineData("")]
        public void NotaDeProdutorRuralComModeloForaDoLeiaute_ERecusada(string modelo)
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: "110042490114",
                modelo: modelo,
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "46410064000242"));

            Assert.Contains("01 ou 04", erro.Message);
        }

        [Fact]
        public void NotaDeProdutorRuralSemInscricaoEstadual_ERecusada()
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: "  ",
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "46410064000242"));

            Assert.Contains("inscrição estadual", erro.Message);
        }

        [Fact]
        public void NotaDeProdutorRuralComCnpjCurto_ERecusada()
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: "110042490114",
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "4641006400024"));

            Assert.Contains("14 dígitos", erro.Message);
        }

        [Theory]
        [InlineData(-1, 351)]
        [InlineData(1000, 351)]
        [InlineData(1, 0)]
        [InlineData(1, 1000000000)]
        public void NotaDeProdutorRuralComSerieOuNumeroForaDaFaixa_ERecusada(int serie, int numero)
        {
            Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: "110042490114",
                modelo: "04",
                serie: serie,
                numeroNotaFiscal: numero,
                emitenteCNPJ: "46410064000242"));
        }

        /// <summary>
        /// O CNPJ alfanumérico da NT 2026.004 vale para a chave de acesso, não para o CNPJ
        /// do refNFP, que o leiaute mantém como TCnpj — só dígitos.
        /// </summary>
        [Fact]
        public void NotaDeProdutorRuralComCpfNaoNumerico_ERecusada()
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: "110042490114",
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCPF: "ABCDEFGHIJK"));

            Assert.Contains("11 dígitos", erro.Message);
        }

        /// <summary>
        /// BA11 é TCodUfIBGE: só as 27 UFs. O enum Estado também carrega ambiente nacional
        /// (AN 91), SVRS (92) e exterior (EX 99), que não são UF.
        /// </summary>
        [Theory]
        [InlineData(DFe.Classes.Entidades.Estado.EX)]
        [InlineData(DFe.Classes.Entidades.Estado.AN)]
        [InlineData(DFe.Classes.Entidades.Estado.SVRS)]
        public void NotaDeProdutorRuralComUfQueNaoEUf_ERecusada(DFe.Classes.Entidades.Estado uf)
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: uf,
                emitenteInscricaoEstadual: "110042490114",
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "46410064000242"));

            Assert.Contains("27 unidades", erro.Message);
        }

        /// <summary>
        /// BA15 é TIeDest: "ISENTO" ou de 2 a 14 dígitos. Presença não basta.
        /// </summary>
        [Theory]
        [InlineData("A")]
        [InlineData("1")]
        [InlineData("123456789012345")]
        [InlineData("11004249011A")]
        public void NotaDeProdutorRuralComInscricaoEstadualForaDoLeiaute_ERecusada(string inscricao)
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                emitenteInscricaoEstadual: inscricao,
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "46410064000242"));

            Assert.Contains("2 a 14", erro.Message);
        }

        [Fact]
        public void NotaDeProdutorRuralComInscricaoIsenta_EAceita()
        {
            var documento = new DocumentoReferenciadoNFP(dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: DFe.Classes.Entidades.Estado.MG,
                emitenteInscricaoEstadual: "ISENTO",
                modelo: "01",
                serie: 0,
                numeroNotaFiscal: 7,
                emitenteCPF: "07783008119");

            Assert.Equal("ISENTO", documento.EmitenteInscricaoEstadual);
        }

        /// <summary>
        /// ide/NFref é maxOccurs=999. A contagem é depois da deduplicação: chave repetida
        /// não ocupa ocorrência.
        /// </summary>
        [Fact]
        public void MaisDeNovecentosENoveReferencias_ERecusado()
        {
            var documentos = new List<DocumentoReferenciado>();

            for (var numero = 1; numero <= 1000; numero++)
            {
                documentos.Add(new DocumentoReferenciadoNF(dataEmissao: new System.DateTime(2026, 11, 1),
                    emitenteCNPJ: "46410064000242",
                    emitenteUF: DFe.Classes.Entidades.Estado.SP,
                    serie: 1,
                    numeroNotaFiscal: numero));
            }

            var erro = Assert.Throws<System.ArgumentException>(() => DadosNFe.GetNFRef(documentos));

            Assert.Contains("máximo 999", erro.Message);
        }

        [Fact]
        public void NovecentasENoveReferencias_SaoAceitas()
        {
            var documentos = new List<DocumentoReferenciado>();

            for (var numero = 1; numero <= 999; numero++)
            {
                documentos.Add(new DocumentoReferenciadoNF(dataEmissao: new System.DateTime(2026, 11, 1),
                    emitenteCNPJ: "46410064000242",
                    emitenteUF: DFe.Classes.Entidades.Estado.SP,
                    serie: 1,
                    numeroNotaFiscal: numero));
            }

            Assert.Equal(999, DadosNFe.GetNFRef(documentos).Count);
        }

        /// <summary>
        /// A régua do refNFP vale também para as outras duas formas compostas: até
        /// 11/11/2026 refNF e refECF aceitavam qualquer coisa e o erro só aparecia na
        /// validação do XML.
        /// </summary>
        [Fact]
        public void NotaModelo1ComCnpjInvalido_ERecusada()
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNF(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteCNPJ: "",
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                serie: 1,
                numeroNotaFiscal: 351));

            Assert.Contains("14 dígitos", erro.Message);
        }

        [Theory]
        [InlineData(-1, 351)]
        [InlineData(1000, 351)]
        [InlineData(1, 0)]
        public void NotaModelo1ComSerieOuNumeroForaDaFaixa_ERecusada(int serie, int numero)
        {
            Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNF(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteCNPJ: "46410064000242",
                emitenteUF: DFe.Classes.Entidades.Estado.SP,
                serie: serie,
                numeroNotaFiscal: numero));
        }

        [Fact]
        public void NotaModelo1ComUfQueNaoEUf_ERecusada()
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNF(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteCNPJ: "46410064000242",
                emitenteUF: DFe.Classes.Entidades.Estado.EX,
                serie: 1,
                numeroNotaFiscal: 351));

            Assert.Contains("27 unidades", erro.Message);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1000, 1)]
        [InlineData(1, -1)]
        [InlineData(1, 1000000)]
        public void CupomFiscalComContadorForaDaFaixa_ERecusado(int numeroECF, int numeroCOO)
        {
            Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoECF(numeroECF: numeroECF, numeroCOO: numeroCOO));
        }

        /// <summary>
        /// A faixa 11 a 53 tem buracos: 18, 30, 34 e 49 não são UF. A conferência é pelo
        /// enum, não por intervalo.
        /// </summary>
        [Theory]
        [InlineData(18)]
        [InlineData(30)]
        [InlineData(34)]
        [InlineData(49)]
        public void NotaDeProdutorRuralComCodigoDeUfInexistente_ERecusada(int codigoUF)
        {
            var erro = Assert.Throws<System.ArgumentException>(() => new DocumentoReferenciadoNFP(
                dataEmissao: new System.DateTime(2026, 11, 1),
                emitenteUF: (DFe.Classes.Entidades.Estado)codigoUF,
                emitenteInscricaoEstadual: "110042490114",
                modelo: "04",
                serie: 1,
                numeroNotaFiscal: 351,
                emitenteCNPJ: "46410064000242"));

            Assert.Contains("27 unidades", erro.Message);
        }

        /// <summary>
        /// AAMM fora do padrão: a referência é descartada em vez de virar 01/01/0001, que
        /// na reescrita sairia como "0101" — dado errado no lugar de dado ausente. Vale
        /// para refNF e refNFP, que compartilham o mesmo leitor.
        /// </summary>
        [Fact]
        public void ReferenciaComAnoMesInvalido_EDescartadaNaLeitura()
        {
            var lido = new NotaFiscal(new NFe.Classes.nfeProc
            {
                NFe = new NFe.Classes.NFe
                {
                    infNFe = new NFe.Classes.Informacoes.infNFe
                    {
                        ide = new ide
                        {
                            NFref = new List<NFref>
                            {
                                new NFref { refNF = new refNF { AAMM = "2613", CNPJ = "46410064000242", serie = 1, nNF = 351 } },
                                new NFref { refNFP = new refNFP { AAMM = "xxxx", CNPJ = "46410064000242", IE = "110042490114", mod = "04", serie = 1, nNF = 351 } }
                            }
                        },
                        emit = new NFe.Classes.Informacoes.Emitente.emit(),
                        total = new NFe.Classes.Informacoes.Total.total(),
                        det = new List<NFe.Classes.Informacoes.Detalhe.det>()
                    }
                }
            });

            Assert.Empty(lido.DocumentosReferenciados);
        }

        /// <summary>
        /// Leitura tolera: XML de terceiro sem CNPJ e sem CPF no refNFP é descartado em vez
        /// de derrubar a leitura da nota inteira.
        /// </summary>
        [Fact]
        public void NotaDeProdutorRuralSemEmitenteNoXml_EDescartadaNaLeitura()
        {
            var lido = new NotaFiscal(new NFe.Classes.nfeProc
            {
                NFe = new NFe.Classes.NFe
                {
                    infNFe = new NFe.Classes.Informacoes.infNFe
                    {
                        ide = new ide
                        {
                            NFref = new List<NFref>
                            {
                                new NFref { refNFP = new refNFP { AAMM = "2611", mod = "04", serie = 1, nNF = 351 } }
                            }
                        },
                        emit = new NFe.Classes.Informacoes.Emitente.emit(),
                        total = new NFe.Classes.Informacoes.Total.total(),
                        det = new List<NFe.Classes.Informacoes.Detalhe.det>()
                    }
                }
            });

            Assert.Empty(lido.DocumentosReferenciados);
        }
    }
}
