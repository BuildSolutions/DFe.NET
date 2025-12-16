namespace NFe.BLL.Configuracao.Entidades.Produtos.ReformaTributaria
{
    public class CClassTribCodigoClassificacaoTributaria
    {
        public CClassTribCodigoClassificacaoTributaria(string cST,
            string cClassTrib,
            decimal iBSAliquotaReducao,
            decimal cBSAliquotaReducao,
            bool possuiRedutorBaseCalculo,
            bool requerTributacaoRegular,
            bool requerCreditoPresumido,
            bool requerReducaoAliquota,
            bool requerDiferimento,
            bool requerMonofasico,
            bool requerMonofasicoSujeitoARetencao,
            bool requerMonofasicoDiferimento,
            bool requerEstornoCredito,
            int isNFCePermitido,
            int isNFePermitido,
            int isNFSePermitido)
        {
            CST = cST;
            CClassTrib = cClassTrib;
            IBSAliquotaReducao = iBSAliquotaReducao;
            CBSAliquotaReducao = cBSAliquotaReducao;
            PossuiRedutorBaseCalculo = possuiRedutorBaseCalculo;
            RequerTributacaoRegular = requerTributacaoRegular;
            RequerCreditoPresumido = requerCreditoPresumido;
            RequerReducaoAliquota = requerReducaoAliquota;
            RequerDiferimento = requerDiferimento;
            RequerMonofasico = requerMonofasico;
            RequerMonofasicoSujeitoARetencao = requerMonofasicoSujeitoARetencao;
            RequerMonofasicoDiferimento = requerMonofasicoDiferimento;
            RequerEstornoCredito = requerEstornoCredito;
            IsNFCePermitido = isNFCePermitido;
            IsNFePermitido = isNFePermitido;
            IsNFSePermitido = isNFSePermitido;
        }

        public string CST { get; set; }
        public string CClassTrib { get; set; }
        public decimal IBSAliquotaReducao { get; set; }
        public decimal CBSAliquotaReducao { get; set; }
        public bool PossuiRedutorBaseCalculo { get; set; }
        public bool RequerTributacaoRegular { get; set; }
        public bool RequerCreditoPresumido { get; set; }
        public bool RequerReducaoAliquota { get; set; }
        public bool RequerDiferimento { get; set; }
        public bool RequerMonofasico { get; set; }
        public bool RequerMonofasicoSujeitoARetencao { get; set; }
        public bool RequerMonofasicoDiferimento { get; set; }
        public bool RequerEstornoCredito { get; set; }
        public int IsNFCePermitido { get; set; }
        public int IsNFePermitido { get; set; }
        public int IsNFSePermitido { get; set; }
    }
}
