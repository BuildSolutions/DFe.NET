using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.RioGrandeDoNorte;

namespace GNRE.BLL.Validators.UF.RioGrandeDoNorte
{
    public class RioGrandeDoNorte100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RioGrandeDoNorte100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraRN.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraRN.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
