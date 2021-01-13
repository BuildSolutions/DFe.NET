using System;
using System.Collections.Generic;
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using GNRE.Classes.Enumerators;

namespace GNRE.Utils.Enderecos
{
    public static class Enderecador
    {
        public static List<EnderecoServico> ObterUrlServico(ConfiguracaoServico configuracaoServico = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;

            if(configServico.VersaoLayout != VersaoServico.Versao200)
            {
                throw new InvalidOperationException($"GNRE não configurada para a versão {configServico.VersaoLayout.XmlDescricao()}.");
            }

            if(configServico.tpAmb == TipoAmbiente.Homologacao)
            {
                return UrlHomologacao(configServico);
            }

            return UrlProducao(configServico);
        }

        private static List<EnderecoServico> UrlProducao(ConfiguracaoServico configuracaoServico)
        {
            var endServico = new List<EnderecoServico>();

            switch (configuracaoServico.cUF)
            {
                case Estado.MS:
                case Estado.SP:
                case Estado.ES:
                    throw new InvalidOperationException($"Não é possível emitir gnre para o estado de {configuracaoServico.cUF} na versão {VersaoServico.Versao200.XmlDescricao()}.");

                case Estado.MT:
                case Estado.MG:
                case Estado.PR:
                case Estado.RS:
                case Estado.AC:
                case Estado.AL:
                case Estado.AM:
                case Estado.BA:
                case Estado.CE:
                case Estado.DF:
                case Estado.GO:
                case Estado.MA:
                case Estado.PA:
                case Estado.PB:
                case Estado.PI:
                case Estado.RJ:
                case Estado.RN:
                case Estado.RO:
                case Estado.SC:
                case Estado.SE:
                case Estado.TO:
                case Estado.AP:
                case Estado.PE:
                case Estado.RR:
                    endServico.Add(new EnderecoServico(EServicosGNRE.ConsultaConfiguracaoUF, VersaoServico.Versao200, configuracaoServico.tpAmb, configuracaoServico.cUF, "https://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF"));
                    endServico.Add(new EnderecoServico(EServicosGNRE.GNREConsultaLote, VersaoServico.Versao200, configuracaoServico.tpAmb, configuracaoServico.cUF, "https://www.gnre.pe.gov.br/gnreWS/services/GnreResultadoLote"));
                    endServico.Add(new EnderecoServico(EServicosGNRE.RecepcaoLote, VersaoServico.Versao200, configuracaoServico.tpAmb, configuracaoServico.cUF, "https://www.gnre.pe.gov.br/gnreWS/services/GnreLoteRecepcao"));
                    break;
                default:
                    throw new InvalidOperationException(
                        "Não achei a url do seu estado(uf), tente com outra unidade federativa");
            }

            return endServico;

        }

        private static List<EnderecoServico> UrlHomologacao(ConfiguracaoServico configuracaoServico)
        {
            var endServico = new List<EnderecoServico>();

            switch (configuracaoServico.cUF)
            {
                case Estado.MS:
                case Estado.SP:
                case Estado.ES:
                    throw new InvalidOperationException($"Não é possível emitir gnre para o estado de {configuracaoServico.cUF} na versão {VersaoServico.Versao200.XmlDescricao()}.");

                case Estado.MT:
                case Estado.MG:
                case Estado.PR:
                case Estado.RS:
                case Estado.AC:
                case Estado.AL:
                case Estado.AM:
                case Estado.BA:
                case Estado.CE:
                case Estado.DF:
                case Estado.GO:
                case Estado.MA:
                case Estado.PA:
                case Estado.PB:
                case Estado.PI:
                case Estado.RJ:
                case Estado.RN:
                case Estado.RO:
                case Estado.SC:
                case Estado.SE:
                case Estado.TO:
                case Estado.AP:
                case Estado.PE:
                case Estado.RR:
                    endServico.Add(new EnderecoServico(EServicosGNRE.ConsultaConfiguracaoUF, VersaoServico.Versao200, configuracaoServico.tpAmb, configuracaoServico.cUF, "https://www.testegnre.pe.gov.br/gnreWS/services/GnreConfigUF"));
                    endServico.Add(new EnderecoServico(EServicosGNRE.GNREConsultaLote, VersaoServico.Versao200, configuracaoServico.tpAmb, configuracaoServico.cUF, "https://www.testegnre.pe.gov.br/gnreWS/services/GnreResultadoLote"));
                    endServico.Add(new EnderecoServico(EServicosGNRE.RecepcaoLote, VersaoServico.Versao200, configuracaoServico.tpAmb, configuracaoServico.cUF, "https://www.testegnre.pe.gov.br/gnreWS/services/GnreLoteRecepcao"));
                    break;
                default:
                    throw new InvalidOperationException(
                        "Não achei a url do seu estado(uf), tente com outra unidade federativa");
            }

            return endServico;
        }
    }
}
