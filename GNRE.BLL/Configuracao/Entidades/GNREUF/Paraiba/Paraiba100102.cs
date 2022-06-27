using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Paraiba
{
    public class Paraiba100102 : GNRE
    {
        public Paraiba100102(Emitente emitente,
            double notaFiscalNumero,
            string chaveAcessoNFe,
            DateTime periodoReferencia,
            DateTime dataVencimento,
            DateTime dataPagamento,
            decimal valorST,
            decimal valorFECP = 0,
            string convenio = null)
        {
            NotaFiscalReferencia = notaFiscalNumero;
            Versao = DFe.Classes.Flags.VersaoServico.Versao200;
            Receita = Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao;
            UF = DFe.Classes.Entidades.Estado.PB;

            Emitente = emitente;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            PeriodoReferencia = periodoReferencia;
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraPB.ChaveAcessoNFeCTe, ETipoCampoExtraPB.ChaveAcessoNFeCTe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
