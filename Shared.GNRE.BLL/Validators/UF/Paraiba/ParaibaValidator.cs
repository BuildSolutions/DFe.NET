using FluentValidation;

namespace GNRE.BLL.Validators.UF.Paraiba
{
    public class ParaibaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public ParaibaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Paraiba100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Paraiba100102Validator());
            });
        }
    }
}
