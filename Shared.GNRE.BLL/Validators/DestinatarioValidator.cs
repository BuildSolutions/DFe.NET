using FluentValidation;
using GNRE.BLL.Configuracao.Entidades;

namespace GNRE.BLL.Validators
{
    public class DestinatarioValidator : AbstractValidator<Destinatario>
    {
        public DestinatarioValidator()
        {
            RuleFor(destinatario => destinatario.Documento).NotNull().WithMessage("Documento do destinatário não informado!");
            RuleFor(destinatario => destinatario.NomeRazaoSocial).NotNull().WithMessage("Nome/Razão Social do destinatário não informado!");
            RuleFor(destinatario => destinatario.MunicipioCodigoIBGE).NotNull().WithMessage("Código IBGE do município do destinatário não informado!")
                .DependentRules(() => RuleFor(destinatario => destinatario.MunicipioCodigoIBGE).Length(5).WithMessage("Código IBGE do município do destinatário deve conter 5 caracteres!"));
        }
    }
}
