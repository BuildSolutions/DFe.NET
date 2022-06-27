using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Ceara;
using GNRE.BLL.Validators.UF;

namespace GNRE.BLL.Validators.UF.Ceara
{
    public class Ceara100099Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Ceara100099Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.Produto).GreaterThan(0).WithMessage("Produto não informado!");
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraCE.DataSaidaMercadoria.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Must(uf => UFValidator.IsDateTime(uf)).WithMessage($"Campos Extras: {ETipoCampoExtraCE.DataSaidaMercadoria.Descricao()} inválida"));
        }
    }
}
