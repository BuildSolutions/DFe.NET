using System.IO;
using DFe.Utils.Extensoes;
using FluentValidation;
using NFe.BLL.Configuracao.Entidades;

namespace NFe.BLL.Validators
{
    public class EmitenteValidator : AbstractValidator<Emitente>
    {
        public EmitenteValidator()
        {
            RuleFor(emit => emit.ECRT).Must(crt => crt.EValido()).WithMessage("Emitente Regime Tributário é inválido!");
            RuleFor(emit => emit.Certificado).NotNull().WithMessage("Certificado Digital do emitente não informado!");
            RuleFor(emit => emit.Certificado.Serial).NotEmpty().When(customer => customer.Certificado != null).WithMessage("Certificado Digital do emitente não informado!");
            RuleFor(emit => emit.DiretorioSalvarXML).NotEmpty().WithMessage("Diretório para salvar arquivo xml não informado!").DependentRules(() =>
            {
                RuleFor(cfg => cfg.DiretorioSalvarXML).Must(emit => Directory.Exists(emit)).WithMessage(cfg => $"Diretório de Arquivos XML não existe![{cfg}]");
            });
            RuleFor(emit => emit.DiretorioSchemas).NotEmpty().WithMessage("Diretório dos schemas não informado!").DependentRules(() =>
            {
                RuleFor(cfg => cfg.DiretorioSchemas).Must(emit => Directory.Exists(emit)).WithMessage(cfg => $"Diretório dos schemas não existe![{cfg}]");
            });
            RuleFor(emit => emit.Pessoa).NotNull().WithMessage("Emitente dados não informado!").DependentRules(() =>
            {
                RuleFor(emit => emit.Pessoa).SetValidator(new PessoaValidator(Enums.EPessoa.Emitente));
            });
        }
    }
}
