using FluentValidation;

namespace GNRE.BLL.Validators.UF.DistritoFederal
{
    public class DistritoFederalValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public DistritoFederalValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new DistritoFederal100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new DistritoFederal100102Validator());
            });
        }
    }
}
