using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    public class MotivoRejeicao
    {
        [XmlElement("motivo")]
        public List<Motivo> motivo { get; set; }

        public override string ToString()
        {
            return string.Join("\r\n", motivo.Select(erro => erro.ToString()));
        }
    }
}
