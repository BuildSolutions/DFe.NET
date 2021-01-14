using System.Security.Cryptography.X509Certificates;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace GNRE.Wsdl.Consulta.Lote
{
    [WebServiceBinding(Name = "GnreConsultaResultadoLote2", Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreResultadoLote")]
    public class GnreConsultaResultadoLote2 : SoapHttpClientProtocol, INfeServico
    {
        public GnreConsultaResultadoLote2(string url, X509Certificate certificado, int timeOut)
        {
            SoapVersion = SoapProtocolVersion.Soap12;
            Url = url;
            Timeout = timeOut;
            ClientCertificates.Add(certificado);
        }

        public gnreCabecMsg gnreCabecMsg { get; set; }

        [SoapDocumentMethod("http://www.gnre.pe.gov.br/webservice/GnreResultadoLote/consultar", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Bare)]
        [return: XmlElement("gnreRespostaMsg", Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreResultadoLote")]
        [WebMethod(MessageName = "consultar")]
        public XmlNode Execute([XmlElement(Namespace = "http://www.gnre.pe.gov.br/gnreWS/services/GnreResultadoLote")] XmlNode gnreDadosMsg)
        {
            var results = Invoke("consultar", new object[] { gnreDadosMsg });
            return (XmlNode)(results[0]);
        }
    }
}
