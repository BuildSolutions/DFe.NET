namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    public class SituacaoProcessamento
    {
        /// <summary>
        /// Código da situação do processamento do lote (Quadros III, IV e V) 
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// Descrição da situação do processamento
        /// </summary>
        public string descricao { get; set; }
    }
}
