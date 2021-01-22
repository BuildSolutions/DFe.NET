using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Maranhao;

namespace GNRE.BLL.Validators.UF.Maranhao
{
    public class Maranhao100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Maranhao100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.Produto).GreaterThan(0).WithMessage("Produto não informado!");
            RuleFor(uf => uf.DetalhamentoReceita).GreaterThan(0).WithMessage("Detalhamento da Receita não informado!");
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraMA.ChaveAcessoNFeCTe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraMA.ChaveAcessoNFeCTe.Descricao()} inválida"));
        }
    }
}
