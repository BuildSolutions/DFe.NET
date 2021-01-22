using FluentValidation;

namespace GNRE.BLL.Validators.UF.RioGrandeDoSul
{
    public class RioGrandeDoSulValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RioGrandeDoSulValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new RioGrandeDoSul100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new RioGrandeDoSul100102Validator());
            });
        }
    }
}
