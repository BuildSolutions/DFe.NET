using DFe.Utils.Extensoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMSSN202: ICMS
    {
        public ICMSSN202(OrigemMercadoria origem,
            DeterminacaoBaseIcmsSt modalidadeCalculoST,
            decimal baseCalculoST,
            decimal aliquotaST,
            decimal valorTotalST,
            decimal aliquotaMVAST,
            decimal baseCaluloFCP,
            decimal aliquotaFCP,
            decimal valorTotalFCP)
        {
            CSOSN = Csosnicms.Csosn202;
            Origem = origem;

            ModalidadeCalculoST = modalidadeCalculoST;
            BaseCalculoST = baseCalculoST;
            ValorTotalST = valorTotalST;
            AliquotaST = aliquotaST;
            AliquotaMVAST = aliquotaMVAST.NuloSeZero();

            BaseCaluloFCP = baseCaluloFCP.NuloSeZero();
            AliquotaFCP = aliquotaFCP.NuloSeZero();
            ValorTotalFCP = valorTotalFCP.NuloSeZero();
        }

        public ICMSSN202(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMSSN202 icms)
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
        }
    }
}
