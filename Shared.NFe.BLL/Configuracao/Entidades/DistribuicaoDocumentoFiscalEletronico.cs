using System;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFe.BLL.Configuracao.Entidades
{
    public class DistribuicaoDocumentoFiscalEletronico
    {
        public DistribuicaoDocumentoFiscalEletronico(
            ETipoDocumentoFiscalDFe eTipoDocumentoFiscalDFe,
            string chaveAcesso,
            string emitenteCNPJ,
            string emitenteRazaoSocial,
            DateTime dataEmissao,
            TipoNFe eTipoNotaFiscal,
            decimal valorTotal,
            ESituacaoNFe eSituacaoNotaFiscal,
            string codigoNFeImportada,
            string xmlPath = null)
        {
            ETipoDocumentoFiscalDFe = eTipoDocumentoFiscalDFe;
            ChaveAcesso = chaveAcesso;
            EmitenteCNPJ = emitenteCNPJ;
            EmitenteRazaoSocial = emitenteRazaoSocial;
            DataEmissao = dataEmissao;
            ETipoNotaFiscal = eTipoNotaFiscal;
            ValorTotal = valorTotal;
            ESituacaoNotaFiscal = eSituacaoNotaFiscal;
            CodigoNFeImportada = codigoNFeImportada;
            XmlPath = xmlPath;
        }

        public ETipoDocumentoFiscalDFe ETipoDocumentoFiscalDFe { get; }
        public string ChaveAcesso { get; }
        public string EmitenteCNPJ { get; }
        public string EmitenteRazaoSocial { get; }
        public DateTime DataEmissao { get; }
        public TipoNFe ETipoNotaFiscal { get; }
        public decimal ValorTotal { get; }
        public ESituacaoNFe ESituacaoNotaFiscal { get; }
        public string CodigoNFeImportada { get; private set; }
        public string XmlPath { get; private set; }

        public void SetXmlPath(string xmlPath)
        {
            XmlPath = xmlPath;
        }

        public void SetCodigoNFeImportada(string codigoNFeImportada)
        {
            CodigoNFeImportada = codigoNFeImportada;
        }
    }
}
