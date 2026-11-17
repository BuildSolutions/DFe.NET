using System;
using System.Collections.Generic;
using System.Linq;
using NFe.BLL.Validators;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFe.BLL.Configuracao.Entidades
{
    /// <summary>
    /// BB01 - gCompraGov, grupo de compras governamentais, no nível do documento (ide).
    /// <para>O percentual de redução entra na alíquota efetiva dos itens:
    /// pAliqEfet = pIBSUF x (1 - pRedAliq/100) x (1 - pRedutor/100). Emitir
    /// gTribCompraGov no item sem este grupo no documento gera rejeição 1033.</para>
    /// </summary>
    public class CompraGovernamental
    {
        public CompraGovernamental(TipoEnteGov tipoEnteGovernamental,
            decimal percentualRedutor,
            TipoOperGov tipoOperacaoGovernamental,
            List<string> chavesDocumentosAnteriores = null)
        {
            var chaves = NormalizarChaves(chavesDocumentosAnteriores);

            ConferirTipos(tipoEnteGovernamental, tipoOperacaoGovernamental);
            ConferirRedutor(percentualRedutor);
            ConferirChavesDeDocumentosAnteriores(tipoOperacaoGovernamental, chaves);

            TipoEnteGovernamental = tipoEnteGovernamental;
            PercentualRedutor = percentualRedutor;
            TipoOperacaoGovernamental = tipoOperacaoGovernamental;
            //AsReadOnly: IReadOnlyList sobre uma List ainda deixa o consumidor
            //fazer cast de volta e mexer na colecao sem passar pelas conferencias.
            ChavesDocumentosAnteriores = chaves.AsReadOnly();
        }

        public CompraGovernamental(gCompraGov compraGovernamental)
        {
            TipoEnteGovernamental = compraGovernamental?.tpEnteGov ?? default(TipoEnteGov);
            PercentualRedutor = compraGovernamental?.pRedutor ?? 0;
            TipoOperacaoGovernamental = compraGovernamental?.tpOperGov ?? default(TipoOperGov);
            //Copia: a lista do DTO continua do chamador, e compartilhar deixaria a
            //entidade mudar por fora depois de construida.
            ChavesDocumentosAnteriores = new List<string>(compraGovernamental?.refDFeAnt ?? new List<string>()).AsReadOnly();
        }

        /// <summary>
        /// Limpa, confere e recusa chave repetida. refDFeAnt e TChDFeRTC - mesmo padrao e
        /// mesmo digito verificador da chave de NF-e - e espaco ao redor ou minuscula
        /// gerariam XML fora do padrao, por isso a chave e normalizada antes de conferida.
        /// <para>A conferencia esta aqui, e nao so na tela do SimpleSolution, porque esta
        /// e a entidade publica da biblioteca: quem chamar de outro lugar tem a mesma
        /// garantia. Vale somente para a montagem; o construtor de leitura nao confere
        /// nada (ver a regra em simplesolution/CLAUDE.md §3.9).</para>
        /// </summary>
        private static List<string> NormalizarChaves(List<string> chavesDocumentosAnteriores)
        {
            var chaves = new List<string>();

            foreach (var chaveInformada in chavesDocumentosAnteriores ?? new List<string>())
            {
                if (string.IsNullOrWhiteSpace(chaveInformada))
                {
                    continue;
                }

                var chave = ChaveAcessoDFe.Normalizar(chaveInformada);

                if (!ChaveAcessoDFe.FormatoValido(chave))
                {
                    throw new ArgumentException($"Chave de documento anterior inválida na compra governamental: \"{chaveInformada}\". O leiaute exige 44 posições no padrão da chave do DF-e.");
                }

                if (!ChaveAcessoDFe.DigitoVerificadorValido(chave))
                {
                    throw new ArgumentException($"Dígito verificador inválido na chave de documento anterior da compra governamental: \"{chave}\".");
                }

                if (chaves.Contains(chave))
                {
                    throw new ArgumentException($"Chave de documento anterior repetida na compra governamental: \"{chave}\".");
                }

                chaves.Add(chave);
            }

            // refDFeAnt e maxOccurs=99 no leiaute.
            if (chaves.Count > 99)
            {
                throw new ArgumentException($"A compra governamental informa {chaves.Count} chaves de documento anterior (refDFeAnt); o leiaute aceita no máximo 99.");
            }

            return chaves;
        }

        /// <summary>
        /// tpEnteGov (BB02) vai de 1 a 6 e tpOperGov (BB04) de 1 a 4. Valor fora do dominio
        /// chega aqui por cast de inteiro - e como o SimpleSolution guarda os dois em coluna
        /// int, e exatamente o que acontece quando a coluna tem lixo.
        /// </summary>
        private static void ConferirTipos(TipoEnteGov tipoEnteGovernamental, TipoOperGov tipoOperacaoGovernamental)
        {
            if (!Enum.IsDefined(typeof(TipoEnteGov), tipoEnteGovernamental))
            {
                throw new ArgumentException($"Tipo de ente governamental inválido na compra governamental: {(int)tipoEnteGovernamental}. O leiaute aceita de 1 a 6.");
            }

            if (!Enum.IsDefined(typeof(TipoOperGov), tipoOperacaoGovernamental))
            {
                throw new ArgumentException($"Tipo de operação com ente governamental inválido: {(int)tipoOperacaoGovernamental}. O leiaute aceita de 1 a 4.");
            }
        }

        /// <summary>
        /// pRedutor (BB03) e um percentual: o tipo do XSD aceita ate tres digitos inteiros,
        /// mas reducao de aliquota so faz sentido de 0 a 100. Negativo aumentaria o imposto
        /// e acima de 100 deixaria a aliquota efetiva negativa.
        /// </summary>
        private static void ConferirRedutor(decimal percentualRedutor)
        {
            if (percentualRedutor < 0 || percentualRedutor > 100)
            {
                throw new ArgumentException($"Percentual de redução da compra governamental fora da faixa: {percentualRedutor}. O percentual vai de 0 a 100.");
            }
        }

        /// <summary>
        /// refDFeAnt e obrigatorio nos tipos 2 e 3 e vedado nos tipos 1 e 4, e o tipo 2
        /// aceita uma unica chave. Sao as rejeicoes 1195, 1196 e 1197 - conferir aqui
        /// evita descobrir isso pela resposta da SEFAZ.
        /// </summary>
        private static void ConferirChavesDeDocumentosAnteriores(TipoOperGov tipoOperacao, List<string> chaves)
        {
            var exigeChave = tipoOperacao == TipoOperGov.RecebimentoPagamentoFornecimentoRealizado
                || tipoOperacao == TipoOperGov.FornecimentoPagamentoRealizado;

            if (!exigeChave && chaves.Count > 0)
            {
                throw new ArgumentException($"Compra governamental do tipo {(int)tipoOperacao} não aceita chave de documento anterior (refDFeAnt).");
            }

            if (exigeChave && chaves.Count == 0)
            {
                throw new ArgumentException($"Compra governamental do tipo {(int)tipoOperacao} exige a chave do documento anterior (refDFeAnt).");
            }

            if (tipoOperacao == TipoOperGov.RecebimentoPagamentoFornecimentoRealizado
                && chaves.Count > 1)
            {
                throw new ArgumentException("Compra governamental do tipo 2 aceita uma única chave de documento anterior (refDFeAnt).");
            }
        }

        /// <summary>tpEnteGov - Tipo de ente governamental.</summary>
        public TipoEnteGov TipoEnteGovernamental { get; }

        /// <summary>pRedutor - Percentual de redução da alíquota em compra governamental.</summary>
        public decimal PercentualRedutor { get; }

        /// <summary>tpOperGov - Tipo de operação com o ente governamental.</summary>
        public TipoOperGov TipoOperacaoGovernamental { get; }

        /// <summary>
        /// refDFeAnt - Chaves dos documentos fiscais anteriores, até 99.
        /// <para>Somente leitura de proposito: o construtor confere obrigatoriedade,
        /// duplicidade, formato, digito verificador e o limite de 99, e uma lista mutavel
        /// deixaria qualquer consumidor desfazer tudo isso depois - inclusive quem monta o
        /// XML, que recebia a mesma referencia.</para>
        /// </summary>
        public IReadOnlyList<string> ChavesDocumentosAnteriores { get; }
    }
}
