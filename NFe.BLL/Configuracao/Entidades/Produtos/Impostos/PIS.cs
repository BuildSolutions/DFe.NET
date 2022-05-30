using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using System.Collections.Generic;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class PIS
    {
        public PIS(CSTPIS cst, decimal baseCalculo, decimal valorTotal, decimal aliquota)
        {
            CST = cst;
            BaseCalculo = baseCalculo;
            ValorTotal = valorTotal;
            Aliquota = aliquota;
        }

        public PIS(PISAliq pis)
        {
            CST = pis.CST;
            BaseCalculo = pis.vBC;
            ValorTotal = pis.vPIS;
            Aliquota = pis.pPIS;
        }

        public PIS(PISOutr pis)
        {
            CST = pis.CST;
            BaseCalculo = pis.vBC ?? 0M;
            ValorTotal = pis.vPIS ?? 0M;
            Aliquota = pis.pPIS ?? 0M;
        }

        public PIS(PISNT pis)
        {
            CST = pis.CST;
        }

        public CSTPIS CST { get; }

        public decimal BaseCalculo { get; }

        public decimal ValorTotal { get; }

        public decimal Aliquota { get; }

        /// <summary>
        /// Devolve a lista de enums do tipo <see cref="CSTPIS"/> que são Tributados por Aliquota/>
        /// </summary>
        public static ISet<CSTPIS> PisCstTributadaPorAliquota = new HashSet<CSTPIS>()
        {
            CSTPIS.pis01, CSTPIS.pis02
        };

        public static PIS ObterPis(NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.PIS pis)
        {
            switch (pis.TipoPIS.GetType().Name)
            {
                case nameof(PISAliq):
                    return new PIS((PISAliq)pis.TipoPIS);
                case nameof(PISNT):
                    return new PIS((PISNT)pis.TipoPIS);
                case nameof(PISOutr):
                    return new PIS((PISOutr)pis.TipoPIS);
                default:
                    return null;
            }
        }
    }
}
