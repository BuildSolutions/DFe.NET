using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Ceara;

namespace GNRE.BLL.Validators.UF.Ceara
{
    public class Ceara100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Ceara100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraCE.DataSaidaMercadoria.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Must(uf => UFValidator.IsDateTime(uf)).WithMessage($"Campos Extras: {ETipoCampoExtraCE.DataSaidaMercadoria.Descricao()} inválida"));
        }
    }
}
