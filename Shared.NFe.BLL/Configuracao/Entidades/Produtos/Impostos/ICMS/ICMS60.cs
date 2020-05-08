using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS60 : ICMS
    {
        public ICMS60(OrigemMercadoria origem)
        {
            CST = Csticms.Cst60;
            Origem = origem;
        }

        //public ICMS60(OrigemMercadoria origem,
        //    decimal baseCalculoICMSRetido = 0,
        //    decimal valorICMSRetido = 0)
        //{
        //    CST = Csticms.Cst60;
        //    Origem = origem;

        //    BaseCalculoICMSRetido = baseCalculoICMSRetido;
        //    ValorICMSRetido = valorICMSRetido;
        //}
    }
}
