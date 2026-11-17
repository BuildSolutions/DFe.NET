using System;
using System.Collections.Generic;
using System.Linq;
using NFe.BLL.Configuracao.Entidades;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// gCompraGov (BB01). refDFeAnt e obrigatorio nos tipos 2 e 3 e vedado nos tipos 1 e 4,
    /// e o tipo 2 aceita uma unica chave - rejeicoes 1195, 1196 e 1197.
    /// </summary>
    public class CompraGovernamentalTestes
    {
        // Chaves reais no padrao TChDFeRTC, com DV valido: a entidade confere o digito,
        // logo chave artificial nao serve mais. cUF 35 + AAMM 2601 + CNPJ alfanumerico +
        // modelo 55 + serie 001 + numero + tpEmis 1 + cNF + DV.
        private const string Chave = "352601ABCDEF12345678550010000003511876888664";
        private const string OutraChave = "352601ABCDEF12345678550010000003521876888661";

        private static CompraGovernamental Grupo(TipoOperGov tipo, params string[] chaves)
        {
            return new CompraGovernamental(tipoEnteGovernamental: TipoEnteGov.Estado,
                percentualRedutor: 5m,
                tipoOperacaoGovernamental: tipo,
                chavesDocumentosAnteriores: chaves.ToList());
        }

        [Theory]
        [InlineData(TipoOperGov.FornecimentoPagamentoPosterior)]
        [InlineData(TipoOperGov.RecebimentoPagamentoFornecimentoPosterior)]
        public void TipoQueVedaChave_RecusaChaveInformada(TipoOperGov tipo)
        {
            var erro = Assert.Throws<ArgumentException>(() => Grupo(tipo, Chave));

            Assert.Contains("não aceita chave", erro.Message);
        }

        [Theory]
        [InlineData(TipoOperGov.RecebimentoPagamentoFornecimentoRealizado)]
        [InlineData(TipoOperGov.FornecimentoPagamentoRealizado)]
        public void TipoQueExigeChave_RecusaSemChave(TipoOperGov tipo)
        {
            var erro = Assert.Throws<ArgumentException>(() => Grupo(tipo));

            Assert.Contains("exige a chave", erro.Message);
        }

        [Fact]
        public void Tipo2_AceitaUmaChaveSo()
        {
            var erro = Assert.Throws<ArgumentException>(() =>
                Grupo(TipoOperGov.RecebimentoPagamentoFornecimentoRealizado, Chave, OutraChave));

            Assert.Contains("uma única chave", erro.Message);

            var grupo = Grupo(TipoOperGov.RecebimentoPagamentoFornecimentoRealizado, Chave);
            Assert.Single(grupo.ChavesDocumentosAnteriores);
        }

        [Fact]
        public void Tipo3_AceitaVariasChaves()
        {
            var grupo = Grupo(TipoOperGov.FornecimentoPagamentoRealizado, Chave, OutraChave);

            Assert.Equal(2, grupo.ChavesDocumentosAnteriores.Count);
        }

        [Theory]
        [InlineData("3526014641006400024265001000000351187688866")]   // 43, curta
        [InlineData("35260146410064000242650010000003511876888ABC")]  // letra fora das posicoes 7 a 18
        public void ChaveFuraOPadrao_ERecusada(string chave)
        {
            var erro = Assert.Throws<ArgumentException>(() =>
                Grupo(TipoOperGov.FornecimentoPagamentoRealizado, chave));

            Assert.Contains("44 posições", erro.Message);
        }

        [Fact]
        public void ChaveComDigitoVerificadorErrado_ERecusada()
        {
            // Mesma chave valida com um digito do numero trocado.
            const string chaveComDvErrado = "352601ABCDEF12345678550010000003511876888665";

            var erro = Assert.Throws<ArgumentException>(() =>
                Grupo(TipoOperGov.FornecimentoPagamentoRealizado, chaveComDvErrado));

            Assert.Contains("Dígito verificador", erro.Message);
        }

        [Fact]
        public void ChaveRepetida_ERecusada()
        {
            // refDFeAnt repetido no mesmo grupo nao tem sentido e a SEFAZ recusa.
            var erro = Assert.Throws<ArgumentException>(() =>
                Grupo(TipoOperGov.FornecimentoPagamentoRealizado, Chave, Chave));

            Assert.Contains("repetida", erro.Message);
        }

        [Fact]
        public void MaisDeNoventaENoveChaves_ERecusado()
        {
            // refDFeAnt e maxOccurs=99 no leiaute. As 100 chaves tem de ser distintas,
            // senao a conferencia de repetida barraria antes do limite.
            var chaves = Enumerable.Range(1, 100)
                .Select(numero => ChaveComNumero(numero))
                .ToArray();

            var erro = Assert.Throws<ArgumentException>(() =>
                Grupo(TipoOperGov.FornecimentoPagamentoRealizado, chaves));

            Assert.Contains("no máximo 99", erro.Message);
        }

        [Fact]
        public void ChaveComEspacoEMinuscula_ENormalizada()
        {
            // Campo de tela devolve o que o operador colou: espaco ao redor e minuscula
            // gerariam XML fora do padrao e DV errado.
            var grupo = Grupo(TipoOperGov.FornecimentoPagamentoRealizado, "  " + Chave.ToLowerInvariant() + "  ");

            Assert.Equal(Chave, Assert.Single(grupo.ChavesDocumentosAnteriores));
        }

        /// <summary>
        /// Monta uma chave valida variando o numero do documento e recalculando o DV, do
        /// mesmo jeito que o ChaveFiscal faz: peso 2 a 9 da direita para a esquerda,
        /// resto 0 ou 1 vira DV zero.
        /// </summary>
        private static string ChaveComNumero(int numero)
        {
            var baseChave = "352601ABCDEF1234567855001" + numero.ToString("D9") + "1" + "87688866";

            int soma = 0;
            int peso = 2;

            for (int indice = baseChave.Length - 1; indice >= 0; indice--)
            {
                soma += (baseChave[indice] - '0') * peso;
                peso = peso < 9 ? peso + 1 : 2;
            }

            var resto = soma % 11;

            return baseChave + (resto == 0 || resto == 1 ? 0 : 11 - resto);
        }

        [Fact]
        public void ChaveEmBranco_NaoConta()
        {
            // Campo de tela vazio nao pode virar chave: no tipo 1 isso seria rejeicao 1195.
            var grupo = Grupo(TipoOperGov.FornecimentoPagamentoPosterior, "", "   ", null);

            Assert.Empty(grupo.ChavesDocumentosAnteriores);
        }

        [Fact]
        public void EnumerAcoesSeguemOLeiaute()
        {
            // TEnteGov tem 6 valores e TOperCompraGov tem 4 no DFeTiposBasicos_v1.00.xsd.
            Assert.Equal(6, Enum.GetValues(typeof(TipoEnteGov)).Length);
            Assert.Equal(4, Enum.GetValues(typeof(TipoOperGov)).Length);
            Assert.Equal(6, (int)TipoEnteGov.ComiteGestorIBS);
            Assert.Equal(3, (int)TipoOperGov.FornecimentoPagamentoRealizado);
        }

        /// <summary>
        /// tpEnteGov vai de 1 a 6 e tpOperGov de 1 a 4. Valor fora do domínio chega por
        /// cast de inteiro — que é exatamente o que acontece quando a coluna do banco tem
        /// lixo.
        /// </summary>
        [Fact]
        public void TipoDeEnteGovernamentalForaDoDominio_ERecusado()
        {
            var erro = Assert.Throws<ArgumentException>(() => new CompraGovernamental(
                tipoEnteGovernamental: (TipoEnteGov)0,
                percentualRedutor: 30m,
                tipoOperacaoGovernamental: TipoOperGov.FornecimentoPagamentoPosterior));

            Assert.Contains("1 a 6", erro.Message);
        }

        [Fact]
        public void TipoDeOperacaoForaDoDominio_ERecusado()
        {
            var erro = Assert.Throws<ArgumentException>(() => new CompraGovernamental(
                tipoEnteGovernamental: TipoEnteGov.Uniao,
                percentualRedutor: 30m,
                tipoOperacaoGovernamental: (TipoOperGov)99));

            Assert.Contains("1 a 4", erro.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1000)]
        [InlineData(100.01)]
        public void RedutorForaDaFaixa_ERecusado(decimal percentualRedutor)
        {
            var erro = Assert.Throws<ArgumentException>(() => new CompraGovernamental(
                tipoEnteGovernamental: TipoEnteGov.Uniao,
                percentualRedutor: percentualRedutor,
                tipoOperacaoGovernamental: TipoOperGov.FornecimentoPagamentoPosterior));

            Assert.Contains("0 a 100", erro.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(30)]
        [InlineData(100)]
        public void RedutorNaFaixa_EAceito(decimal percentualRedutor)
        {
            var grupo = new CompraGovernamental(tipoEnteGovernamental: TipoEnteGov.Uniao,
                percentualRedutor: percentualRedutor,
                tipoOperacaoGovernamental: TipoOperGov.FornecimentoPagamentoPosterior);

            Assert.Equal(percentualRedutor, grupo.PercentualRedutor);
        }

        private static NFe.Classes.Informacoes.infNFe NotaCom(gCompraGov compraGovernamental, IBSCBS impostoDoItem)
        {
            return new NFe.Classes.Informacoes.infNFe
            {
                ide = new ide { gCompraGov = compraGovernamental },
                det = new List<NFe.Classes.Informacoes.Detalhe.det>
                {
                    new NFe.Classes.Informacoes.Detalhe.det
                    {
                        nItem = 1,
                        imposto = new NFe.Classes.Informacoes.Detalhe.Tributacao.imposto { IBSCBS = impostoDoItem }
                    }
                }
            };
        }

        private static IBSCBS ItemComGrupo(bool comReducao, bool comTributacaoGovernamental)
        {
            gRed Reducao() => comReducao ? new gRed { pRedAliq = 0m, pAliqEfet = 0m } : null;

            return new IBSCBS
            {
                CST = CSTIBSCBS.cst000,
                cClassTrib = "000001",
                gIBSCBS = new gIBSCBS
                {
                    vBC = 100m,
                    gIBSUF = new gIBSUF { pIBSUF = 0.1m, vIBSUF = 0.1m, gRed = Reducao() },
                    gIBSMun = new gIBSMun { pIBSMun = 0m, vIBSMun = 0m, gRed = Reducao() },
                    gCBS = new gCBS { pCBS = 0.9m, vCBS = 0.9m, gRed = Reducao() },
                    gTribCompraGov = comTributacaoGovernamental
                        ? new gTribCompraGov { pAliqIBSUF = 0.1m, vTribIBSUF = 0.1m, pAliqIBSMun = 0m, vTribIBSMun = 0m, pAliqCBS = 0.9m, vTribCBS = 0.9m }
                        : null
                }
            };
        }

        /// <summary>
        /// gCompraGov no documento obriga o gRed nos três grupos do item (UB26-20, UB45-20 e
        /// UB64-20 — rejeições 1033, 1074 e 1079). Os dois níveis são montados por caminhos
        /// diferentes do mapper e ninguém enxergava o outro: o erro só aparecia na resposta
        /// da SEFAZ, sem dizer qual item.
        /// </summary>
        [Fact]
        public void CompraGovernamentalComItemSemReducao_ERecusada()
        {
            var nota = NotaCom(new gCompraGov { tpEnteGov = TipoEnteGov.Estado, pRedutor = 30m, tpOperGov = TipoOperGov.FornecimentoPagamentoPosterior },
                ItemComGrupo(comReducao: false, comTributacaoGovernamental: true));

            var erro = Assert.Throws<ArgumentException>(() => DadosNFe.ConferirCoerenciaDaCompraGovernamental(nota));

            Assert.Contains("Item 1", erro.Message);
            Assert.Contains("gRed", erro.Message);
        }

        [Fact]
        public void CompraGovernamentalComItemCompleto_EAceita()
        {
            var nota = NotaCom(new gCompraGov { tpEnteGov = TipoEnteGov.Estado, pRedutor = 30m, tpOperGov = TipoOperGov.FornecimentoPagamentoPosterior },
                ItemComGrupo(comReducao: true, comTributacaoGovernamental: true));

            DadosNFe.ConferirCoerenciaDaCompraGovernamental(nota);
        }

        /// <summary>
        /// UB82a-30 (rejeição 1144): sem gCompraGov no documento, o item não pode trazer
        /// gTribCompraGov.
        /// </summary>
        [Fact]
        public void ItemComTributacaoGovernamentalSemGrupoNoDocumento_ERecusado()
        {
            var nota = NotaCom(null, ItemComGrupo(comReducao: true, comTributacaoGovernamental: true));

            var erro = Assert.Throws<ArgumentException>(() => DadosNFe.ConferirCoerenciaDaCompraGovernamental(nota));

            Assert.Contains("gTribCompraGov", erro.Message);
        }

        [Fact]
        public void NotaComumSemCompraGovernamental_EAceita()
        {
            var nota = NotaCom(null, ItemComGrupo(comReducao: false, comTributacaoGovernamental: false));

            DadosNFe.ConferirCoerenciaDaCompraGovernamental(nota);
        }
    }
}
