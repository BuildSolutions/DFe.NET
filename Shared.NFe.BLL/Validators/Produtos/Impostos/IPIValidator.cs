using FluentValidation;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators.Produtos.Impostos
{
    public class IPIValidator : AbstractValidator<IPI>
    {
        public IPIValidator()
        {
            RuleFor(ipi => ipi.CST).Must(cst => cst.EValido()).WithMessage("CST do IPI é inválido!");
            RuleFor(ipi => ipi.BaseCalculo).GreaterThan(0).When(ipi => IPI.IpiCstTributada.Contains(ipi.CST)).WithMessage("Base de Cálculo do IPI é inválido!");
            RuleFor(ipi => ipi.ValorTotal).GreaterThan(0).When(ipi => IPI.IpiCstTributada.Contains(ipi.CST)).WithMessage("Valor do IPI é inválido!");
            RuleFor(ipi => ipi.Aliquota).GreaterThan(0).When(ipi => IPI.IpiCstTributada.Contains(ipi.CST)).WithMessage("Alíquota do IPI é inválida!");
            RuleFor(ipi => ipi.CodigoEnquadramento).GreaterThan(0).WithMessage("Alíquota do IPI é inválida!");
        }
    }
}
