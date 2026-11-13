using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.CreditoPresumido
{
    /// <summary>
    /// gIBSCredPres — Grupo de Informações do Crédito Presumido referente a IBS (UB123)
    /// Grupo de Informações do Crédito Presumido da IBS, quando aproveitado pelo emitente do documento.
    /// vCredPres = Base × Percentual.
    /// vCredPresCondSus = Base × Percentual.
    /// </summary>
    public class IBSGrupoCreditoPresumido : GrupoCreditoPresumidoBase, IIBSGrupo
    {
        public IBSGrupoCreditoPresumido(decimal percentualCredito,
            decimal? valorCredito,
            decimal? valorCreditoEmCondicaoSuspensiva) : base(percentualCredito, valorCredito, valorCreditoEmCondicaoSuspensiva) { }

        /// <summary>
        /// Leitura: o braço do xs:choice é preservado. vCredPresCondSus presente no XML
        /// significa crédito em condição suspensiva, e aí vCredPres fica nulo — mesmo que o
        /// valor suspensivo seja zero.
        /// </summary>
        public IBSGrupoCreditoPresumido(gIBSCredPres creditoPresumidoIBS)
            : base(percentualCredito: creditoPresumidoIBS?.pCredPres ?? 0,
                  valorCredito: creditoPresumidoIBS?.vCredPresCondSus == null ? creditoPresumidoIBS?.vCredPres : null,
                  valorCreditoPresumidoEmCondicaoSuspensiva: creditoPresumidoIBS?.vCredPresCondSus) { }
    }
}
