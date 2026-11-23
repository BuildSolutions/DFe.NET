using FluentValidation;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;
using DFe.Utils.Extensoes;
using DFe.Classes.Entidades;

namespace NFe.BLL.Validators
{
    public class PessoaValidator : AbstractValidator<Pessoa>
    {
        /// <summary>
        /// <paramref name="isNFCe"/> dispensa a IE do destinatario: na NFC-e (modelo 65) ela nao e
        /// exigida - o mapeamento manda indIEDest = 9 (nao contribuinte) e omite a IE. Antes disso a
        /// regra inferia "e NFC-e" pela AUSENCIA de endereco, e passou a barrar emissao legitima quando a
        /// NFC-e para CNPJ comecou a levar dest/enderDest (rejeicao 181, NT 2026.007).
        /// </summary>
        public PessoaValidator(EPessoa ePessoa, bool isNFCe = false)
        {
            RuleFor(pessoa => pessoa.PessoaTipo).Must(tipo => tipo.EValido()).WithMessage($"{ePessoa} tipo pessoa é inválido!");
            RuleFor(pessoa => pessoa.NomeRazaoSocial).NotEmpty().WithMessage($"{ePessoa} nome não informado!");
            RuleFor(pessoa => pessoa.RGInscricaoEstadual).NotEmpty().When(pessoa => !_isEstrangeiro(pessoa.Endereco?.MunicipioEstadoSigla) && !_isNFCe(pessoa.Endereco) && !isNFCe && pessoa.PessoaTipo == ETipoPessoa.Juridica).WithMessage($"{ePessoa} IE não informado!");
            RuleFor(pessoa => pessoa.CPFCNPJ).Length(14).When(pessoa => !_isEstrangeiro(pessoa.Endereco?.MunicipioEstadoSigla) && pessoa.PessoaTipo == ETipoPessoa.Juridica).WithMessage($"{ePessoa} CNPJ é inválido!");
            RuleFor(pessoa => pessoa.CPFCNPJ).Length(11).When(pessoa => !_isEstrangeiro(pessoa.Endereco?.MunicipioEstadoSigla) && pessoa.PessoaTipo == ETipoPessoa.Fisica).WithMessage($"{ePessoa} CPF é inválido!");
            //RuleFor(pessoa => pessoa.Endereco).NotNull().When.WithMessage($"{ePessoa.ToString()} Endereço não informado!").DependentRules(() =>
            //{
                RuleFor(pessoa => pessoa.Endereco).SetValidator(new EnderecoValidator(ePessoa)).When(pessoa => pessoa.Endereco != null);
            //});
        }

        private bool _isEstrangeiro(Estado? uf)
        {
            return uf != null && uf == Estado.EX;
        }

        private bool _isNFCe(Endereco endereco)
        {
            return endereco == null;
        }
    }
}
