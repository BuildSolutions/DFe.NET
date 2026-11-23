using DFe.Utils.Extensoes;
using FluentValidation;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;

namespace NFe.BLL.Validators
{
    public class EnderecoValidator : AbstractValidator<Endereco>
    {
        public EnderecoValidator(EPessoa ePessoa)
        {
            RuleFor(endereco => endereco.Logradouro).NotEmpty().WithMessage($"{ePessoa} logradouro não informado!");
            RuleFor(endereco => endereco.Numero).NotEmpty().When(_ => ePessoa == EPessoa.Destinatario).WithMessage($"{ePessoa} número não informado!");
            RuleFor(endereco => endereco.Bairro).NotEmpty().WithMessage($"{ePessoa} bairro não informado!");
            // CEP do DESTINATARIO e opcional no leiaute (TEndereco: CEP com minOccurs="0"; a obrigatoria
            // e a do emitente, em TEnderEmi), entao aqui so se confere o formato quando ele vem. Exigir
            // sempre barrava NFC-e com CNPJ cujo cadastro nao tem CEP - e na NFC-e o grupo dest/enderDest
            // passou a ser obrigatorio por causa da rejeicao 181 (NT 2026.007).
            RuleFor(endereco => endereco.CEP.RetornaNumeros()).Length(8).When(endereco => ePessoa != EPessoa.Destinatario || !string.IsNullOrEmpty(endereco.CEP)).WithMessage($"{ePessoa} cep é inválido!");
            RuleFor(endereco => endereco.MunicipioCodigoIBGE).Must(x => x > 999999 && x < 10000000).WithMessage($"{ePessoa} código IBGE é inválido!");
            RuleFor(endereco => endereco.MunicipioNome).NotEmpty().WithMessage($"{ePessoa} município não informado!");
            RuleFor(endereco => endereco.MunicipioEstadoSigla.GetValueOrDefault()).Must(uf => uf.EValido()).When(_ => ePessoa != EPessoa.Transportadora).WithMessage($"{ePessoa} estado não informado!");
            RuleFor(endereco => endereco.PaisNome).NotEmpty().WithMessage($"{ePessoa} nome do país não informado!");
            RuleFor(endereco => endereco.PaisCodigo).NotEmpty().WithMessage($"{ePessoa} código do país não informado!");
        }
    }
}
