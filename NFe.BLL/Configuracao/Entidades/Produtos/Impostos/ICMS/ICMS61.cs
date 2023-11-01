using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS61 : ICMS
    {
        public ICMS61(OrigemMercadoria origem,
            decimal aliquotaAdRemICMSRetido,
            decimal valorICMSRetido,
            decimal? baseCalculoICMSRetido)
        {
            CSOSN = Csosnicms.Cst61;
            CST = Csticms.Cst61;
            Origem = origem;
            AliquotaAdRemICMSRetido = aliquotaAdRemICMSRetido;
            ValorICMSRetido = valorICMSRetido;
            BaseCalculoICMSRetido = baseCalculoICMSRetido;
        }

        public ICMS61(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS61 icms)
        {
            CSOSN = Csosnicms.Cst61;
            CST = Csticms.Cst61;
            Origem = icms.orig;

            AliquotaAdRemICMSRetido = icms.adRemICMSRet;
            ValorICMSRetido = icms.vICMSMonoRet;
            BaseCalculoICMSRetido = icms.qBCMonoRet;
        }
    }
}