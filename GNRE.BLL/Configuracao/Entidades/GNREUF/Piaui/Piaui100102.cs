using System;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Piaui
{
    public class Piaui100102 : GNRE
    {
        public Piaui100102(Emitente emitente,
            Destinatario destinatario,
            double notaFiscalNumero,
            DateTime dataVencimento,
            DateTime dataPagamento,
            decimal valorST,
            decimal valorFECP = 0,
            string convenio = null)
        {
            NotaFiscalReferencia = notaFiscalNumero;
            Versao = DFe.Classes.Flags.VersaoServico.Versao200;
            Receita = Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao;
            UF = DFe.Classes.Entidades.Estado.PI;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.NotaFiscal;
            DocumentoNumero = notaFiscalNumero.ToString();

            Convenio = convenio.SanitizeString();
        }
    }
}
