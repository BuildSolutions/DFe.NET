using System.Collections.Generic;
using System.Xml.Serialization;

namespace GNRE.Classes.Servicos.Recepcao
{
    [XmlRoot(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TLote_GNRE
    {
        public TLote_GNRE(GuiasGNRE guias)
        {
            this.guias = guias;
            versao = "2.00";
        }

        internal TLote_GNRE()
        {
        }

        //[XmlElement("guias")]
        public GuiasGNRE guias { get; set; }

        [XmlAttribute]
        public string versao { get; set; }
    }
}
