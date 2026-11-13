using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gDevTrib — Devolução total ou parcial do tributo ("cashback" na própria nota),
    /// LC 214/25 art. 118. Valor já informado.
    /// <para>O percentual (<c>pDevTrib</c>) é <b>opcional no leiaute</b> e anulável aqui: a
    /// CBS da NF-e o exige e os demais grupos não o levam. Quem decide por tributo e por
    /// modelo é a montagem, em <c>DFe.BLL.DocumentoFiscalEletronico</c>.</para>
    /// </summary>
    public class CBSIBSGrupoDevolucaoTributo : ICBSIBSGrupo
    {
        public CBSIBSGrupoDevolucaoTributo(decimal valorDevolvido, decimal? percentualDevolucao = null)
        {
            ValorDevolvido = valorDevolvido;
            PercentualDevolucao = percentualDevolucao;
        }

        public CBSIBSGrupoDevolucaoTributo(gDevTrib devolucaoTributos)
        {
            ValorDevolvido = devolucaoTributos.vDevTrib;
            PercentualDevolucao = devolucaoTributos.pDevTrib;
        }

        /// <summary>vDevTrib — Valor devolvido do tributo.</summary>
        public decimal ValorDevolvido { get; set; }

        /// <summary>pDevTrib — Percentual de devolução do tributo. Nulo quando não informado.</summary>
        public decimal? PercentualDevolucao { get; set; }
    }

}
