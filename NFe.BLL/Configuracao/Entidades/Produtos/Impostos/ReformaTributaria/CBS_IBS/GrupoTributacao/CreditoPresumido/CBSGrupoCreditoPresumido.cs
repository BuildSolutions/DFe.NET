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
            decimal? valorCredito,
            decimal? valorCreditoEmCondicaoSuspensiva) : base(percentualCredito, valorCredito,valorCreditoEmCondicaoSuspensiva) { }

        /// <summary>
        /// Leitura: o braço do xs:choice é preservado. vCredPresCondSus presente no XML
        /// significa crédito em condição suspensiva, e aí vCredPres fica nulo — mesmo que o
        /// valor suspensivo seja zero.
        /// </summary>
        public CBSGrupoCreditoPresumido(gIBSCredPres creditoPresumidoCBS)
                : base(percentualCredito: creditoPresumidoCBS?.pCredPres ?? 0,
                      valorCredito: creditoPresumidoCBS?.vCredPresCondSus == null ? creditoPresumidoCBS?.vCredPres : null,
                      valorCreditoPresumidoEmCondicaoSuspensiva: creditoPresumidoCBS?.vCredPresCondSus) { }
    }
}
