using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// Classe abstrata base para todos os grupos CBS/IBS
    /// </summary>
    public abstract class CBSIBSGrupoBase : ICBSIBSGrupo
    {
        protected CBSIBSGrupoBase(EImpostoReformaTributariaTipo eImpostoReformaTributariaTipo,
            decimal? aliquota,
            decimal? valor,

            decimal? percentualDiferimento = null,
            decimal? valorDiferimento = null,

            decimal? valorDevolucaoTributos = null,

            decimal? percentualReducaoAliquota = null,
            decimal? aliquotaEfetiva = null)
        {
            EImpostoReformaTributariaTipo = eImpostoReformaTributariaTipo;
            AliquotaPercentual = aliquota ?? 0;
            ValorTotal = valor ?? 0;
            //if (aliquota.HasValue && aliquota.Value > 0)
            //{
            //    GrupoTributacaoNormal = new CBSIBSGrupoTributacaoNormal(aliquota ?? 0, valor ?? 0);
            //}

            if (percentualReducaoAliquota.HasValue && percentualReducaoAliquota.Value > 0)
            {
                GrupoReducao = new CBSIBSGrupoReducao(percentualReducaoAliquota ?? 0, aliquotaEfetiva ?? 0);
            }

            if (percentualDiferimento.HasValue && percentualDiferimento.Value > 0)
            {
                GrupoDiferimento = new CBSIBSGrupoDiferimento(percentualDiferimento ?? 0, valorDiferimento ?? 0);
            }

            if (valorDevolucaoTributos.HasValue && valorDevolucaoTributos.Value > 0)
            {
                GrupoDevolucaoTributo = new CBSIBSGrupoDevolucaoTributo(valorDevolucaoTributos ?? 0);
            }
        }

        public EImpostoReformaTributariaTipo EImpostoReformaTributariaTipo { get; protected set; }
        //public CBSIBSGrupoTributacaoNormal GrupoTributacaoNormal { get; protected set; }
        public CBSIBSGrupoDiferimento GrupoDiferimento { get; protected set; }
        public CBSIBSGrupoDevolucaoTributo GrupoDevolucaoTributo { get; protected set; }
        public CBSIBSGrupoReducao GrupoReducao { get; protected set; }
        /// <summary>pIBS/pCBS — Alíquota em percentual (ex.: 1.5 = 1,5%).</summary>
        public decimal AliquotaPercentual { get; }
        public decimal ValorTotal { get; protected set; }
    }
}
