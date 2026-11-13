using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using NFe.BLL.Enums;
using NFe.BLL.Validators;
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
            var cnpj = emitenteCNPJ.SanitizeString();

            Conferir(cnpj, emitenteUF, serie, numeroNotaFiscal);

            TipoDocumento = EDocumentoReferenciado.NF;
            Modelo = refMod.modelo;
            DataEmissao = dataEmissao.ToString("yyMM");
            EmitenteCNPJ = cnpj;
            EmitenteCodigoUF = emitenteUF;
            Serie = serie;
            Numero = numeroNotaFiscal;
        }

        /// <summary>
        /// Mesma régua do refNFP: CNPJ com 14 dígitos (BA06), UF entre as 27 unidades da
        /// federação (BA04), série de 0 a 999 (BA08) e número de 1 a 999999999 (BA09).
        /// Antes o construtor aceitava tudo e o erro só aparecia na validação do XML.
        /// </summary>
        private static void Conferir(string emitenteCNPJ, Estado emitenteUF, int serie, int numeroNotaFiscal)
        {
            if (!ReferenciaDeDocumento.SomenteDigitos(emitenteCNPJ, 14))
            {
                throw new ArgumentException($"CNPJ inválido na NF modelo 1/1A referenciada (refNF): \"{emitenteCNPJ}\". O leiaute exige 14 dígitos.");
            }

            if (!ReferenciaDeDocumento.UFValida(emitenteUF))
            {
                throw new ArgumentException($"UF inválida na NF modelo 1/1A referenciada (refNF): {emitenteUF}. O leiaute aceita somente as 27 unidades da federação.");
            }

            if (!ReferenciaDeDocumento.SerieValida(serie))
            {
                throw new ArgumentException($"Série inválida na NF modelo 1/1A referenciada (refNF): {serie}. O leiaute aceita de 0 a 999.");
            }

            if (!ReferenciaDeDocumento.NumeroValido(numeroNotaFiscal))
            {
                throw new ArgumentException($"Número inválido na NF modelo 1/1A referenciada (refNF): {numeroNotaFiscal}. O leiaute aceita de 1 a 999999999.");
            }
        }

        public string DataEmissao { get; }

        public string EmitenteCNPJ { get; }

        public Estado EmitenteCodigoUF { get; }

        public refMod Modelo { get; }

        public int Serie { get; }

        public int Numero { get; }
    }
}
