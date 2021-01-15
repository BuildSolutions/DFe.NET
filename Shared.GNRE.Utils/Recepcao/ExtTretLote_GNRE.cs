using DFe.Utils;
using GNRE.Classes.Servicos.Recepcao.Retorno;

namespace GNRE.Utils.Recepcao
{
    public static class ExtTretLote_GNRE
    {
        /// <summary>
        /// Carrega um arquivo XML para um objeto da classe de retorno da GNRE
        /// </summary>
        /// <param name="tRetLote_GNRE"></param>
        /// <param name="arquivoXml">arquivo XML</param>
        /// <returns>Retorna um resultado da GNRE carregada com os dados do XML</returns>
        public static TRetLote_GNRE CarregarDeArquivoXml(this TRetLote_GNRE tRetLote_GNRE, string arquivoXml)
        {
            var s = FuncoesXml.ObterNodeDeArquivoXml(typeof(TRetLote_GNRE).Name, arquivoXml);
            return FuncoesXml.XmlStringParaClasse<TRetLote_GNRE>(s);
        }

        /// <summary>
        ///     Coverte uma string XML no formato NFe para um objeto TretLote_GNRE
        /// </summary>
        /// <param name="tRetLote_GNRE"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo TretLote_GNRE</returns>
        public static TRetLote_GNRE CarregarDeXmlString(this TRetLote_GNRE tRetLote_GNRE, string xmlString)
        {
            return FuncoesXml.XmlStringParaClasse<TRetLote_GNRE>(xmlString);
        }

        /// <summary>
        ///     Converte o objeto TresultLote_GNRE para uma string no formato XML
        /// </summary>
        /// <param name="tRetLote_GNRE"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TRetLote_GNRE</returns>
        public static string ObterXmlString(this TRetLote_GNRE tRetLote_GNRE)
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
