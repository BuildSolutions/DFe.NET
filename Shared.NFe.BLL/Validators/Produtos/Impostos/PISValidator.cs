using FluentValidation;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos
{
    public class PISValidator : AbstractValidator<PIS>
    {
        public PISValidator()
        {
            RuleFor(pis => pis.CST).Must(cst => cst.EValido()).WithMessage($"CST do PIS é inválido!");
            RuleFor(pis => pis.BaseCalculo).GreaterThan(0).When(pis => PIS.PisCstTributadaPorAliquota.Contains(pis.CST)).WithMessage($"Base de Cálculo do PIS é inválido!");
            RuleFor(pis => pis.ValorTotal).GreaterThan(0).When(pis => PIS.PisCstTributadaPorAliquota.Contains(pis.CST)).WithMessage($"Valor do PIS é inválido!");
            RuleFor(pis => pis.Aliquota).GreaterThan(0).When(pis => PIS.PisCstTributadaPorAliquota.Contains(pis.CST)).WithMessage($"Alíquota do PIS é inválida!");
        }
    }
}
