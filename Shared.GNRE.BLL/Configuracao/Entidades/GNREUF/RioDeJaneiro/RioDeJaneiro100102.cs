using System;
using System.Collections.Generic;
using DFe.Utils;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.RioDeJaneiro
{
    public class RioDeJaneiro100102 : GNRE
    {
        public RioDeJaneiro100102(Emitente emitente,
            Destinatario destinatario,
            int produto,
            double notaFiscalNumero,
            string chaveAcessoNFe,
            DateTime dataEmissao,
            DateTime dataVencimento,
            DateTime dataPagamento,
            decimal valorST,
            decimal valorFECP = 0,
            string convenio = null)
        {
            NotaFiscalReferencia = notaFiscalNumero;
            Versao = DFe.Classes.Flags.VersaoServico.Versao200;
            Receita = Classes.Enumerators.EReceita.ICMSConsumidorFinalNaoContribuinteOutraUFOperacao;
            UF = DFe.Classes.Entidades.Estado.RJ;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.ChaveDFe;
            DocumentoNumero = chaveAcessoNFe;

            Produto = produto;
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraRJ.DataEmissao, ETipoCampoExtraRJ.DataEmissao.ToString(), dataEmissao.ParaDataString()) };
            Convenio = convenio.SanitizeString();
        }
    }
}
