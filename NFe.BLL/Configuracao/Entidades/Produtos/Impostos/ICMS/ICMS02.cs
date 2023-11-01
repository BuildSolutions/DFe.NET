using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS02 : ICMS
    {
        public ICMS02(OrigemMercadoria origem,
            decimal aliquotaAdRem,
            decimal valorTotal,
            decimal? qtdBaseCalculo)
        {
            CSOSN = Csosnicms.Cst02;
            CST = Csticms.Cst02;
            Origem = origem;
            AliquotaAdRemICMSRetido = aliquotaAdRem;
            ValorTotal = valorTotal;

            if (qtdBaseCalculo.GetValueOrDefault() > 0)
            {
                BaseCalculo = qtdBaseCalculo.Value;
            }
        }

        public ICMS02(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS02 icms)
        {
            CSOSN = Csosnicms.Cst02;
            CST = Csticms.Cst02;
            Origem = icms.orig;

            AliquotaAdRemICMS = icms.adRemICMS;
            ValorTotal = icms.vICMSMono;

            if (icms.qBCMono.GetValueOrDefault() > 0)
            {
                BaseCalculo = icms.qBCMono.Value;
            }
        }
    }
}