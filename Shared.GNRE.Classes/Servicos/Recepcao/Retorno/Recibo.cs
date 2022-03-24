using System;
using System.Xml.Serialization;
using DFe.Utils;

namespace GNRE.Classes.Servicos.Recepcao.Retorno
{
    public class Recibo
    {
        /// <summary>
        /// Número do recibo gerado pelo portal GNRE
        /// </summary>
        public long numero { get; set; }

        [XmlIgnore]
        public DateTime? dataHoraRecibo { get; set; }

        [XmlElement(ElementName = "dataHoraRecibo")]
        public string ProxyDataHoraRecibo
        {
            get
            {
                if (dataHoraRecibo == null)
                {
                    return null;
                }

                return dataHoraRecibo.Value.ParaDataString();
            }
            set => dataHoraRecibo = Convert.ToDateTime(value);
        }

        public int tempoEstimadoProc { get; set; }
    }
}
