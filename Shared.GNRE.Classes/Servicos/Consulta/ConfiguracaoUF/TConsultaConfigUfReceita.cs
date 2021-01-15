using System.Xml.Serialization;
using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF
{
    public class TConsultaConfigUfReceita
    {
        public TConsultaConfigUfReceita(ECourier courier, int? valor)
        {
            //this.ProxyCourier = courier.ToString();

            this.courier = courier;

            if(valor.HasValue && valor.Value > 0)
            {
                this.valor = valor.ToString().PadLeft(6, '0');
            }
        }

        internal TConsultaConfigUfReceita()
        {

        }

        [XmlText]
        public string valor { get; set; }

        //public bool ShouldSerializevalor()
        //{
        //    return courier == ECourier.Sim && valor.HasValue;
        //}

        //[XmlIgnore]
        [XmlAttribute]
        public ECourier courier { get; set; }

        //[XmlIgnore]
        //public bool courierSpecified { get; set; }
    }
}
