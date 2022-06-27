using System;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Bahia
{
    public class Bahia100099 : GNRE
    {
        public Bahia100099(Emitente emitente,
            int produto,
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
            UF = DFe.Classes.Entidades.Estado.BA;

            Emitente = emitente;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.NotaFiscal;
            DocumentoNumero = notaFiscalNumero.ToString();

            EPeriodoApuracao = Classes.Enumerators.EPeriodoApuracao.Mensal;

            Produto = produto;
            PeriodoReferencia = periodoReferencia;
            Convenio = convenio.SanitizeString();
        }
    }
}
