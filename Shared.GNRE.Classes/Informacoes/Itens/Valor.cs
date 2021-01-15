using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Informacoes.Itens
{
    public class Valor
    {
        [System.Xml.Serialization.XmlAttribute]
        public ETipoValor tipo { get; set; }

        [System.Xml.Serialization.XmlText]
        public string valor { get; set; }
    }
}
