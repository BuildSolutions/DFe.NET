using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators;
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
            decimal valorCredito,
            decimal valorCreditoEmCondicaoSuspensiva) : base(EImpostoReformaTributariaTipoSimplificado.IBS, percentualCredito, valorCredito, valorCreditoEmCondicaoSuspensiva) { }

        public IBSGrupoCreditoPresumido(gIBSCredPres creditoPresumidoIBS)
            : base(eImpostoReformaTributariaTipoSimplificado: EImpostoReformaTributariaTipoSimplificado.IBS,
                  percentualCredito: creditoPresumidoIBS?.pCredPres ?? 0,
                  valorCredito: creditoPresumidoIBS?.vCredPres ?? 0,
                  valorCreditoPresumidoEmCondicaoSuspensiva: creditoPresumidoIBS?.vCredPresCondSus ?? 0) { }
    }
}
