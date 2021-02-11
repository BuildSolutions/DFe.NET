using FluentValidation;

namespace GNRE.BLL.Validators.UF.MatoGrosso
{
    public class MatoGrossoValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public MatoGrossoValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new MatoGrosso100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new MatoGrosso100102Validator());
            });
        }
    }
}
