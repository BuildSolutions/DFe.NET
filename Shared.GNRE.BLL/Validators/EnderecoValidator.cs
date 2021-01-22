using DFe.Utils.Extensoes;
using FluentValidation;
using GNRE.BLL.Configuracao.ValueObjects;

namespace GNRE.BLL.Validators
{
    public class EnderecoValidator : AbstractValidator<Endereco>
    {
        public EnderecoValidator()
        {
            RuleFor(endereco => endereco.EnderecoCompleto).NotEmpty().WithMessage("Emitente endereço não informado!");
            RuleFor(endereco => endereco.MunicipioCodigoIBGE).Length(5).WithMessage("Emitente código IBGE é inválido!");
            RuleFor(endereco => endereco.MunicipioNome).NotEmpty().WithMessage("Emitente município não informado!");
            RuleFor(endereco => endereco.MunicipioEstadoSigla).Must(uf => uf.EValido()).WithMessage("Emitente estado não informado!");
        }
    }
}
