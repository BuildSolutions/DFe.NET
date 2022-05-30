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
        /// Carrega um arquivo XML para um objeto da classe de retorno da consulta de lote da GNRE
        /// </summary>
        /// <param name="resultadoLoteGNRE"></param>
        /// <param name="arquivoXml">arquivo XML</param>
        /// <returns>Retorna um resultado da consulta de lote da GNRE carregada com os dados do XML</returns>
        public static TResultLote_GNRE CarregarDeArquivoXml(this TResultLote_GNRE resultadoLoteGNRE, string arquivoXml)
        {
            var s = FuncoesXml.ObterNodeDeArquivoXml(typeof(TResultLote_GNRE).Name, arquivoXml);
            return FuncoesXml.XmlStringParaClasse<TResultLote_GNRE>(s);
        }

        /// <summary>
        ///     Coverte uma string XML no formato NFe para um objeto TConfigUf
        /// </summary>
        /// <param name="resultadoLoteGNRE"></param>
        /// <param name="xmlString"></param>
        /// <returns>Retorna um objeto do tipo TResultLote_GNRE</returns>
        public static TResultLote_GNRE CarregarDeXmlString(this TResultLote_GNRE resultadoLoteGNRE, string xmlString)
        {
            return FuncoesXml.XmlStringParaClasse<TResultLote_GNRE>(xmlString);
        }

        /// <summary>
        ///     Converte o objeto TresultLote_GNRE para uma string no formato XML
        /// </summary>
        /// <param name="tResultLote_GNRE"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto TConfigUf</returns>
        public static string ObterXmlString(this TResultLote_GNRE resultadoLoteGNRE)
        {
            return FuncoesXml.ClasseParaXmlString(resultadoLoteGNRE);
        }
    }
}
