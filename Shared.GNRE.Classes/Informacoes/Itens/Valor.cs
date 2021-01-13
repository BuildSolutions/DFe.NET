using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Informacoes.Itens
{
    public class Valor
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ETipoValor tipo { get; set; }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string valor { get; set; }
    }
}
