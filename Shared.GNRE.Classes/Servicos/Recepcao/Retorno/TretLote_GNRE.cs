using System.Xml.Serialization;
using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Recepcao.Retorno
{
    [XmlRoot(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TRetLote_GNRE : IRetornoServico
    {
        /// <summary>
        /// Identificação do ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente ambiente { get; set; }

        [XmlElement(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
        public SituacaoRecepcao situacaoRecepcao { get; set; }

        public Recibo recibo { get; set; }
    }
}
