using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.RioGrandeDoNorte
{
    public class RioGrandeDoNorte100099 : GNRE
    {
        public RioGrandeDoNorte100099(Emitente emitente,
            Destinatario destinatario,
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
            Receita = Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao;
            UF = DFe.Classes.Entidades.Estado.RN;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            PeriodoReferencia = periodoReferencia;
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraRN.ChaveAcessoNFe, ETipoCampoExtraRN.ChaveAcessoNFe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
