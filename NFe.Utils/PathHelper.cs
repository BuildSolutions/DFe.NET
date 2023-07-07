/********************************************************************************/
/* Projeto: Biblioteca ZeusNFe                                                  */
/* Biblioteca C# para emissão de Nota Fiscal Eletrônica - NFe e Nota Fiscal de  */
/* Consumidor Eletrônica - NFC-e (http://www.nfe.fazenda.gov.br)                */
/*                                                                              */
/* Direitos Autorais Reservados (c) 2014 Adenilton Batista da Silva             */
/*                                       Zeusdev Tecnologia LTDA ME             */
/*                                                                              */
/*  Você pode obter a última versão desse arquivo no GitHub                     */
/* localizado em https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe               */
/*                                                                              */
/*                                                                              */
/*  Esta biblioteca é software livre; você pode redistribuí-la e/ou modificá-la */
/* sob os termos da Licença Pública Geral Menor do GNU conforme publicada pela  */
/* Free Software Foundation; tanto a versão 2.1 da Licença, ou (a seu critério) */
/* qualquer versão posterior.                                                   */
/*                                                                              */
/*  Esta biblioteca é distribuída na expectativa de que seja útil, porém, SEM   */
/* NENHUMA GARANTIA; nem mesmo a garantia implícita de COMERCIABILIDADE OU      */
/* ADEQUAÇÃO A UMA FINALIDADE ESPECÍFICA. Consulte a Licença Pública Geral Menor*/
/* do GNU para mais detalhes. (Arquivo LICENÇA.TXT ou LICENSE.TXT)              */
/*                                                                              */
/*  Você deve ter recebido uma cópia da Licença Pública Geral Menor do GNU junto*/
/* com esta biblioteca; se não, escreva para a Free Software Foundation, Inc.,  */
/* no endereço 59 Temple Street, Suite 330, Boston, MA 02111-1307 USA.          */
/* Você também pode obter uma copia da licença em:                              */
/* http://www.opensource.org/licenses/lgpl-license.php                          */
/*                                                                              */
/* Zeusdev Tecnologia LTDA ME - adenilton@zeusautomacao.com.br                  */
/* http://www.zeusautomacao.com.br/                                             */
/* Rua Comendador Francisco josé da Cunha, 111 - Itabaiana - SE - 49500-000     */
/********************************************************************************/

using System;
using System.IO;
using DFe.Utils.Extensoes;

namespace NFe.Utils
{
    public static class PathHelper
    {
        private static string ObterDiretorioArquivo(ConfiguracaoServico configuracaoServico, EFolderType eFolderType)
        {
            var caminhoXmlGerados = string.Empty;

            switch (eFolderType)
            {
                case EFolderType.AUTORIZADA:
                case EFolderType.DENEGADA:
                case EFolderType.INUTILIZADA:
                case EFolderType.CARTACORRECAO:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Processados");
                case EFolderType.CANCELADA:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Cancelados");
                case EFolderType.GERADOS:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Gerados");
                case EFolderType.LOTES:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "Lotes");
                case EFolderType.SUFRAMA:
                    return Path.Combine(configuracaoServico.DiretorioSalvarXml, "LotesSUFRAMA");
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
            nomeArquivo = nomeArquivo.Replace($".{extensaoArquivo}", "");

            var ano = dataOperacao.ToString("yyyy");
            var mes = dataOperacao.ToString("MM_yyyy");

            string nomeArquivoXML;
            switch (eFolderType)
            {
                case EFolderType.AUTORIZADA:
                case EFolderType.DENEGADA:
                    nomeArquivoXML = $"{nomeArquivo.RetornaNumeros()}-proc";
                    break;
                case EFolderType.INUTILIZADA:
                    nomeArquivoXML = $"{nomeArquivo.RetornaNumeros()}-proc-inu";
                    break;
                case EFolderType.CANCELADA:
                    nomeArquivoXML = $"{nomeArquivo.RetornaNumeros()}-procEventoNFe";
                    break;
                case EFolderType.GERADOS:
                case EFolderType.LOTES:
                    nomeArquivoXML = nomeArquivo;
                    break;
                case EFolderType.CARTACORRECAO:
                    nomeArquivoXML = $"{nomeArquivo.RetornaNumeros()}-procEventoNFe";
                    break;
                case EFolderType.SUFRAMA:
                    nomeArquivoXML = $"nfe_{nomeArquivo.RetornaNumeros()}-suframa";
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

        public static string SalvarArquivo(ConfiguracaoServico configuracaoServico,
            EFolderType eFolderType,
            string nomeArquivo,
            DateTime dataOperacao,
            string xml,
            string extensao)
        {
            var stw = new StreamWriter(ObterNomeArquivo(configuracaoServico,
            eFolderType,
            nomeArquivo,
            dataOperacao,
            extensao));
            stw.WriteLine(AddUTF8Header(xml));
            stw.Close();

            return nomeArquivo;
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
            stw.WriteLine(AddUTF8Header(xml));
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

        private static string AddUTF8Header(string xml)
        {
            string utf8Header = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            if (string.IsNullOrEmpty(xml)
                || xml.StartsWith(utf8Header))
            {
                return xml;
            }

            return utf8Header + xml;
        }
    }
}
