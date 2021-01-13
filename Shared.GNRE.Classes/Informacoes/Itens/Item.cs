using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DFe.Utils;
using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Destinatario;
using GNRE.Classes.Informacoes.Itens.PeriodoReferencia;
using GNRE.Classes.Itens.Extras;

namespace GNRE.Classes.Informacoes.Itens
{
    public class Item
    {
        public EReceita receita { get; set; }

        private int? _detalhamentoReceita;
        public string detalhamentoReceita
        {
            get
            {
                if (_detalhamentoReceita == null)
                {
                    return null;
                }

                return _detalhamentoReceita.Value.ToString().PadLeft(6, '0');
            }
            set
            {
                int.TryParse(value, out int intAux);
                _detalhamentoReceita = intAux;
            }
        }

        public DocumentoOrigem documentoOrigem { get; set; }

        public int? produto { get; set; }

        public bool ShouldSerializeproduto()
        {
            return produto.HasValue;
        }

        public Referencia referencia { get; set; }

        [XmlIgnore]
        public DateTime? dataVencimento { get; set; }

        [XmlElement(ElementName = "dataVencimento")]
        public string ProxyDataVencimento
        {
            get
            {
                if (dataVencimento == null)
                {
                    return null;
                }

                return dataVencimento.Value.ParaDataString();
            }
            set => dataVencimento = Convert.ToDateTime(value);
        }

        public Valor valor { get; set; }

        public string convenio { get; set; }

        public ContribuinteDestinatario contribuinteDestinatario { get; set; }

        [XmlElement("camposExtras")]
        public List<CamposExtras> camposExtras { get; set; }
    }
}
