using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Bahia;

namespace GNRE.BLL.Validators.UF.Bahia
{
    public class Bahia100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Bahia100102Validator()
        {
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraBA.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraBA.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
