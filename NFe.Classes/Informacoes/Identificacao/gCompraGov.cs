using System.Collections.Generic;
using System.Xml.Serialization;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFe.Classes.Informacoes.Identificacao
{
    /// <summary>
    /// BB01 - Grupo de Compras Governamentais.
    /// </summary>
    public class gCompraGov
    {
        private decimal _pRedutor;

        /// <summary>BB02 - Tipo de ente governamental.</summary>
        public TipoEnteGov tpEnteGov { get; set; }

        /// <summary>BB03 - Percentual de redução da alíquota em compra governamental,
        /// conforme o art. 472/370 da LC 214/2025.</summary>
        public decimal pRedutor
        {
            get => _pRedutor.Arredondar(4);
            set => _pRedutor = value.Arredondar(4);
        }

        /// <summary>BB04 - Tipo de operação com o ente governamental.</summary>
        public TipoOperGov tpOperGov { get; set; }

        /// <summary>
        /// BB05 - Chaves de acesso dos documentos fiscais anteriores, até 99.
        /// <para>Obrigatório para tpOperGov 2 e 3 e vedado para 1 e 4 (rejeições 1195 e
        /// 1196). O tipo 2 aceita uma única chave (rejeição 1197); o tipo 3 aceita
        /// várias. A chave deve ser de emitente com o mesmo CNPJ base.</para>
        /// </summary>
        [XmlElement("refDFeAnt")]
        public List<string> refDFeAnt { get; set; }

        public bool ShouldSerializerefDFeAnt()
        {
            return refDFeAnt?.Count > 0;
        }
    }
}
