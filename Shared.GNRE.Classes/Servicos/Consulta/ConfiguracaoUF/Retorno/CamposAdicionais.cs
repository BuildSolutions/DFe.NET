using System;
using System.Collections.Generic;
using System.Text;
using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno
{
    public class CamposAdicionais
    {
        /// <summary>
        /// Especifica cada campo adicional
        /// </summary>
        public List<CampoAdicional> campoAdicional { get; set; }
    }

    public class CampoAdicional
    {
        /// <summary>
        /// Informa se o campo adicional é obrigatório
        /// </summary>
        public string obrigatorio { get; set; }

        /// <summary>
        /// Código do campo adicional no ambiente informado
        /// </summary>
        public ECampoExtra codigo { get; set; }

        /// <summary>
        /// Tipo do campo adicional:
        /// T – Texto;
        /// N – Numérico;
        /// D – Data.
        /// Obs.: o tipo “D” deve ser no formato: AAAA-MM-DD
        /// </summary>
        public string tipo { get; set; }

        /// <summary>
        /// Tamanho do valor para o campo adicional.Caso tipo seja “T” informar o tamanho máximo do texto.
        /// Caso seja “N” informar o tamanho da parte inteira do número
        /// </summary>
        public int Tamanho { get; set; }

        /// <summary>
        /// Número de casas decimais caso seja do tipo “N” 
        /// </summary>
        public int casasDecimais { get; set; }

        /// <summary>
        /// Descrição do campo adicional
        /// </summary>
        public string titulo { get; set; }
    }
}
