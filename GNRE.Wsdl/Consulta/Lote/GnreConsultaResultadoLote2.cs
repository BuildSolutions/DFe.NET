using System.Security.Cryptography.X509Certificates;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace GNRE.Wsdl.Consulta.Lote
{
    [WebServiceBinding(Name = "GnreConsultaResultadoLoteServico", Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreResultadoLote")]
    public class GnreConsultaResultadoLote2 : SoapHttpClientProtocol, INfeServico
    {
        public GnreConsultaResultadoLote2(string url, X509Certificate certificado, int timeOut)
        {
            SoapVersion = SoapProtocolVersion.Soap12;
            Url = url;
            Timeout = timeOut;
            ClientCertificates.Add(certificado);
        }

        [XmlAttribute(Namespace = "http://www.gnre.pe.gov.br/webservice/GnreResultadoLote")]
        public gnreCabecMsg gnreCabecMsg { get; set; }

        [SoapHeader("gnreCabecMsg")]
        [SoapDocumentMethod("http://www.gnre.pe.gov.br/webservice/GnreResultadoLote/consultar", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Bare)]
        [return: XmlElement("gnreRespostaMsg", Namespace = "http://www.gnre.pe.gov.br/webservice/GnreResultadoLote")]
        [WebMethod(MessageName = "consultar")]
        public XmlNode Execute([XmlElement(Namespace = "http://www.gnre.pe.gov.br/webservice/GnreResultadoLote")] XmlNode gnreDadosMsg)
        {
            var results = Invoke("consultar", new object[] { gnreDadosMsg });
            return (XmlNode)(results[0]);
        }
    }
}
