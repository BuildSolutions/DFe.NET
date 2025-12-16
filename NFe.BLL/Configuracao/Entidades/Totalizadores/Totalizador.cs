using NFe.Classes.Informacoes.Total;

namespace NFe.BLL.Configuracao.Entidades.Totalizadores
{
    public class Totalizador
    {
        public Totalizador(TotalizadorICMS totalizadorICMS, TotalizadorCBSIBS totalizadorCBSIBS)
        {
            TotalizadorICMS = totalizadorICMS;
            TotalizadorCBSIBS = totalizadorCBSIBS;
        }

        public Totalizador(total total)
        {
            TotalizadorICMS = new TotalizadorICMS(icmsBaseCalculo: total?.ICMSTot?.vBC ?? 0,
                icmsTotal: total?.ICMSTot?.vICMS ?? 0,
                icmsDesonerado: total?.ICMSTot?.vICMSDeson ?? 0M,
                icmsSTBaseCalculo: total?.ICMSTot?.vBCST ?? 0,
                icmsSTTotal: total?.ICMSTot?.vST ?? 0,
                fcpSubstituicaoTributaria: total?.ICMSTot?.vFCPST ?? 0M,
                produtosTotal: total?.ICMSTot?.vProd ?? 0,
                frete: total?.ICMSTot?.vFrete ?? 0,
                seguro: total?.ICMSTot?.vSeg ?? 0,
                desconto: total?.ICMSTot?.vDesc ?? 0,
                impostoImportacao: total?.ICMSTot?.vII ?? 0,
                ipi: total?.ICMSTot?.vIPI ?? 0,
                pis: total?.ICMSTot?.vPIS ?? 0,
                cofins: total?.ICMSTot?.vCOFINS ?? 0,
                outrasDespesasAcessorias: total?.ICMSTot?.vOutro ?? 0,
                nFeValorTotal: total?.ICMSTot?.vNF ?? 0,
                tributosIBPT: total?.ICMSTot?.vTotTrib ?? 0M,
                icmsUFDestino: total?.ICMSTot?.vICMSUFDest ?? 0M,
                icmsUFOrigem: total?.ICMSTot?.vICMSUFRemet ?? 0M,
                fcpUFDestino: total?.ICMSTot?.vFCPUFDest ?? 0M,
                valorICMSMonofasicoRetido: total?.ICMSTot?.vICMSMonoRet ?? 0M,
                quantidadeBaseCalculoMonofasicoRetido: total?.ICMSTot?.qBCMonoRet ?? 0M,
                valorIRRetido: 0);

            TotalizadorCBSIBS = new TotalizadorCBSIBS(total.IBSCBSTot);
        }

        public TotalizadorICMS TotalizadorICMS { get; }

        public TotalizadorCBSIBS TotalizadorCBSIBS { get; }
    }
}
