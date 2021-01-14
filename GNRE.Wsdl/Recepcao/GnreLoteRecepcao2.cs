using System.Security.Cryptography.X509Certificates;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace GNRE.Wsdl.Recepcao
{
    [WebServiceBindingAttribute(Name = "GnreLoteRecepcao2", Namespace = "http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao")]
    public class GnreLoteRecepcao2 : SoapHttpClientProtocol, INfeServico
    {
        public GnreLoteRecepcao2(string url, X509Certificate certificado, int timeOut)
        {
            SoapVersion = SoapProtocolVersion.Soap12;
            Url = url;
            Timeout = timeOut;
            ClientCertificates.Add(certificado);
        }

        public gnreCabecMsg gnreCabecMsg { get; set; }

        [SoapDocumentMethod("http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao/processar", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Bare)]
        [return: XmlElement("processarResponse", Namespace = "http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao")]
        [WebMethod(MessageName = "processar")]
        public XmlNode Execute([XmlElement(Namespace = "http://www.gnre.pe.gov.br/webservice/GnreLoteRecepcao")] XmlNode gnreDadosMsg)
        {
            var results = Invoke("processar", new object[] { gnreDadosMsg });
            return (XmlNode)(results[0]);
        }
    }
}
