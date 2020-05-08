using FluentValidation;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos.ICMS
{
    public class ICMS30Validator : AbstractValidator<Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS>
    {
        public ICMS30Validator()
        {
            RuleFor(icms => icms.CST).Must(cst => cst.EValido()).WithMessage($"CST do ICMS é inválido!");
            RuleFor(icms => icms.BaseCalculoST).GreaterThan(0).WithMessage($"Base de Cálculo do ICMS-ST é inválida!");
            RuleFor(icms => icms.AliquotaST).GreaterThan(0).WithMessage($"Alíquota do ICMS-ST é inválida!");
            RuleFor(icms => icms.ValorTotalST).GreaterThan(0).WithMessage($"Valor Total do ICMS-ST é inválido!");
        }
    }
}
