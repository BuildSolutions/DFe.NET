using DFe.Utils;
using GNRE.Classes.Servicos.Consulta.Lote;

namespace GNRE.Utils.Consulta.Lote
{
    public static class ExtTconsLote_GNRE
    {
        /// <summary>
        /// Converte o objeto TconsLote_GNRE para uma string no formato XML
        /// </summary>
        /// <param name="pedConsulta"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TconsLote_GNRE</returns>
        public static string ObterXmlString(this TconsLote_GNRE pedConsulta)
        {
            return FuncoesXml.ClasseParaXmlString(pedConsulta);
        }
    }
}
