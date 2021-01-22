using FluentValidation;

namespace GNRE.BLL.Validators.UF.Amapa
{
    public class AmapaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public AmapaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Amapa100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Amapa100102Validator());
            });
        }
    }
}
