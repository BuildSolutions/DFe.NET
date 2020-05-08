namespace NFe.BLL.Configuracao.Entidades
{
    public class Totalizador
    {
        public Totalizador(
            decimal icmsBaseCalculo,
            decimal icmsTotal,
            decimal icmsDesonerado,
            decimal icmsSTBaseCalculo,
            decimal icmsSTTotal,
            decimal fcpSubstituicaoTributaria,
            decimal produtosTotal,
            decimal frete,
            decimal seguro,
            decimal desconto,
            decimal impostoImportacao,
            decimal ipi,
            decimal pis,
            decimal cofins,
            decimal outrasDespesasAcessorias,
            decimal nFeValorTotal,
            decimal tributosIBPT,
            decimal icmsUFDestino,
            decimal icmsUFOrigem,
            decimal fcpUFDestino)
        {
            ICMSBaseCalculo = icmsBaseCalculo;
            ICMSTotal = icmsTotal;
            ICMSDesonerado = icmsDesonerado;
            ICMSSTBaseCalculo = icmsSTBaseCalculo;
            ICMSSTTotal = icmsSTTotal;
            FCPSubstituicaoTributaria = fcpSubstituicaoTributaria;
            ProdutosTotal = produtosTotal;
            Frete = frete;
            Seguro = seguro;
            Desconto = desconto;
            ImpostoImportacao = impostoImportacao;
            IPI = ipi;
            PIS = pis;
            COFINS = cofins;
            OutrasDespesasAcessorias = outrasDespesasAcessorias;
            NFeValorTotal = nFeValorTotal;
            TributosIBPT = tributosIBPT;
            ICMSUFDestino = icmsUFDestino;
            ICMSUFOrigem = icmsUFOrigem;
            FCPUFDestino = fcpUFDestino;
        }

        public decimal ICMSBaseCalculo { get; }

        public decimal ICMSTotal { get; }

        public decimal ICMSDesonerado { get; }

        public decimal ICMSSTBaseCalculo { get; }

        public decimal ICMSSTTotal { get; }

        public decimal FCPSubstituicaoTributaria { get; }

        public decimal ProdutosTotal { get; }

        public decimal Frete { get; }

        public decimal Seguro { get; }

        public decimal Desconto { get; }

        public decimal ImpostoImportacao { get; }

        public decimal IPI { get; }

        public decimal PIS { get; }

        public decimal COFINS { get; }

        public decimal OutrasDespesasAcessorias { get; }

        public decimal NFeValorTotal { get; }

        public decimal TributosIBPT { get; }

        public decimal ICMSUFDestino { get; }

        public decimal ICMSUFOrigem { get; }

        public decimal FCPUFDestino { get; }
    }
}
