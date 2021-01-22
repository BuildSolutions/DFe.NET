using FluentValidation;

namespace GNRE.BLL.Validators.UF.Ceara
{
    public class CearaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public CearaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Ceara100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Ceara100102Validator());
            });
        }
    }
}
