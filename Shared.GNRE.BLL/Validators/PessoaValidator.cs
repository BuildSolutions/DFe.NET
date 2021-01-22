using DFe.Utils.Extensoes;
using FluentValidation;
using GNRE.BLL.Configuracao.ValueObjects;

namespace GNRE.BLL.Validators
{
    public class PessoaValidator : AbstractValidator<Pessoa>
    {
        public PessoaValidator()
        {
            RuleFor(pessoa => pessoa.ETipoDocumento).Must(tipo => tipo.EValido()).WithMessage($"Emitente tipo documento é inválido!");
            RuleFor(pessoa => pessoa.NomeRazaoSocial).NotEmpty().WithMessage($"Emitente nome não informado!");
            RuleFor(pessoa => pessoa.Documento).Length(14).When(pessoa => pessoa.ETipoDocumento == Enums.ETipoDocumentoPessoa.CNPJ).WithMessage("Emitente CNPJ não informado!");
            RuleFor(pessoa => pessoa.Endereco).SetValidator(new EnderecoValidator()).When(pessoa => pessoa.Endereco != null && pessoa.ETipoDocumento == Enums.ETipoDocumentoPessoa.CNPJ);
        }
    }
}
