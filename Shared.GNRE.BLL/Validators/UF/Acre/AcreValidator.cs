using FluentValidation;

namespace GNRE.BLL.Validators.UF.Acre
{
    public class AcreValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public AcreValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Acre100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Acre100102Validator());
            });
        }
    }
}
