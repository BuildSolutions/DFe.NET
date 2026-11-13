using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gEstornoCred — Grupo de estorno de crédito do IBS e da CBS (UB119).
    /// <para>Não participa de nenhum xs:choice: em TTribNFe é irmão opcional do choice, no
    /// mesmo nível de CST e cClassTrib, logo convivê com qualquer um dos grupos da
    /// operação.</para>
    /// </summary>
    public class CBSIBSGrupoEstornoCredito
    {
        public CBSIBSGrupoEstornoCredito(decimal valorEstornoIBS, decimal valorEstornoCBS)
        {
            ValorEstornoIBS = valorEstornoIBS;
            ValorEstornoCBS = valorEstornoCBS;
        }

        public CBSIBSGrupoEstornoCredito(gEstornoCred estornoCredito)
        {
            ValorEstornoIBS = estornoCredito?.vIBSEstCred ?? 0;
            ValorEstornoCBS = estornoCredito?.vCBSEstCred ?? 0;
        }

        /// <summary>vIBSEstCred — valor do IBS a estornar.</summary>
        public decimal ValorEstornoIBS { get; }

        /// <summary>vCBSEstCred — valor da CBS a estornar.</summary>
        public decimal ValorEstornoCBS { get; }
    }
}
