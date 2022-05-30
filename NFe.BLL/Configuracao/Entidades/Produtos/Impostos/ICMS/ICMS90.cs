using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS90 : ICMS
    {
        public ICMS90(OrigemMercadoria origem,
            DeterminacaoBaseIcms modalidadeCalculo,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal)
        {
            CST = Csticms.Cst90;
            Origem = origem;
            ModalidadeCalculo = modalidadeCalculo;
            BaseCalculo = baseCalculo;
            Aliquota = aliquota;
            ValorTotal = valorTotal;
        }

        public ICMS90(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS90 icms)
        {
            CST = Csticms.Cst90;
            Origem = icms.orig;
            ModalidadeCalculo = icms.modBC;
            BaseCalculo = icms.vBC ?? 0;
            Aliquota = icms.pICMS ?? 0;
            ValorTotal = icms.vICMS ?? 0;
            AliquotaReducaoBaseCalculoST = icms.pRedBCST;
        }
    }
}
