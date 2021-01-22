using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Amazonas
{
    public class Amazonas100102 : GNRE
    {
        public Amazonas100102(Emitente emitente,
            Destinatario destinatario,
            int produto,
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
            UF = DFe.Classes.Entidades.Estado.AM;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.ChaveNFe;
            DocumentoNumero = chaveAcessoNFe;

            EPeriodoApuracao = Classes.Enumerators.EPeriodoApuracao.Mensal;
            PeriodoReferencia = periodoReferencia;

            Produto = produto;
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraAM.ChaveAcessoNFeCTe, ETipoCampoExtraAM.ChaveAcessoNFeCTe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
