namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class Imposto
    {
        public Imposto(
            ICMS.ICMS icms,
            PIS pis,
            COFINS cofins,
            IPI ipi,
            PartilhaICMS partilhaICMS = null,
            TributosIBPT tributosIBPT = null,
            ImpostoImportacao impostoImportacao = null)
        {
            IPI = ipi;
            PIS = pis;
            COFINS = cofins;
            ImpostoImportacao = impostoImportacao;
            PartilhaICMS = partilhaICMS;
            TributosIBPT = tributosIBPT;
            ICMS = icms;
        }

        public IPI IPI { get; set; }

        public PIS PIS { get; set; }

        public COFINS COFINS { get; set; }

        public ImpostoImportacao ImpostoImportacao { get; set; }

        public PartilhaICMS PartilhaICMS { get; set; }

        public TributosIBPT TributosIBPT { get; set; }

        public ICMS.ICMS ICMS { get; set; }
    }
}
