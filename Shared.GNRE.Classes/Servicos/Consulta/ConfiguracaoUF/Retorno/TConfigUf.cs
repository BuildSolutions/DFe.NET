using System;
using System.Collections.Generic;
using System.Text;
using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class TConfigUf
    {
        /// <summary>
        /// Identificação do Ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente Ambiente { get; set; }

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
