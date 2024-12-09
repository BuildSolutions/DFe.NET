using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFe.Utils;
using MDFe.Classes.Extencoes;
using MDFe.Classes.Retorno;
using MDFe.Utils.Configuracoes;

namespace MDFe.Classes.Extensoes
{
    public static class ExtMDFeProcMDFe
    {
        public static string XmlString(this MDFeProcMDFe mdfe)
        {
            return FuncoesXml.ClasseParaXmlString(mdfe);
        }

        public static void SalvarXmlEmDisco(this MDFeProcMDFe mdfe, string nomeArquivo = null, MDFeConfiguracao cfgMdfe = null)
        {
            var config = cfgMdfe ?? MDFeConfiguracao.Instancia;
            if (config.NaoSalvarXml()) return;

            if (string.IsNullOrEmpty(nomeArquivo))
            {
                var data = mdfe.MDFe.InfMDFe.Ide.DhEmi;
                var strFolderYear = data.ToString("yyyy");
                var strFolderMonth = data.ToString("MM_yyyy");

                nomeArquivo = System.IO.Path.Combine(config.CaminhoSalvarXml,
                    "Processados",
                    strFolderYear,
                    strFolderMonth) + @"\" + mdfe.MDFe.Chave() + "-procMDFe.xml";
            }

            FuncoesXml.ClasseParaArquivoXml(mdfe, nomeArquivo, true);
        }
    }
}
