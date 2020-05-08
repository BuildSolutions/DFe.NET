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
    }
}
