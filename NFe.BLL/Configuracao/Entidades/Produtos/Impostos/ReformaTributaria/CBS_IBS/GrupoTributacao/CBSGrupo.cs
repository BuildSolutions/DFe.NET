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

            decimal? percentualReducaoAliquota = null,
            decimal? aliquotaEfetiva = null) : base(Enumerators.EImpostoReformaTributariaTipo.CBS,
                                                     aliquota,
                                                     valor,
                                                     percentualDiferimento,
                                                     valorDiferimento,
                                                     valorDevolucaoTributos,
                                                     percentualReducaoAliquota,
                                                     aliquotaEfetiva)
        { }

        public CBSGrupo(gIBSCBS ibsCbs) : base(eImpostoReformaTributariaTipo: Enumerators.EImpostoReformaTributariaTipo.IBSUF,
                                                     aliquota: ibsCbs?.gCBS?.pCBS ?? 0,
                                                     valor: ibsCbs?.gCBS?.vCBS ?? 0,
                                                     percentualDiferimento: ibsCbs?.gCBS?.gDif?.pDif ?? 0,
                                                     valorDiferimento: ibsCbs?.gCBS?.gDif?.vDif ?? 0,
                                                     valorDevolucaoTributos: ibsCbs?.gCBS?.gDevTrib?.vDevTrib ?? 0,
                                                     percentualReducaoAliquota: ibsCbs?.gCBS?.gRed?.pRedAliq ?? 0,
                                                     aliquotaEfetiva: ibsCbs?.gCBS?.gRed?.pAliqEfet ?? 0) { }
    }
}
