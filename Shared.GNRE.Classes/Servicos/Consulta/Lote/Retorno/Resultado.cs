using System.Collections.Generic;
using System.Xml.Serialization;

namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    public class Resultado
    {
        [XmlElement("guia")]
        public List<GuiasGNRE> guia { get; set; }
    }
}
