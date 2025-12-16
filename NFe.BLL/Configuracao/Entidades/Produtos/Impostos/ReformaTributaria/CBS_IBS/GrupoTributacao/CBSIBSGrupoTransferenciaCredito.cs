using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gTransfCred — Transferências de Crédito (UB106)
    /// </summary>
    public class CBSIBSGrupoTransferenciaCredito : ICBSIBSGrupo
    {
        public CBSIBSGrupoTransferenciaCredito(decimal valorASerTransferidoCBS,
            decimal valorASerTransferidoIBS)
        {
            ValorASerTransferidoCBS = valorASerTransferidoCBS;
            ValorASerTransferidoIBS = valorASerTransferidoIBS;
        }

        public CBSIBSGrupoTransferenciaCredito(gTransfCred transferenciaCredito)
        {
            ValorASerTransferidoCBS = transferenciaCredito.vCBS;
            ValorASerTransferidoIBS = transferenciaCredito.vIBS;
        }

        /// <summary>vIBS — Valor do IBS a ser transferido (UB107)
        public decimal ValorASerTransferidoIBS { get; private set; }

        /// <summary>vCBS — Valor do CBS a ser transferido (UB108)
        public decimal ValorASerTransferidoCBS { get; private set; }
    }
}
