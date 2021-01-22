using FluentValidation;

namespace GNRE.BLL.Validators.UF.Alagoas
{
    public class AlagoasValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public AlagoasValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Alagoas100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Alagoas100102Validator());
            });
        }
    }
}
