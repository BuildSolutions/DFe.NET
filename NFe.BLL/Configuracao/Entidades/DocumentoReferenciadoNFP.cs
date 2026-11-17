using System;
using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using NFe.BLL.Enums;
using NFe.BLL.Validators;

namespace NFe.BLL.Configuracao.Entidades
{
    /// <summary>
    /// BA10 - refNFP, nota fiscal de produtor rural (modelo 04 ou nota avulsa 01).
    /// <para>É uma das seis formas do xs:choice de NFref. O emitente é identificado por
    /// CNPJ <b>ou</b> CPF, nunca os dois: o leiaute também faz essa escolha.</para>
    /// </summary>
    public class DocumentoReferenciadoNFP : DocumentoReferenciado
    {
        public DocumentoReferenciadoNFP(DateTime dataEmissao,
            Estado emitenteUF,
            string emitenteInscricaoEstadual,
            string modelo,
            int serie,
            int numeroNotaFiscal,
            string emitenteCNPJ = null,
            string emitenteCPF = null)
        {
            var cnpj = emitenteCNPJ.SanitizeString();
            var cpf = emitenteCPF.SanitizeString();

            ConferirEmitente(cnpj, cpf);
            ConferirModelo(modelo);
            ConferirInscricaoEstadual(emitenteInscricaoEstadual);
            ConferirSerieENumero(serie, numeroNotaFiscal);
            ConferirUF(emitenteUF);

            TipoDocumento = EDocumentoReferenciado.NFProdutorRural;
            DataEmissao = dataEmissao.ToString("yyMM");
            EmitenteCodigoUF = emitenteUF;
            EmitenteCNPJ = cnpj;
            EmitenteCPF = cpf;
            EmitenteInscricaoEstadual = emitenteInscricaoEstadual.SanitizeString();
            Modelo = modelo;
            Serie = serie;
            Numero = numeroNotaFiscal;
        }

        /// <summary>BA12 - AAMM da emissão.</summary>
        public string DataEmissao { get; }

        /// <summary>BA11 - UF do emitente.</summary>
        public Estado EmitenteCodigoUF { get; }

        /// <summary>BA13 - CNPJ do emitente. Nulo quando o emitente é pessoa física.</summary>
        public string EmitenteCNPJ { get; }

        /// <summary>BA14 - CPF do emitente. Nulo quando o emitente é pessoa jurídica.</summary>
        public string EmitenteCPF { get; }

        /// <summary>BA15 - Inscrição estadual do emitente.</summary>
        public string EmitenteInscricaoEstadual { get; }

        /// <summary>BA16 - Modelo do documento: 04 (produtor rural) ou 01 (nota avulsa).</summary>
        public string Modelo { get; }

        /// <summary>BA17 - Série.</summary>
        public int Serie { get; }

        /// <summary>BA18 - Número.</summary>
        public int Numero { get; }

        /// <summary>
        /// BA16 aceita só 01 (nota avulsa) e 04 (produtor rural): é um xs:enumeration no
        /// leiaute, e qualquer outro valor vira rejeição de schema sem apontar o item.
        /// </summary>
        private static void ConferirModelo(string modelo)
        {
            if (modelo != "01" && modelo != "04")
            {
                throw new ArgumentException($"Modelo inválido na nota de produtor rural referenciada (refNFP): \"{modelo}\". O leiaute aceita 01 ou 04.");
            }
        }

        /// <summary>
        /// BA17 é TSerie (0 a 999) e BA18 vai de 1 a 999999999 no leiaute. Fora da faixa o
        /// objeto só falharia na validação do XML, longe de quem o construiu.
        /// </summary>
        private static void ConferirSerieENumero(int serie, int numeroNotaFiscal)
        {
            if (!ReferenciaDeDocumento.SerieValida(serie))
            {
                throw new ArgumentException($"Série inválida na nota de produtor rural referenciada (refNFP): {serie}. O leiaute aceita de 0 a 999.");
            }

            if (!ReferenciaDeDocumento.NumeroValido(numeroNotaFiscal))
            {
                throw new ArgumentException($"Número inválido na nota de produtor rural referenciada (refNFP): {numeroNotaFiscal}. O leiaute aceita de 1 a 999999999.");
            }
        }

        /// <summary>
        /// BA11 é TCodUfIBGE: só as 27 unidades da federação. O enum <see cref="Estado"/>
        /// também carrega os códigos de ambiente nacional (AN 91), SVRS (92) e exterior
        /// (EX 99), que não são UF e não cabem aqui.
        /// </summary>
        private static void ConferirUF(Estado emitenteUF)
        {
            if (!ReferenciaDeDocumento.UFValida(emitenteUF))
            {
                throw new ArgumentException($"UF inválida na nota de produtor rural referenciada (refNFP): {emitenteUF}. O leiaute aceita somente as 27 unidades da federação.");
            }
        }

        /// <summary>
        /// BA15 é TIeDest: "ISENTO" ou de 2 a 14 dígitos. Presença não basta - "A" passava.
        /// </summary>
        private static void ConferirInscricaoEstadual(string emitenteInscricaoEstadual)
        {
            var inscricao = emitenteInscricaoEstadual?.Trim();

            if (string.IsNullOrWhiteSpace(inscricao))
            {
                throw new ArgumentException("A nota de produtor rural referenciada (refNFP) exige a inscrição estadual do emitente.");
            }

            if (inscricao == "ISENTO")
            {
                return;
            }

            if (inscricao.Length < 2 || inscricao.Length > 14 || !ReferenciaDeDocumento.SomenteDigitos(inscricao))
            {
                throw new ArgumentException($"Inscrição estadual inválida na nota de produtor rural referenciada (refNFP): \"{inscricao}\". O leiaute aceita \"ISENTO\" ou de 2 a 14 dígitos.");
            }
        }

        /// <summary>
        /// CNPJ e CPF são um xs:choice no leiaute. Mandar os dois, ou nenhum, é rejeição de
        /// schema sem apontar o item - mais barato recusar aqui. O tamanho entra junto: o
        /// leiaute fixa 14 para o CNPJ e 11 para o CPF.
        /// </summary>
        private static void ConferirEmitente(string emitenteCNPJ, string emitenteCPF)
        {
            var temCNPJ = !string.IsNullOrWhiteSpace(emitenteCNPJ);
            var temCPF = !string.IsNullOrWhiteSpace(emitenteCPF);

            if (temCNPJ == temCPF)
            {
                throw new ArgumentException("A nota de produtor rural referenciada (refNFP) exige CNPJ ou CPF do emitente, e somente um dos dois.");
            }

            //Somente digitos: o CNPJ alfanumerico da NT 2026.004 vale para a chave de
            //acesso, nao para o CNPJ do refNFP, que o leiaute mantem como TCnpj.
            if (temCNPJ && !ReferenciaDeDocumento.SomenteDigitos(emitenteCNPJ, 14))
            {
                throw new ArgumentException($"CNPJ inválido na nota de produtor rural referenciada (refNFP): \"{emitenteCNPJ}\". O leiaute exige 14 dígitos.");
            }

            if (temCPF && !ReferenciaDeDocumento.SomenteDigitos(emitenteCPF, 11))
            {
                throw new ArgumentException($"CPF inválido na nota de produtor rural referenciada (refNFP): \"{emitenteCPF}\". O leiaute exige 11 dígitos.");
            }
        }

    }
}
