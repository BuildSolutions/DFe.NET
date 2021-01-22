using FluentValidation;

namespace GNRE.BLL.Validators.UF.Roraima
{
    public class RoraimaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RoraimaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Roraima100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Roraima100102Validator());
            });
        }
    }
}
