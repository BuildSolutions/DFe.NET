using FluentValidation;

namespace GNRE.BLL.Validators.UF.Maranhao
{
    public class MaranhaoValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public MaranhaoValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Maranhao100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Maranhao100102Validator());
            });
        }
    }
}
