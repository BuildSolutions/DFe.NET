using System.Xml.Serialization;
using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Consulta.Lote
{
    [XmlRoot(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TConsLote_GNRE
    {
        public TConsLote_GNRE(TipoAmbiente ambiente, int numeroRecibo)
        {
            this.ambiente = ambiente;
            this.numeroRecibo = numeroRecibo;
        }

        internal TConsLote_GNRE()
        {

        }

        /// <summary>
        /// Identificação do ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente ambiente { get; set; }

        /// <summary>
        /// Número do recibo de gerado pelo portal GNRE
        /// </summary>
        public int numeroRecibo { get; set; }
    }
}
