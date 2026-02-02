using FluentValidation;
using NFe.BLL.Configuracao.Entidades.Produtos;
using NFe.BLL.Validators.Produtos.Impostos;

namespace NFe.BLL.Validators.Produtos
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator(bool deveValidarQuantidadeEValores)
        {
            RuleFor(produto => produto.Referencia).NotEmpty().WithMessage(produto => $"Referência do produto {produto.Descricao} não informado!");
            RuleFor(produto => produto.Descricao).NotEmpty().WithMessage(produto => $"Descrição não informado!");
            RuleFor(produto => produto.NCM).NotEmpty().WithMessage(produto => $"NCM do produto {produto.Descricao} não informado!");
            RuleFor(produto => produto.NCM).Matches(@"^\d{8}$").WithMessage(produto => $"NCM do produto {produto.Descricao} é inválido!");
            //RuleFor(produto => produto.CEST).NotEmpty().WithMessage($"CEST do NCM não informado!");
            //RuleFor(produto => produto.CEST).Matches(@"^\d{7}$").WithMessage($"CEST do NCM é inválido!");
            RuleFor(produto => produto.CFOP).NotEmpty().WithMessage(produto => $"CFOP do produto {produto.Descricao} não informado!");
            RuleFor(produto => produto.UnidadeCompra).NotEmpty().WithMessage(produto => $"Unidade de Medida do produto {produto.Descricao}  não informada!");
            RuleFor(produto => produto.UnidadeTributacao).MaximumLength(6).WithMessage(produto => $"Unidade de Tributação é inválida do produto {produto.Descricao} não informado!");
            RuleFor(produto => produto.Quantidade).GreaterThan(0).When(produto => deveValidarQuantidadeEValores).WithMessage(produto => $"Quantidade do produto {produto.Descricao} informada é inválida!");
            RuleFor(produto => produto.ValorUnitario).GreaterThan(0).When(produto => deveValidarQuantidadeEValores).WithMessage(produto => $"Valor unitário do produto {produto.Descricao} informado é inválido!");
            RuleFor(produto => produto.Impostos).NotNull().WithMessage(produto => $"Imposto do produto {produto.Descricao} não informado!").DependentRules(() =>
            {
                RuleFor(nfe => nfe.Impostos).SetValidator(new ImpostoValidator()).WithMessage((_, imposto) => $"Dados inválidos do Imposto {imposto}");
            });
        }
    }
}
