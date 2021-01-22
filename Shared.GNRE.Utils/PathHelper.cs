using System;
using System.IO;
using GNRE.Classes.Enumerators;

namespace GNRE.Utils
{
    public static class PathHelper
    {
        private static string ObterDiretorioArquivo(ConfiguracaoServico configuracaoServico, EFolderType eFolderType)
        {
            var caminhoXmlGerados = string.Empty;

            switch (eFolderType)
            {
                case EFolderType.AUTORIZADA:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Processados");
                case EFolderType.GERADOS:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Gerados");
                case EFolderType.LOTES:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Lotes");
                default:
                    return string.Empty;
            }
        }

        private static string ObterNomeArquivo(ConfiguracaoServico configuracaoServico,
            EFolderType eFolderType,
            string nomeArquivo,
            DateTime dataOperacao,
            string extensaoArquivo)
        {
            var ano = dataOperacao.ToString("yyyy");
            var mes = dataOperacao.ToString("MM_yyyy");

            string nomeArquivoXML;
            switch (eFolderType)
            {
                case EFolderType.AUTORIZADA:
                    nomeArquivoXML = $"{nomeArquivo}-proc";
                    break;
                case EFolderType.GERADOS:
                case EFolderType.LOTES:
                    nomeArquivoXML = nomeArquivo;
                    break;
                case EFolderType.TEMP:
                default:
                    ano = string.Empty;
                    mes = string.Empty;
                    extensaoArquivo = string.Empty;
                    nomeArquivoXML = Path.GetTempFileName();
                    break;
            }

            var arquivoSalvarGerados = Path.Combine(ObterDiretorioArquivo(configuracaoServico, eFolderType),
                ano,
                mes,
                string.Concat($"{nomeArquivoXML}.{extensaoArquivo}"));

            if (!Directory.Exists(Path.GetDirectoryName(arquivoSalvarGerados)))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(arquivoSalvarGerados));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Erro ao salvar aquivo xml no disco", ex);
                }
            }

            return arquivoSalvarGerados;
        }

        public static string ObterNomeArquivoXML(ConfiguracaoServico configuracaoServico,
            EFolderType eFolderType,
            string nomeArquivo,
            DateTime dataOperacao)
        {
            return ObterNomeArquivo(configuracaoServico,
            eFolderType,
            nomeArquivo,
            dataOperacao,
            "xml");
        }

        public static string SalvarArquivoXML(ConfiguracaoServico configuracaoServico,
            EFolderType eFolderType,
            string nomeArquivo,
            DateTime dataOperacao,
            string xml)
        {
            var nomeArquivoXML = ObterNomeArquivoXML(configuracaoServico,
            eFolderType,
            nomeArquivo,
            dataOperacao);

            var stw = new StreamWriter(nomeArquivoXML);
            stw.WriteLine(xml);
            stw.Close();

            return nomeArquivoXML;
        }

        public static string ObterNomeArquivoPDF(ConfiguracaoServico configuracaoServico,
            EFolderType eFolderType,
            string nomeArquivo,
            DateTime dataOperacao)
        {
            return ObterNomeArquivo(configuracaoServico,
            eFolderType,
            nomeArquivo,
            dataOperacao,
            "pdf");
        }
    }
}
