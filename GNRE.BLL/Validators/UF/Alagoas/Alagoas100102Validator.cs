using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Alagoas;

namespace GNRE.BLL.Validators.UF.Alagoas
{
    public class Alagoas100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Alagoas100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
        }
    }
}
