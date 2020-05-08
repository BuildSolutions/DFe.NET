using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using System.Collections.Generic;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class IPI
    {
        public IPI(CSTIPI cst, decimal baseCalculo, decimal valorTotal, decimal aliquota, int codigoEnquadramento)
        {
            CST = cst;
            BaseCalculo = baseCalculo;
            ValorTotal = valorTotal;
            Aliquota = aliquota;
            CodigoEnquadramento = codigoEnquadramento;
        }

        public CSTIPI CST { get; }

        public decimal BaseCalculo { get; }

        public decimal ValorTotal { get; }

        public decimal Aliquota { get; }

        public int CodigoEnquadramento { get; }

        /// <summary>
        /// Devolve a lista de enums do tipo <see cref="CSTIPI"/> que são Tributados/>
        /// </summary>
        public static ISet<CSTIPI> IpiCstTributada = new HashSet<CSTIPI>()
        {
            CSTIPI.ipi00, CSTIPI.ipi49, CSTIPI.ipi50, CSTIPI.ipi99
        };
    }
}
