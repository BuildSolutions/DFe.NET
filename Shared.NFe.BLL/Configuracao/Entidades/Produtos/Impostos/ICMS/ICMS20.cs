using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS20 : ICMS
    {
        public ICMS20(OrigemMercadoria origem,
            DeterminacaoBaseIcms modalidadeCalculo,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal,
            decimal aliquotaReducaoBaseCalculo)
            //decimal valorICMSDesonerado,
            //MotivoDesoneracaoIcms motivoDesoneracao)
        {
            CST = Csticms.Cst20;
            Origem = origem;
            ModalidadeCalculo = modalidadeCalculo;
            BaseCalculo = baseCalculo;
            Aliquota = aliquota;
            ValorTotal = valorTotal;

            AliquotaReducaoBaseCalculo = aliquotaReducaoBaseCalculo;
            //ValorICMSDesonerado = valorICMSDesonerado;
            //MotivoDesoneracao = motivoDesoneracao;
        }

        public ICMS20(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS20 icms)
        {
            CST = Csticms.Cst20;
            Origem = icms.orig;
            ModalidadeCalculo = icms.modBC;
            BaseCalculo = icms.vBC;
            Aliquota = icms.pICMS;
            ValorTotal = icms.vICMS;

            AliquotaReducaoBaseCalculo = icms.pRedBC;
        }
    }
}
