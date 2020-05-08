using FluentValidation;
using NFe.Utils;
using System.IO;

namespace NFe.BLL.Validators
{
    public class ConfiguracaoServicoValidator : AbstractValidator<ConfiguracaoServico>
    {
        public ConfiguracaoServicoValidator()
        {
            RuleFor(cfg => cfg.Certificado).NotNull().WithMessage("Certificado digital não informado!");
            RuleFor(cfg => cfg.Certificado.Serial).NotEmpty().WithMessage("Chave do Certificado digital não informado!");
            RuleFor(cfg => cfg.DiretorioSalvarXml).NotEmpty().WithMessage("Diretório de Arquivos XML não informado!");
            RuleFor(cfg => cfg.DiretorioSalvarXml).Must(Directory.Exists).WithMessage(cfg => $"Diretório de Arquivos XML não existe![{cfg}]");
            RuleFor(cfg => cfg.DiretorioSchemas).NotEmpty().WithMessage("Diretório de Schemas XML não informado!");
            RuleFor(cfg => cfg.DiretorioSchemas).Must(Directory.Exists).WithMessage(cfg => $"Diretório de Schemas XML não existe![{cfg}]");
        }
    }
}
