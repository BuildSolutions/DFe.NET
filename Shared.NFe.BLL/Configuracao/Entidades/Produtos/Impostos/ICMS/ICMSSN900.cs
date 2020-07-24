using DFe.Utils.Extensoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSSN900 : ICMS
    {
        public ICMSSN900(OrigemMercadoria origem,
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
            CSOSN = Csosnicms.Csosn900;
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

        public ICMSSN900(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN900 icms)
        {
            CSOSN = Csosnicms.Csosn900;
            Origem = icms.orig;
            ModalidadeCalculo = icms.modBC;
            BaseCalculo = icms.vBC.GetValueOrDefault();
            Aliquota = icms.pICMS.GetValueOrDefault();
            ValorTotal = icms.vICMS.GetValueOrDefault();

            ModalidadeCalculoST = icms.modBCST.GetValueOrDefault();
            BaseCalculoST = icms.vBCST.GetValueOrDefault();
            ValorTotalST = icms.vICMSST.GetValueOrDefault();
            AliquotaST = icms.pICMSST.GetValueOrDefault();
            AliquotaMVAST = icms.pMVAST.NuloSeZero();
        }
    }
}
