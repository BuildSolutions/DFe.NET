using System;
using System.Collections.Generic;
using System.Text;

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

        public decimal ValorBaseCalculo { get; }

        public decimal ValorDespesaAduaneira { get; }

        public decimal ValorImpostoImportacao { get; }

        public decimal ValorIOF { get; }
    }
}
