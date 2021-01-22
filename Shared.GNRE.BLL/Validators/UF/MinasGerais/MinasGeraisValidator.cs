using FluentValidation;

namespace GNRE.BLL.Validators.UF.MinasGerais
{
    public class MinasGeraisValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public MinasGeraisValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new MinasGerais100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new MinasGerais100102Validator());
            });
        }
    }
}
