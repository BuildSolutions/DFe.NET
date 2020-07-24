using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ICMS
{
    public class ICMS51 : ICMS
    {
        public ICMS51(OrigemMercadoria origem)
        {
            CST = Csticms.Cst51;
            Origem = origem;
        }

        public ICMS51(NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.ICMS51 icms)
        {
            CST = Csticms.Cst51;
            Origem = icms.orig;
        }

        //public ICMS51(OrigemMercadoria origem,
        //    DeterminacaoBaseIcms? modalidadeCalculo = null,
        //    decimal? baseCalculo = null,
        //    decimal? aliquota = null,
        //    decimal? valorTotal = null,
        //    decimal? valorICMSOperacao = null,
        //    decimal? percentualDiferimento = null,
        //    decimal? valorDiferimento = null,
        //    decimal? aliquotaReducaoBaseCalculo = null,
        //    decimal? baseCaluloFCP = null,
        //    decimal? aliquotaFCP = null,
        //    decimal? valorTotalFCP = null)
        //{
        //    CST = Csticms.Cst51;
        //    Origem = origem;
        //    ModalidadeCalculo = modalidadeCalculo;
        //    BaseCalculo = baseCalculo;
        //    Aliquota = aliquota;
        //    ValorTotal = valorTotal;
        //    ValorICMSOperacao = valorICMSOperacao;
        //    PercentualDiferimento = percentualDiferimento;
        //    ValorDiferimento = valorDiferimento;

        //    AliquotaReducaoBaseCalculo = aliquotaReducaoBaseCalculo;

        //    BaseCaluloFCP = baseCaluloFCP;
        //    AliquotaFCP = aliquotaFCP;
        //    ValorTotalFCP = valorTotalFCP;
        //}
    }
}