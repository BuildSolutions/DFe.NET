using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Informacoes.Itens
{
    public class DocumentoOrigem
    {
        public DocumentoOrigem(ETipoDocumento tipo, string valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        internal DocumentoOrigem()
        {

        }

        [System.Xml.Serialization.XmlAttribute]
        public ETipoDocumento tipo { get; set; }

        [System.Xml.Serialization.XmlText]
        public string valor { get; set; }
    }
}
