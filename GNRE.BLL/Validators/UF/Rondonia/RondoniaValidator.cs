using FluentValidation;

namespace GNRE.BLL.Validators.UF.Rondonia
{
    public class RondoniaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public RondoniaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Rondonia100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Rondonia100102Validator());
            });
        }
    }
}
