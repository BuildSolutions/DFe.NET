namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    public class Motivo
    {
        /// <summary>
        /// Código do motivo da rejeição
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// Descrição do motivo da rejeição
        /// </summary>
        public string descricao { get; set; }

        /// <summary>
        /// Campo onde ocorreu o motivo da rejeição
        /// </summary>
        public string campo { get; set; }
    }
}
