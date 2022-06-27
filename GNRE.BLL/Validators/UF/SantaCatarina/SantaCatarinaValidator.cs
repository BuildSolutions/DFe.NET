using FluentValidation;

namespace GNRE.BLL.Validators.UF.SantaCatarina
{
    public class SantaCatarinaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public SantaCatarinaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new SantaCatarina100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new SantaCatarina100102Validator());
            });
        }
    }
}
