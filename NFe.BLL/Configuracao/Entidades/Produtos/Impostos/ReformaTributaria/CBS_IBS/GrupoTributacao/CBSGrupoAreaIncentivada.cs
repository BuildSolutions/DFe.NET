using System;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gALCZFMCBS (UB66a) — operação em área incentivada (ALC/ZFM) amparada por alíquota
    /// zero da CBS, artigos 451 e 466 da LC 214/25. Vai dentro do gCBS.
    /// <para>Carrega o que a CBS <b>seria</b> fora da área incentivada: a alíquota efetiva
    /// e o valor. A rejeição 1218 confere o valor contra o calculado.</para>
    /// </summary>
    public class CBSGrupoAreaIncentivada
    {
        public CBSGrupoAreaIncentivada(tpALCZFMCBS tipoAplicacao,
            decimal aliquotaEfetivaRegular,
            decimal valorTributoRegular,
            string numeroProcessoSuframa = null)
        {
            ConferirTipoAplicacao(tipoAplicacao);
            ConferirProcessoSuframa(tipoAplicacao, numeroProcessoSuframa);

            TipoAplicacao = tipoAplicacao;
            AliquotaEfetivaRegular = aliquotaEfetivaRegular;
            ValorTributoRegular = valorTributoRegular;
            NumeroProcessoSuframa = numeroProcessoSuframa?.Trim();
        }

        public CBSGrupoAreaIncentivada(gALCZFMCBS areaIncentivada) : this(
            tipoAplicacao: areaIncentivada.tpALCZFMCBS,
            aliquotaEfetivaRegular: areaIncentivada.pAliqEfetRegCBS,
            valorTributoRegular: areaIncentivada.vTribRegCBS,
            numeroProcessoSuframa: areaIncentivada.nProcSuframa)
        {
        }

        /// <summary>UB66b - Tipo de aplicação da alíquota zero da CBS.</summary>
        public tpALCZFMCBS TipoAplicacao { get; }

        /// <summary>UB66c - Número do processo na Suframa, de 8 a 12 caracteres.</summary>
        public string NumeroProcessoSuframa { get; }

        /// <summary>UB66d - Alíquota efetiva da CBS que incidiria fora da área incentivada.</summary>
        public decimal AliquotaEfetivaRegular { get; }

        /// <summary>UB66e - Valor da CBS que seria devido fora da área incentivada.</summary>
        public decimal ValorTributoRegular { get; }

        /// <summary>
        /// tpALCZFMCBS (UB66b) aceita 1 e 2. Valor fora do domínio chega por cast de
        /// inteiro, do mesmo jeito que acontece em CompraGovernamental.
        /// </summary>
        private static void ConferirTipoAplicacao(tpALCZFMCBS tipoAplicacao)
        {
            if (!Enum.IsDefined(typeof(tpALCZFMCBS), tipoAplicacao))
            {
                throw new ArgumentException($"Tipo de aplicação da alíquota zero em área incentivada inválido: {(int)tipoAplicacao}. O leiaute aceita 1 ou 2.");
            }
        }

        /// <summary>
        /// UB66c-10: o número do processo na Suframa é obrigatório quando
        /// tpALCZFMCBS = 2 (rejeição 1192) e tem de 8 a 12 caracteres. Nos demais tipos
        /// não é exigido - informar não é vedado, e por isso não é recusado aqui.
        /// </summary>
        private static void ConferirProcessoSuframa(tpALCZFMCBS tipoAplicacao, string numeroProcessoSuframa)
        {
            var processo = numeroProcessoSuframa?.Trim();
            var informado = !string.IsNullOrWhiteSpace(processo);

            if (tipoAplicacao == tpALCZFMCBS.tpAlcZfmCbs2 && !informado)
            {
                throw new ArgumentException("A operação em área incentivada do tipo 2 exige o número do processo na Suframa (nProcSuframa).");
            }

            if (informado
                && (processo.Length < 8 || processo.Length > 12))
            {
                throw new ArgumentException($"Número do processo na Suframa fora do tamanho do leiaute (8 a 12 caracteres): \"{processo}\".");
            }
        }
    }
}
