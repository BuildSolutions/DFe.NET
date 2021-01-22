using FluentValidation;

namespace GNRE.BLL.Validators.UF.RioDeJaneiro
{
    public class RioDeJaneiroValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RioDeJaneiroValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new RioDeJaneiro100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new RioDeJaneiro100102Validator());
            });
        }
    }
}
