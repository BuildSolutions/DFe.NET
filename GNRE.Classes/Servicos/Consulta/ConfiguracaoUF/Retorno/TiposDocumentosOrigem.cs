using System.Collections.Generic;
using System.Xml.Serialization;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class TiposDocumentosOrigem
    {
        /// <summary>
        /// Especifica cada tipo de documento de origem
        /// </summary>
        [XmlElement("tipoDocumentoOrigem")]
        public List<TipoDocumentoOrigem> tipoDocumentoOrigem { get; set; }
    }

    public class TipoDocumentoOrigem
    {
        /// <summary>
        /// Código do tipo de documento de origem
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// Descrição do tipo de documento de origem
        /// </summary>
        public string descricao { get; set; }
    }
}
