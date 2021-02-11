using FluentValidation;

namespace GNRE.BLL.Validators.UF.Piaui
{
    public class PiauiValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public PiauiValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Piaui100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Piaui100102Validator());
            });
        }
    }
}
