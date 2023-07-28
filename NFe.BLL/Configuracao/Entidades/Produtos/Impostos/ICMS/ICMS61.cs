using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS61 : ICMS
    {
        public ICMS61(OrigemMercadoria origem)
        {
            CST = Csticms.Cst61;
            Origem = origem;
        }

        public ICMS61(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS61 icms)
        {
            CST = Csticms.Cst61;
            Origem = icms.orig;
        }
    }
}