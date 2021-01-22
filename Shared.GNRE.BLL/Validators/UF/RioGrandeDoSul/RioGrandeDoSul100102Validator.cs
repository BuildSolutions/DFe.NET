using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.RioGrandeDoSul;

namespace GNRE.BLL.Validators.UF.RioGrandeDoSul
{
    public class RioGrandeDoSul100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RioGrandeDoSul100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(uf => uf.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Length(44).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.ChaveNFe.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraRS.ChaveAcessoNFeCTe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraRS.ChaveAcessoNFeCTe.Descricao()} inválida"));
        }
    }
}
