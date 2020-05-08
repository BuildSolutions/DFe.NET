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
    }
}
