using System;
using System.Xml.Serialization;
using DFe.Classes;
using DFe.Classes.Entidades;
using DFe.Utils;
using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Emitente;
using GNRE.Classes.Informacoes.Itens;

namespace GNRE.Classes.Informacoes.Dados
{
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class TDadosGNRE
    {
        public TDadosGNRE()
        {
            versao = "2.00";
        }

        [XmlAttributeAttribute()]
        public string versao { get; set; }

        /// <summary>
        /// Contém a sigla da UF favorecida.Campo com 2 dígitos.
        /// </summary>
        [XmlIgnore]
        public Estado _ufFavorecida { get; set; }

        [XmlElement(ElementName = "ufFavorecida")]
        public string ProxyUF
        {
            get => Enum.GetName(typeof(Estado), _ufFavorecida);
            set => _ufFavorecida = (Estado)Enum.Parse(typeof(Estado), value);
        }

        /// <summary>
        /// Contém código do tipo da GNRE.
        ///0 - Guia Simples
        ///1 - Guia com Múltiplos Documentos de Origem
        ///2 - Guia com Múltiplas Receitas
        /// </summary>
        [XmlIgnore]
        public ETipoGNRE _tipoGnre { get; set; }

        [XmlElement(ElementName = "tipoGnre")]
        public string ProxyTipoGNRE
        {
            get => _tipoGnre.XmlDescricao();
            set => _tipoGnre = (ETipoGNRE)Enum.Parse(typeof(ETipoGNRE), value);
        }

        public ContribuinteEmitente contribuinteEmitente { get; set; }

        public ItensGNRE itensGNRE { get; set; }

        private decimal? _valorGNRE;
        public decimal? valorGNRE
        {
            get => _valorGNRE.Arredondar(2);
            set => _valorGNRE = value.Arredondar(2);
        }

        public bool ShouldSerializevalorGNRE()
        {
            return valorGNRE.HasValue;
        }

        [XmlIgnore]
        public DateTime? dataPagamento { get; set; }

        [XmlElement(ElementName = "dataPagamento")]
        public string ProxyDataPagamento
        {
            get
            {
                if (dataPagamento == null)
                {
                    return null;
                }

                return dataPagamento.Value.ParaDataString();
            }
            set => dataPagamento = Convert.ToDateTime(value);
        }
    }
}
