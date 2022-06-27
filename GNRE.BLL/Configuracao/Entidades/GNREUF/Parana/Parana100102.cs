using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Parana
{
    public class Parana100102 : GNRE
    {
        public Parana100102(Emitente emitente,
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
            UF = DFe.Classes.Entidades.Estado.PR;

            Emitente = emitente;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.NotaFiscal;
            DocumentoNumero = notaFiscalNumero.ToString();

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraPR.ChaveAcessoNFeCTe, ETipoCampoExtraPR.ChaveAcessoNFeCTe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
