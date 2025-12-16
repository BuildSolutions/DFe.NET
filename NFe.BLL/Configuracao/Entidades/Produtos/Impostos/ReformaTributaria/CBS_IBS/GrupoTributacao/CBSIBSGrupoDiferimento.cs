using System;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gDif — Diferimento do IBS/CBS.
    /// Possui Percentual e Valor Diferido.
    /// vDif = Base × Percentual.
    /// </summary>
    public class CBSIBSGrupoDiferimento : ICBSIBSGrupo
    {
        private readonly decimal _aliquotaEfetiva;

        public CBSIBSGrupoDiferimento(decimal percentualDiferimento,
            decimal valorDiferimento)
        {
            PercentualDiferimento = percentualDiferimento;
            ValorDiferimento = valorDiferimento;
        }

        public CBSIBSGrupoDiferimento(gDif diferimento)
        {
            PercentualDiferimento = diferimento.pDif;
            ValorDiferimento = diferimento.vDif;
        }

        /// <summary>pDif — Percentual de diferimento (0–100).</summary>
        public decimal PercentualDiferimento { get; }
        /// <summary>vDif — Valor diferido.</summary>
        public decimal ValorDiferimento { get; private set; }
    }
}