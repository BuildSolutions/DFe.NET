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
        public CBSIBSGrupoCreditoPresumido(TipocCredPres? codigoClassificacaoCreditoPresumido,
            decimal baseCalculoCreditoPresumido,

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
            CodigoClassificacaoCreditoPresumido = codigoClassificacaoCreditoPresumido;
            BaseCalculoCreditoPresumido = baseCalculoCreditoPresumido;

            if (cbsPercentualCreditoPresumido.HasValue && cbsPercentualCreditoPresumido.Value > 0)
            {
                CBSGrupoCreditoPresumido = new CBSGrupoCreditoPresumido(percentualCredito: cbsPercentualCreditoPresumido ?? 0,
                    valorCredito: cbsValorCreditoPresumido,
                    valorCreditoEmCondicaoSuspensiva: cbsValorCreditoPresumidoEmCondicaoSuspensiva);
            }

            if (ibsPercentualCreditoPresumido.HasValue && ibsPercentualCreditoPresumido.Value > 0)
            {
                IBSGrupoCreditoPresumido = new IBSGrupoCreditoPresumido(percentualCredito: ibsPercentualCreditoPresumido ?? 0,
                    valorCredito: ibsValorCreditoPresumido,
                    valorCreditoEmCondicaoSuspensiva: ibsValorCreditoPresumidoEmCondicaoSuspensiva);
            }

            // vCredPres e vCredPresCondSus so existem DENTRO de gCBSCredPres/gIBSCredPres,
            // e o subgrupo so e montado com pCredPres > 0. Valor informado sem percentual
            // desaparecia do item em silencio, enquanto os totalizadores da nota continuavam
            // somando o mesmo valor - nota com credito no total e sem credito em item nenhum.
            if (CBSGrupoCreditoPresumido == null
                && (cbsValorCreditoPresumido.HasValue || cbsValorCreditoPresumidoEmCondicaoSuspensiva.HasValue))
            {
                throw new ArgumentException("Crédito presumido de CBS com valor informado e sem percentual (pCredPres): o percentual é obrigatório no grupo.");
            }

            if (IBSGrupoCreditoPresumido == null
                && (ibsValorCreditoPresumido.HasValue || ibsValorCreditoPresumidoEmCondicaoSuspensiva.HasValue))
            {
                throw new ArgumentException("Crédito presumido de IBS com valor informado e sem percentual (pCredPres): o percentual é obrigatório no grupo.");
            }

            // O grupo gCredPresIBSZFM so pode sair quando ha competencia de apuracao:
            // sem ela o grupo iria para o XML com competApur vazio e a SEFAZ rejeita.
            if (!string.IsNullOrEmpty(zfmDataCompetenciaApuracao))
            {
                IBSGrupoCreditoPresumidoZFM = new IBSGrupoCreditoPresumidoZFM(dataCompetenciaApuracao: zfmDataCompetenciaApuracao,
                    tipoCreditoPresumidoIBSZFM: zfmTipoCreditoPresumido,
                    valorCreditoPresumidoIBSZFM: zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado);
            }

            // competApur e obrigatorio em TCredPresIBSZFM: tipo ou valor de ZFM informados
            // sem competencia nao tem como ir ao XML e desapareceriam em silencio.
            if (IBSGrupoCreditoPresumidoZFM == null
                && (zfmTipoCreditoPresumido != tpCredPresIBSZFM.tpCredPresIbsZfm0
                    || zfmValorCreditoPresumidoCalculadoSobreSaldoDevedorApurado != 0))
            {
                throw new ArgumentException("Crédito presumido de IBS da ZFM informado sem a competência de apuração (competApur): a competência é obrigatória no grupo.");
            }

            // cCredPres e obrigatorio em TCredPresOper e e ele que diz que a operacao
            // existe. Com subgrupo de CBS ou de IBS montado e codigo nulo, o grupo inteiro
            // seria descartado na geracao do XML e o credito desapareceria em silencio.
            if ((CBSGrupoCreditoPresumido != null || IBSGrupoCreditoPresumido != null)
                && !CodigoClassificacaoCreditoPresumido.HasValue)
            {
                throw new ArgumentException("Crédito presumido com percentual informado e sem código de classificação (cCredPres): o código é obrigatório no grupo da operação.");
            }

            // "00 - Sem Credito Presumido" existe no enum para representar a AUSENCIA do
            // credito, nao um credito de codigo 00: o grupo gCredPresOper inteiro nao deve
            // sair nesse caso. Sem esta recusa o chamador escolhia 00 achando que era um
            // codigo valido e levava a mensagem seguinte, que fala de percentual e nao
            // explica o que fazer.
            if (CodigoClassificacaoCreditoPresumido == TipocCredPres.SemCreditoPresumido)
            {
                throw new ArgumentException("Código de crédito presumido \"00 - Sem Crédito Presumido\" significa que não há crédito presumido: não informe o grupo em vez de informá-lo com esse código.");
            }

            // Todos os 13 codigos da tabela cCredPres (Anexo IV da NT 2025.002) tem
            // ind_gCBSCredPres = 1 ou ind_gIBSCredPres = 1: nenhum permite o grupo da
            // operacao sem ao menos um dos dois subgrupos. O schema deixa os dois como
            // minOccurs=0 porque qual deles vale depende do codigo, nao porque os dois
            // possam faltar.
            if (CodigoClassificacaoCreditoPresumido.HasValue
                && CBSGrupoCreditoPresumido == null
                && IBSGrupoCreditoPresumido == null)
            {
                throw new ArgumentException($"Crédito presumido da operação com código {CodigoClassificacaoCreditoPresumido} e sem percentual de CBS nem de IBS: a tabela cCredPres exige o crédito de pelo menos um dos dois.");
            }

            // gCredPresOper e gCredPresIBSZFM estao no mesmo xs:choice do leiaute
            // (TCIBSCBS, DFeTiposBasicos_v1.00.xsd): somente um dos dois pode existir no
            // item. Recusar aqui e melhor do que escolher um em silencio.
            if (CodigoClassificacaoCreditoPresumido.HasValue
                && IBSGrupoCreditoPresumidoZFM != null)
            {
                throw new ArgumentException("Crédito presumido da operação e crédito presumido de IBS da ZFM não podem ser informados no mesmo item: o leiaute aceita somente um dos dois grupos.");
            }
        }

        public CBSIBSGrupoCreditoPresumido(gCredPresOper creditoPresumidoOperacao, gCredPresIBSZFM creditoPresumidoZFM)
        {
            // CBSIBSGrupo chama este construtor quando qualquer um dos dois grupos existe
            // no XML lido, entao nenhum dos dois pode ser desreferenciado direto. A leitura
            // nao recusa os dois juntos de proposito: XML de terceiro fora do leiaute deve
            // ser lido como esta, a recusa fica na montagem.
            if (creditoPresumidoOperacao != null)
            {
                BaseCalculoCreditoPresumido = creditoPresumidoOperacao.vBCCredPres;
                CodigoClassificacaoCreditoPresumido = creditoPresumidoOperacao.cCredPres;

                if (creditoPresumidoOperacao.gCBSCredPres != null)
                {
                    CBSGrupoCreditoPresumido = new CBSGrupoCreditoPresumido(creditoPresumidoCBS: creditoPresumidoOperacao.gCBSCredPres);
                }

                if (creditoPresumidoOperacao.gIBSCredPres != null)
                {
                    IBSGrupoCreditoPresumido = new IBSGrupoCreditoPresumido(creditoPresumidoIBS: creditoPresumidoOperacao.gIBSCredPres);
                }
            }

            if (creditoPresumidoZFM != null)
            {
                IBSGrupoCreditoPresumidoZFM = new IBSGrupoCreditoPresumidoZFM(creditoPresumidoZFM: creditoPresumidoZFM);
            }
        }

        /// <summary>vBCCredPres — Valor da Base de Cálculo do Crédito Presumido da Operação(UB121)</summary>
        public decimal BaseCalculoCreditoPresumido { get; }

        /// <summary>cCredPres - Código de Classificação do Crédito Presumido (UB122).
        /// <para><b>Nulo</b> quando o item não tem crédito presumido da operação: é assim
        /// que a ausência se representa. O código 00 ("sem crédito presumido") existe no
        /// enum porque está na tabela oficial, mas significa a mesma ausência e o construtor
        /// o recusa - informar o grupo com código 00 seria dizer no XML que há crédito
        /// presumido de código "nenhum".</para>
        /// Utilizar tabela cCredPres (Anexo IV).
        /// Exemplos:
        /// 1 - Aquisição de Produtor Rural não contribuinte.
        /// 2 - Tomador de serviço de transporte de TAC PF não contrib.
        /// 3 - Aquisição de pessoa física com destino a reciclagem.
        /// 4 - Aquisição de bens móveis de PF não contrib. para revenda (veículos / brechó).
        /// 5 - Regime opcional para cooperativa
        /// </summary>
        public TipocCredPres? CodigoClassificacaoCreditoPresumido { get; }

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
