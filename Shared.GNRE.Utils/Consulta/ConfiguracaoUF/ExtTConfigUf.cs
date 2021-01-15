using DFe.Utils;
using GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno;

namespace GNRE.Utils.Consulta.ConfiguracaoUF
{
    public static class ExtTConfigUf
    {
        /// <summary>
        ///     Coverte uma string XML no formato NFe para um objeto TConfigUf
        /// </summary>
        /// <param name="tConfigUf"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo TConfigUf</returns>
        public static TConfigUf CarregarDeXmlString(this TConfigUf tConfigUf, string xmlString)
        {
            return FuncoesXml.XmlStringParaClasse<TConfigUf>(xmlString);
        }

        /// <summary>
        ///     Converte o objeto TConfigUf para uma string no formato XML
        /// </summary>
        /// <param name="tConfigUf"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TConfigUf</returns>
        public static string ObterXmlString(this TConfigUf tConfigUf)
        {
            return FuncoesXml.ClasseParaXmlString(tConfigUf);
        }

        /// <summary>
        /// Verifica se esta autorizado
        /// </summary>
        /// <param name="TConfigUf"></param>
        /// <returns>bool</returns>
        public static bool Sucesso(this TConfigUf TConfigUf)
        {
            return GNRESituacao.ConsultaConfiguracaoUFProcessada(TConfigUf.situacaoConsulta.codigo);
        }
    }
}
