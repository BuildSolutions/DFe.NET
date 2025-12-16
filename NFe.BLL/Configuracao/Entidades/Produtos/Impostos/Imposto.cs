using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class Imposto
    {
        public Imposto(
            ICMS.ICMS icms,
            CBSIBSImposto cbsibs,
            PIS pis,
            COFINS cofins,
            IPI ipi,
            PartilhaICMS partilhaICMS = null,
            TributosIBPT tributosIBPT = null,
            ImpostoImportacao impostoImportacao = null)
        {
            CBSIBS = cbsibs;
            IPI = ipi;
            PIS = pis;
            COFINS = cofins;
            ImpostoImportacao = impostoImportacao;
            PartilhaICMS = partilhaICMS;
            TributosIBPT = tributosIBPT;
            ICMS = icms;
        }

        public Imposto(imposto imposto)
        {
            if (imposto.IPI != null)
            {
                IPI = IPI.ObterIpi(imposto.IPI);
            }
            if (imposto.PIS != null)
            {
                PIS = PIS.ObterPis(imposto.PIS);
            }
            if (imposto.COFINS != null)
            {
                COFINS = COFINS.ObterCOFINS(imposto.COFINS);
            }
            if (imposto.II != null)
            {
                ImpostoImportacao = new ImpostoImportacao(imposto.II);
            }
            if (imposto.ICMSUFDest != null)
            {
                PartilhaICMS = new PartilhaICMS(imposto.ICMSUFDest);
            }
            if (imposto.ICMS != null)
            {
                ICMS = Impostos.ICMS.ICMS.ObterIcms(imposto.ICMS.TipoICMS);
            }

            if (imposto.IBSCBS != null)
            {
                CBSIBS = new CBSIBSImposto(imposto.IBSCBS);
            }

            TributosIBPT = new TributosIBPT(0, 0, 0, imposto.vTotTrib ?? 0);
        }

        public IPI IPI { get; set; }

        public PIS PIS { get; set; }

        public COFINS COFINS { get; set; }

        public ImpostoImportacao ImpostoImportacao { get; set; }

        public PartilhaICMS PartilhaICMS { get; set; }

        public TributosIBPT TributosIBPT { get; set; }

        public ICMS.ICMS ICMS { get; set; }

        public CBSIBSImposto CBSIBS { get; set; }
    }
}
