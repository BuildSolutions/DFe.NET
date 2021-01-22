using System;
using System.Xml.Serialization;
using DFe.Classes.Entidades;
using GNRE.Classes.ValueObjects;

namespace GNRE.Classes.Informacoes.Emitente
{
    public class ContribuinteEmitente : Pessoa
    {
        public string endereco { get; set; }

        public string municipio { get; set; }

        /// <summary>
        /// Contém a sigla da UF favorecida.Campo com 2 dígitos.
        /// </summary>
        [XmlIgnore]
        public Estado uf { get; set; }

        [XmlElement(ElementName = "uf")]
        public string ProxyUF
        {
            get => Enum.GetName(typeof(Estado), uf);
            set => uf = (Estado)Enum.Parse(typeof(Estado), value);
        }

        public string cep { get; set; }

        public string telefone { get; set; }
    }
}
