using System;
using System.Security.Cryptography.X509Certificates;
using GNRE.Classes.Enumerators;
using GNRE.Utils;
using GNRE.Utils.Enderecos;
using GNRE.Wsdl;
using GNRE.Wsdl.Consulta.ConfiguracaoUF;
using GNRE.Wsdl.Consulta.Lote;
using GNRE.Wsdl.Recepcao;

namespace GNRE.Servicos
{
    public static class ServicoGNREFactory
    {
        /// <summary>
        /// Cria o cliente <see cref="SoapHttpClientProtocol"/> para os serviços passados no parâmetro <paramref name="servico"/>
        /// </summary>
        /// <param name="servico">Tipo </param>
        /// <param name="cfg">Configuração do serviço</param>
        /// <param name="certificado">Certificado</param>
        /// <returns></returns>        
        public static INfeServico CriaWsdlOutros(EServicosGNRE servico, ConfiguracaoServico cfg, X509Certificate2 certificado)
        {
            var enderecos = Enderecador.ObterUrlServico(cfg);

            string url = enderecos.Find(end => end.ServicoGNRE == servico)?.Url;

            switch (servico)
            {
                case EServicosGNRE.RecepcaoLote:
                    return new GnreLoteRecepcao2(url, certificado, cfg.TimeOut);
                case EServicosGNRE.GNREResultadoLote:
                    return new GnreConsultaResultadoLote2(url, certificado, cfg.TimeOut);
                case EServicosGNRE.ConsultaConfiguracaoUF:
                    return new GnreConfiguracaoUF2(url, certificado, cfg.TimeOut);
                default:
                    throw new ArgumentOutOfRangeException("servicoGnre", servico, null);
            }
        }
    }
}