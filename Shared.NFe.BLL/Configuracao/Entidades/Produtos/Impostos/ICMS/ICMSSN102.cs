using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSSN102 : ICMS
    {
        public ICMSSN102(EIcms102 csosn,
            OrigemMercadoria origem)
        {
            switch (csosn)
            {
                case EIcms102.ICMS102:
                    CSOSN = Csosnicms.Csosn102;
                    break;
                case EIcms102.ICMS103:
                    CSOSN = Csosnicms.Csosn103;
                    break;
                case EIcms102.ICMS300:
                    CSOSN = Csosnicms.Csosn300;
                    break;
                case EIcms102.ICMS400:
                    CSOSN = Csosnicms.Csosn400;
                    break;
            }

            Origem = origem;
        }
    }
}
