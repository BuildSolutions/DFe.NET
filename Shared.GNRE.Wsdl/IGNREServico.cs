using System.Xml;

namespace GNRE.Wsdl
{
    public interface INfeServico
    {
        gnreCabecMsg gnreCabecMsg { get; set; }
        XmlNode Execute(XmlNode nfeDadosMsg);
    }
}
