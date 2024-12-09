using System;
using System.Collections.Generic;
using DFe.Utils;
using NFe.BLL.Configuracao.Entidades;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.ConsultaCadastro;
using NFe.Classes.Servicos.DistribuicaoDFe;
using NFe.Classes.Servicos.Inutilizacao;
using NFe.Classes.Servicos.Recepcao;
using NFe.Classes.Servicos.Recepcao.Retorno;
using NFe.Classes.Servicos.Suframa;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;

namespace NFe.BLL
{
    public class OperacoesNFe : IDisposable
    {
        private readonly ConfiguracaoServico _cfgServico;
        private ServicosNFe _servicosNFe;
        private ServicosNFe _servicosNFeInstancia
        {
            get
            {
                _servicosNFe = _servicosNFe ?? new ServicosNFe(_cfgServico);
                return _servicosNFe;
            }
            set
            {
                _servicosNFe = value;
            }
        }

        public OperacoesNFe(ConfiguracaoServico cfgServico)
        {
            _cfgServico = cfgServico;
            //_servicosNFe = new ServicosNFe(cfgServico);
        }

        public (bool sucesso, string produtoDescricao, string ncm, string cest, string erro) ConsultarGtinProduto(string codigoBarrasGtin)
        {
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            var consultaGtin = _servicosNFeInstancia.ConsultaGtin(codigoBarrasGtin.PadLeft(14, '0'));
            if (consultaGtin.retConsGtin.cStat == 9490)
            {
                return (sucesso: true,
                produtoDescricao: consultaGtin.retConsGtin.xProd,
                ncm: consultaGtin.retConsGtin.NCM,
                cest: consultaGtin.retConsGtin.CEST,
                erro: string.Empty);
            }
         
            return (sucesso: false, 
                produtoDescricao: string.Empty,
                ncm: string.Empty,
                cest: string.Empty,
                erro: string.Format("Não foi possível consultar o código de barras.{0}{0}Status:{1}{0}Motivo:{2}",
                    Environment.NewLine,
                    consultaGtin.retConsGtin.cStat,
                    consultaGtin.retConsGtin.xMotivo));
        }

        public bool ConsultarStatusSefaz(out string erro)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            var statusServico = _servicosNFeInstancia.NfeStatusServico();
            if (statusServico.Retorno.cStat == 107)
            {
                return true;
            }

            erro = string.Format("Não foi possível verificar o status do SEFAZ-SP.{0}{0}Status:{1}{0}Motivo:{2}",
                            Environment.NewLine,
                            statusServico.Retorno.cStat,
                            statusServico.Retorno.xMotivo);
            return false;
        }

        public RetornoNfeDistDFeInt DownloadXmlNFe(string uf, string cnpj, string chaveAcesso, out string erro)
        {
            erro = string.Empty;
            var retornoConsulta = _servicosNFeInstancia.NfeDistDFeInteresse(uf, cnpj, chNFE: chaveAcesso);

            if (retornoConsulta.Retorno.cStat == 138)
            {
                return retornoConsulta;
            }
            else if (retornoConsulta.Retorno.cStat == 137)
            {
                erro = "Arquivo xml não liberado para download.\r\n\rPrazo de até 24hrs para ser liberado o download do arquivo xml.";
                return null;
            }

            erro = string.Format("({0}) {1}",
                    retornoConsulta.Retorno.cStat,
                    retornoConsulta.Retorno.xMotivo);
            return null;
        }

        /// <summary>
        ///     Consulta a situação cadastral, com base na UF/Documento
        ///     <para>O documento pode ser: CPF ou CNPJ. O serviço avaliará o tamanho da string passada e determinará se a coonsulta será por CPF ou por CNPJ</para>
        /// </summary>
        /// <param name="uf">Sigla da UF consultada, informar 'SU' para SUFRAMA.</param>
        /// <param name="tipoDocumento">Tipo do documento</param>
        /// <param name="documento">CPF ou CNPJ</param>
        /// <returns>Retorna um objeto da classe RetornoNfeConsultaCadastro com o retorno do serviço NfeConsultaCadastro</returns>
        public bool ConsultaCadastroDestinatario(string uf, ConsultaCadastroTipoDocumento tipoDocumento,
            string documento, out string erro)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            var retornoConsulta = _servicosNFeInstancia.NfeConsultaCadastro(uf, tipoDocumento, documento);

