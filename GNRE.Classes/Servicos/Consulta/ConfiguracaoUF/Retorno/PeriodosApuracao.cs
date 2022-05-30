using System.Collections.Generic;
using System.Xml.Serialization;
using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class PeriodosApuracao
    {
        /// <summary>
        /// Especifica cada período de apuração.
        /// </summary>
        [XmlElement("periodoApuracao")]
        public List<PeriodoApuracao> periodoApuracao { get; set; }
    }

    public class PeriodoApuracao
    {
        /// <summary>
        /// Código do período de apuração:
        /// 0 – mensal
        /// 1 - 1ª Quinzena
        /// 2 - 2ª Quinzena
        /// 3 - 1° Decêndio
        /// 4 - 2° Decêndio
        /// 5 - 3° Decêndio
        /// </summary>
        public EPeriodoApuracao codigo { get; set; }

        /// <summary>
        /// Descrição do período de apuração.
        /// </summary>
        public string descricao { get; set; }
    }
}
