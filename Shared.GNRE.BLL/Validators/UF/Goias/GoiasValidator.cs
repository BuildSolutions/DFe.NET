using FluentValidation;

namespace GNRE.BLL.Validators.UF.Goias
{
    public class GoiasValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public GoiasValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Goias100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Goias100102Validator());
            });
        }
    }
}
