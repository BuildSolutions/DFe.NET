using DFe.Utils.Extensoes;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS40 : ICMS
    {
        public ICMS40(ECstIcms40 cstIcms40,
            OrigemMercadoria origem,
            decimal valorICMSDesonerado)
        {
            switch (cstIcms40)
            {
                case ECstIcms40.CST40:
                    CST = Csticms.Cst40;
                    break;
                case ECstIcms40.CST41:
                    CST = Csticms.Cst41;
                    break;
                case ECstIcms40.CST50:
                    CST = Csticms.Cst51;
                    break;
            }

            Origem = origem;

            ValorICMSDesonerado = valorICMSDesonerado.NuloSeZero();

            if (valorICMSDesonerado == 0)
            {
                MotivoDesoneracao = MotivoDesoneracaoIcms.MdiSuframa;
            }
        }
    }
}
