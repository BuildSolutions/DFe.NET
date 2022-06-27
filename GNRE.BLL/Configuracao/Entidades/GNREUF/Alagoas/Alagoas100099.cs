using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Alagoas
{
    public class Alagoas100099 : GNRE
    {
        public Alagoas100099(Emitente emitente,
            Destinatario destinatario,
            int produto,
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
            Receita = Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao;
            UF = DFe.Classes.Entidades.Estado.AL;

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
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraAL.ChaveAcessoNFe, ETipoCampoExtraAL.ChaveAcessoNFe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
