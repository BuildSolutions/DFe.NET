using System.Collections.Generic;
using System.Xml.Serialization;
using GNRE.Classes.Informacoes.Dados;

namespace GNRE.Classes
{
    public class GuiasGNRE
    {
        public GuiasGNRE(List<TDadosGNRE> tDadosGNRE)
        {
            TDadosGNRE = tDadosGNRE;
        }

        internal GuiasGNRE()
        {

        }

        [XmlElement("TDadosGNRE")]
        public List<TDadosGNRE> TDadosGNRE { get; set; }
    }
}
