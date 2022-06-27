using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Amapa;

namespace GNRE.BLL.Validators.UF.Amapa
{
    public class Amapa100099Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Amapa100099Validator()
        {
            RuleFor(uf => uf.Destinatario).NotNull().WithMessage("Dados do destinatário não informado!")
                .DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraAP.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraAP.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
