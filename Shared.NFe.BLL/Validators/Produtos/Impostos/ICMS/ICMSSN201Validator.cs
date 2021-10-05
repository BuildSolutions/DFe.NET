using FluentValidation;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos.ICMS
{
    public class ICMSSN201Validator : AbstractValidator<Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS>
    {
        public ICMSSN201Validator()
        {
            //RuleFor(icms => icms.CSOSN).Must(cst => cst.EValido()).WithMessage("CSOSN do ICMS é inválido!");
            RuleFor(icms => icms.BaseCalculoST).GreaterThan(0).WithMessage("Base de Cálculo do ICMS-ST é inválida!");
            RuleFor(icms => icms.AliquotaST).GreaterThan(0).WithMessage("Alíquota do ICMS-ST é inválida!");
            RuleFor(icms => icms.ValorTotalST).GreaterThan(0).WithMessage("Valor Total do ICMS-ST é inválido!");

            RuleFor(icms => icms.RepasseCreditoAliquota).GreaterThan(0).WithMessage("Alíquota de Repasse do ICMS é inválida!");
            RuleFor(icms => icms.RepasseCreditoValor).GreaterThan(0).WithMessage("Valor de Repasse do ICMS é inválido!");
        }
    }
}
