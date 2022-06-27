using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using CTe.CTeOSDocumento.Common;

namespace GNRE.Wsdl.Recepcao
{
    public class GnreLoteRecepcao2 : INfeServico
    {
        //Envelope SOAP para envio
        private SoapEnvelope soapEnvelope;

        //Configurações do WSDL para estabelecimento da comunicação
        private readonly WsdlConfiguracao configuracao;

        public GnreLoteRecepcao2(string url, X509Certificate certificado, int timeOut)
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
            soapEnvelope = new SoapEnvelope
            {
                head = new ResponseHead<gnreCabecMsg>
                {
                    nfeCabecMsg = gnreCabecMsg
                }
            };

            soapEnvelope.body = new ResponseBody<XmlNode>
            {
                gnreDadosMsg = gnreDadosMsg
            };

            return RequestBuilderAndSender.Execute(soapEnvelope, configuracao,
                actionUrn: "http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao/processar",
                responseElementName: "processarResponse"
                ).FirstChild;
        }

        /// <summary>
        /// Classe base para a serialização no formato do envelope SOAP.
        /// </summary>
        [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public class SoapEnvelope : CommonSoapEnvelope
        {
            [XmlElement(ElementName = "Header", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
            public ResponseHead<gnreCabecMsg> head { get; set; }

            [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
            public ResponseBody<XmlNode> body { get; set; }
        }

        /// <summary>
        /// Classe para o cabeçalho do Envelope SOAP
        /// </summary>
        public class ResponseHead<T> : CommonResponseHead
        {
            [XmlElement(Namespace = "http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao")]
            public T nfeCabecMsg { get; set; }
        }

        /// <summary>
        /// Classe para o corpo do Envelope SOAP
        /// </summary>
        public class ResponseBody<T> : CommonResponseBody
        {
            [XmlElement(Namespace = "http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao")]
            public T gnreDadosMsg { get; set; }
        }
    }
}