using DFe.Utils;
using GNRE.Classes.Servicos.Recepcao.Retorno;

namespace GNRE.Utils.Recepcao
{
    public static class ExtTretLote_GNRE
    {
        /// <summary>
        ///     Coverte uma string XML no formato NFe para um objeto TretLote_GNRE
        /// </summary>
        /// <param name="tRetLote_GNRE"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo TretLote_GNRE</returns>
        public static TretLote_GNRE CarregarDeXmlString(this TretLote_GNRE tRetLote_GNRE, string xmlString)
        {
            return FuncoesXml.XmlStringParaClasse<TretLote_GNRE>(xmlString);
        }

        /// <summary>
        ///     Converte o objeto TresultLote_GNRE para uma string no formato XML
        /// </summary>
        /// <param name="tRetLote_GNRE"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TresultLote_GNRE</returns>
        public static string ObterXmlString(this TretLote_GNRE tRetLote_GNRE)
        {
            return FuncoesXml.ClasseParaXmlString(tRetLote_GNRE);
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
