using System.Xml.Serialization;
using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    [XmlRoot(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TResultLote_GNRE : IRetornoServico
    {
        /// <summary>
        /// Identificação do ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente ambiente { get; set; }

        public int numeroRecibo { get; set; }

        public SituacaoProcessamento situacaoProcess { get; set; }

        public Resultado resultado { get; set; }
    }
}
