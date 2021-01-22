using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Paraiba;

namespace GNRE.BLL.Validators.UF.Paraiba
{
    public class Paraiba100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Paraiba100102Validator()
        {
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraPB.ChaveAcessoNFeCTe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraPB.ChaveAcessoNFeCTe.Descricao()} inválida"));
        }
    }
}
