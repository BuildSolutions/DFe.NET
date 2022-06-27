using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Parana;

namespace GNRE.BLL.Validators.UF.Parana
{
    public class Parana100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Parana100102Validator()
        {
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraPR.ChaveAcessoNFeCTe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraPR.ChaveAcessoNFeCTe.Descricao()} inválida"));
        }
    }
}
