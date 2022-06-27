using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Amazonas;

namespace GNRE.BLL.Validators.UF.Amazonas
{
    public class Amazonas100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Amazonas100102Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Length(44).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.ChaveNFe.Descricao()} é inválida"));
            RuleFor(uf => uf.Produto).GreaterThan(0).WithMessage("Produto não informado!");
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraAM.ChaveAcessoNFeCTe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraAM.ChaveAcessoNFeCTe.Descricao()} inválida"));
        }
    }
}
