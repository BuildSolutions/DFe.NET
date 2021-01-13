using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using DFe.Classes.Flags;
using DFe.Utils;
using GNRE.Classes.Enumerators;
using GNRE.Utils.Excecoes;

namespace GNRE.Utils.Validator
{
    public static class Validador
    {
        internal static string ObterArquivoSchema(EServicosGNRE servicoGNRE, VersaoServico versaoServico)
        {
            switch (versaoServico)
            {
                case VersaoServico.Versao200:
                    switch (servicoGNRE)
                    {
                        case EServicosGNRE.RecepcaoLote:
                            return "lote_gnre_v2.00.xsd";
                        case EServicosGNRE.GNREConsultaLote:
                            return "lote_gnre_consulta_v1.00.xsd";
                        case EServicosGNRE.ConsultaConfiguracaoUF:
                            return "consulta_config_uf_v1.00.xsd";
                    }
                    break;
                case VersaoServico.Versao100:
                case VersaoServico.Versao300:
                case VersaoServico.Versao310:
                case VersaoServico.Versao400:
                default:
                    throw new InvalidOperationException($"GNRE não configurada para a versão {versaoServico.XmlDescricao()}.");
            }

            return null;
        }

        public static void Valida(EServicosGNRE servicoGNRE, VersaoServico versaoServico, string stringXml, bool loteNfe = true, ConfiguracaoServico cfgServico = null)
        {
            var pathSchema = String.Empty;

            pathSchema = cfgServico == null || (string.IsNullOrWhiteSpace(cfgServico.DiretorioSchemas))
                ? ConfiguracaoServico.Instancia.DiretorioSchemas
                : cfgServico.DiretorioSchemas;

            Valida(servicoGNRE, versaoServico, stringXml, pathSchema);
        }

        public static void Valida(EServicosGNRE servicoGNRE, VersaoServico versaoServico, string stringXml, string pathSchema = null)
        {
            if (!Directory.Exists(pathSchema))
                throw new Exception("Diretório de Schemas não encontrado: \n" + pathSchema);

            var arquivoSchema = Path.Combine(pathSchema, ObterArquivoSchema(servicoGNRE, versaoServico, stringXml, loteNfe));

            // Define o tipo de validação
            var cfg = new XmlReaderSettings { ValidationType = ValidationType.Schema };

            // Carrega o arquivo de esquema
            var schemas = new XmlSchemaSet();
            schemas.XmlResolver = new XmlUrlResolver();

            cfg.Schemas = schemas;
            // Quando carregar o eschema, especificar o namespace que ele valida
            // e a localização do arquivo 
            schemas.Add(null, arquivoSchema);
            // Especifica o tratamento de evento para os erros de validacao
            cfg.ValidationEventHandler += ValidationEventHandler;
            // cria um leitor para validação
            var validator = XmlReader.Create(new StringReader(stringXml), cfg);
            try
            {
                // Faz a leitura de todos os dados XML
                while (validator.Read())
                {
                }
            }
            catch (XmlException err)
            {
                // Um erro ocorre se o documento XML inclui caracteres ilegais
                // ou tags que não estão aninhadas corretamente
                throw new Exception("Ocorreu o seguinte erro durante a validação XML:" + "\n" + err.Message);
            }
            finally
            {
                validator.Close();
            }
        }

        internal static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            throw new ValidacaoSchemaException(args.Message);
        }
    }
}