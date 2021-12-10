using FluentValidation;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos.ICMS
{
    public class ICMSSN101Validator : AbstractValidator<Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS>
    {
        public ICMSSN101Validator()
        {
            //RuleFor(icms => icms.CSOSN).Must(cst => cst.EValido()).WithMessage("CSOSN do ICMS é inválido!");
            //RuleFor(icms => icms.RepasseCreditoAliquota).GreaterThan(0).WithMessage("Alíquota de Repasse do ICMS é inválida!");
            //RuleFor(icms => icms.RepasseCreditoValor).GreaterThan(0).WithMessage("Valor de Repasse do ICMS é inválido!");
        }
    }
}
