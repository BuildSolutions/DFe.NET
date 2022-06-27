using DFe.Utils;
using FluentValidation;

namespace GNRE.BLL.Validators.UF.Piaui
{
    public class Piaui100099Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Piaui100099Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.Produto).GreaterThan(0).WithMessage("Produto não informado!");
        }
    }
}
