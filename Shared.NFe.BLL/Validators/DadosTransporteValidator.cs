using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators
{
    public class DadosTransporteValidator : AbstractValidator<Transporte>
    {
        public DadosTransporteValidator()
        {
            RuleFor(transp => transp.ModalidadeFrete).Must(modalidade => modalidade.EValido()).WithMessage($"Modalidade de Frete é inválida!");
            //RuleFor(transp => transp.QuantidadeVolumes).GreaterThan(0).When(transp => transp.QuantidadeVolumes != null).WithMessage($"Quantidade de volumes é inválida!");
            //RuleFor(transp => transp.PesoBruto).GreaterThan(0).When(transp => transp.PesoBruto != null).WithMessage($"Peso Bruto é inválido!");
            //RuleFor(transp => transp.PesoLiquido).GreaterThan(0).When(transp => transp.PesoLiquido != null).WithMessage($"Peso Líquido é inválido!");
        }
    }
}
