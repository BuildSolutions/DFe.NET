using System;
using System.Collections.Generic;
using System.Text;
using DFe.Utils;
using GNRE.Classes.Servicos.Consulta.Lote.Retorno;

namespace GNRE.Utils.Consulta.Lote
{
    public static class ExtTresultLote_GNRE
    {
        /// <summary>
        ///     Coverte uma string XML no formato NFe para um objeto TresultLote_GNRE
        /// </summary>
        /// <param name="tResultLote_GNRE"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo TresultLote_GNRE</returns>
        public static TresultLote_GNRE CarregarDeXmlString(this TresultLote_GNRE tResultLote_GNRE, string xmlString)
        {
            return FuncoesXml.XmlStringParaClasse<TresultLote_GNRE>(xmlString);
        }

        /// <summary>
        ///     Converte o objeto TresultLote_GNRE para uma string no formato XML
        /// </summary>
        /// <param name="tResultLote_GNRE"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TresultLote_GNRE</returns>
        public static string ObterXmlString(this TresultLote_GNRE tResultLote_GNRE)
        {
            return FuncoesXml.ClasseParaXmlString(tResultLote_GNRE);
        }

        ///// <summary>
        ///// Verifica se esta autorizado
        ///// </summary>
        ///// <param name="tResultLote_GNRE"></param>
        ///// <returns>bool</returns>
        //public static bool LoteProcessado(this TresultLote_GNRE tResultLote_GNRE)
        //{
        //    return GNRESituacao.LoteProcessado(tResultLote_GNRE.situacaoProcess.codigo);
        //}
    }
}
