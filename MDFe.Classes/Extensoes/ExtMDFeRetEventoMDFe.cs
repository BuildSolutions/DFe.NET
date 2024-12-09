using System;
using System.IO;
using DFe.Utils;
using MDFe.Classes.Retorno.MDFeEvento;
using MDFe.Utils.Configuracoes;

namespace MDFe.Classes.Extencoes
{
    public static class ExtMDFeRetEventoMDFe
    {
        public static void SalvarXmlEmDisco(this MDFeRetEventoMDFe retEvento, string chave, MDFeConfiguracao cfgMdfe = null)
        {
            var config = cfgMdfe ?? MDFeConfiguracao.Instancia;
            if (config.NaoSalvarXml()) return;

            var caminhoXml = config.CaminhoSalvarXml;

            //var arquivoSalvar = Path.Combine(caminhoXml, chave + "-env.xml");

            var data = DateTime.Now;
            if (retEvento.InfEvento.DhRegEvento != null)
            {
                data = (DateTime)retEvento.InfEvento.DhRegEvento;
            }

            var strFolderYear = data.ToString("yyyy");
            var strFolderMonth = data.ToString("MM_yyyy");

            var arquivoSalvar = System.IO.Path.Combine(caminhoXml,
                "Eventos",
                strFolderYear,
                strFolderMonth) + @"\" + chave + "-env.xml";

            FuncoesXml.ClasseParaArquivoXml(retEvento, arquivoSalvar, true);
        }
    }
}