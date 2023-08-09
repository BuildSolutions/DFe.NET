using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS53 : ICMS
    {
        public ICMS53(OrigemMercadoria origem)
        {
            CST = Csticms.Cst53;
            Origem = origem;
        }

        public ICMS53(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS53 icms)
        {
            CST = Csticms.Cst53;
            Origem = icms.orig;
        }
    }
}