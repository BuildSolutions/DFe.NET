using FluentValidation;

namespace GNRE.BLL.Validators.UF.Pernambuco
{
    public class PernambucoValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public PernambucoValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Pernambuco100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Pernambuco100102Validator());
            });
        }
    }
}
