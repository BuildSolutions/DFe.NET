using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.BLL.Validators
{
    public class ConfiguracaoCscValidator : AbstractValidator<ConfiguracaoCsc>
    {
        public ConfiguracaoCscValidator()
        {
            RuleFor(cfg => cfg.CIdToken).NotEmpty().WithMessage("Id do Token CSC Não Informado!");
            RuleFor(cfg => cfg.Csc).NotEmpty().WithMessage("CSC não Informado!");
        }
    }
}
