using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gDev — Devolução total ou parcial do tributo.
    /// Valor já informado.
    /// </summary>
    public class CBSIBSGrupoDevolucaoTributo : ICBSIBSGrupo
    {
        public CBSIBSGrupoDevolucaoTributo(decimal valorDevolvido)
        {
            ValorDevolvido = valorDevolvido;
        }

        public CBSIBSGrupoDevolucaoTributo(gDevTrib devolucaoTributos)
        {
            ValorDevolvido = devolucaoTributos.vDevTrib;
        }

        /// <summary>vDev — Valor devolvido do tributo.</summary>
        public decimal ValorDevolvido { get; set; }
    }

}
