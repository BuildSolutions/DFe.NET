using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSSN201 : ICMS
    {
        public ICMSSN201(OrigemMercadoria origem,
            DeterminacaoBaseIcmsSt modalidadeCalculoST,
            decimal baseCalculoST,
            decimal aliquotaST,
            decimal valorTotalST,
            decimal aliquotaMVAST,
            decimal baseCaluloFCP,
            decimal aliquotaFCP,
            decimal valorTotalFCP)
        {
            CSOSN = Csosnicms.Csosn201;
            Origem = origem;

            ModalidadeCalculoST = modalidadeCalculoST;
            BaseCalculoST = baseCalculoST;
            ValorTotalST = valorTotalST;
            AliquotaST = aliquotaST;
            AliquotaMVAST = aliquotaMVAST;

            BaseCaluloFCP = baseCaluloFCP;
            AliquotaFCP = aliquotaFCP;
            ValorTotalFCP = valorTotalFCP;

            RepasseCreditoAliquota = 0;
            RepasseCreditoValor = 0;

            if(aliquotaMVAST == 0)
            {
                AliquotaMVAST = null;
            }

            if(aliquotaFCP == 0)
            {
                AliquotaFCP = null;
                BaseCaluloFCP = null;
                ValorTotalFCP = null;
            }
        }
    }
}