            if (retornoConsulta.Retorno.infCons.cStat != 0)
            {
                string dataBaixaContribuinte = retornoConsulta.Retorno.infCons?.infCad?.dBaixa;
                if (!string.IsNullOrEmpty(dataBaixaContribuinte))
                {
                    dataBaixaContribuinte = string.Format("Empresa baixada em: {0:dd/MM/yyyy HH:mm:ss}", retornoConsulta.Retorno.infCons.infCad.dBaixa);
                }

                string contribuinteNaoHabilitado = string.Empty;
                if (retornoConsulta.Retorno.infCons.infCad?.cSit == 0)
                {
                    contribuinteNaoHabilitado = "Contribuinte não habilitado";

                    erro = string.Format("{2}O Cliente Selecionado possui alertas na receita federal que restringem a emissão da NFe.{2}{2}({0}) {1} {2}{3}{2}{4}",
                        retornoConsulta.Retorno.infCons.cStat,
                        retornoConsulta.Retorno.infCons.xMotivo,
                        Environment.NewLine,
                        dataBaixaContribuinte,
                        contribuinteNaoHabilitado);
                    return false;
                }

                return true;
            }

            erro = "Falha ao consultar o status do destinatário no SEFAZ.";
            return false;
        }

        /// <summary>
        ///     Envia um evento do tipo "Carta de Correção"
        /// </summary>
        /// <returns>Retorna um objeto da classe <see cref="RetornoRecepcaoEvento"/> com o retorno do serviço <see cref="RecepcaoEvento"/></returns>
        public RetornoRecepcaoEvento RecepcaoEventoCancelamento(int idlote, int sequenciaEvento, string protocoloCancelamento,
            string chaveNFe, string correcao, string cpfcnpj, out string erro)
        {
            return RecepcaoEvento(NFeTipoEvento.TeNfeCancelamento,
            idlote, sequenciaEvento, chaveNFe,
            correcao, cpfcnpj, out erro, protocoloCancelamento);
        }

        /// <summary>
        ///     Envia um evento do tipo "Cancelamento"
        /// </summary>
        /// <returns>Retorna um objeto da classe <see cref="RetornoRecepcaoEvento"/> com o retorno do serviço <see cref="RecepcaoEvento"/></returns>
        public RetornoRecepcaoEvento RecepcaoEventoCartaCorrecao(int idlote, int sequenciaEvento,
            string chaveNFe, string correcao, string cpfcnpj, out string erro)
        {
            return RecepcaoEvento(NFeTipoEvento.TeNfeCartaCorrecao,
            idlote, sequenciaEvento, chaveNFe,
            correcao, cpfcnpj, out erro);
        }

        public RetornoRecepcaoEvento RecepcaoEventoManifestoDestinatario(int idlote, NFeTipoEvento nfeTipoEventoManifestacaoDestinatario, int sequenciaEvento,
            string chaveNFe, string correcao, string cpfcnpj, out string erro)
        {
            return RecepcaoEvento(nfeTipoEventoManifestacaoDestinatario,
            idlote, sequenciaEvento, chaveNFe,
            correcao, cpfcnpj, out erro);
        }

        private RetornoRecepcaoEvento RecepcaoEvento(NFeTipoEvento tipoEvento, 
            int idlote, int sequenciaEvento, string chaveNFe,
            string correcao, string cpfcnpj, out string erro, string protocoloCancelamento = null)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);
            RetornoRecepcaoEvento retornoConsulta;

            switch (tipoEvento)
            {
                case NFeTipoEvento.TeNfeCartaCorrecao:
                    retornoConsulta = _servicosNFeInstancia.RecepcaoEventoCartaCorrecao(idlote, sequenciaEvento, chaveNFe, correcao, cpfcnpj);
                    break;
                case NFeTipoEvento.TeNfeCancelamento:
                    retornoConsulta = _servicosNFeInstancia.RecepcaoEventoCancelamento(idlote, sequenciaEvento, protocoloCancelamento, chaveNFe, correcao, cpfcnpj);
                    break;
                //case NFeTipoEvento.TeNfeCancelamentoSubstituicao:
                //    break;
                case NFeTipoEvento.TeMdConfirmacaoDaOperacao:
                case NFeTipoEvento.TeMdCienciaDaOperacao:
                case NFeTipoEvento.TeMdDesconhecimentoDaOperacao:
                case NFeTipoEvento.TeMdOperacaoNaoRealizada:
                    retornoConsulta = _servicosNFeInstancia.RecepcaoEventoManifestacaoDestinatario(idlote, sequenciaEvento, chaveNFe, tipoEvento, cpfcnpj, null);
                    break;
                //    break;
                //    break;
                //    break;
                default:
                    erro = $"Serviço de não configurado.\r\n\r\nEntre em contato com o suporte técnico do sistema.";
                    return null;
            }

            if (!ValidarRetornoEvento(retornoConsulta, out erro))
            {
                return null;
            }

            return retornoConsulta;
        }

        private bool ValidarRetornoEvento(RetornoRecepcaoEvento retornoEvento, out string erro)
        {
            erro = string.Empty;
            if (retornoEvento.Retorno.cStat != 128 // Lote processado
                && retornoEvento.Retorno.cStat != 135 // Recebido pelo Sistema de Registro de Eventos, com vinculação do evento na NFe
                && retornoEvento.Retorno.cStat != 138 // Recebido pelo Sistema de Registro de Eventos – vinculação do evento à respectiva NF-e prejudicada 
                && retornoEvento.Retorno.cStat != 155) // Cancelamento homologado fora de prazo
            {
                erro = string.Format("Falha ao registrar o evento da {2}.\r\n({0}) {1}",
                    retornoEvento.Retorno.cStat,
                    retornoEvento.Retorno.xMotivo,
                    _cfgServico.ModeloDocumento);
                return false;
            }

            var tpEvento = retornoEvento.ProcEventosNFe[0]?.evento.infEvento.tpEvento;
            int strCStat = retornoEvento.ProcEventosNFe[0]?.retEvento?.infEvento?.cStat ?? 0;
            string strchNFe = retornoEvento.ProcEventosNFe[0].evento.infEvento.chNFe;
            int.TryParse(strchNFe.Substring(25, 9), out int nroNFe);
            string xMotivo = retornoEvento.ProcEventosNFe[0]?.retEvento?.infEvento?.xMotivo;

            if (strCStat != 135
                && strCStat != 138 // Recebido pelo Sistema de Registro de Eventos – vinculação do evento à respectiva NF-e prejudicada 
                && strCStat != 155) // Cancelamento homologado fora de prazo
            {
                if (strCStat == 573
                    && (tpEvento == NFeTipoEvento.TeMdCienciaDaOperacao
                        || tpEvento == NFeTipoEvento.TeMdDesconhecimentoDaOperacao
                        || tpEvento == NFeTipoEvento.TeMdConfirmacaoDaOperacao
                        || tpEvento == NFeTipoEvento.TeMdOperacaoNaoRealizada))
                {
                    return true;
                }

                if (tpEvento == NFeTipoEvento.TeNfeCancelamento // Cancelamento
                    && strCStat == 690)
                {
                    xMotivo += "\r\n\r\nCancele a CT-e ou a MDF-e vinculada à esta NF-e e repita a operação.\r\nSe a CT-e ou a MDF-e já foi cancelada recentemente aguardar 15 minutos e tentar novamente.";
                }

                erro = string.Format("Não foi possível registrar o evento ({4}) para a {5} {0}{1}{1}Status: {1}{2}{1}{1}Motivo: {1}{3}",
                    nroNFe,
                    Environment.NewLine,
                    strCStat,
                    xMotivo,
                    tpEvento.Descricao(),
                    _cfgServico.ModeloDocumento);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Envia uma ou mais NFe
        /// </summary>
        /// <param name="idLote">ID do Lote</param>
        /// <param name="indSinc">Indicador de Sincronização</param>
        /// <param name="nFes">Lista de NFes a serem enviadas</param>
        /// <returns>Retorna um objeto da classe RetornoNFeAutorizacao com com os dados do resultado da transmissão</returns>
        public RetornoNFeAutorizacao NFeAutorizacao(int idLote, IndicadorSincronizacao indSinc, List<Classes.NFe> nfes, out string erro)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);

            var retornoConsulta = _servicosNFeInstancia.NFeAutorizacao(idLote, indSinc, nfes);

            ValidarAutorizacao(retornoConsulta.Retorno, out erro);

            return retornoConsulta;
        }

        private bool ValidarAutorizacao(retEnviNFe retornoAutorizacao, out string erro)
        {
            erro = string.Empty;

            if (retornoAutorizacao.cStat == 104)
            {
                if (retornoAutorizacao.protNFe.infProt.cStat != 100)
                {
                    erro = string.Format("({0}) {1}",
                        retornoAutorizacao.protNFe.infProt.cStat,
                        retornoAutorizacao.protNFe.infProt.xMotivo);

                    if (retornoAutorizacao.protNFe.infProt.cStat == 204)
                    {
                        erro += "\r\n\r\nClique no botão Serializar Protocolo.";
                    }

                    return false;
                }

                return true;
            }
            else if (retornoAutorizacao.cStat != 103)
            {
                erro = string.Format("({0}) {1}",
                    retornoAutorizacao.cStat,
                    retornoAutorizacao.xMotivo);
            }

            return true;
        }

        /// <summary>
        ///     Recebe o retorno do processamento de uma ou mais NFe's pela SEFAZ
        /// </summary>
        /// <param name="recibo"></param>
        /// <returns>Retorna um objeto da classe RetornoNFeRetAutorizacao com com os dados do processamento do lote</returns>
        public RetornoNFeRetAutorizacao NFeRetAutorizacao(string recibo, out string erro)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);

            var consultarRecibo = _servicosNFeInstancia.NFeRetAutorizacao(recibo);

            if (!ValidarRetornoAutorizacao(consultarRecibo.Retorno, out erro))
            {
                return null;
            }

            return consultarRecibo;
        }

        private bool ValidarRetornoAutorizacao(retConsReciNFe retornoAutorizacao, out string erro)
        {
            erro = string.Empty;
            if (retornoAutorizacao.cStat != 104)
            {
                if (retornoAutorizacao.cStat == 105) // Lote em processamento
                {
                    erro = string.Format("({0}) {1}\r\n\r\nServidor do SEFAZ está sobrecarregado.\r\nAguarde alguns minutos e tente novamente mais tarde.",
                        retornoAutorizacao.cStat,
                        retornoAutorizacao.xMotivo);
                    return false;
                }

                erro = string.Format("({0}) {1}",
                    retornoAutorizacao.cStat,
                    retornoAutorizacao.xMotivo);
                return false;
            }

            if (retornoAutorizacao.protNFe[0].infProt.cStat != 100) // Denegada
            {
                erro = string.Format("({0}) {1}",
                        retornoAutorizacao.protNFe[0].infProt.cStat,
                        retornoAutorizacao.protNFe[0].infProt.xMotivo);

                if (retornoAutorizacao.protNFe[0].infProt.cStat == 204)
                {
                    erro += "\r\n\r\nClique no botão Serializar Protocolo.";
                }

                return false;
            }

            return true;
        }

        /// <summary>
        ///     Consulta a Situação da NFe
        /// </summary>
        /// <returns>Retorna um objeto da classe RetornoNfeConsultaProtocolo com os dados da Situação da NFe</returns>
        public RetornoNfeConsultaProtocolo NfeConsultaProtocolo(string chave, out string erro)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);

            var retornoConsulta = _servicosNFeInstancia.NfeConsultaProtocolo(chave);

            if (!ValidarRetornoConsultaProtocolo(retornoConsulta.Retorno, out erro))
            {
                return null;
            }

            return retornoConsulta;
        }

        private bool ValidarRetornoConsultaProtocolo(retConsSitNFe retornoConsulta, out string erro)
        {
            erro = string.Empty;
            if (retornoConsulta.cStat != 100
                && retornoConsulta.cStat != 101)
            {
                erro = string.Format("({0}) {1}",
                    retornoConsulta.cStat,
                    retornoConsulta.xMotivo);
                return false;
            }

            if (retornoConsulta.protNFe.infProt.cStat != 100) // Denegada
            {
                erro = string.Format("({0}) {1}",
                        retornoConsulta.protNFe.infProt.cStat,
                        retornoConsulta.protNFe.infProt.xMotivo);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Inutiliza uma faixa de números
        /// </summary>
        /// <param name="cnpj"></param>
        /// <param name="ano"></param>
        /// <param name="serie"></param>
        /// <param name="numeroInicial"></param>
        /// <param name="numeroFinal"></param>
        /// <param name="justificativa"></param>
        /// <returns>Retorna um objeto da classe RetornoNfeInutilizacao com o retorno do serviço NfeInutilizacao</returns>
        public RetornoNfeInutilizacao NfeInutilizacao(string cnpj, int ano, int serie,
            int numeroInicial, int numeroFinal, string justificativa, out string erro)
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);

            var versaoServico = ServicoNFe.NfeInutilizacao.VersaoServicoParaString(_cfgServico.VersaoNfeInutilizacao);

            var retornoInutilizacao = _servicosNFeInstancia.NfeInutilizacao(cnpj, ano, _cfgServico.ModeloDocumento, serie, numeroInicial, numeroFinal, justificativa);

            if (!ValidarRetornoInutilizacao(retornoInutilizacao.Retorno, out erro))
            {
                return null;
            }

            return retornoInutilizacao;

        }

        private bool ValidarRetornoInutilizacao(retInutNFe retornoInutilizacao, out string erro)
        {
            erro = string.Empty;
            if (retornoInutilizacao.infInut.cStat != 102)
            {
                erro = string.Format("({0}) {1}",
                    retornoInutilizacao.infInut.cStat,
                    retornoInutilizacao.infInut.xMotivo);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Serviço destinado à distribuição de informações resumidas e documentos fiscais eletrônicos de interesse de um ator, seja este pessoa física ou jurídica.
        /// </summary>
        /// <param name="ufAutor">Código da UF do Autor</param>
        /// <param name="documento">CNPJ/CPF do interessado no DF-e</param>
        /// <param name="ultNSU">Último NSU recebido pelo Interessado</param>
        /// <param name="nSU">Número Sequencial Único</param>
        /// <param name="chNFE">Chave eletronica da NF-e</param>
        /// <returns>Retorna um objeto da classe RetornoNfeDistDFeInt com os documentos de interesse do CNPJ/CPF pesquisado</returns>
        public RetornoNfeDistDFeInt DistribuicaoDocumentosFiscais(string ufAutor, string documento, out string erro, string ultNSU = "0", string nSU = "0", string chNFE = "")
        {
            erro = string.Empty;
            //var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);

            var retornoConsulta = _servicosNFeInstancia.NfeDistDFeInteresse(ufAutor, documento, ultNSU, nSU, chNFE);

            if (!ValidarRetornoDistribuicaoDocumentosFiscais(retornoConsulta.Retorno, out erro))
            {
                return null;
            }

            return retornoConsulta;
        }

        private bool ValidarRetornoDistribuicaoDocumentosFiscais(retDistDFeInt retornoConsulta, out string erro)
        {
            erro = string.Empty;
            if (retornoConsulta == null
                || retornoConsulta.cStat != 138)
            {
                erro = string.Format("({0}) {1}",
                    retornoConsulta.cStat,
                    retornoConsulta.xMotivo);
                return false;
            }

            return true;
        }

        public RetornoLoteSuframa GerarArquivoLoteSuframa(Suframa suframa, out string erro)
        {
            var notasFiscais = new List<loteNotaFiscal>();
            suframa.ChavesAcessoNFes.ForEach(nfe => notasFiscais.Add(new loteNotaFiscal() { chaveAcesso = nfe, txZero = false }));

            erro = string.Empty;

            return _servicosNFeInstancia.NFeGerarArquivoLoteSuframa(
                dataEmissao: suframa.DataEmissao,
                destinatarioCNPJ: suframa.DestinatarioCNPJ,
                transportadoraCNPJ: suframa.TransportadoraCNPJ,
                destinatarioInscricaoSuframa: suframa.DestinatarioInscricaoSuframa,
                chavesAcessoNFes: notasFiscais,
                numeroLoteSuframa: suframa.NumeroLoteSuframa,
                ufDestino: suframa.UfDestino.ToString(),
                ufOrigem: suframa.UfOrigem.ToString()
                );
        }

        public void Dispose()
        {
            _servicosNFe?.Dispose();
            _servicosNFe = null;
        }
    }
}
