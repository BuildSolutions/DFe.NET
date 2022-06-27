using DFe.Utils;
using FluentValidation;
using GNRE.BLL.Configuracao.Entidades.GNREUF.SantaCatarina;

namespace GNRE.BLL.Validators.UF.SantaCatarina
{
    public class SantaCatarina100099Validator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public SantaCatarina100099Validator()
        {
            RuleFor(uf => uf.TipoDocumento).NotNull().WithMessage("Tipo de Documento não informado!")
                .DependentRules(() => RuleFor(uf => uf.DocumentoNumero).Length(44).WithMessage($"Número do Documento: {Classes.Enumerators.ETipoDocumento.ChaveDFe.Descricao()} é inválida"));
            RuleFor(uf => uf.camposExtras).NotNull().WithMessage($"Campos Extras: {ETipoCampoExtraSC.ChaveAcessoNFe.Descricao()} não informado!")
                .DependentRules(() => RuleFor(uf => uf.camposExtras[0].Valor).Length(44).WithMessage($"Campos Extras: {ETipoCampoExtraSC.ChaveAcessoNFe.Descricao()} inválida"));
        }
    }
}
