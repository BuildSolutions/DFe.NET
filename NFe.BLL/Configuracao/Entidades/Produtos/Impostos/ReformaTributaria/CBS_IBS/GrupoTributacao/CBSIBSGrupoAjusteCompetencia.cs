using System;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gAjusteCompet — Grupo de ajuste de competência do IBS e da CBS (UB118).
    /// <para>É uma das opções do xs:choice de TTribNFe, junto com gIBSCBS, gIBSCBSMono e
    /// gTransfCred: quando preenchido, nenhum dos outros três pode sair.</para>
    /// </summary>
    public class CBSIBSGrupoAjusteCompetencia
    {
        public CBSIBSGrupoAjusteCompetencia(string anoMesCompetenciaApuracao,
            decimal valorAjusteIBS,
            decimal valorAjusteCBS)
        {
            // competApur nao e minOccurs=0 em TAjusteCompet: sem ela o grupo nao pode
            // sair, e deixar passar vazio viraria rejeicao de schema sem causa aparente.
            if (string.IsNullOrEmpty(anoMesCompetenciaApuracao))
            {
                throw new ArgumentException("Ajuste de competência sem período de apuração: o leiaute exige competApur no formato AAAA-MM.");
            }

            CompetenciaDeApuracao.Conferir(anoMesCompetenciaApuracao, "do ajuste");

            AnoMesCompetenciaApuracao = anoMesCompetenciaApuracao;
            ValorAjusteIBS = valorAjusteIBS;
            ValorAjusteCBS = valorAjusteCBS;
        }

        public CBSIBSGrupoAjusteCompetencia(gAjusteCompet ajusteCompetencia)
        {
            AnoMesCompetenciaApuracao = ajusteCompetencia?.competApur;
            ValorAjusteIBS = ajusteCompetencia?.vIBS ?? 0;
            ValorAjusteCBS = ajusteCompetencia?.vCBS ?? 0;
        }

        /// <summary>competApur — ano e mês de referência do período de apuração, AAAA-MM.</summary>
        public string AnoMesCompetenciaApuracao { get; }

        /// <summary>vIBS — valor do IBS do ajuste.</summary>
        public decimal ValorAjusteIBS { get; }

        /// <summary>vCBS — valor da CBS do ajuste.</summary>
        public decimal ValorAjusteCBS { get; }
    }
}
