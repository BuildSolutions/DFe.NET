using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Pernambuco
{
    public class Pernambuco100102 : GNRE
    {
        public Pernambuco100102(Emitente emitente,
            Destinatario destinatario,
            double notaFiscalNumero,
            string chaveAcessoNFe,
            DateTime dataVencimento,
            DateTime dataPagamento,
            decimal valorST,
            decimal valorFECP = 0,
            string convenio = null)
        {
            NotaFiscalReferencia = notaFiscalNumero;
            Versao = DFe.Classes.Flags.VersaoServico.Versao200;
            Receita = Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao;
            UF = DFe.Classes.Entidades.Estado.PE;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.ChaveDFe;
            DocumentoNumero = chaveAcessoNFe;

            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraPE.ChaveAcessoNFeCTe, ETipoCampoExtraPE.ChaveAcessoNFeCTe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
