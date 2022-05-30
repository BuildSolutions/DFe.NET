using DFe.Utils.Extensoes;
using FluentValidation;
using NFe.BLL.Configuracao.Entidades;

namespace NFe.BLL.Validators
{
    public class SuframaValidator : AbstractValidator<Suframa>
    {
        public SuframaValidator()
        {
            RuleFor(suframa => suframa.NumeroLoteSuframa).GreaterThan(0).WithMessage("Número do Lote Suframa é inválido!");
            RuleFor(suframa => suframa.UfDestino).Must(suframa => suframa.EValido()).WithMessage("UF de destino é inválida!");
            RuleFor(suframa => suframa.UfOrigem).Must(suframa => suframa.EValido()).WithMessage("UF de origem é inválida!");
            RuleFor(suframa => suframa.DestinatarioCNPJ).Length(14).WithMessage("O CNPJ do Destinaário é inválido!");
            RuleFor(suframa => suframa.DestinatarioInscricaoSuframa).NotEmpty().WithMessage("Inscrição Suframa do Destinaário é inválido!");
            RuleFor(suframa => suframa.ChavesAcessoNFes).NotNull().WithMessage("Chaves de Acesso das NFes não informado!").DependentRules(() =>
            {
                RuleForEach(suframa => suframa.ChavesAcessoNFes).Length(44).WithMessage((_, chaveAcesso) => $"Dados inválidos da Chave de Acesso: {chaveAcesso}");
            });
        }
    }
}
