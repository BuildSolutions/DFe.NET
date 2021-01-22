using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DFe.Classes.Entidades;
using DFe.Classes.Extensoes;
using DFe.Utils;
using DFe.Utils.Assinatura;
using GNRE.Classes;
using GNRE.Classes.Enumerators;
using GNRE.Classes.Servicos.Consulta.ConfiguracaoUF;
using GNRE.Classes.Servicos.Consulta.ConfiguracaoUF.Retorno;
using GNRE.Classes.Servicos.Consulta.Lote;
using GNRE.Classes.Servicos.Consulta.Lote.Retorno;
using GNRE.Classes.Servicos.Recepcao;
using GNRE.Classes.Servicos.Recepcao.Retorno;
using GNRE.Servicos.Retorno;
using GNRE.Utils;
using GNRE.Utils.Consulta.ConfiguracaoUF;
using GNRE.Utils.Consulta.Lote;
using GNRE.Utils.Excecoes;
using GNRE.Utils.Recepcao;
using GNRE.Utils.Validator;
using GNRE.Wsdl;
using Shared.DFe.Utils;

namespace GNRE.Servicos
{
    public class ServicosGNRE : IDisposable
    {
        private readonly X509Certificate2 _certificado;
        private readonly bool _controlarCertificado;
        private readonly ConfiguracaoServico _cFgServico;
        //private readonly string _path;

