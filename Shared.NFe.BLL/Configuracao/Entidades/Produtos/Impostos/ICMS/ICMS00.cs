using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS00 : ICMS
    {
        public ICMS00(OrigemMercadoria origem,
            DeterminacaoBaseIcms modalidadeCalculo,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal)
        {
            CST = Csticms.Cst00;
            Origem = origem;
            ModalidadeCalculo = modalidadeCalculo;
            BaseCalculo = baseCalculo;
            Aliquota = aliquota;
            ValorTotal = valorTotal;
        }

        //public ICMS00(OrigemMercadoria origem,
        //    DeterminacaoBaseIcms modalidadeCalculo,
        //    decimal baseCalculo,
        //    decimal aliquota,
        //    decimal valorTotal,
        //    decimal? aliquotaFCP = null,
        //    decimal? valorTotalFCP = null)
        //{
        //    CST = Csticms.Cst00;
        //    Origem = origem;
        //    ModalidadeCalculo = modalidadeCalculo;
        //    BaseCalculo = baseCalculo;
        //    Aliquota = aliquota;
        //    ValorTotal = valorTotal;
        //    AliquotaFCP = aliquotaFCP;
        //    ValorTotalFCP = valorTotalFCP;
        //}
    }
}
