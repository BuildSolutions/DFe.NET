using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.CreditoPresumido;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.CreditoPresumido;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// gIBSCBS — Grupo de Informações do ibs, cbs do item
    /// </summary>
    public class CBSIBSGrupo : ICBSIBSGrupo
    {
        private readonly IBSUFGrupo _ibsUFGrupo;
        private readonly IBSMunicipioGrupo _ibsMunicipioGrupo;

        public CBSIBSGrupo(decimal baseCalculo,
            IBSGrupo ibsGrupo,
            CBSGrupo cbsGrupo,
            CBSIBSGrupoTributacaoRegular grupoTributacaoRegular,
            CBSIBSGrupoTributacaoCompraGoverno grupoTributacaoCompraGoverno,
            CBSIBSGrupoMonofasico grupoTributacaoMonofasico,
            CBSIBSGrupoTransferenciaCredito grupoTransferenciaCredito,
            CBSIBSGrupoCreditoPresumido grupoCreditoPresumido)
        {
            BaseCalculo = baseCalculo;
            IBSGrupo = ibsGrupo;
            CBSGrupo = cbsGrupo;
            GrupoTributacaoRegular = grupoTributacaoRegular;
            GrupoTributacaoCompraGoverno = grupoTributacaoCompraGoverno;
            GrupoTributacaoMonofasico = grupoTributacaoMonofasico;
            GrupoTransferenciaCredito = grupoTransferenciaCredito;
        }

        public CBSIBSGrupo(IBSCBS ibsCbs)
        {
            var ibsUf = new IBSUFGrupo(ibsCbs: ibsCbs?.gIBSCBS);
            var ibsMunicipio = new IBSMunicipioGrupo(ibsCbs: ibsCbs?.gIBSCBS);

            BaseCalculo = ibsCbs.gIBSCBS.vBC;
            
            IBSGrupo = new IBSGrupo(ibsUFGrupo: ibsUf, ibsMunicipioGrupo: ibsMunicipio, valorTotal: ibsCbs?.gIBSCBS?.vIBS ?? 0);
            CBSGrupo = new CBSGrupo(ibsCbs: ibsCbs?.gIBSCBS);
            
            GrupoTributacaoRegular = new CBSIBSGrupoTributacaoRegular(tributacaoRegular: ibsCbs?.gIBSCBS?.gTribRegular);
            GrupoTributacaoCompraGoverno = new CBSIBSGrupoTributacaoCompraGoverno(compraGoverno: ibsCbs?.gIBSCBS?.gTribCompraGov);
            GrupoTributacaoMonofasico = new CBSIBSGrupoMonofasico(monofasico: ibsCbs.gIBSCBSMono);
            GrupoTransferenciaCredito = new CBSIBSGrupoTransferenciaCredito(transferenciaCredito: ibsCbs?.gTransfCred);
            GrupoCreditoPresumido = new CBSIBSGrupoCreditoPresumido(creditoPresumidoOperacao: ibsCbs?.gCredPresOper, creditoPresumidoZFM: ibsCbs?.gCredPresIBSZFM);
        }

        /// <summary>vBc - Base de cálculo do ibs e cbs (UB15)</summary>
        public decimal BaseCalculo { get; }

        /// <summary>Grupo de Informações do ibs UF e UBS Município</summary>
        public IBSGrupo IBSGrupo { get; private set; }

        /// <summary>gCBS — Grupo de Informações da cbs (UB55)</summary>
        public CBSGrupo CBSGrupo { get; }

        /// <summary>gTribRegular — Grupo de informações da Tributação Regular (UB68)</summary>
        public CBSIBSGrupoTributacaoRegular GrupoTributacaoRegular { get; }

        /// <summary>gTribCompraGov — Grupo de informações da composição do valor do ibs e da cbs em compras governamentais(UB82a)</summary>
        public CBSIBSGrupoTributacaoCompraGoverno GrupoTributacaoCompraGoverno { get; }

        /// <summary>gMono — Grupo de informações da Tributação Monofásica Padrão (UB84a)</summary>
        public CBSIBSGrupoMonofasico GrupoTributacaoMonofasico { get; }

        /// <summary>gTransfCred — Transferências de Crédito (UB106)</summary>
        public CBSIBSGrupoTransferenciaCredito GrupoTransferenciaCredito { get; }

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
        public CBSIBSGrupoCreditoPresumido GrupoCreditoPresumido { get; }
    }
}
