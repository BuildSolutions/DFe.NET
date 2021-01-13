using System;
using System.Collections.Generic;
using System.Text;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class DetalhamentosReceita
    {
        /// <summary>
        /// Especifica cada detalhamento da receita
        /// </summary>
        public List<DetalhamentoReceita> detalhamentoReceita { get; set; }
    }

    public class DetalhamentoReceita
    {
        /// <summary>
        /// Código do detalhamento da receita
        /// </summary>
        public int codigo { get; set; }

        /// <summary>
        /// Descrição do detalhamento da receita
        /// </summary>
        public string descricao { get; set; }
    }
}
