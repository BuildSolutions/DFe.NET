using DFe.Utils.Extensoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSST : ICMS
    {
        public ICMSST(OrigemMercadoria origem,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal)
        {
            CST = Csticms.Cst60;
            BaseCalculo = baseCalculo;
            Aliquota = aliquota;
            ValorTotal = valorTotal;
            Origem = origem;
        }

        public ICMSST(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSST icms)
        {
            CST = icms.CST;
            Origem = icms.orig;
            BaseCalculo = icms.vBCEfet.GetValueOrDefault();
            Aliquota = icms.pICMSEfet.GetValueOrDefault();
            ValorTotal = icms.vICMSEfet.GetValueOrDefault();
        }
    }
}