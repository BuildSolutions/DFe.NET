using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSSN101 : ICMS
    {
        public ICMSSN101(OrigemMercadoria origem,
            decimal repasseCreditoAliquota,
            decimal repasseCreditoValor)
        {
            CSOSN = Csosnicms.Csosn101;
            Origem = origem;

            RepasseCreditoAliquota = repasseCreditoAliquota;
            RepasseCreditoValor = repasseCreditoValor;
        }

        public ICMSSN101(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN101 icms)
        {
            CSOSN = Csosnicms.Csosn101;
            Origem = icms.orig;

            RepasseCreditoAliquota = icms.pCredSN;
            RepasseCreditoValor = icms.vCredICMSSN;
        }
    }
}
