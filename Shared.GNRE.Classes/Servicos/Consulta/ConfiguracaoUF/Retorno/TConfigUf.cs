using System.Xml.Serialization;
using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    [XmlRoot(Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TConfigUf : IRetornoServico
    {
        /// <summary>
        /// Identificação do ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente ambiente { get; set; }

        public SituacaoConsulta situacaoConsulta { get; set; }

        /// <summary>
        /// Informa se a Uf favorecida é obrigatória.
        /// </summary>
        public string exigeUfFavorecida { get; set; }

        /// <summary>
        /// Informa se a Receita é obrigatória
        /// </summary>
        public string exigeReceita { get; set; }

        /// <summary>
        /// Receitas associadas à Uf informada
        /// </summary>
        public Receitas receitas { get; set; }
    }
}
