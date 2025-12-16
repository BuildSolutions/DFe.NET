using System;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gCredPresIBSZFM — Informações do crédito presumido de IBS para fornecimentos a partir da ZFM (UB109)
    /// </summary>
    public class IBSGrupoCreditoPresumidoZFM : IIBSGrupo
    {
        public IBSGrupoCreditoPresumidoZFM(string dataCompetenciaApuracao,
            tpCredPresIBSZFM tipoCreditoPresumidoIBSZFM,
            decimal valorCreditoPresumidoIBSZFM)
        {
            AnoMesCompetenciaApuracao = dataCompetenciaApuracao;
            TipoCreditoPresumidoIBSZFM = tipoCreditoPresumidoIBSZFM;
            ValorCreditoPresumidoIBSZFM = valorCreditoPresumidoIBSZFM;
        }

        public IBSGrupoCreditoPresumidoZFM(gCredPresIBSZFM creditoPresumidoZFM)
        {
            AnoMesCompetenciaApuracao = creditoPresumidoZFM.competApur;
            TipoCreditoPresumidoIBSZFM = creditoPresumidoZFM.tpCredPresIBSZFM;
            ValorCreditoPresumidoIBSZFM = creditoPresumidoZFM?.vCredPresIBSZFM ?? 0;
        }

        /// <summary>
        /// competApur - Ano e mês referência do período de apuração (AAAA-MM) (UB132)
        /// Informar período atual ou retroativo.
        /// </summary>
        public string AnoMesCompetenciaApuracao { get; }

        /// <summary>tpCredPresIBSZFM — Tipo de classificação de acordo com o art. 450, § 1º, da LC 214/25 para o cálculo do crédito presumido na ZFM (UB110)
        /// Classificação conforme percentuais definidos no art. 450, § 1º, da LC 214/25 para o cálculo do crédito presumido:
        /// 0 - Sem Crédito Presumido
        /// 1 - Bens de consumo final (55%)
        /// 2 - Bens de capital (75%)
        /// 3 - Bens intermediários (90,25%)
        /// 4 - Bens de informática e outros definidos em legislação (100%)</summary>
        public tpCredPresIBSZFM TipoCreditoPresumidoIBSZFM { get; }

        /// <summary>vCredPresIBSZFM — Valor do crédito presumido calculado sobre o saldo devedor apurado (UB111)
        /// É obrigatório para nota de crédito com tpNFCredito = 02 - Apropriação de crédito presumido de IBS sobre o saldo devedor na ZFM (art. 450, § 1º, LC 214/25)
        /// Vedado para documentos que não sejam nota de crédito com tpNFCredito = 02 - Apropriação de crédito presumido de IBS sobre o saldo devedor na ZFM(art. 450, § 1º, LC 214/25)</summary>
        public decimal ValorCreditoPresumidoIBSZFM { get; }
    }
}
