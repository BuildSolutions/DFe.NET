using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.CreditoPresumido
{
    /// <summary>
    /// gCredPres — Grupo de Informações do Crédito Presumido do IBS, quando aproveitado pelo emitente do documento. Exemplos: 1 - Aquisição de PR não contribuinte. 2 - Tomador de serviço de transporte de TAC PF não contrib. 3 - Aquisição de pessoa física com destino a reciclagem. 4 - Aquisição de bens móveis de PF não contrib. para revenda (veículos / brechó). 5 - Regime opcional para cooperativa..
    /// vCredPres = Base × Percentual.
    /// vCredPresCondSus = Base × Percentual.
    /// </summary>
    public abstract class GrupoCreditoPresumidoBase : IIBSGrupo, ICBSGrupo
    {
        /// <summary>
        /// vCredPres e vCredPresCondSus sao um xs:choice de TCredPres: o credito ou esta
        /// aproveitado, ou esta em condicao suspensiva. Os dois valores sao anulaveis para
        /// guardar QUAL dos dois veio - colapsar ausencia em zero fazia um
        /// vCredPresCondSus zero voltar como vCredPres zero na reescrita, trocando de
        /// braco do choice.
        /// </summary>
        public GrupoCreditoPresumidoBase(decimal percentualCredito,
            decimal? valorCredito,
            decimal? valorCreditoPresumidoEmCondicaoSuspensiva)
        {
            PercentualCredito = percentualCredito;
            ValorCredito = valorCredito;
            ValorCreditoEmCondicaoSuspensiva = valorCreditoPresumidoEmCondicaoSuspensiva;
        }

        /// <summary>pCredPres — Percentual do crédito presumido.</summary>
        public decimal PercentualCredito { get; protected set; }

        /// <summary>vCredPres — Valor do crédito. Nulo quando o crédito está em condição suspensiva.</summary>
        public decimal? ValorCredito { get; protected set; }

        /// <summary>vCredPresCondSus — Valor do crédito em condição suspensiva. Nulo quando o crédito está aproveitado.</summary>
        public decimal? ValorCreditoEmCondicaoSuspensiva { get; protected set; }
    }
}
