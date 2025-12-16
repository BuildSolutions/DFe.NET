using System.ComponentModel;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators
{
    public enum EOperacaoGovernoTipo : uint
    {
        [Description("NÃO SE APLICA")]
        NaoSeAplica = 0,

        [Description("FORNECIMENTO")]
        Fornecimento = 1,

        [Description("RECEBIMENTO DO PAGAMENTO, CONFORME FATO GERADOR DO IBS/CBS")]
        RecebimentoDoPagamentoConformeFatoGeradorDoIBSOuCBS = 2,
    }
}
