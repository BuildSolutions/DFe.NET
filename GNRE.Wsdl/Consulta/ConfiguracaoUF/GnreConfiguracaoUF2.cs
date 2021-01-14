using System.Security.Cryptography.X509Certificates;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace GNRE.Wsdl.Consulta.ConfiguracaoUF
{
    [WebServiceBindingAttribute(Name = "GnreLoteRecepcao2", Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF")]
    internal class GnreConfiguracaoUF2 : SoapHttpClientProtocol, INfeServico
    {
        public GnreConfiguracaoUF2(string url, X509Certificate certificado, int timeOut)
        {
            SoapVersion = SoapProtocolVersion.Soap12;
            Url = url;
            Timeout = timeOut;
            ClientCertificates.Add(certificado);
        }

        public gnreCabecMsg gnreCabecMsg { get; set; }

        [SoapDocumentMethod("http://www.gnre.pe.gov.br/webservice/GnreConfigUF/consultar", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Bare)]
        [return: XmlElement("gnreRespostaMsg", Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF")]
        [WebMethod(MessageName = "consultar")]
        public XmlNode Execute([XmlElement(Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF")] XmlNode gnreDadosMsg)
        {
            var results = Invoke("consultar", new object[] { gnreDadosMsg });
            return (XmlNode)(results[0]);
        }
    }
}