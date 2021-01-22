using System;
using System.Collections.Generic;
using DFe.Utils;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Ceara
{
    public class Ceara100099 : GNRE
    {
        public Ceara100099(Emitente emitente,
            Destinatario destinatario,
            int produto,
            double notaFiscalNumero,
            DateTime dataSaidaMercadoria,
            DateTime dataVencimento,
            DateTime dataPagamento,
            decimal valorST,
            decimal valorFECP = 0,
            string convenio = null)
        {
            NotaFiscalReferencia = notaFiscalNumero;
            Versao = DFe.Classes.Flags.VersaoServico.Versao200;
            Receita = Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao;
            UF = DFe.Classes.Entidades.Estado.CE;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.NotaFiscal;
            DocumentoNumero = notaFiscalNumero.ToString();

            Produto = produto;
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraCE.DataSaidaMercadoria, ETipoCampoExtraCE.DataSaidaMercadoria.ToString(), dataSaidaMercadoria.ParaDataString()) };
            Convenio = convenio.SanitizeString();
        }
    }
}
