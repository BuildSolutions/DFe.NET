using System.Collections.Generic;
using System.Xml.Serialization;
using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class Receitas
    {
        /// <summary>
        /// Especifica cada receita associada à UF.
        /// Obs: A receita 100056 pode vir duplicada, caso a mesma tenha uma especificidade para empresas de Courier.
        /// Será diferenciada com o uso do parâmetro courier = “S”. 
        /// </summary>
        [XmlElement("receita")]
        public List<Receita> receita { get; set; }
    }

    public class Receita
    {
        [XmlAttribute]
        public string descricao { get; set; }

        [XmlAttribute]
        public int codigo { get; set; }

        /// <summary>
        /// Informa se o contribuinte emitente é obrigatório.
        /// </summary>
        public string exigeContribuinteEmitente { get; set; }

        /// <summary>
        /// Informa se o detalhamento da receita é obrigatório.
        /// </summary>
        public string exigeDetalhamentoReceita { get; set; }

        /// <summary>
        /// Lista de detalhamentos associados à receita
        /// </summary>
        public DetalhamentosReceita detalhamentosReceita { get; set; }

        /// <summary>
        /// Informa se o produto é obrigatório
        /// </summary>
        public string exigeProduto { get; set; }

        /// <summary>
        /// Lista de produtos associados à receita
        /// </summary>
        public Produtos produtos { get; set; }

        /// <summary>
        /// Informa se o período de referência é obrigatório
        /// </summary>
        public string exigePeriodoReferencia { get; set; }

        /// <summary>
        /// Informa se o período de apuração é obrigatório.
        /// Obs.: Aparecerá apenas se o período de referência for exigido
        /// </summary>
        public string exigePeriodoApuracao { get; set; }

        /// <summary>
        /// Lista de períodos associados à receita
        /// </summary>
        public PeriodosApuracao periodosApuracao { get; set; }

        /// <summary>
        /// Informa se a parcela é obrigatória.
        /// Obs.: Aparecerá apenas se o período de referência for exigido
        /// </summary>
        public string exigeParcela { get; set; }

        /// <summary>
        /// Informa qual o valor a ser preenchido:
        /// P – Valor Principal
        /// T – Valor Total
        /// A – Valor Principal ou Valor Total
        /// N - Nenhum
        /// </summary>
        public string valorExigido { get; set; }

        /// <summary>
        /// Informa se o documento de origem é obrigatório
        /// </summary>
        public string exigeDocumentoOrigem { get; set; }

        /// <summary>
        /// Lista de tipos de documentos de origem associados à receita
        /// </summary>
        [XmlElement("tiposDocumentosOrigem")]
        public List<TiposDocumentosOrigem> tiposDocumentosOrigem { get; set; }

        /// <summary>
        /// Versões (do XML) que exibirão os Documentos de Origem da Receita.
        /// </summary>
        [XmlElement("versoesXmlDocOrigem")]
        public List<string> versoesXmlDocOrigem { get; set; }

        /// <summary>
        /// Informa se o contribuinte destinatário é obrigatório.
        /// </summary>
        public string exigeContribuinteDestinatario { get; set; }

        /// <summary>
        /// Informa se a data de vencimento é obrigatória.
        /// </summary>
        public string exigeDataVencimento { get; set; }

        /// <summary>
        /// Informa se a data de pagamento é obrigatória.
        /// </summary>
        public string exigeDataPagamento { get; set; }

        /// <summary>
        /// Informa se o convênio é:
        /// N - Não exigido
        /// S - Opcional
        /// O - Exigido(deve-se informar)
        /// </summary>
        public string exigeConvenio { get; set; } //TODO: Enum

        /// <summary>
        /// Informa se há algum campo adicional obrigatório
        /// </summary>
        public string exigeCamposAdicionais { get; set; }

        /// <summary>
        /// Lista de campos adicionais associados à receita
        /// </summary>
        public CamposAdicionais camposAdicionais { get; set; }

        /// <summary>
        /// Informa se a Receita exige Valor Fecp
        /// </summary>
        public string exigeValorFecp { get; set; }
    }
}
