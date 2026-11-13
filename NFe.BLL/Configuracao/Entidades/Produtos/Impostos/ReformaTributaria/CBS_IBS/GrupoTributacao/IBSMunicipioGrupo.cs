using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gIBSMun — Grupo de Informações do IBS para o município (UB36)
    /// </summary>
    public class IBSMunicipioGrupo : CBSIBSGrupoBase
    {
        public IBSMunicipioGrupo(decimal? aliquota,
            decimal? valor,

            decimal? percentualDiferimento = null,
            decimal? valorDiferimento = null,

            decimal? valorDevolucaoTributos = null,

            decimal? percentualReducaoAliquota = null,
            decimal? aliquotaEfetiva = null,

            bool exigeGrupoReducao = false) : base(aliquota: aliquota,
                                                     valor: valor,
                                                     percentualDiferimento: percentualDiferimento,
                                                     valorDiferimento: valorDiferimento,
                                                     valorDevolucaoTributos: valorDevolucaoTributos,
                                                     percentualReducaoAliquota: percentualReducaoAliquota,
                                                     aliquotaEfetiva: aliquotaEfetiva,
                                                     exigeGrupoReducao: exigeGrupoReducao) { }

        public IBSMunicipioGrupo(gIBSCBS ibsCbs) : base(aliquota: ibsCbs?.gIBSMun?.pIBSMun ?? 0,
                                                     valor: ibsCbs?.gIBSMun?.vIBSMun ?? 0,
                                                     percentualDiferimento: ibsCbs?.gIBSMun?.gDif?.pDif ?? 0,
                                                     valorDiferimento: ibsCbs?.gIBSMun?.gDif?.vDif ?? 0,
                                                     valorDevolucaoTributos: ibsCbs?.gIBSMun?.gDevTrib?.vDevTrib ?? 0,
                                                     percentualDevolucaoTributos: ibsCbs?.gIBSMun?.gDevTrib?.pDevTrib,
                                                     percentualReducaoAliquota: ibsCbs?.gIBSMun?.gRed?.pRedAliq ?? 0,
                                                     aliquotaEfetiva: ibsCbs?.gIBSMun?.gRed?.pAliqEfet ?? 0,
                                                     exigeGrupoReducao: ibsCbs?.gIBSMun?.gRed != null) { }
    }
}
