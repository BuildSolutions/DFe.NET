using System;
using NFe.BLL.Enums;
using NFe.BLL.Validators;

namespace NFe.BLL.Configuracao.Entidades
{
    /// <summary>
    /// Documento referenciado por chave de acesso. Cobre as três formas do grupo BA que
    /// são uma chave de 44 posições: refNFe (BA02), refNFeSig (BA02a) e refCTe (BA19).
    /// <para>As formas compostas - refNF, refNFP e refECF - têm classes próprias, porque
    /// não são chave e sim um conjunto de campos.</para>
    /// </summary>
    public class DocumentoReferenciadoNFe : DocumentoReferenciado
    {
        public DocumentoReferenciadoNFe(string chaveAcesso,
            EDocumentoReferenciado tipoDocumento = EDocumentoReferenciado.NFe)
        {
            ConferirTipoDeDocumento(tipoDocumento);

            var chave = ChaveAcessoDFe.Normalizar(chaveAcesso);

            ConferirChaveDeAcesso(chave, tipoDocumento);
            ConferirModeloDaChave(chave, tipoDocumento);

            TipoDocumento = tipoDocumento;
            ChaveAcesso = chave;
        }

        public string ChaveAcesso { get; }

        /// <summary>
        /// Esta classe serve somente as formas que sao uma chave de acesso. refNF, refNFP
        /// e refECF tem estrutura propria e nao cabem aqui.
        /// </summary>
        private static void ConferirTipoDeDocumento(EDocumentoReferenciado tipoDocumento)
        {
            if (tipoDocumento != EDocumentoReferenciado.NFe
                && tipoDocumento != EDocumentoReferenciado.NFeSigilosa
                && tipoDocumento != EDocumentoReferenciado.CTe)
            {
                throw new ArgumentException($"O documento referenciado {tipoDocumento} não é referenciado por chave de acesso.");
            }
        }

        /// <summary>
        /// O modelo vive na propria chave (posicoes 21 e 22) e tem de combinar com a tag
        /// escolhida: refNFe e refNFeSig sao NF-e e NFC-e, refCTe e CT-e. Sem esta
        /// conferencia dava para referenciar uma chave de NFC-e como refCTe, que e
        /// rejeicao da SEFAZ sem dizer qual referencia.
        /// <para>O modelo 59 (CF-e-SAT) entra no refNFe porque e assim que a NF-e de
        /// fechamento de CFOP 5929/6929 referencia o cupom do SAT: nao existe refCFe no
        /// leiaute.</para>
        /// <para>refNFeSig e "chave de acesso com codigo numerico zerado": o cNF (posicoes
        /// 36 a 43) tem de ser oito zeros, senao e uma chave normal na tag errada.</para>
        /// </summary>
        private static void ConferirModeloDaChave(string chaveAcesso, EDocumentoReferenciado tipoDocumento)
        {
            var modelo = chaveAcesso.Substring(20, 2);

            var modelosAceitos = tipoDocumento == EDocumentoReferenciado.CTe
                ? new[] { "57", "67" }
                : new[] { "55", "65", "59" };

            if (Array.IndexOf(modelosAceitos, modelo) < 0)
            {
                throw new ArgumentException($"A chave do documento referenciado {tipoDocumento} é do modelo {modelo}, que não corresponde ao tipo informado: \"{chaveAcesso}\".");
            }

            if (tipoDocumento == EDocumentoReferenciado.NFeSigilosa
                && chaveAcesso.Substring(35, 8) != "00000000")
            {
                throw new ArgumentException($"A chave do documento referenciado {tipoDocumento} tem de ter o código numérico (cNF) zerado: \"{chaveAcesso}\".");
            }
        }

        /// <summary>
        /// A chave fora do padrao vira rejeicao de schema sem apontar o item, por isso e
        /// recusada aqui.
        /// </summary>
        private static void ConferirChaveDeAcesso(string chaveAcesso, EDocumentoReferenciado tipoDocumento)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
            {
                throw new ArgumentException($"Documento referenciado {tipoDocumento} sem chave de acesso.");
            }

            if (!ChaveAcessoDFe.FormatoValido(chaveAcesso))
            {
                throw new ArgumentException($"Chave de acesso inválida no documento referenciado {tipoDocumento}: \"{chaveAcesso}\". O leiaute exige 44 posições no padrão da chave do DF-e.");
            }

            // Numero trocado passa pelo padrao e volta como rejeicao de chave invalida,
            // sem dizer qual referencia - por isso o DV e conferido aqui.
            //
            // refNFeSig entra na mesma conferencia. O leiaute diz so "chave de acesso com
            // codigo numerico zerado" (leiauteNFe_v4.00.xsd) e usa o MESMO tipo TChNFe do
            // refNFe, entao a leitura e que o DV e recalculado sobre o que vai no campo -
            // manter o DV da chave original cairia na rejeicao de DV. Nao achei a regra
            // escrita na NT; se algum dia aparecer rejeicao de refNFeSig valido, e aqui.
            if (!ChaveAcessoDFe.DigitoVerificadorValido(chaveAcesso))
            {
                throw new ArgumentException($"Dígito verificador inválido na chave do documento referenciado {tipoDocumento}: \"{chaveAcesso}\".");
            }
        }
    }
}
