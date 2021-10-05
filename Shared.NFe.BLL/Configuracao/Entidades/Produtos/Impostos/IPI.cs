using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
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

        public IPI(IPITrib ipi, int codigoEnquadramento)
        {
            CST = ipi.CST;
            BaseCalculo = ipi.vBC ?? 0;
            ValorTotal = ipi.vIPI ?? 0;
            Aliquota = ipi.pIPI ?? 0;
            CodigoEnquadramento = codigoEnquadramento;
        }

        public IPI(IPINT ipi, int codigoEnquadramento)
        {
            CST = ipi.CST;
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
            CSTIPI.ipi00, CSTIPI.ipi50
        };

        public static IPI ObterIpi(NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.IPI ipi)
        {
            switch (ipi.TipoIPI.GetType().Name)
            {
                case nameof(IPITrib):
                    return new IPI((IPITrib)ipi.TipoIPI, ipi.cEnq);
                case nameof(IPINT):
                    return new IPI((IPINT)ipi.TipoIPI, ipi.cEnq);
                default:
                    return null;
            }
        }
    }
}
