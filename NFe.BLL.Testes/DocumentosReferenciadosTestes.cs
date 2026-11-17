using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Enums;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// Grupo BA (ide/NFref). É um xs:choice de seis formas; três são uma chave de 44
    /// posições - refNFe (BA02), refNFeSig (BA02a) e refCTe (BA19) - e são as únicas que
    /// DocumentoReferenciadoNFe atende.
    /// </summary>
    public class DocumentosReferenciadosTestes
    {
        private const string NamespaceNfe = "http://www.portalfiscal.inf.br/nfe";

        // TChNFe mora em tiposBasico_v4.00.xsd, que e o que o leiauteNFe_v4.00 inclui.
        private const string SchemaEnvelope =
            "<xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"" + NamespaceNfe + "\" targetNamespace=\"" + NamespaceNfe + "\" elementFormDefault=\"qualified\">" +
            "<xs:include schemaLocation=\"tiposBasico_v4.00.xsd\"/>" +
            "<xs:element name=\"chave\" type=\"TChNFe\"/>" +
            "</xs:schema>";

        private const string ChaveValida = "35260146410064000242650010000003511876888666";

        /// <summary>Modelo 57: refCTe só aceita chave de CT-e.</summary>
        private const string ChaveCTe = "35260146410064000242570010000003511876888660";

        /// <summary>Modelo 65 com o cNF zerado, que é o que refNFeSig exige.</summary>
        private const string ChaveSigilosa = "35260146410064000242650010000003511000000007";

        /// <summary>
        /// Pergunta ao proprio XSD se a chave serve, em vez de confiar na regex da
        /// entidade. Se o leiaute mudar o pattern de TChNFe, o teste acusa.
        /// </summary>
        private static bool OXsdAceita(string chave)
        {
            var schemas = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            using (var reader = XmlReader.Create(new StringReader(SchemaEnvelope), new XmlReaderSettings(),
                       Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "envelope.xsd")))
            {
                schemas.Add(NamespaceNfe, reader);
            }

            var erros = new List<string>();
            var cfg = new XmlReaderSettings { ValidationType = ValidationType.Schema, Schemas = schemas };
            cfg.ValidationEventHandler += (sender, e) => erros.Add(e.Message);

            var xml = $"<chave xmlns=\"{NamespaceNfe}\">{chave}</chave>";
            using (var reader = XmlReader.Create(new StringReader(xml), cfg))
            {
                while (reader.Read()) { }
            }

            return erros.Count == 0;
        }

        private static bool AEntidadeAceita(string chave)
        {
            try
            {
                new DocumentoReferenciadoNFe(chave);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Chave que o XSD recusa, a entidade tem de recusar tambem - nao faz sentido a
        /// entidade aceitar o que o schema vai barrar.
        /// </summary>
        [Theory]
        [InlineData("3526014641006400024265001000000351187688866")]   // 43, curta
        [InlineData("352601464100640002426500100000035118768886666")] // 45, longa
        [InlineData("35260146410064000242650010000003511876888ABC")]  // letra fora das posicoes 7 a 18
        [InlineData("")]
        public void ChaveRecusadaPeloXsd_ERecusadaPelaEntidade(string chave)
        {
            Assert.False(OXsdAceita(chave));
            Assert.False(AEntidadeAceita(chave));
        }

        /// <summary>
        /// A entidade e DE PROPOSITO mais restrita que o XSD: alem do pattern de TChNFe,
        /// confere o digito verificador. Numero trocado passa pelo schema e volta como
        /// rejeicao de chave invalida, sem dizer qual referencia e.
        /// </summary>
        [Fact]
        public void ChaveComDigitoVerificadorErrado_OXsdAceitaEAEntidadeRecusa()
        {
            // Mesma chave valida com um digito do numero trocado: DV correto seria 7.
            const string chaveComDvErrado = "35260146410064000242650010000093511876888666";

            Assert.True(OXsdAceita(chaveComDvErrado));
            Assert.False(AEntidadeAceita(chaveComDvErrado));
        }

        [Fact]
        public void ChaveValidaEAceitaPelosDois()
        {
            Assert.True(OXsdAceita(ChaveValida));
            Assert.True(AEntidadeAceita(ChaveValida));
        }

        [Fact]
        public void ChaveComLetraNasPosicoesPermitidas_EAceita()
        {
            // TChNFe e [0-9]{6}[0-9A-Z]{12}[0-9]{26}: letra so vale da 7a a 18a posicao,
            // que e justamente onde cabe o CNPJ alfanumerico da NT 2026.004.
            // cUF 35 + AAMM 2601 + CNPJ ABCDEF12345678 + modelo 55 + serie 001 +
            // numero 000000351 + tpEmis 1 + cNF 87688866 + DV 4.
            const string chave = "352601ABCDEF12345678550010000003511876888664";

            Assert.Equal(44, chave.Length);
            Assert.True(OXsdAceita(chave));
            Assert.True(AEntidadeAceita(chave));
        }

        [Theory]
        [InlineData(EDocumentoReferenciado.NFe, ChaveValida)]
        [InlineData(EDocumentoReferenciado.NFeSigilosa, ChaveSigilosa)]
        [InlineData(EDocumentoReferenciado.CTe, ChaveCTe)]
        public void TiposPorChave_SaoAceitos(EDocumentoReferenciado tipo, string chave)
        {
            var documento = new DocumentoReferenciadoNFe(chave, tipo);

            Assert.Equal(tipo, documento.TipoDocumento);
            Assert.Equal(chave, documento.ChaveAcesso);
        }

        /// <summary>
        /// O modelo mora na própria chave (posições 21 e 22) e tem de combinar com a tag:
        /// refNFe e refNFeSig são NF-e e NFC-e, refCTe é CT-e. Usar chave de NFC-e como
        /// refCTe é rejeição da SEFAZ sem dizer qual referência.
        /// </summary>
        [Theory]
        [InlineData(EDocumentoReferenciado.CTe, ChaveValida)]
        [InlineData(EDocumentoReferenciado.NFe, ChaveCTe)]
        [InlineData(EDocumentoReferenciado.NFeSigilosa, ChaveCTe)]
        public void ChaveDeModeloIncompativelComOTipo_ERecusada(EDocumentoReferenciado tipo, string chave)
        {
            var erro = Assert.Throws<ArgumentException>(() => new DocumentoReferenciadoNFe(chave, tipo));

            Assert.Contains("não corresponde ao tipo informado", erro.Message);
        }

        /// <summary>
        /// refNFeSig é "chave de acesso com código numérico zerado": chave normal nessa tag
        /// não é uma segunda forma de referenciar o mesmo documento.
        /// </summary>
        [Fact]
        public void ChaveSigilosaComCodigoNumericoPreenchido_ERecusada()
        {
            var erro = Assert.Throws<ArgumentException>(() => new DocumentoReferenciadoNFe(ChaveValida, EDocumentoReferenciado.NFeSigilosa));

            Assert.Contains("código numérico (cNF) zerado", erro.Message);
        }

        /// <summary>
        /// Modelo 59 (CF-e-SAT) entra no refNFe: é assim que a NF-e de CFOP 5929/6929
        /// referencia o cupom do SAT, porque não existe refCFe no leiaute.
        /// </summary>
        [Fact]
        public void ChaveDeCupomSat_EAceitaComoRefNFe()
        {
            var documento = new DocumentoReferenciadoNFe("35260146410064000242590010000003511876888668");

            Assert.Equal(EDocumentoReferenciado.NFe, documento.TipoDocumento);
        }

        [Theory]
        [InlineData(EDocumentoReferenciado.NF)]
        [InlineData(EDocumentoReferenciado.ECF)]
        public void TiposCompostos_SaoRecusados(EDocumentoReferenciado tipo)
        {
            // refNF e refECF nao sao chave: sao conjuntos de campos, com classe propria.
            var erro = Assert.Throws<ArgumentException>(() => new DocumentoReferenciadoNFe(ChaveValida, tipo));

            Assert.Contains("não é referenciado por chave de acesso", erro.Message);
        }

        [Fact]
        public void CadaDocumentoPreencheUmaOpcaoSoDoChoice()
        {
            var documentos = new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFe(ChaveValida),
                new DocumentoReferenciadoNFe(ChaveSigilosa, EDocumentoReferenciado.NFeSigilosa),
                new DocumentoReferenciadoNFe(ChaveCTe, EDocumentoReferenciado.CTe)
            };

            var nFref = DadosNFe.GetNFRef(documentos);

            Assert.Equal(3, nFref.Count);

            Assert.Equal(ChaveValida, nFref[0].refNFe);
            Assert.Null(nFref[0].refNFeSig);
            Assert.Null(nFref[0].refCTe);

            Assert.Equal(ChaveSigilosa, nFref[1].refNFeSig);
            Assert.Null(nFref[1].refNFe);
            Assert.Null(nFref[1].refCTe);

            Assert.Equal(ChaveCTe, nFref[2].refCTe);
            Assert.Null(nFref[2].refNFe);
            Assert.Null(nFref[2].refNFeSig);

            // nenhum deles e forma composta
            foreach (var item in nFref)
            {
                Assert.Null(item.refNF);
                Assert.Null(item.refNFP);
                Assert.Null(item.refECF);
            }
        }

        [Fact]
        public void ListaVaziaOuNula_NaoGeraNFref()
        {
            Assert.Empty(DadosNFe.GetNFRef(null));
            Assert.Empty(DadosNFe.GetNFRef(new List<DocumentoReferenciado>()));
        }

        /// <summary>
        /// Chave repetida no NFref e rejeicao. Na NF-e de CFOP 5929/6929 a lista chega de
        /// duas fontes - o que o operador cadastrou em notafiscal_dfereferenciado e o que
        /// NFeOperacoes deriva da venda (SAT/NFC-e) - e nada impede a mesma chave nas duas.
        /// </summary>
        [Fact]
        public void ChaveRepetidaNoMesmoTipo_SaiUmaVezSo()
        {
            var nFref = DadosNFe.GetNFRef(new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFe(ChaveValida),
                new DocumentoReferenciadoNFe(ChaveValida)
            });

            Assert.Single(nFref);
            Assert.Equal(ChaveValida, nFref[0].refNFe);
        }

        /// <summary>
        /// Chaves distintas em tags distintas saem as duas. A mesma chave nas duas tags nao
        /// e possivel: refNFeSig exige cNF zerado, e nesse caso o DV muda - sao chaves
        /// diferentes por construcao.
        /// </summary>
        [Fact]
        public void MesmaChaveEmTiposDiferentes_SaemAsDuas()
        {
            var nFref = DadosNFe.GetNFRef(new List<DocumentoReferenciado>
            {
                new DocumentoReferenciadoNFe(ChaveValida),
                new DocumentoReferenciadoNFe(ChaveSigilosa, EDocumentoReferenciado.NFeSigilosa)
            });

            Assert.Equal(2, nFref.Count);
            Assert.Equal(ChaveValida, nFref[0].refNFe);
            Assert.Equal(ChaveSigilosa, nFref[1].refNFeSig);
        }
    }
}
