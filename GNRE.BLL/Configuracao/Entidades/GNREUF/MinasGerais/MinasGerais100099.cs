using System;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.MinasGerais
{
    public class MinasGerais100099 : GNRE
    {
        public MinasGerais100099(Emitente emitente,
            Destinatario destinatario,
            double notaFiscalNumero,
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
            UF = DFe.Classes.Entidades.Estado.MG;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.NotaFiscal;
            DocumentoNumero = notaFiscalNumero.ToString();

            PeriodoReferencia = periodoReferencia;
            Convenio = convenio.SanitizeString();
        }
    }
}
