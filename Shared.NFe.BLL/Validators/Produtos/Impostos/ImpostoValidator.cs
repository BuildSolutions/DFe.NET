using DFe.Utils.Extensoes;
using FluentValidation;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using NFe.BLL.Validators.Produtos.Impostos.ICMS;

namespace NFe.BLL.Validators.Produtos.Impostos
{
    public class ImpostoValidator : AbstractValidator<Imposto>
    {
        public ImpostoValidator()
        {
            RuleFor(imposto => imposto.ICMS).NotNull().WithMessage("Imposto ICMS não informado!");
            When(cst => cst.ICMS.CST.EValido(), () =>
            {
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMS00Validator()).When(icms => icms.ICMS.CST == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csticms.Cst00).WithMessage("ICMS 00 Inválido");
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMS10Validator()).When(icms => icms.ICMS.CST == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csticms.Cst10).WithMessage("ICMS 10 Inválido");
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMS20Validator()).When(icms => icms.ICMS.CST == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csticms.Cst20).WithMessage("ICMS 20 Inválido");
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMS30Validator()).When(icms => icms.ICMS.CST == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csticms.Cst30).WithMessage("ICMS 30 Inválido");
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMS70Validator()).When(icms => icms.ICMS.CST == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csticms.Cst70).WithMessage("ICMS 70 Inválido");
                //RuleFor(imposto => imposto.ICMS).SetValidator(new ICMS90Validator()).When(icms => icms.ICMS.CST == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csticms.Cst90).WithMessage("ICMS 90 Inválido");
            });

            RuleFor(imposto => imposto.ICMS).NotNull().WithMessage("Imposto ICMS não informado!");
            When(cst => cst.ICMS.CSOSN.EValido(), () =>
            {
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMSSN101Validator()).When(icms => icms.ICMS.CSOSN == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csosnicms.Csosn101).WithMessage("ICMS 101 Inválido");
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMSSN201Validator()).When(icms => icms.ICMS.CSOSN == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csosnicms.Csosn201).WithMessage("ICMS 201 Inválido");
                RuleFor(imposto => imposto.ICMS).SetValidator(new ICMSSN202Validator()).When(icms => icms.ICMS.CSOSN == Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.Csosnicms.Csosn202).WithMessage("ICMS 202 Inválido");
            });

            RuleFor(imposto => imposto.PIS).NotNull().WithMessage("Imposto PIS não informado!").DependentRules(() =>
            {
                RuleFor(imposto => imposto.PIS).SetValidator(new PISValidator());
            });

            RuleFor(imposto => imposto.COFINS).NotNull().WithMessage("Imposto COFINS não informado!").DependentRules(() =>
            {
                RuleFor(imposto => imposto.COFINS).SetValidator(new COFINSValidator());
            });

            //RuleFor(imposto => imposto.IPI).NotNull().WithMessage("Imposto IPI não informado!").DependentRules(() =>
            When(imposto => imposto.IPI != null, () => 
            {
                RuleFor(imposto => imposto.IPI).SetValidator(new IPIValidator());
            });
        }
    }
}
