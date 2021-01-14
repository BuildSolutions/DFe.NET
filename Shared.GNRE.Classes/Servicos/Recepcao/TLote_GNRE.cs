using System.Collections.Generic;
using System.Xml.Serialization;

namespace GNRE.Classes.Servicos.Recepcao
{
    [XmlRootAttribute(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TLote_GNRE
    {
        public TLote_GNRE(List<GuiasGNRE> guias)
        {
            this.guias = guias;
            versao = "2.00";
        }

        internal TLote_GNRE()
        {

        }

        [XmlElement("guias")]
        public List<GuiasGNRE> guias { get; set; }

        [XmlAttributeAttribute()]
        public string versao { get; set; }
    }
}
