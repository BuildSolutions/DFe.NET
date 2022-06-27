using FluentValidation;

namespace GNRE.BLL.Validators.UF.Bahia
{
    public class BahiaValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public BahiaValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Bahia100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Bahia100102Validator());
            });
        }
    }
}