        /// <summary>
        /// Cria uma instância da Classe responsável pelos serviços relacionados à GNRE
        /// </summary>
        /// <param name="cFgServico"></param>
        public ServicosGNRE(ConfiguracaoServico cFgServico, X509Certificate2 certificado = null)
        {
            _cFgServico = cFgServico;
            _controlarCertificado = certificado == null;
            if (_controlarCertificado)
            {
                _certificado = CertificadoDigital.ObterCertificado(cFgServico.Certificado);
            }
            else
            {
                _certificado = certificado;
            }

           // _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //Define a versão do protocolo de segurança
            ServicePointManager.SecurityProtocol = cFgServico.ProtocoloDeSeguranca;

            if (_cFgServico.ValidarCertificadoDoServidor)
            {
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
        }

        private string SalvarArquivoXml(string nomeArquivo, string xmlString)
        {
            if (!_cFgServico.SalvarXmlServicos)
            {
                return null;
            }

            return PathHelper.SalvarArquivoXML(_cFgServico, EFolderType.LOTES, nomeArquivo, DateTime.Now, xmlString);
        }

        private INfeServico CriarServico(EServicosGNRE servico)
        {
            return ServicoGNREFactory.CriaWsdlOutros(servico, _cFgServico, _certificado);
        }

        /// <summary>
        /// Envia o Lote de GNREs
        /// </summary>
        /// <param name="gnres"></param>
        /// <returns>Retorna um objeto da classe RetornoRecepcaoGNRE com o retorno do serviço GNREAutorizacao</returns>
        public RetornoRecepcaoGNRE GNREAutorizacao(GuiasGNRE gnres)
        {
            var ufOriginal = _cFgServico.cUF;
            try
            {

                var versaoServico = _cFgServico.VersaoLayout.GetVersaoString();
                #region Cria o objeto wdsl para consulta

                gnres.TDadosGNRE.ForEach(guia =>
                {
                    if (!GNREUFDisponivel.Contains(guia.ufFavorecida))
                    {
                        throw new InvalidOperationException($"Não é possível emitir gnre para o estado de {guia.ufFavorecida} na versão { _cFgServico.VersaoLayout.XmlDescricao()}.");
                    }
                });

                _cFgServico.cUF = gnres.TDadosGNRE[0].ufFavorecida;

                var ws = CriarServico(EServicosGNRE.RecepcaoLote);

                ws.gnreCabecMsg = new gnreCabecMsg
                {
                    versaoDados = versaoServico
                };

                #endregion

                #region Cria o objeto TLote_GNRE

                var pedEnvio = new TLote_GNRE(gnres);

                #endregion

                #region Valida, Envia os dados e obtém a resposta

                var xmlPedidoAutorizacao = _cFgServico.RemoverAcentos
                    ? pedEnvio.ObterXmlString().RemoverAcentos()
                    : pedEnvio.ObterXmlString();

                Validador.Valida(EServicosGNRE.RecepcaoLote, _cFgServico.VersaoLayout, xmlPedidoAutorizacao, cfgServico: _cFgServico);
                var dadosGNRE = new XmlDocument();
                dadosGNRE.LoadXml(xmlPedidoAutorizacao);

                SalvarArquivoXml($"{DateTime.Now.ParaDataHoraString()}-env-lot.xml", xmlPedidoAutorizacao);

                XmlNode retorno;
                try
                {
                    retorno = ws.Execute(dadosGNRE);
                }
                catch (WebException ex)
                {
                    throw FabricaComunicacaoException.ObterException(EServicosGNRE.RecepcaoLote, ex);
                }

                var retornoXmlString = retorno.OuterXml;
                var retAutorizacaoLoteGNRE = new TRetLote_GNRE().CarregarDeXmlString(retornoXmlString);

                SalvarArquivoXml($"{DateTime.Now.ParaDataHoraString()}-rec.xml", retornoXmlString);

                return new RetornoRecepcaoGNRE(xmlPedidoAutorizacao, retAutorizacaoLoteGNRE.ObterXmlString(),
                    retornoXmlString, retAutorizacaoLoteGNRE);

                #endregion
            }
            finally
            {
                _cFgServico.cUF = ufOriginal;
            }
        }

        /// <summary>
        /// Consulta o resultado do lote
        /// </summary>
        /// <param name="numeroRecibo"></param>
        /// <returns>Retorna um objeto da classe RetornoProcessamentoLote com o retorno do serviço GNREConsultaResultadoLote</returns>
        public RetornoProcessamentoLote GNREConsultaResultadoLote(int numeroRecibo, Estado uf)
        {
            var ufOriginal = _cFgServico.cUF;
            try
            {
                var versaoServico = _cFgServico.VersaoLayout.GetVersaoString();

                #region Cria o objeto wdsl para consulta
                _cFgServico.cUF = uf;

                if (!GNREUFDisponivel.Contains(uf))
                {
                    throw new InvalidOperationException($"Não é possível emitir consultar a configuração para o estado de {uf} na versão { _cFgServico.VersaoLayout.XmlDescricao()}.");
                }

                var ws = CriarServico(EServicosGNRE.GNREResultadoLote);

                ws.gnreCabecMsg = new gnreCabecMsg
                {
                    versaoDados = versaoServico
                };

                #endregion

                #region Cria o objeto TLote_GNRE

                var pedConsulta = new TConsLote_GNRE(_cFgServico.tpAmb, numeroRecibo);

                #endregion

                #region Valida, Envia os dados e obtém a resposta

                var xmlConsulta = _cFgServico.RemoverAcentos
                    ? pedConsulta.ObterXmlString().RemoverAcentos()
                    : pedConsulta.ObterXmlString();

                Validador.Valida(EServicosGNRE.GNREResultadoLote, _cFgServico.VersaoLayout, xmlConsulta, _cFgServico);
                var dadosConsultaLote = new XmlDocument();
                dadosConsultaLote.LoadXml(xmlConsulta);

                SalvarArquivoXml($"{numeroRecibo}-ped-rec.xml", xmlConsulta);

                XmlNode retorno;
                try
                {
                    retorno = ws.Execute(dadosConsultaLote);
                }
                catch (WebException ex)
                {
                    throw FabricaComunicacaoException.ObterException(EServicosGNRE.RecepcaoLote, ex);
                }

                var retornoXmlString = retorno.OuterXml;
                var retResultadoLoteGNRE = new TResultLote_GNRE().CarregarDeXmlString(retornoXmlString);

                SalvarArquivoXml($"{numeroRecibo}-pro-rec.xml", retornoXmlString);

                return new RetornoProcessamentoLote(xmlConsulta, retResultadoLoteGNRE.ObterXmlString(),
                    retornoXmlString, retResultadoLoteGNRE);

                #endregion
            }
            finally
            {
                _cFgServico.cUF = ufOriginal;
            }
        }

        /// <summary>
        /// Consulta as configurações da GNRE para a uf informada
        /// </summary>
        /// <param name="gnres"></param>
        /// <param name="receita"></param>
        /// <returns>Retorna um objeto da classe RetornoConsultaConfiguracaoUF com o retorno do serviço GNREConsultaConfiguracaoUF</returns>
        public RetornoConsultaConfiguracaoUF GNREConsultaConfiguracaoUF(Estado uf, TConsultaConfigUfReceita receita = null)
        {
            var ufOriginal = _cFgServico.cUF;
            try
            {
                var versaoServico = _cFgServico.VersaoLayout.GetVersaoString();

                #region Cria o objeto wdsl para consulta
                _cFgServico.cUF = uf;

                if (!GNREUFDisponivel.Contains(uf))
                {
                    throw new InvalidOperationException($"Não é possível emitir consultar a configuração para o estado de {uf} na versão { _cFgServico.VersaoLayout.XmlDescricao()}.");
                }

                var ws = CriarServico(EServicosGNRE.ConsultaConfiguracaoUF);

                ws.gnreCabecMsg = new gnreCabecMsg
                {
                    versaoDados = "1.00"
                };

                #endregion

                #region Cria o objeto TLote_GNRE

                var pedConsulta = new TConsultaConfigUf(_cFgServico.tpAmb, uf, receita);

                #endregion

                #region Valida, Envia os dados e obtém a resposta

                var xmlConsulta = _cFgServico.RemoverAcentos
                    ? pedConsulta.ObterXmlString().RemoverAcentos()
                    : pedConsulta.ObterXmlString();

                Validador.Valida(EServicosGNRE.ConsultaConfiguracaoUF, _cFgServico.VersaoLayout, xmlConsulta, _cFgServico);
                var dadosConsultaLote = new XmlDocument();
                dadosConsultaLote.LoadXml(xmlConsulta);

                SalvarArquivoXml($"{DateTime.Now.ParaDataHoraString()}-ped-uf.xml", xmlConsulta);

                XmlNode retorno;
                try
                {
                    retorno = ws.Execute(dadosConsultaLote);
                }
                catch (WebException ex)
                {
                    throw FabricaComunicacaoException.ObterException(EServicosGNRE.RecepcaoLote, ex);
                }

                var retornoXmlString = retorno.OuterXml;
                var retConfiguracaoUF = new TConfigUf().CarregarDeXmlString(retornoXmlString);

                SalvarArquivoXml($"{DateTime.Now.ParaDataHoraString()}-uf.xml", retornoXmlString);

                return new RetornoConsultaConfiguracaoUF(xmlConsulta, retConfiguracaoUF.ObterXmlString(),
                    retornoXmlString, retConfiguracaoUF);

                #endregion

            }
            finally
            {
                _cFgServico.cUF = ufOriginal;
            }
}

        /// <summary>
        /// Devolve a lista de enums do tipo <see cref="CSTIPI"/> que são Tributados/>
        /// </summary>
        public static ISet<Estado> GNREUFDisponivel = new HashSet<Estado>()
        {
            Estado.AC,
            Estado.AL,
            Estado.AM,
            Estado.AP,
            Estado.BA,
            Estado.CE,
            Estado.DF,
            Estado.GO,
            Estado.MA,
            Estado.MG,
            Estado.MT,
            Estado.PA,
            Estado.PB,
            Estado.PE,
            Estado.PI,
            Estado.PR,
            Estado.RN,
            Estado.RJ,
            Estado.RO,
            Estado.RR,
            Estado.RS,
            Estado.SC,
            Estado.SE,
            Estado.TO,
        };

        #region Implementação do padrão Dispose

        // Flag: Dispose já foi chamado?
        private bool _disposed;

        // Implementação protegida do padrão Dispose.
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing
                && !_cFgServico.Certificado.ManterDadosEmCache && _controlarCertificado)
            {
                _certificado.Reset();
            }

            _disposed = disposing;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ServicosGNRE()
        {
            Dispose(false);
        }

        #endregion
    }
}
