using DFe.Utils;
using FluentValidation;

namespace GNRE.BLL.Validators.UF.DistritoFederal
{
    public class DistritoFederal100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public DistritoFederal100102Validator()
        {
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
        }
    }
}
