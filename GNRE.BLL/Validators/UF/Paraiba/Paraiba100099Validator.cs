using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Paraiba;

namespace GNRE.BLL.Validators.UF.Paraiba
{
    public class Paraiba100099Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Paraiba100099Validator()
        {
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraPB.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraPB.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
