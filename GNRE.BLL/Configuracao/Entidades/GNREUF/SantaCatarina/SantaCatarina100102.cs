using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.SantaCatarina
{
    public class SantaCatarina100102 : GNRE
    {
        public SantaCatarina100102(Emitente emitente,
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
            UF = DFe.Classes.Entidades.Estado.SC;

            Emitente = emitente;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.ChaveDFe;
            DocumentoNumero = chaveAcessoNFe;

            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraSC.ChaveAcessoNFe, ETipoCampoExtraSC.ChaveAcessoNFe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
