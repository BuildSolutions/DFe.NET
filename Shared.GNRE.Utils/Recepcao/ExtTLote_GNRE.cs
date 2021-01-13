using System;
using System.Globalization;
using DFe.Utils;
using GNRE.Classes.Servicos.Recepcao;

namespace GNRE.Utils.Recepcao
{
    public static class ExtTLote_GNRE
    {
        /// <summary>
        ///     Carrega um arquivo XML para um objeto da classe GNRE
        /// </summary>
        /// <param name="gnre"></param>
        /// <param name="arquivoXml">arquivo XML</param>
        /// <returns>Retorna uma GNRE carregada com os dados do XML</returns>
        public static TLote_GNRE CarregarDeArquivoXml(this TLote_GNRE gnre, string arquivoXml)
        {
            var s = FuncoesXml.ObterNodeDeArquivoXml(typeof(TLote_GNRE).Name, arquivoXml);
            return FuncoesXml.XmlStringParaClasse<TLote_GNRE>(s);
        }

        /// <summary>
        /// Converte o objeto TLote_GNRE para uma string no formato XML
        /// </summary>
        /// <param name="pedEnvio"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TLote_GNRE</returns>
        public static string ObterXmlString(this TLote_GNRE pedEnvio)
        {
            return FuncoesXml.ClasseParaXmlString(pedEnvio);
        }

        /// <summary>
        ///     Coverte uma string XML no formato GNRE para um objeto NFe
        /// </summary>
        /// <param name="gnre"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo GNRE</returns>
        public static TLote_GNRE CarregarDeXmlString(this TLote_GNRE gnre, string xmlString)
        {
            var s = FuncoesXml.ObterNodeDeStringXml(typeof(TLote_GNRE).Name, xmlString);
            return FuncoesXml.XmlStringParaClasse<TLote_GNRE>(s);
        }

        /// <summary>
        ///     Grava os dados do objeto GNRE em um arquivo XML
        /// </summary>
        /// <param name="gnre">Objeto GNRE</param>
        /// <param name="arquivoXml">Diretório com nome do arquivo a ser gravado</param>
        public static void SalvarArquivoXml(this TLote_GNRE gnre, string arquivoXml)
        {
            FuncoesXml.ClasseParaArquivoXml(gnre, arquivoXml);
        }

        /// <summary>
        ///     Gera id, cdv, assina e faz alguns ajustes nos dados da classe GNRE antes de utilizá-la
        /// </summary>
        /// <param name="gnre"></param>
        /// <param name="cfgServico"></param>
        /// <returns>Retorna um objeto GNRE devidamente tradado</returns>
        //public static TLote_GNRE Valida(this TLote_GNRE gnre, ConfiguracaoServico cfgServico = null)
        //{
        //    if (gnre == null) throw new ArgumentNullException("gnre");

        //    var versao = (Decimal.Parse(gnre.versao, CultureInfo.InvariantCulture));

        //    var xmlNfe = gnre.ObterXmlString();
        //    var config = cfgServico ?? ConfiguracaoServico.Instancia;
        //    if (versao < 3)
        //        Validador.Valida(ServicoNFe.NfeRecepcao, config.VersaoNfeRecepcao, xmlNfe, false, config);
        //    if (versao >= 3)
        //        Validador.Valida(ServicoNFe.NFeAutorizacao, config.VersaoNFeAutorizacao, xmlNfe, false, config);

        //    return gnre; //Para uso no formato fluent
        //}
    }
}
