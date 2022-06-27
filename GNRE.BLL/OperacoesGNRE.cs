using System;
using System.Linq;
using System.Text;
using DFe.Classes.Entidades;
using GNRE.Classes;
using GNRE.Classes.Servicos.Consulta.ConfiguracaoUF;
using GNRE.Classes.Servicos.Consulta.Lote.Retorno;
using GNRE.Classes.Servicos.Recepcao.Retorno;
using GNRE.Servicos;
using GNRE.Servicos.Retorno;
using GNRE.Utils;

namespace GNRE.BLL
{
    public class OperacoesGNRE : IDisposable
    {
        private readonly ConfiguracaoServico _cfgServico;
        private ServicosGNRE _ServicosGNRE;
        private ServicosGNRE _ServicosGNREInstancia
        {
            get
            {
                _ServicosGNRE = _ServicosGNRE ?? new ServicosGNRE(_cfgServico);
                return _ServicosGNRE;
            }
            set
            {
                _ServicosGNRE = value;
            }
        }

        public OperacoesGNRE(ConfiguracaoServico cfgServico)
        {
            _cfgServico = cfgServico;
            //_ServicosGNRE = new ServicosGNRE(cfgServico);
        }

        public RetornoConsultaConfiguracaoUF ConsultarConfiguracoesUF(Estado uf, TConsultaConfigUfReceita receita, out string erro)
        {
            erro = string.Empty;
            var retornoConsulta = _ServicosGNREInstancia.GNREConsultaConfiguracaoUF(uf, receita);

            if (retornoConsulta.Retorno.situacaoConsulta.codigo == 450)
            {
                return retornoConsulta;
            }

            erro = string.Format("({0}) {1}",
                    retornoConsulta.Retorno.situacaoConsulta.codigo,
                    retornoConsulta.Retorno.situacaoConsulta.descricao);
            return null;
        }

        /// <summary>
        ///     Envia uma ou mais GNRE
        /// </summary>
        /// <param name="gnres">Lista de GNREs a serem enviadas</param>
        /// <returns>Retorna um objeto da classe RetornoRecepcaoGNRE com com os dados do resultado da transmissão</returns>
        public RetornoRecepcaoGNRE GNREAutorizacao(GuiasGNRE gnres, out string erro)
        {
            var retornoAutorizacao = _ServicosGNREInstancia.GNREAutorizacao(gnres);

            ValidarAutorizacao(retornoAutorizacao.Retorno, out erro);

            return retornoAutorizacao;
        }

        private bool ValidarAutorizacao(TRetLote_GNRE retornoAutorizacao, out string erro)
        {
            erro = string.Empty;

            if (retornoAutorizacao.situacaoRecepcao.codigo != 100)
            {
                erro = string.Format("({0}) {1}",
                    retornoAutorizacao.situacaoRecepcao.codigo,
                    retornoAutorizacao.situacaoRecepcao.descricao);
            }

            return true;
        }

        /// <summary>
        /// Recebe o retorno do processamento de um lote de GNREs pela SEFAZ
        /// </summary>
        /// <param name="recibo"></param>
        /// <param name="uf"></param>
        /// <returns>Retorna um objeto da classe GNREConsultaLote com com os dados do processamento do lote</returns>
        public RetornoProcessamentoLote GNREConsultaLote(long recibo, Estado uf, out string erro)
        {
            var consultarRecibo = _ServicosGNREInstancia.GNREConsultaResultadoLote(recibo, uf);

            if (!ValidarRetornoAutorizacao(consultarRecibo.Retorno, out erro))
            {
                return null;
            }

            return consultarRecibo;
        }

        private bool ValidarRetornoAutorizacao(TResultLote_GNRE retornoAutorizacao, out string erro)
        {
            erro = string.Empty;
            if (retornoAutorizacao.situacaoProcess.codigo == 402)
            {
                return true;
            }

            if (retornoAutorizacao.situacaoProcess.codigo == 403)
            {
                var sbErro = new StringBuilder();

                sbErro.AppendLine($"Erro ao processar a GNRE");
                foreach (var resultado in retornoAutorizacao.resultado.guia.Where(guia => guia.situacaoGuia != Classes.Enumerators.EConsultaLoteStatus.ProcessadaComSucesso))
                {
                    sbErro.AppendLine("");
                    sbErro.AppendLine($"Status: {resultado.situacaoGuia}");

                    if (!string.IsNullOrEmpty(resultado.itensGNRE.item?.documentoOrigem?.valor))
                    {
                        sbErro.AppendLine($"NFe: {resultado.itensGNRE.item?.documentoOrigem?.valor}");
                    }

                    if (resultado.motivosRejeicao?.motivo?.Count > 0)
                    {
                        sbErro.AppendLine($"UF: {resultado.ufFavorecida} - Valor Total: {resultado.valorGNRE:C2}")
                          .AppendLine(resultado.motivosRejeicao.ToString());
                    }
                }

                erro = sbErro.ToString();
                return true;
            }

            if (retornoAutorizacao.situacaoProcess.codigo == 401)
            {
                erro = string.Format("({0}) {1}\r\n\r\nServidor do SEFAZ está sobrecarregado.\r\nAguarde alguns minutos e tente novamente mais tarde.",
                    retornoAutorizacao.situacaoProcess.codigo,
                    retornoAutorizacao.situacaoProcess.descricao);
                return false;
            }

            erro = string.Format("({0}) {1}",
                    retornoAutorizacao.situacaoProcess.codigo,
                    retornoAutorizacao.situacaoProcess.descricao);
            return false;
        }

        public void Dispose()
        {
            _ServicosGNRE?.Dispose();
            _ServicosGNREInstancia?.Dispose();
        }
    }
}
