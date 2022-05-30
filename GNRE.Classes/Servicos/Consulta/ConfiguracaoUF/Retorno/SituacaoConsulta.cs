namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class SituacaoConsulta
    {
        /// <summary>
        /// Código do retorno que indica a situação da recepção do lote: Vide Anexo I – quadros 1 e 2
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// Descrição literal do retorno
        /// </summary>
        public string descricao { get; set; }
    }
}
