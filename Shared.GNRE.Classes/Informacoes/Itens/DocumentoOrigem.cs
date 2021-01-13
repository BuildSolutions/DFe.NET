using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Informacoes.Itens
{
    public class DocumentoOrigem
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ETipoDocumento tipo { get; set; }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string valor { get; set; }
    }
}
