using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using System.Collections.Generic;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class COFINS
    {
        public COFINS(CSTCOFINS cst, decimal baseCalculo, decimal valorTotal, decimal aliquota)
        {
            CST = cst;
            BaseCalculo = baseCalculo;
            ValorTotal = valorTotal;
            Aliquota = aliquota;
        }

        public CSTCOFINS CST { get; }

        public decimal BaseCalculo { get; }

        public decimal ValorTotal { get; }

        public decimal Aliquota { get; }

        /// <summary>
        /// Devolve a lista de enums do tipo <see cref="CSTCOFINS"/> que são Tributados/>
        /// </summary>
        public static ISet<CSTCOFINS> CofinsCstTributadaPorAliquota = new HashSet<CSTCOFINS>()
        {
             CSTCOFINS.cofins01, CSTCOFINS.cofins02
        };
    }
}
