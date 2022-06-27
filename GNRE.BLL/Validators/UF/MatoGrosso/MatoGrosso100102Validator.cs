using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.MatoGrosso;

namespace GNRE.BLL.Validators.UF.MatoGrosso
{
    public class MatoGrosso100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public MatoGrosso100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.DetalhamentoReceita).GreaterThan(0).WithMessage("Detalhamento da Receita não informado!");
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraMT.ChaveAcessoNFeCTe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraMT.ChaveAcessoNFeCTe.Descricao()} inválida"));
        }
    }
}
