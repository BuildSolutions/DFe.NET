using FluentValidation;

namespace GNRE.BLL.Validators.UF.Sergipe
{
    public class SergipeValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public SergipeValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Sergipe100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Sergipe100102Validator());
            });
        }
    }
}
