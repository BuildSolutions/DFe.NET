using FluentValidation;

namespace GNRE.BLL.Validators.UF.Para
{
    public class ParaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public ParaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Para100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Para100102Validator());
            });
        }
    }
}
