using DFe.Utils;
using GNRE.Classes.Informacoes.Dados;

namespace GNRE.Utils.GNRE
{
    public static class ExtGuiasGNRE
    {
        /// <summary>
        ///     Carrega um arquivo XML para um objeto da classe de guia de GNRE
        /// </summary>
        /// <param name="dadosGNRE"></param>
        /// <param name="arquivoXml">arquivo XML</param>
        /// <returns>Retorna dos dados da GNRE carregada com os dados do XML</returns>
        public static TDadosGNRE CarregarDeArquivoXml(this TDadosGNRE dadosGNRE, string arquivoXml)
        {
            var s = FuncoesXml.ObterNodeDeArquivoXml(typeof(TDadosGNRE).Name, arquivoXml);
            return FuncoesXml.XmlStringParaClasse<TDadosGNRE>(s);
        }

        /// <summary>
        /// Converte o objeto TDadosGNRE para uma string no formato XML
        /// </summary>
        /// <param name="dadosGNRE"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TDadosGNRE</returns>
        public static string ObterXmlString(this TDadosGNRE dadosGNRE)
        {
            return FuncoesXml.ClasseParaXmlString(dadosGNRE);
        }

        /// <summary>
        /// Coverte uma string XML no formato de dados da GNRE para um objeto NFe
        /// </summary>
        /// <param name="dadosGNRE"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo GNRE</returns>
        public static TDadosGNRE CarregarDeXmlString(this TDadosGNRE dadosGNRE, string xmlString)
        {
            var s = FuncoesXml.ObterNodeDeStringXml(typeof(TDadosGNRE).Name, xmlString);
            return FuncoesXml.XmlStringParaClasse<TDadosGNRE>(s);
        }
    }
}
