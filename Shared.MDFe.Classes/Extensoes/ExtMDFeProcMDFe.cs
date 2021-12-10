using System;
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

        public static void SalvarXmlEmDisco(this MDFeProcMDFe mdfe, string nomeArquivo = null)
        {
            if (MDFeConfiguracao.NaoSalvarXml())
            {
                return;
            }

            if (string.IsNullOrEmpty(nomeArquivo))
            {
                var data = mdfe.MDFe.InfMDFe.Ide.DhEmi;
                var strFolderYear = data.ToString("yyyy");
                var strFolderMonth = data.ToString("MM_yyyy");

                nomeArquivo = System.IO.Path.Combine(MDFeConfiguracao.CaminhoSalvarXml,
                    "processados",
                    strFolderYear,
                    strFolderMonth) + @"\" + mdfe.MDFe.Chave() + "-procMDFe.xml";
            }

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(nomeArquivo)))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(nomeArquivo));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Erro ao salvar aquivo xml no disco", ex);
                }
            }

            FuncoesXml.ClasseParaArquivoXml(mdfe, nomeArquivo);
        }
    }
}

