using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSSN500 : ICMS
    {
        public ICMSSN500(OrigemMercadoria origem)
        {
            CSOSN = Csosnicms.Csosn500;
            Origem = origem;
        }
    }
}
