using System.Collections.Generic;
using System.Xml.Serialization;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class Produtos
    {
        /// <summary>
        /// Especifica cada produto
        /// </summary>
        [XmlElement("produto")]
        public List<Produto> produto { get; set; }
    }

    public class Produto
    {
        /// <summary>
        /// Código do produto
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// Descrição do produto 
        /// </summary>
        public string descricao { get; set; }
    }
}
