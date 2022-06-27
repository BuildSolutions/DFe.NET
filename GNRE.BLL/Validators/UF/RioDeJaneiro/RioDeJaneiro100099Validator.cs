using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.RioDeJaneiro;

namespace GNRE.BLL.Validators.UF.RioDeJaneiro
{
    public class RioDeJaneiro100099Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RioDeJaneiro100099Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Length(44).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.ChaveDFe.Descricao()} é inválida"));
            RuleFor(uf => uf.Produto).GreaterThan(0).WithMessage("Produto não informado!");
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraRJ.DataEmissao.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Must(uf => UFValidator.IsDateTime(uf)).WithMessage($"Campos Extras: {ETipoCampoExtraRJ.DataEmissao.Descricao()} inválida"));
        }
    }
}
