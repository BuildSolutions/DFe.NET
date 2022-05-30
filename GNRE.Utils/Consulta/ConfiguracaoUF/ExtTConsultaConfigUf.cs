using DFe.Utils;
using GNRE.Classes.Servicos.Consulta.ConfiguracaoUF;

namespace GNRE.Utils.Consulta.ConfiguracaoUF
{
    public static class ExtTConsultaConfigUf
    {
        /// <summary>
        /// Converte o objeto TConsultaConfigUf para uma string no formato XML
        /// </summary>
        /// <param name="pedConsulta"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TConsultaConfigUf</returns>
        public static string ObterXmlString(this TConsultaConfigUf pedConsulta)
        {
            return FuncoesXml.ClasseParaXmlString(pedConsulta);
        }
    }
}
