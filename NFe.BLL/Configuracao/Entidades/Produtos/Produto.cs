using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using NFe.Classes.Informacoes.Detalhe;

namespace NFe.BLL.Configuracao.Entidades.Produtos
{
    public class Produto
    {
        public Produto(string referencia,
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
            string dadosAdicionais,
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
            int? pedidoCompraItem = null,
            string codigoBeneficioFiscal = null,
            string documentoReferenciadoChaveAcesso = null,
            int? documentoReferenciadoItem = null)
        {
            Referencia = referencia.SanitizeString();
            CodigoBarras = codigoBarras.SanitizeString();
            Descricao = descricao.SanitizeString().SubstringMaxLength(120);
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
            DadosAdicionais = dadosAdicionais.SanitizeString();
            IBPTValor = ibptValor;
            CodigoBeneficioFiscal = codigoBeneficioFiscal.SanitizeString();
            DocumentoReferenciadoChaveAcesso = documentoReferenciadoChaveAcesso.SanitizeString();
            DocumentoReferenciadoItem = documentoReferenciadoItem;

            if(string.IsNullOrEmpty(PedidoCompraNumero))
            {
                PedidoCompraItem = null;
            }
        }

        /// <summary>
        /// Produto lido de um item de outro DFe, guardando a origem para o
        /// referenciamento por item (grupo VC01 DFeReferenciado).
        /// </summary>
        /// <param name="produto">Item do documento de origem</param>
        /// <param name="chaveAcessoOrigem">Chave de acesso do documento de origem</param>
        public Produto(det produto, string chaveAcessoOrigem)
            : this(produto)
        {
            DocumentoReferenciadoChaveAcesso = chaveAcessoOrigem.SanitizeString();
            DocumentoReferenciadoItem = produto.nItem > 0 ? produto.nItem : (int?)null;
        }

        public Produto(det produto)
        {
            Referencia = produto.prod.cProd.SanitizeString();
            CodigoBarras = string.IsNullOrEmpty(produto.prod.cEAN) || string.IsNullOrEmpty(produto.prod.cEANTrib) || produto.prod.cEAN.Equals(produto.prod.cEANTrib) ? produto.prod.cEAN.SanitizeString() : produto.prod.cEANTrib.SanitizeString();
            Descricao = produto.prod.xProd.SanitizeString();
            NCM = produto.prod.NCM.SanitizeString();
            CEST = produto.prod.CEST.SanitizeString();
            CFOP = produto.prod.CFOP;
            UnidadeCompra = produto.prod.uCom.SanitizeString();
            Quantidade = produto.prod.qCom;
            ValorUnitario = produto.prod.vUnCom;
            ValorTotal = produto.prod.vProd;
            Desconto = produto.prod.vDesc ?? 0;
            UnidadeTributacao = produto.prod.uTrib.SanitizeString();
            QuantidadeTributacao = produto.prod.qTrib;
            ValorUnitarioTributacao = produto.prod.vUnTrib;
            Frete = produto.prod.vFrete ?? 0;
            OutrasDespesasAcessorias = produto.prod.vOutro ?? 0;
            //EIndicadorTotal = indicadorTotal;
            PedidoCompraNumero = produto.prod.xPed.SanitizeString();
            PedidoCompraItem = produto.prod.nItemPed;
            DadosCombustivel = null;
            DeclaracaoImportacao = null;
            Impostos = new Imposto(produto.imposto);
            IBPTValor = produto.imposto.vTotTrib ?? 0;
            CodigoBeneficioFiscal = produto.prod.cBenef.SanitizeString();

            if (string.IsNullOrEmpty(PedidoCompraNumero))
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

        public string DadosAdicionais { get; }

        public string CodigoBeneficioFiscal { get; }

        /// <summary>
        /// VC02 - Chave de acesso do DFe de onde este item veio. Obrigatorio na
        /// NF-e de devolucao (finNFe = 4), regra VC02-14.
        /// </summary>
        public string DocumentoReferenciadoChaveAcesso { get; }

        /// <summary>
        /// VC03 - Numero do item (nItem) deste produto no DFe referenciado.
        /// </summary>
        public int? DocumentoReferenciadoItem { get; }
    }
}
