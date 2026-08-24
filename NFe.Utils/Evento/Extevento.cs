using System;
using System.Security.Cryptography.X509Certificates;
using DFe.Utils;
using NFe.Classes.Servicos.Evento;
using NFe.Classes.Servicos.Tipos;
using NFe.Utils.Assinatura;

namespace NFe.Utils.Evento
{
    public static class Extevento
    {
        /// <summary>
        ///     Converte o objeto evento para uma string no formato XML
        /// </summary>
        /// <param name="pedEvento"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto evento</returns>
        public static string ObterXmlString(this evento pedEvento)
        {
            return FuncoesXml.ClasseParaXmlString(pedEvento);
        }

        /// <summary>
        ///     Obtém o Id de um evento (infEvento/@Id): literal "ID" + tpEvento + chNFe + nSeqEvento com 2 dígitos
        ///     <para>
        ///         Uso opcional, para quando o evento for montado fora da biblioteca — por exemplo, para assinar com o
        ///         certificado numa máquina cliente e depois transmitir com a sobrecarga que recebe o evento já
        ///         assinado. Os métodos que assinam internamente continuam calculando o Id sozinhos.
        ///     </para>
        /// </summary>
        /// <param name="tpEvento">Código do evento</param>
        /// <param name="chNFe">Chave de acesso da NF-e vinculada ao evento</param>
        /// <param name="nSeqEvento">Sequencial do evento para o mesmo tipo de evento</param>
        /// <returns>Retorna o conteúdo do atributo infEvento/@Id</returns>
        public static string ObterId(NFeTipoEvento tpEvento, string chNFe, int nSeqEvento)
        {
            return "ID" + ((int)tpEvento) + chNFe + nSeqEvento.ToString().PadLeft(2, '0');
        }

        /// <summary>
        ///     Assina um objeto evento
        /// </summary>
        /// <param name="evento"></param>
        /// <param name="certificadoDigital">Informe o certificado digital, se já possuir esse em cache, evitando novo acesso ao certificado</param>
        /// <param name="signatureMethodSignedXml"></param>
        /// <param name="digestMethodReference"></param>
        /// <param name="removerAcentos"></param>
        /// <returns>Retorna um objeto do tipo evento assinado</returns>
        public static evento Assina(this evento evento, X509Certificate2 certificadoDigital,
            string signatureMethodSignedXml = "http://www.w3.org/2000/09/xmldsig#rsa-sha1",
            string digestMethodReference = "http://www.w3.org/2000/09/xmldsig#sha1", bool removerAcentos = false)
        {
            var eventoLocal = evento;
            if (eventoLocal.infEvento.Id == null)
                throw new Exception("Não é possível assinar um objeto evento sem sua respectiva Id!");

            var assinatura = Assinador.ObterAssinatura(eventoLocal, eventoLocal.infEvento.Id, certificadoDigital, false, signatureMethodSignedXml, digestMethodReference, removerAcentos);
            eventoLocal.Signature = assinatura;
            return eventoLocal;
        }
    }
}