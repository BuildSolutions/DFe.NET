using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.Rondonia;

namespace GNRE.BLL.Validators.UF.Rondonia
{
    public class Rondonia100102Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public Rondonia100102Validator()
        {
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Must(uf => UFValidator.IsNumber(uf)).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.NotaFiscal.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraRO.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraRO.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
