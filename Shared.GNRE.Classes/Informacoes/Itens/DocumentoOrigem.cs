using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Informacoes.Itens
{
    public class DocumentoOrigem
    {
        [System.Xml.Serialization.XmlAttribute]
        public ETipoDocumento tipo { get; set; }

        [System.Xml.Serialization.XmlText]
        public string valor { get; set; }
    }
}
