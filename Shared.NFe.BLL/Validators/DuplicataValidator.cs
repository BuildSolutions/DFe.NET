using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using System;

namespace NFe.BLL.Validators
{
    public class DuplicataValidator : AbstractValidator<Duplicata>
    {
        public DuplicataValidator()
        {
            RuleFor(dup => dup.Codigo).NotEmpty().WithMessage($"Duplicata da NF-e é inválida!");
            RuleFor(dup => dup.DataVencimento.Date).GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage(dup => $"Data de Vencimento da Duplicata: {dup.Codigo} é menor do que a Data Atual do Servidor: {DateTime.Now:dd/MM/yyyy}");
        }
    }
}
