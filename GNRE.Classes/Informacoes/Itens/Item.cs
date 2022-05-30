using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DFe.Utils;
using DFe.Utils.Extensoes;
using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Destinatario;
using GNRE.Classes.Informacoes.Itens.PeriodoReferencia;
using GNRE.Classes.Itens.Extras;

namespace GNRE.Classes.Informacoes.Itens
{
    public class Item
    {
        public Item(EReceita receita,
            ContribuinteDestinatario contribuinteDestinatario = null,
            DocumentoOrigem documentoOrigem = null,
            int? detalhamentoReceita = null,
            int? produto = null,
            Referencia referencia = null,
            DateTime? dataVencimento = null,
            List<Valor> valor = null,
            string convenio = null,
            List<CamposExtras> camposExtras = null)
        {
            this.receita = receita;
            _detalhamentoReceita = detalhamentoReceita;
            this.documentoOrigem = documentoOrigem;
            this.produto = produto;
            this.referencia = referencia;
            this.dataVencimento = dataVencimento;
            this.valor = valor;
            this.convenio = convenio;
            this.contribuinteDestinatario = contribuinteDestinatario;
            this.camposExtras = camposExtras;
        }

        internal Item()
        {

        }

        public EReceita receita { get; set; }

        private int? _detalhamentoReceita;
        public string detalhamentoReceita
        {
            get
            {
                if (_detalhamentoReceita.NuloSeZero() == null)
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

        [XmlElement("documentoOrigem")]
        public DocumentoOrigem documentoOrigem { get; set; }

        public int? produto { get; set; }

        public bool ShouldSerializeproduto()
        {
            return produto.HasValue && produto.Value > 0;
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

        [XmlElement("valor")]
        public List<Valor> valor { get; set; }

        public string convenio { get; set; }

        public bool ShouldSerializeconvenio()
        {
            return !string.IsNullOrEmpty(convenio);
        }

        public ContribuinteDestinatario contribuinteDestinatario { get; set; }

        [XmlElement("camposExtras")]
        public List<CamposExtras> camposExtras { get; set; }
    }
}
