using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class ImpostoImportacao
    {
        public ImpostoImportacao(decimal valorBaseCalculo, decimal valorDespesaAduaneira, decimal valorImpostoImportacao, decimal valorIOF)
        {
            ValorBaseCalculo = valorBaseCalculo;
            ValorDespesaAduaneira = valorDespesaAduaneira;
            ValorImpostoImportacao = valorImpostoImportacao;
            ValorIOF = valorIOF;
        }

        public ImpostoImportacao(II impostoImportacao)
        {
            ValorBaseCalculo = impostoImportacao.vBC;
            ValorDespesaAduaneira = impostoImportacao.vDespAdu;
            ValorImpostoImportacao = impostoImportacao.vII;
            ValorIOF = impostoImportacao.vIOF;
        }

        public decimal ValorBaseCalculo { get; }

        public decimal ValorDespesaAduaneira { get; }

        public decimal ValorImpostoImportacao { get; }

        public decimal ValorIOF { get; }
    }
}
