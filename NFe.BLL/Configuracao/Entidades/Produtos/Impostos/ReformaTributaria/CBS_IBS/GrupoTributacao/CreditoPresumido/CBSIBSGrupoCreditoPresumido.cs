using System;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.CreditoPresumido;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.CreditoPresumido
{
    /// <summary>
    /// gCredPresOper — Crédito Presumido da Operação (UB120)
    /// Observação 1: a permissão ou vedação do
    /// preenchimento deste grupo está condicionada
    /// ao indicador “ind_gCredPresOper” da tabela de
    /// cClassTrib do IBS e da CBS.
    /// Observação 2: O valor "1" do indicador
    /// “ind_gCredPresOper” significa que o
    /// contribuinte pode utilizar o crédito presumido,
    /// sem obrigatoriedade (permite, mas não exige)
    /// </summary>
    public class CBSIBSGrupoCreditoPresumido : ICBSIBSGrupo
    {
        public CBSIBSGrupoCreditoPresumido(TipocCredPres codigoClassificacaoCreditoPresumido,

            decimal? cbsPercentualCreditoPresumido,
            decimal? cbsValorCreditoPresumido,
            decimal? cbsValorCreditoPresumidoEmCondicaoSuspensiva,

            decimal? ibsPercentualCreditoPresumido,
            decimal? ibsValorCreditoPresumido,
            decimal? ibsValorCreditoPresumidoEmCondicaoSuspensiva,

            string zfmDataCompetenciaApuracao,
            tpCredPresIBSZFM zfmTipoCreditoPresumido,
            decimal zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado)
        {
            if (cbsPercentualCreditoPresumido.HasValue && cbsPercentualCreditoPresumido.Value > 0)
            {
                CBSGrupoCreditoPresumido = new CBSGrupoCreditoPresumido(percentualCredito: cbsPercentualCreditoPresumido ?? 0,
                    valorCredito: cbsValorCreditoPresumido ?? 0,
                    valorCreditoEmCondicaoSuspensiva: cbsValorCreditoPresumidoEmCondicaoSuspensiva ?? 0);
            }

            if (cbsPercentualCreditoPresumido.HasValue && cbsPercentualCreditoPresumido.Value > 0)
            {
                IBSGrupoCreditoPresumido = new IBSGrupoCreditoPresumido(percentualCredito: ibsPercentualCreditoPresumido ?? 0,
                    valorCredito: ibsValorCreditoPresumido ?? 0,
                    valorCreditoEmCondicaoSuspensiva: ibsValorCreditoPresumidoEmCondicaoSuspensiva ?? 0);
            }

            IBSGrupoCreditoPresumidoZFM = new IBSGrupoCreditoPresumidoZFM(dataCompetenciaApuracao: zfmDataCompetenciaApuracao,
                tipoCreditoPresumidoIBSZFM: zfmTipoCreditoPresumido,
                valorCreditoPresumidoIBSZFM: zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado);
        }

        public CBSIBSGrupoCreditoPresumido(gCredPresOper creditoPresumidoOperacao, gCredPresIBSZFM creditoPresumidoZFM)
        {
            BaseCalculoCreditoPresumido = creditoPresumidoOperacao?.vBCCredPres ?? 0;
            CodigoClassificacaoCreditoPresumido = creditoPresumidoOperacao.cCredPres;

            if (creditoPresumidoOperacao.gCBSCredPres != null)
            {
                CBSGrupoCreditoPresumido = new CBSGrupoCreditoPresumido(creditoPresumidoCBS: creditoPresumidoOperacao.gCBSCredPres);
            }

            if (creditoPresumidoOperacao.gIBSCredPres != null)
            {
                IBSGrupoCreditoPresumido = new IBSGrupoCreditoPresumido(creditoPresumidoIBS: creditoPresumidoOperacao.gIBSCredPres);
            }

            IBSGrupoCreditoPresumidoZFM = new IBSGrupoCreditoPresumidoZFM(creditoPresumidoZFM: creditoPresumidoZFM);
        }

        /// <summary>vBCCredPres — Valor da Base de Cálculo do Crédito Presumido da Operação(UB121)</summary>
        public decimal BaseCalculoCreditoPresumido { get; }

        /// <summary>cCredPres - Código de Classificação do Crédito Presumido (UB122)
        /// Utilizar tabela cCredPres (Anexo IV).
        /// Exemplos:
        /// 1 - Aquisição de Produtor Rural não contribuinte.
        /// 2 - Tomador de serviço de transporte de TAC PF não contrib.
        /// 3 - Aquisição de pessoa física com destino a reciclagem.
        /// 4 - Aquisição de bens móveis de PF não contrib. para revenda (veículos / brechó).
        /// 5 - Regime opcional para cooperativa
        /// </summary>
        public TipocCredPres CodigoClassificacaoCreditoPresumido { get; }

        /// <summary>
        /// gIBSCredPres - Grupo de Informações do Crédito Presumido referente ao IBS (UB123)
        /// Grupo de Informações do Crédito Presumido do IBS, quando aproveitado pelo emitente do documento.
        /// Observação: a obrigatoriedade ou vedação do preenchimento deste grupo está condicionada ao indicador “ind_gIBSCredPres” da tabela de cCredPres do IBS e da CBS
        /// </summary>
        public IBSGrupoCreditoPresumido IBSGrupoCreditoPresumido { get; private set; }

        /// <summary>
        /// gCBSCredPres - Grupo de Informações do Crédito Presumido referente ao CBS (UB127)
        /// Grupo de Informações do Crédito Presumido do CBS, quando aproveitado pelo emitente do documento.
        /// Observação: a obrigatoriedade ou vedação do preenchimento deste grupo está condicionada ao indicador “ind_gCBSCredPres” da tabela de cCredPres do IBS e da CBS
        /// </summary>
        public CBSGrupoCreditoPresumido CBSGrupoCreditoPresumido { get; private set; }

        /// <summary>
        /// gCredPresIBSZFM - Grupo para apropriação de crédito presumido de IBS sobre o saldo devedor na ZFM(art. 450, § 1º, LC 214/25) (UB131)
        /// Observação: a obrigatoriedade ou vedação do preenchimento deste grupo está condicionada ao indicador “ind_gCredPresIBSZFM” da tabela de CST do IBS e da CBS.
        /// </summary>
        public IBSGrupoCreditoPresumidoZFM IBSGrupoCreditoPresumidoZFM { get; private set; }
    }
}
