using System.Xml;

namespace GNRE.Wsdl
{
    public interface IGnreServico
    {
        gnreCabecMsg gnreCabecMsg { get; set; }
        XmlNode Execute(XmlNode nfeDadosMsg);
    }
}
