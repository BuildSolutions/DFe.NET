using FluentValidation;

namespace GNRE.BLL.Validators.UF.Tocantins
{
    public class TocantinsValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public TocantinsValidator()
        {
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Tocantins100099Validator());
            });
            When(uf => uf.Receita == Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao, () =>
            {
                RuleFor(uf => uf).SetValidator(new Tocantins100102Validator());
            });
        }
    }
}
