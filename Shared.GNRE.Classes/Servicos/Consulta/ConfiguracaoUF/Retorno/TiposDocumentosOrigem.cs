using System.Collections.Generic;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class TiposDocumentosOrigem
    {
        /// <summary>
        /// Especifica cada tipo de documento de origem
        /// </summary>
        public List<TipoDocumentoOrigem> tipoDocumentoOrigem { get; set; }
    }

    public class TipoDocumentoOrigem
    {
        /// <summary>
        /// Código do tipo de documento de origem
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Descrição do tipo de documento de origem
        /// </summary>
        public string Descricao { get; set; }
    }
}
