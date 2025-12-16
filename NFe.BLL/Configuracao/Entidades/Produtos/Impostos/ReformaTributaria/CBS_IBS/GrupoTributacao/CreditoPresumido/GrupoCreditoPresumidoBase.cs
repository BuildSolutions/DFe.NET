using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators;
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
        public GrupoCreditoPresumidoBase(EImpostoReformaTributariaTipoSimplificado eImpostoReformaTributariaTipoSimplificado,
            decimal percentualCredito,
            decimal valorCredito,
            decimal valorCreditoPresumidoEmCondicaoSuspensiva)
        {
            EImpostoReformaTributariaTipoSimplificado = eImpostoReformaTributariaTipoSimplificado;
            PercentualCredito = percentualCredito;
            ValorCredito = valorCredito;
            ValorCreditoEmCondicaoSuspensiva = valorCreditoPresumidoEmCondicaoSuspensiva;
        }

        public EImpostoReformaTributariaTipoSimplificado EImpostoReformaTributariaTipoSimplificado { get; protected set; }

        /// <summary>pCredPres — Percentual do crédito presumido.</summary>
        public decimal PercentualCredito { get; protected set; }

        /// <summary>vCredPres — Valor do crédito.</summary>
        public decimal ValorCredito { get; protected set; }

        /// <summary>vCredPresCondSus — Valor do Crédito Presumido em condição suspensiva..</summary>
        public decimal ValorCreditoEmCondicaoSuspensiva { get; protected set; }
    }
}
