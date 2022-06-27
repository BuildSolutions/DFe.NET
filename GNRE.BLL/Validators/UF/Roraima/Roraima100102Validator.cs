using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Roraima;

namespace GNRE.BLL.Validators.UF.Roraima
{
    public class Roraima100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Roraima100102Validator()
        {
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraRR.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraRR.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
