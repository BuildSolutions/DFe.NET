using DFe.Utils.Extensoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS30 : ICMS
    {
        public ICMS30(OrigemMercadoria origem,
            DeterminacaoBaseIcmsSt modalidadeCalculoST,
            decimal baseCalculoST,
            decimal aliquotaST,
            decimal valorTotalST,
            decimal aliquotaMVAST,
            decimal aliquotaReducaoBaseCalculo,
            decimal valorICMSDesonerado)
        {
            CST = Csticms.Cst30;
            Origem = origem;

            ModalidadeCalculoST = modalidadeCalculoST;
            BaseCalculoST = baseCalculoST;
            ValorTotalST = valorTotalST;
            AliquotaST = aliquotaST;
            AliquotaMVAST = aliquotaMVAST.NuloSeZero();

            AliquotaReducaoBaseCalculo = aliquotaReducaoBaseCalculo.NuloSeZero();
            ValorICMSDesonerado = valorICMSDesonerado.NuloSeZero();

            if (valorICMSDesonerado == 0)
            {
                MotivoDesoneracao = MotivoDesoneracaoIcms.MdiSuframa;
            }
        }

        public ICMS30(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS30 icms)
        {
            CST = Csticms.Cst30;
            Origem = icms.orig;

            ModalidadeCalculoST = icms.modBCST;
            BaseCalculoST = icms.vBCST;
            ValorTotalST = icms.vICMSST;
            AliquotaST = icms.pICMSST ;
            AliquotaMVAST = icms.pMVAST.NuloSeZero();

            AliquotaReducaoBaseCalculo = icms.pRedBCST.NuloSeZero();
            ValorICMSDesonerado = icms.vICMSDeson.NuloSeZero();

            if (ValorICMSDesonerado == 0)
            {
                MotivoDesoneracao = MotivoDesoneracaoIcms.MdiSuframa;
            }
        }

        //public ICMS30(OrigemMercadoria origem,
        //    DeterminacaoBaseIcmsSt modalidadeCalculoST,
        //    decimal baseCalculoST,
        //    decimal aliquotaST,
        //    decimal valorTotalST,
        //    decimal? aliquotaMVAST = null,
        //    decimal? aliquotaReducaoBaseCalculo = null,
        //    decimal? valorICMSDesonerado = null,
        //    MotivoDesoneracaoIcms? motivoDesoneracao = null,
        //    decimal? baseCaluloFCP = null,
        //    decimal? aliquotaFCP = null,
        //    decimal? valorTotalFCP = null)
        //{
        //    CST = Csticms.Cst30;
        //    Origem = origem;

        //    ModalidadeCalculoST = modalidadeCalculoST;
        //    BaseCalculoST = baseCalculoST;
        //    ValorTotalST = valorTotalST;
        //    AliquotaST = aliquotaST;
        //    AliquotaMVAST = aliquotaMVAST;

        //    AliquotaReducaoBaseCalculo = aliquotaReducaoBaseCalculo;
        //    ValorICMSDesonerado = valorICMSDesonerado;
        //    MotivoDesoneracao = motivoDesoneracao;

        //    BaseCaluloFCP = baseCaluloFCP;
        //    AliquotaFCP = aliquotaFCP;
        //    ValorTotalFCP = valorTotalFCP;
        //}
    }
}