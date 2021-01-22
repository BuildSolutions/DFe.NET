using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using GNRE.BLL.Validators.UF;
using GNRE.BLL.Validators.UF.Acre;

namespace GNRE.BLL.Validators
{
    public class GNREValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public GNREValidator()
        {
            RuleFor(uf => uf.Versao).NotNull().WithMessage("Versão da GNRE não informada!");
            RuleFor(uf => uf.UF).NotNull().WithMessage("UF não informada!")
                .DependentRules(() => RuleFor(uf => uf).SetValidator(new UFValidator()));
        }
    }
}
