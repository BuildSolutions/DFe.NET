using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators
{
    public class DestinatarioValidator : AbstractValidator<Destinatario>
    {
        public DestinatarioValidator()
        {
            RuleFor(destinatario => destinatario.SuframaNumero).NotEmpty().When(destinatario => destinatario.IsSuframa).WithMessage($"Destinatário informado com suframa e código suframa não informado!");
            RuleFor(destinatario => destinatario.EConsumidorFinal).Must(consumidorFinal => consumidorFinal.EValido()).WithMessage($"Destinatário (Enum)Consumidor Final é inválido!");
            RuleFor(destinatario => destinatario.Pessoa).NotNull().WithMessage($"Destinatário dados não informado!").DependentRules(() =>
             {
                 RuleFor(destinatario => destinatario.Pessoa).SetValidator(new PessoaValidator(Enums.EPessoa.Destinatario));
                 RuleFor(destinatario => destinatario.EConsumidorFinal).Equal(ConsumidorFinal.cfConsumidorFinal).When(destinatario => destinatario.Pessoa.InscricaoEstadualIsento).WithMessage($"Destinatário com IE isenta não foi informado como consumidor final!");
             });
        }
    }
}
