using FluentValidation;

namespace GNRE.BLL.Validators.UF.Amazonas
{
    public class AmazonasValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public AmazonasValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Amazonas100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Amazonas100102Validator());
            });
        }
    }
}
