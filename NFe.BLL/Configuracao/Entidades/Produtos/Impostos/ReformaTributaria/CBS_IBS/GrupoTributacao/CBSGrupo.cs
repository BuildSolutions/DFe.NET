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
                                                     aliquota: ibsCbs?.gIBSUF?.pIBSUF ?? 0,
                                                     valor: ibsCbs?.gIBSUF?.vIBSUF ?? 0,
                                                     percentualDiferimento: ibsCbs?.gIBSUF?.gDif?.pDif ?? 0,
                                                     valorDiferimento: ibsCbs?.gIBSUF?.gDif?.vDif ?? 0,
                                                     valorDevolucaoTributos: ibsCbs?.gIBSUF?.gDevTrib?.vDevTrib ?? 0,
                                                     percentualReducaoAliquota: ibsCbs?.gIBSUF?.gRed?.pRedAliq ?? 0,
                                                     aliquotaEfetiva: ibsCbs?.gIBSUF?.gRed?.pAliqEfet ?? 0) { }
    }
}
