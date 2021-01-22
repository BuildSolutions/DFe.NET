using System.Xml.Serialization;

namespace NFe.Classes.Servicos.Suframa
{
    [XmlType(AnonymousType = true, Namespace = "http://www.portal.fucapi.br")]
    public class loteNotaFiscal
    {

        [XmlAttribute()]
        public string chaveAcesso { get; set; }

        [XmlAttribute()]
        public bool txZero { get; set; }
    }
}
