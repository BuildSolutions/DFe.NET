using System;
using System.Collections.Generic;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.Entidades.GNREUF.Acre
{
    public class Acre100099 : GNRE
    {
        public Acre100099(Emitente emitente,
            Destinatario destinatario,
            int detalhamentoReceita,
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
            Receita = Classes.Enumerators.EReceita.ICMSSubstituicaoTributariaOperacao;
            UF = DFe.Classes.Entidades.Estado.AC;

            Emitente = emitente;
            Destinatario = destinatario;

            ValorST = valorST;
            ValorFECP = valorFECP;
            ValorTotal = valorST + valorFECP;

            DataPagamento = dataPagamento;
            DataVencimento = dataVencimento;

            TipoDocumento = Classes.Enumerators.ETipoDocumento.NotaFiscal;
            DocumentoNumero = notaFiscalNumero.ToString();

            DetalhamentoReceita = detalhamentoReceita;
            Produto = produto.NuloSeZero();
            PeriodoReferencia = periodoReferencia;
            camposExtras = new List<CampoExtra>() { new CampoExtra((int)ETipoCampoExtraAC.ChaveAcessoNFe, ETipoCampoExtraAC.ChaveAcessoNFe.ToString(), chaveAcessoNFe) };
            Convenio = convenio.SanitizeString();
        }
    }
}
