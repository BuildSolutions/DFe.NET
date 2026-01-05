using DFe.Utils;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS
{
    /// <summary>
    /// IBSCBS — Informações do Imposto de Bens e Serviços - IBS e da Contribuição de Bens e Serviços - CBS
    /// </summary>
    public class CBSIBSImposto
    {
        public CBSIBSImposto(CSTIBSCBS cst, int classificacaoTributaria, bool isDoacao, CBSIBSGrupo cbsIbs)
        {
            Cst = cst;
            ClassificacaoTributaria = classificacaoTributaria.ToString("D6");
            CbsIbs = cbsIbs;
        }

        public CBSIBSImposto(IBSCBS ibsCbs)
        {
            if (ibsCbs == null)
            {
                return;
            }

            Cst = ibsCbs.CST;
            ClassificacaoTributaria = ibsCbs.cClassTrib;
            CbsIbs = new CBSIBSGrupo(ibsCbs);
        }

        /// <summary>cClassTrib - Código de Situação Tributária do IBS e CBS (UB13)
        /// Utilizar tabela CÓDIGO DE CLASSIFICAÇÃO TRIBUTÁRIA DO IBS E DA CBS</summary>
        public CSTIBSCBS Cst { get; }

        /// <summary>CST - Código de Classificação Tributária do IBS e CBS (UB14)
        /// Utilizar tabela CÓDIGO DE CLASSIFICAÇÃO TRIBUTÁRIA DO IBS E DA CBS</summary>
        public string ClassificacaoTributaria { get; }

        /// <summary>indDoacao - Indica a natureza da operação de doação, orientando a apuração e a geração de débitos ou estornos conforme o cenário(UB14a)
        /// Informar “1” quando doação</summary>
        public bool? IsDoacao { get; }

        /// <summary>Tag: IBSCBS/gIBSCBS (UB15)</summary>
        public CBSIBSGrupo CbsIbs { get; }
    }
}
