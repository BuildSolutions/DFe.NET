namespace NFe.BLL.Configuracao.Entidades.Produtos.ReformaTributaria
{
    public class CSTCodigoClassificacaoTributaria
    {
        public CSTCodigoClassificacaoTributaria(string cst,
            bool requerCBSIBS,
            bool requerMonofasicoCBSIBS,
            bool requerReducaoAliquotaCBSIBS,
            bool requerDiferimento,
            bool requerTransferenciaCredito,
            bool requerReducaoBaseCalculo,
            bool requerCreditoPresumidoIBSZFM,
            bool requerAjusteCompetencia)
        {
            CST = cst;
            RequerCBSIBS = requerCBSIBS;
            RequerMonofasicoCBSIBS = requerMonofasicoCBSIBS;
            RequerReducaoAliquotaCBSIBS = requerReducaoAliquotaCBSIBS;
            RequerDiferimento = requerDiferimento;
            RequerTransferenciaCredito = requerTransferenciaCredito;
            RequerReducaoBaseCalculo = requerReducaoBaseCalculo;
            RequerCreditoPresumidoIBSZFM = requerCreditoPresumidoIBSZFM;
            RequerAjusteCompetencia = requerAjusteCompetencia;
        }

        public string CST { get; set; }
        public bool RequerCBSIBS { get; }
        public bool RequerMonofasicoCBSIBS { get; }
        public bool RequerReducaoAliquotaCBSIBS { get; }
        public bool RequerDiferimento { get; set; }
        public bool RequerTransferenciaCredito { get; set; }
        public bool RequerReducaoBaseCalculo { get; }
        public bool RequerCreditoPresumidoIBSZFM { get; set; }
        public bool RequerAjusteCompetencia { get; set; }
    }
}
