using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using NFe.Classes.Informacoes.Detalhe;

namespace NFe.BLL.Configuracao.Entidades.Produtos
{
    public class Produto
    {
        public Produto(
            string referencia,
            string descricao,
            string ncm,
            string cest,
            int cfop,
            string unidadeCompra,
            decimal quantidade,
            decimal valorUnitario,
            decimal valorTotal,
            //IndicadorTotal indicadorTotal,
            Imposto impostos,
            string codigoBarras = null,
            string unidadeTributacao = null,
            decimal quantidadeTributacao = 0,
            decimal valorUnitarioTributacao = 0,
            DadosCombustivel dadosCombustivel = null,
            DeclaracaoImportacao declaracaoImportacao = null,
            decimal outrasDespesasAcessorias = 0,
            decimal frete = 0,
            decimal desconto = 0,
            decimal ibptValor = 0,
            string pedidoCompraNumero = null,
            int? pedidoCompraItem = null)
        {
            Referencia = referencia.SanitizeString();
            CodigoBarras = codigoBarras.SanitizeString();
            Descricao = descricao.SanitizeString();
            NCM = ncm.SanitizeString();
            CEST = cest.SanitizeString();
            CFOP = cfop;
            UnidadeCompra = unidadeCompra.SanitizeString();
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
            Desconto = desconto;
            UnidadeTributacao = unidadeTributacao.SanitizeString();
            QuantidadeTributacao = quantidadeTributacao;
            ValorUnitarioTributacao = valorUnitarioTributacao;
            Frete = frete;
            OutrasDespesasAcessorias = outrasDespesasAcessorias;
            //EIndicadorTotal = indicadorTotal;
            PedidoCompraNumero = pedidoCompraNumero.SanitizeString();
            PedidoCompraItem = pedidoCompraItem;
            DadosCombustivel = dadosCombustivel;
            DeclaracaoImportacao = declaracaoImportacao;
            Impostos = impostos;
            IBPTValor = ibptValor;

            if(string.IsNullOrEmpty(PedidoCompraNumero))
            {
                PedidoCompraItem = null;
            }
        }

        public string Referencia { get; }

        public string CodigoBarras { get; }

        public string Descricao { get; }

        public string NCM { get; }

        public string CEST { get; }

        public int CFOP { get; }

        public string UnidadeCompra { get; }

        public decimal Quantidade { get; }

        public decimal ValorUnitario { get; }

        public decimal ValorTotal { get; }

        public decimal Desconto { get; }

        public string UnidadeTributacao { get; }

        public decimal QuantidadeTributacao { get; }

        public decimal ValorUnitarioTributacao { get; }

        public decimal Frete { get; }

        public decimal OutrasDespesasAcessorias { get; }

        //public IndicadorTotal EIndicadorTotal { get; }

        public string PedidoCompraNumero { get; }

        public int? PedidoCompraItem { get; }

        public DadosCombustivel DadosCombustivel { get; }

        public DeclaracaoImportacao DeclaracaoImportacao { get; }

        public Imposto Impostos { get; }

        public decimal IBPTValor { get; }
    }
}
