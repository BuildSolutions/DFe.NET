using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gCBS — Grupo de Informações da CBS (UB55)
    /// </summary>
    public class CBSGrupo : CBSIBSGrupoBase
    {
        public CBSGrupo(decimal? aliquota,
            decimal? valor,

            decimal? percentualDiferimento = null,
            decimal? valorDiferimento = null,

            decimal? valorDevolucaoTributos = null,
            decimal? percentualDevolucaoTributos = null,

            decimal? percentualReducaoAliquota = null,
            decimal? aliquotaEfetiva = null,

            bool exigeGrupoReducao = false,

            CBSGrupoAreaIncentivada grupoAreaIncentivada = null) : base(aliquota: aliquota,
                                                     valor: valor,
                                                     percentualDiferimento: percentualDiferimento,
                                                     valorDiferimento: valorDiferimento,
                                                     valorDevolucaoTributos: valorDevolucaoTributos,
                                                     percentualDevolucaoTributos: percentualDevolucaoTributos,
                                                     percentualReducaoAliquota: percentualReducaoAliquota,
                                                     aliquotaEfetiva: aliquotaEfetiva,
                                                     exigeGrupoReducao: exigeGrupoReducao)
        {
            GrupoAreaIncentivada = grupoAreaIncentivada;
        }

        public CBSGrupo(gIBSCBS ibsCbs) : base(aliquota: ibsCbs?.gCBS?.pCBS ?? 0,
                                                     valor: ibsCbs?.gCBS?.vCBS ?? 0,
                                                     percentualDiferimento: ibsCbs?.gCBS?.gDif?.pDif ?? 0,
                                                     valorDiferimento: ibsCbs?.gCBS?.gDif?.vDif ?? 0,
                                                     valorDevolucaoTributos: ibsCbs?.gCBS?.gDevTrib?.vDevTrib ?? 0,
                                                     percentualDevolucaoTributos: ibsCbs?.gCBS?.gDevTrib?.pDevTrib,
                                                     percentualReducaoAliquota: ibsCbs?.gCBS?.gRed?.pRedAliq ?? 0,
                                                     aliquotaEfetiva: ibsCbs?.gCBS?.gRed?.pAliqEfet ?? 0,
                                                     exigeGrupoReducao: ibsCbs?.gCBS?.gRed != null)
        {
            if (ibsCbs?.gCBS?.gALCZFMCBS != null)
            {
                GrupoAreaIncentivada = new CBSGrupoAreaIncentivada(ibsCbs.gCBS.gALCZFMCBS);
            }
        }

        /// <summary>
        /// gALCZFMCBS (UB66a) - área incentivada (ALC/ZFM) com alíquota zero da CBS. Nulo
        /// fora dessas áreas, que é o caso da maioria das operações.
        /// </summary>
        public CBSGrupoAreaIncentivada GrupoAreaIncentivada { get; }
    }
}
