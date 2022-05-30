using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DFe.Utils;

namespace NFe.Classes.Servicos.Suframa
{
    [XmlType(AnonymousType = true, Namespace = "http://www.portal.fucapi.br")]
    [XmlRoot(Namespace = "http://www.portal.fucapi.br", IsNullable = false)]
    public class lote : IRetornoServico
    {
        public lote(
            string cnpjDestinatario,
            string cnpjTransp,
            string inscSufDestinatario,
            string ufDestino,
            string ufOrigem,
            long qtdeNF,
            List<loteNotaFiscal> notasFiscais,
            long nro,
            DateTime dtEmissao)
        {
            this.versao_sw = "6.0";
            this.cnpjDestinatario = cnpjDestinatario;
            this.cnpjTransp = cnpjTransp ?? "";
            this.inscSufDestinatario = inscSufDestinatario;
            this.ufDestino = ufDestino;
            this.ufOrigem = ufOrigem;
            this.qtdeNF = qtdeNF;
            this.notasFiscais = notasFiscais;
            this.dtEmissao = dtEmissao;
            _nro = nro;
        }

        internal lote()
        {

        }

        public string cnpjDestinatario { get; set; }

        [XmlElement(ElementName = "cnpjTransp", IsNullable = true)]
        public string cnpjTransp { get; set; }

        public string inscSufDestinatario { get; set; }

        public string ufDestino { get; set; }

        public string ufOrigem { get; set; }

        public long qtdeNF { get; set; }

        [XmlArrayItem("notaFiscal", IsNullable = false)]
        public List<loteNotaFiscal> notasFiscais { get; set; }

        [XmlIgnore]
        private long _nro { get; set; }

        [XmlAttribute(AttributeName = "nro")]
        public string nro
        {
            get
            {
                if (_nro == 0)
                {
                    return null;
                }

                return _nro.ToString().PadLeft(9, '0');
            }
            set => _nro = long.Parse(value);
        }

        [XmlAttribute]
        public string versao_sw { get; set; }

        [XmlIgnore]
        public DateTime? dtEmissao { get; set; }

        [XmlAttribute(AttributeName = "dtEmissao")]
        public string ProxyDataEmissao
        {
            get
            {
                if (dtEmissao == null)
                {
                    return null;
                }

                return dtEmissao.Value.ToString("dd/MM/yyyy");
            }
            set => dtEmissao = Convert.ToDateTime(value);
        }
    }
}
