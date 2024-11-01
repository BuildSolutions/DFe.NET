using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using System;
using System.Linq;
using DFe.Utils.Extensoes;
using NFe.BLL.Validators.Produtos;

namespace NFe.BLL.Validators
{
    public class NotaFiscalValidator : AbstractValidator<NotaFiscal>
    {
        public NotaFiscalValidator(DFe.Classes.Flags.ModeloDocumento modeloDocumento)
        {
            RuleFor(nfe => nfe.Numero).GreaterThan(0).WithMessage("Numero da NFe não informado!");
            RuleFor(nfe => nfe.NaturezaOperacaoDescricao).NotNull().WithMessage("Operação Fiscal não informada!");
            RuleFor(nfe => nfe.ETipoNFe).Must(tipo => tipo.EValido()).WithMessage("Tipo da NFe é inválido!");
            RuleFor(nfe => nfe.EFinalidadeNFe).Must(finalidade => finalidade.EValido()).WithMessage("Finalidade da NFe é inválido!");
            RuleFor(nfe => nfe.EPresencaComprador).Must(indPres => indPres.EValido()).WithMessage("Indicador de Presença do Comprador da NFe é inválido!");
            //RuleFor(nfe => nfe.EIndicadorIntermediador).Null().When(indPres => !NotaFiscal.IntermediadorObrigadorio.Contains(indPres.EPresencaComprador)).WithMessage("Indicador de Intermediador Obrigatório para os tipos de intermediário (2, 3, 4, 9)!");
            //RuleFor(nfe => nfe.EIndicadorIntermediador).NotNull().When(indPres => NotaFiscal.IntermediadorObrigadorio.Contains(indPres.EPresencaComprador)).WithMessage("Indicador de Intermediador Obrigatório para os tipos de intermediário (2, 3, 4, 9)!");
            RuleFor(nfe => nfe.DadosIntermediador).NotNull().When(nfe => nfe.EIndicadorIntermediador.EValido() && nfe.EIndicadorIntermediador == Classes.Informacoes.Identificacao.Tipos.IndicadorIntermediador.iiSitePlataformaTerceiros).WithMessage("Dados do intermediador não informados!");
            RuleFor(nfe => nfe.DataEmissao.Date).LessThanOrEqualTo(DateTime.Now.Date).WithMessage(_ => $"Data de Emissão é maior do que a Data Atual do Servidor: {DateTime.Now:dd/MM/yyyy}");
            //RuleFor(nfe => nfe.DataSaida.GetValueOrDefault().Date).LessThanOrEqualTo(DateTime.Now.Date).When(nfe => nfe.DataSaida != null || modeloDocumento == DFe.Classes.Flags.ModeloDocumento.NFe).WithMessage(_ => $"Data de Saída é maior do que a Data Atual do Servidor: {DateTime.Now:dd/MM/yyyy}");
            RuleFor(nfe => nfe.Emitente).NotNull().WithMessage("Emitente não informado!").DependentRules(() => RuleFor(nfe => nfe.Emitente).SetValidator(new EmitenteValidator()));
            RuleFor(nfe => nfe.Destinatario).NotNull().When(_ => modeloDocumento != DFe.Classes.Flags.ModeloDocumento.NFCe).WithMessage("Destinatário não informado!").DependentRules(() => RuleFor(nfe => nfe.Destinatario).SetValidator(new DestinatarioValidator()));
            RuleFor(nfe => nfe.DadosTransporte).SetValidator(new DadosTransporteValidator()).When(nfe => nfe.DadosTransporte != null).WithMessage("Dados de Transporte inválidos!");
            RuleFor(nfe => nfe.Total).NotNull().WithMessage("Totalizador da NFe não informado!").DependentRules(() => RuleFor(nfe => nfe.Total).SetValidator(new TotalizadorValidator()));
            RuleForEach(nfe => nfe.Duplicatas).SetValidator(_ => new DuplicataValidator()).When(nfe => nfe.Duplicatas?.Count > 0).WithMessage((_, duplicata) => $"Dados inválidos do Parcela: {duplicata.Codigo}");
            RuleFor(nfe => nfe.Total.NFeValorTotal).Equal(parc => parc.Duplicatas.Sum(dup => dup.Valor)).When(nfe => nfe.DeveValidarValorTotalDaNfeEDuplicatas && nfe.Duplicatas?.Count > 0).WithMessage("Valor Total da NF-e não corresponde com o Somatório Total das duplicatas.");
            
            RuleFor(nfe => nfe.Produtos).NotNull().WithMessage("Produtos da NFe não informado!").DependentRules(() =>
            {
                RuleForEach(nfe => nfe.Produtos).SetValidator(new ProdutoValidator()).WithMessage((_, produto) => $"Dados inválidos do Produto: ({produto.Referencia}) {produto.Descricao}");
            });
            //RuleForEach(nfe => nfe.Produtos).SetValidator(new ProdutoValidator()).WithMessage((_, produto) => $"Dados inválidos do Produto: ({produto.Referencia}) {produto.Descricao}");
            RuleFor(nfe => nfe.FormasPagamento).NotNull().When(_ => modeloDocumento == DFe.Classes.Flags.ModeloDocumento.NFCe).WithMessage("Forma de Pagamento da NFCe não informado!").DependentRules(() =>
            {
                RuleForEach(nfe => nfe.FormasPagamento).SetValidator(new PagamentoValidator()).WithMessage((_, pagamento) => $"Dados inválidos do Pagamento: {pagamento.FormaPagamento}");
            });
        }


    }
}
