using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Tocantins
{
    public class Tocantins100102 : GNRE
    {
        public Tocantins100102(Emitente emitente,
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
            Receita = Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao;
            UF = DFe.Classes.Entidades.Estado.TO;

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
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraTO.ChaveAcessoNFe, ETipoCampoExtraTO.ChaveAcessoNFe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
