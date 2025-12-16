using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.CreditoPresumido
{
    /// <summary>
    /// gCBSCredPres — Grupo de Informações do Crédito Presumido referente a CBS (UB78)
    /// Grupo de Informações do Crédito Presumido da CBS, quando aproveitado pelo emitente do documento.
    /// vCredPres = Base × Percentual.
    /// vCredPresCondSus = Base × Percentual.
    /// </summary>
    public class CBSGrupoCreditoPresumido : GrupoCreditoPresumidoBase, ICBSGrupo
    {
        public CBSGrupoCreditoPresumido(decimal percentualCredito,
            decimal valorCredito,
            decimal valorCreditoEmCondicaoSuspensiva) : base(EImpostoReformaTributariaTipoSimplificado.CBS, percentualCredito, valorCredito,valorCreditoEmCondicaoSuspensiva) { }

        public CBSGrupoCreditoPresumido(gIBSCredPres creditoPresumidoCBS)
                : base(eImpostoReformaTributariaTipoSimplificado: EImpostoReformaTributariaTipoSimplificado.CBS,
                      percentualCredito: creditoPresumidoCBS?.pCredPres ?? 0,
                      valorCredito: creditoPresumidoCBS?.vCredPres ?? 0,
                      valorCreditoPresumidoEmCondicaoSuspensiva: creditoPresumidoCBS?.vCredPresCondSus ?? 0) { }
    }
}
