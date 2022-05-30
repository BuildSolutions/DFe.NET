using DFe.Utils;
using GNRE.Classes;

namespace GNRE.Utils.GNRE
{
    public static class ExtGuiasGNRERetorno
    {
        /// <summary>
        /// Carrega um arquivo XML para um objeto da classe de guia de GNRE
        /// </summary>
        /// <param name="dadosGNRE"></param>
        /// <param name="arquivoXml">arquivo XML</param>
        /// <returns>Retorna dos dados da GNRE carregada com os dados do XML</returns>
        public static GuiasGNRERetorno CarregarDeArquivoXml(this GuiasGNRERetorno dadosGNRE, string arquivoXml)
        {
            var s = FuncoesXml.ObterNodeDeArquivoXml(typeof(GuiasGNRERetorno).Name, arquivoXml);
            return FuncoesXml.XmlStringParaClasse<GuiasGNRERetorno>(s);
        }

        /// <summary>
        /// Converte o objeto GuiasGNRERetorno para uma string no formato XML
        /// </summary>
        /// <param name="dadosGNRE"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto GuiasGNRERetorno</returns>
        public static string ObterXmlString(this GuiasGNRERetorno dadosGNRE)
        {
            return FuncoesXml.ClasseParaXmlString(dadosGNRE);
        }

        /// <summary>
        /// Coverte uma string XML no formato de dados da GNRE para um objeto NFe
        /// </summary>
        /// <param name="dadosGNRE"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo GNRE</returns>
        public static GuiasGNRERetorno CarregarDeXmlString(this GuiasGNRERetorno dadosGNRE, string xmlString)
        {
            var s = FuncoesXml.ObterNodeDeStringXml(typeof(GuiasGNRERetorno).Name, xmlString);
            return FuncoesXml.XmlStringParaClasse<GuiasGNRERetorno>(s);
        }
    }
}
