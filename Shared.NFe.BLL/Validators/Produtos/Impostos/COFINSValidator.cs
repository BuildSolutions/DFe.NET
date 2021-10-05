using FluentValidation;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos
{
    public class COFINSValidator : AbstractValidator<COFINS>
    {
        public COFINSValidator()
        {
            RuleFor(cofins => cofins.CST).Must(cst => cst.EValido()).WithMessage("CST do COFINS é inválido!");
            RuleFor(cofins => cofins.BaseCalculo).GreaterThan(0).When(cofins => COFINS.CofinsCstTributadaPorAliquota.Contains(cofins.CST)).WithMessage("Base de Cálculo do COFINS é inválido!");
            RuleFor(cofins => cofins.ValorTotal).GreaterThan(0).When(cofins => COFINS.CofinsCstTributadaPorAliquota.Contains(cofins.CST)).WithMessage("Valor do COFINS é inválido!");
            RuleFor(cofins => cofins.Aliquota).GreaterThan(0).When(cofins => COFINS.CofinsCstTributadaPorAliquota.Contains(cofins.CST)).WithMessage("Alíquota do COFINS é inválida!");
        }
    }
}