using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using System;

namespace NFe.BLL.Configuracao.Entidades
{
    public class DocumentoReferenciadoNF : DocumentoReferenciado
    {
        public DocumentoReferenciadoNF(DateTime dataEmissao,
            string emitenteCNPJ,
            Estado emitenteUF,
            int serie,
            int numeroNotaFiscal)
        {
            TipoDocumento = EDocumentoReferenciado.NF;
            Modelo = refMod.modelo;
            DataEmissao = dataEmissao.ToString("yyMM");
            EmitenteCNPJ = emitenteCNPJ.SanitizeString();
            EmitenteCodigoUF = emitenteUF;
            Serie = serie;
            Numero = numeroNotaFiscal;
        }

        public string DataEmissao { get; }

        public string EmitenteCNPJ { get; }

        public Estado EmitenteCodigoUF { get; }

        public refMod Modelo { get; }

        public int Serie { get; }

        public int Numero { get; }
    }
}
