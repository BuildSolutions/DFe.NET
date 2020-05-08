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
            RuleFor(pag => pag.Valor).GreaterThan(0).WithMessage(pag =>  $"Valor inválido da Forma de Pagamento: {pag.FormaPagamento.ToString()}.");
        }
    }
}
