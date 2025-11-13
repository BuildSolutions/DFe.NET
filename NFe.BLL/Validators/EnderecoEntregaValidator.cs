using FluentValidation;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;
using DFe.Utils.Extensoes;
using DFe.Classes.Entidades;
using NFe.BLL.Configuracao.Entidades;

namespace NFe.BLL.Validators
{
    public class EnderecoEntregaValidator : AbstractValidator<EnderecoEntrega>
    {
        public EnderecoEntregaValidator(EPessoa ePessoa)
        {
            RuleFor(pessoa => pessoa.CPFCNPJ).Length(14).When(pessoa => pessoa.CPFCNPJ.Length != 11).WithMessage($"Endereço de entrega - Campo CNPJ é inválido!");
            RuleFor(pessoa => pessoa.CPFCNPJ).Length(11).When(pessoa => pessoa.CPFCNPJ.Length != 14).WithMessage($"Endereço de entrega - Campo CPF é inválido!");
            //RuleFor(pessoa => pessoa.RGIE).When(pessoa => !string.IsNullOrEmpty(pessoa.CPFCNPJ) && pessoa.CPFCNPJ.Length == 14).WithMessage($"Endereço de entrega - Campo CNPJ é inválido!");
            RuleFor(pessoa => pessoa.Endereco).SetValidator(new EnderecoValidator(ePessoa)).When(pessoa => pessoa.Endereco != null);
        }
    }
}
