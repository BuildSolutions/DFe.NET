using DFe.Utils;
using NFe.Classes.Servicos.Suframa;

namespace NFe.Utils.Suframa
{
    public static class Extlote
    {
        /// <summary>
        /// Converte o objeto lote para uma string no formato XML
        /// </summary>
        /// <param name="loteSuframa"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto lote</returns>
        public static string ObterXmlString(this lote loteSuframa)
        {
            return FuncoesXml.ClasseParaXmlString(loteSuframa);
        }
    }
}
