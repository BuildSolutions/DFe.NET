using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
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

        public COFINS(COFINSAliq COFINS)
        {
            CST = COFINS.CST;
            BaseCalculo = COFINS.vBC;
            ValorTotal = COFINS.vCOFINS;
            Aliquota = COFINS.pCOFINS;
        }

        public COFINS(COFINSOutr COFINS)
        {
            CST = COFINS.CST;
            BaseCalculo = COFINS.vBC ?? 0M;
            ValorTotal = COFINS.vCOFINS ?? 0M;
            Aliquota = COFINS.pCOFINS ?? 0M;
        }

        public COFINS(COFINSNT COFINS)
        {
            CST = COFINS.CST;
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

        public static COFINS ObterCOFINS(NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.COFINS COFINS)
        {
            switch (COFINS.TipoCOFINS.GetType().Name)
            {
                case nameof(COFINSAliq):
                    return new COFINS((COFINSAliq)COFINS.TipoCOFINS);
                case nameof(COFINSNT):
                    return new COFINS((COFINSNT)COFINS.TipoCOFINS);
                case nameof(COFINSOutr):
                    return new COFINS((COFINSOutr)COFINS.TipoCOFINS);
                default:
                    return null;
            }
        }
    }
}
