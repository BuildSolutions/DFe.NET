using System.Collections.Generic;
using NFe.Classes.Informacoes.Pagamento;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Pagamento
    {
        public Pagamento(FormaPagamento formaPagamento, decimal valor)
        {
            FormaPagamento = formaPagamento;
            Valor = valor;
        }

        public FormaPagamento FormaPagamento { get; private set; }

        public decimal Valor { get; private set; }

        public PagamentoIntegracaoCartao pagamentoIntegracaoCartao { get; private set; }

        public void InformarDadosCartao(TipoIntegracaoPagamento eTipoIntegracaoPagamento,
            string cnpjCredenciadoraCartao = null,
            BandeiraCartao? bandeiraCartao = null,
            string codigoAutorizacaoTransacao = null)
        {
            pagamentoIntegracaoCartao = new PagamentoIntegracaoCartao(eTipoIntegracaoPagamento,
                cnpjCredenciadoraCartao,
                bandeiraCartao,
                codigoAutorizacaoTransacao);
        }

        /// <summary>
        /// Devolve a lista de enums do tipo <see cref="FormaPagamento"/> que necessitam informar tag de dados do cartão/>
        /// </summary>
        public static ISet<FormaPagamento> FormaPagamentoCartao = new HashSet<FormaPagamento>()
        {
            FormaPagamento.fpCartaoCredito, FormaPagamento.fpCartaoDebito
        };
    }
}
