using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS10 : ICMS
    {
        public ICMS10(OrigemMercadoria origem,
            DeterminacaoBaseIcms modalidadeCalculo,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal,
            DeterminacaoBaseIcmsSt modalidadeCalculoST,
            decimal baseCalculoST,
            decimal aliquotaST,
            decimal valorTotalST,
            decimal aliquotaMVAST,
            decimal baseCaluloFCP,
            decimal aliquotaFCP,
            decimal valorTotalFCP)
        {
            CST = Csticms.Cst10;
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

            BaseCaluloFCP = baseCaluloFCP.NuloSeZero();
            AliquotaFCP = aliquotaFCP.NuloSeZero();
            ValorTotalFCP = valorTotalFCP.NuloSeZero();
        }

        public ICMS10(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS10 icms)
        {
            CST = Csticms.Cst10;
            Origem = icms.orig;
            ModalidadeCalculo = icms.modBC;
            BaseCalculo = icms.vBC;
            Aliquota = icms.pICMS;
            ValorTotal = icms.vICMS;

            ModalidadeCalculoST = icms.modBCST;
            BaseCalculoST = icms.vBCST;
            ValorTotalST = icms.vICMSST;
            AliquotaST = icms.pICMSST;
            AliquotaMVAST = icms.pMVAST.NuloSeZero();

            BaseCaluloFCP = icms.vBCFCPST;
            AliquotaFCP = icms.pFCPST;
            ValorTotalFCP = icms.vFCPST;
        }

        //public ICMS10(OrigemMercadoria origem,
        //    DeterminacaoBaseIcms modalidadeCalculo,
        //    decimal baseCalculo,
        //    decimal aliquota,
        //    decimal valorTotal,
        //    DeterminacaoBaseIcmsSt modalidadeCalculoST,
        //    decimal baseCalculoST,
        //    decimal aliquotaST,
        //    decimal valorTotalST,
        //    decimal? aliquotaMVAST = null,
        //    decimal? aliquotaReducaoBaseCalculo = null,
        //    decimal? baseCaluloFCP = null,
        //    decimal? aliquotaFCP = null,
        //    decimal? valorTotalFCP = null)
        //{
        //    CST = Csticms.Cst10;
        //    Origem = origem;
        //    ModalidadeCalculo = modalidadeCalculo;
        //    BaseCalculo = baseCalculo;
        //    Aliquota = aliquota;
        //    ValorTotal = valorTotal;

        //    ModalidadeCalculoST = modalidadeCalculoST;
        //    BaseCalculoST = baseCalculoST;
        //    ValorTotalST = valorTotalST;
        //    AliquotaST = aliquotaST;
        //    AliquotaMVAST = aliquotaMVAST;
        //    AliquotaReducaoBaseCalculo = aliquotaReducaoBaseCalculo;

        //    BaseCaluloFCP = baseCaluloFCP;
        //    AliquotaFCP = aliquotaFCP;
        //    ValorTotalFCP = valorTotalFCP;
        //}
    }

    public class ICMSPart : ICMS
    {
        public ICMSPart(OrigemMercadoria origem,
            DeterminacaoBaseIcms modalidadeCalculo,
            decimal baseCalculo,
            decimal aliquota,
            decimal valorTotal,
            DeterminacaoBaseIcmsSt modalidadeCalculoST,
            decimal baseCalculoST,
            decimal aliquotaST,
            decimal valorTotalST,
            decimal aliquotaMVAST,
            decimal baseCaluloFCP,
            decimal aliquotaFCP,
            decimal valorTotalFCP)
        {
            CST = Csticms.Cst10;
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

            BaseCaluloFCP = baseCaluloFCP.NuloSeZero();
            AliquotaFCP = aliquotaFCP.NuloSeZero();
            ValorTotalFCP = valorTotalFCP.NuloSeZero();
        }

        public ICMSPart(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSPart icms)
        {
            CST = Csticms.CstPart10;
            Origem = icms.orig;
            ModalidadeCalculo = icms.modBC;
            BaseCalculo = icms.vBC;
            Aliquota = icms.pICMS;
            ValorTotal = icms.vICMS;

            ModalidadeCalculoST = icms.modBCST;
            BaseCalculoST = icms.vBCST;
            ValorTotalST = icms.vICMSST;
            AliquotaST = icms.pICMSST;
            AliquotaMVAST = icms.pMVAST.NuloSeZero();

            BaseCaluloFCP = icms.vBCFCPST;
            AliquotaFCP = icms.pFCPST;
            ValorTotalFCP = icms.vFCPST;
        }
    }
}
