using DFe.Utils.Extensoes;
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
            AliquotaMVAST = aliquotaMVAST.NuloSeZero();

            BaseCaluloFCP = baseCaluloFCP.NuloSeZero();
            AliquotaFCP = aliquotaFCP.NuloSeZero();
            ValorTotalFCP = valorTotalFCP.NuloSeZero();

            RepasseCreditoAliquota = 0;
            RepasseCreditoValor = 0;
        }

        public ICMSSN201(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN201 icms)
        {
            CSOSN = Csosnicms.Csosn201;
            Origem = icms.orig;

            ModalidadeCalculoST = icms.modBCST;
            BaseCalculoST = icms.vBCST;
            ValorTotalST = icms.vICMSST;
            AliquotaST = icms.pICMSST;
            AliquotaMVAST = icms.pMVAST.NuloSeZero();

            BaseCaluloFCP = icms.vBCFCPST.NuloSeZero();
            AliquotaFCP = icms.pFCPST.NuloSeZero();
            ValorTotalFCP = icms.vFCPST.NuloSeZero();

            RepasseCreditoAliquota = 0;
            RepasseCreditoValor = 0;
        }
    }
}
