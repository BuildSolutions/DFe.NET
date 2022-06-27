using FluentValidation;

namespace GNRE.BLL.Validators.UF.RioGrandeDoNorte
{
    public class RioGrandeDoNorteValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RioGrandeDoNorteValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new RioGrandeDoNorte100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new RioGrandeDoNorte100102Validator());
            });
        }
    }
}
