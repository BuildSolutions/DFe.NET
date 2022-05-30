using NFe.Classes.Informacoes.Pagamento;

namespace NFe.BLL.Configuracao.Entidades
{
    public class PagamentoIntegracaoCartao
    {
        public PagamentoIntegracaoCartao(TipoIntegracaoPagamento eTipoIntegracaoPagamento,
            string cnpjCredenciadoraCartao = null,
            BandeiraCartao? bandeiraCartao = null,
            string codigoAutorizacaoTransacao = null)
        {
            ETipoIntegracaoPagamento = eTipoIntegracaoPagamento;
            CNPJCredenciadoraCartao = cnpjCredenciadoraCartao;
            BandeiraCartao = bandeiraCartao;
            CodigoAutorizacaoTransacao = codigoAutorizacaoTransacao;
        }

        public TipoIntegracaoPagamento ETipoIntegracaoPagamento { get; }

        public string CNPJCredenciadoraCartao { get; }

        public BandeiraCartao? BandeiraCartao { get; private set; }

        public string CodigoAutorizacaoTransacao { get; }

    }
}
