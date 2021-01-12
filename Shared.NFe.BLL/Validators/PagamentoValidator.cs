using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators
{
    public class PagamentoValidator : AbstractValidator<Pagamento>
    {
        public PagamentoValidator()
        {
            RuleFor(pag => pag.FormaPagamento).Must(pag => pag.EValido()).WithMessage($"Forma de Pagamento é inválida!");
            RuleFor(pag => pag.Valor).GreaterThan(0).WithMessage(pag =>  $"Valor inválido da Forma de Pagamento: {pag.FormaPagamento}.");
            RuleFor(pag => pag.pagamentoIntegracaoCartao).NotNull().When(pag => pag.FormaPagamento == Classes.Informacoes.Pagamento.FormaPagamento.fpCartaoCredito
            || pag.FormaPagamento == Classes.Informacoes.Pagamento.FormaPagamento.fpCartaoDebito).WithMessage("Não informados os dados do cartão de crédito / débito!");
        }
    }
}
