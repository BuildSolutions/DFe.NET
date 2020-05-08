using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS10 : ICMS
    {
        public ICMS10(OrigemMercadoria origem,
            DeterminacaoBaseIcms modalidadeCalculo,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal,
            DeterminacaoBaseIcmsSt modalidadeCalculoST,
            decimal baseCalculoST,
            decimal aliquotaST,
            decimal valorTotalST,
            decimal aliquotaMVAST)
        {
            CST = Csticms.Cst10;
            Origem = origem;
            ModalidadeCalculo = modalidadeCalculo;
            BaseCalculo = baseCalculo;
            Aliquota = aliquota;
            ValorTotal = valorTotal;

            ModalidadeCalculoST = modalidadeCalculoST;
            BaseCalculoST = baseCalculoST;
            ValorTotalST = valorTotalST;
            AliquotaST = aliquotaST;
            AliquotaMVAST = aliquotaMVAST.NuloSeZero();
        }

        //public ICMS10(OrigemMercadoria origem,
        //    DeterminacaoBaseIcms modalidadeCalculo,
        //    decimal baseCalculo,
        //    decimal aliquota,
        //    decimal valorTotal,
        //    DeterminacaoBaseIcmsSt modalidadeCalculoST,
        //    decimal baseCalculoST,
        //    decimal aliquotaST,
        //    decimal valorTotalST,
        //    decimal? aliquotaMVAST = null,
        //    decimal? aliquotaReducaoBaseCalculo = null,
        //    decimal? baseCaluloFCP = null,
        //    decimal? aliquotaFCP = null,
        //    decimal? valorTotalFCP = null)
        //{
        //    CST = Csticms.Cst10;
        //    Origem = origem;
        //    ModalidadeCalculo = modalidadeCalculo;
        //    BaseCalculo = baseCalculo;
        //    Aliquota = aliquota;
        //    ValorTotal = valorTotal;

        //    ModalidadeCalculoST = modalidadeCalculoST;
        //    BaseCalculoST = baseCalculoST;
        //    ValorTotalST = valorTotalST;
        //    AliquotaST = aliquotaST;
        //    AliquotaMVAST = aliquotaMVAST;
        //    AliquotaReducaoBaseCalculo = aliquotaReducaoBaseCalculo;

        //    BaseCaluloFCP = baseCaluloFCP;
        //    AliquotaFCP = aliquotaFCP;
        //    ValorTotalFCP = valorTotalFCP;
        //}
    }
}
