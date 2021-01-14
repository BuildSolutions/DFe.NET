using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using CTe.CTeOSDocumento.Common;

namespace GNRE.Wsdl.Consulta.ConfiguracaoUF
{
    public class GnreConfiguracaoUF2 : INfeServico
    {
        //Envelope SOAP para envio
        private SoapEnvelope soapEnvelope;

        //Configurações do WSDL para estabelecimento da comunicação
        private readonly WsdlConfiguracao configuracao;

        public GnreConfiguracaoUF2(string url, X509Certificate certificado, int timeOut)
        {
            configuracao = new WsdlConfiguracao()
            {
                Url = url,
                CertificadoDigital = new X509Certificate2(certificado),
                TimeOut = timeOut
            };
        }

        public gnreCabecMsg gnreCabecMsg { get; set; }

        public XmlNode Execute(XmlNode gnreDadosMsg)
        {
            soapEnvelope = new SoapEnvelope()
            {
            };

            soapEnvelope.body = new ResponseBody<XmlNode>
            {
                gnreDadosMsg = gnreDadosMsg
            };

            return RequestBuilderAndSender.Execute(soapEnvelope, configuracao,
                actionUrn: "http://www.gnre.pe.gov.br/webservice/GnreConfigUF/consultar",
                responseElementName: "gnreRespostaMsg"
                ).FirstChild;
        }

        /// <summary>
        /// Classe base para a serialização no formato do envelope SOAP.
        /// </summary>
        [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public class SoapEnvelope : CommonSoapEnvelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
            public ResponseBody<XmlNode> body { get; set; }
        }

        /// <summary>
        /// Classe para o corpo do Envelope SOAP
        /// </summary>
        public class ResponseBody<T> : CommonResponseBody
        {
            [XmlElement(Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF")]
            public T gnreDadosMsg { get; set; }
        }
    }
}