using FluentValidation;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos.ICMS
{
    public class ICMS10Validator : AbstractValidator<Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS>
    {
        public ICMS10Validator()
        {
            //RuleFor(icms => icms.CST).Must(cst => cst.EValido()).WithMessage("CST do ICMS é inválido!");
            RuleFor(icms => icms.BaseCalculo).GreaterThan(0).WithMessage("Base de Cálculo do ICMS é inválida!");
            RuleFor(icms => icms.Aliquota).GreaterThan(0).WithMessage("Alíquota do ICMS é inválida!");
            RuleFor(icms => icms.ValorTotal).GreaterThan(0).WithMessage("Valor Total do ICMS é inválido!");

            RuleFor(icms => icms.BaseCalculoST).GreaterThan(0).WithMessage("Base de Cálculo do ICMS-ST é inválida!");
            RuleFor(icms => icms.AliquotaST).GreaterThan(0).WithMessage("Alíquota do ICMS-ST é inválida!");
            RuleFor(icms => icms.ValorTotalST).GreaterThan(0).WithMessage("Valor Total do ICMS-ST é inválido!");
        }
    }
}
