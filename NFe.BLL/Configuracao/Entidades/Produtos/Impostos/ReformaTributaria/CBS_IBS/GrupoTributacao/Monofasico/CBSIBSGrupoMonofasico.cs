using NFe.Classes.Informacoes.Detalhe.Tributacao;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico
{
    /// <summary>
    /// gMono — Grupo de informações da Tributação Monofásica Padrão (UB84a)
    /// Pode conter vários subgrupos.
    /// 
    /// </summary>
    public class CBSIBSGrupoMonofasico
    {
        public CBSIBSGrupoMonofasico(CBSIBSGrupoMonofasicoTributado grupoMonofasicoTributado,
            CBSIBSGrupoMonofasicoRetido grupoMonofasicoRetido,
            CBSIBSGrupoMonofasicoSujeitoRetencao grupoMonofasicoSujeitoRetencao,
            CBSIBSGrupoMonofasicoDiferimento grupoMonofasicoDiferido,
            decimal valorTotalMonofasicoCBS,
            decimal valorTotalMonofasicoIBS)
        {
            GrupoMonofasicoTributado = grupoMonofasicoTributado;
            GrupoMonofasicoRetido = grupoMonofasicoRetido;
            GrupoMonofasicoSujeitoRetencao = grupoMonofasicoSujeitoRetencao;
            GrupoMonofasicoDiferido = grupoMonofasicoDiferido;
            ValorTotalMonofasicoCBS = valorTotalMonofasicoCBS;
            ValorTotalMonofasicoIBS = valorTotalMonofasicoIBS;
        }

        public CBSIBSGrupoMonofasico(gIBSCBSMono monofasico)
        {
            GrupoMonofasicoTributado = new CBSIBSGrupoMonofasicoTributado(monofasicoPadrao: monofasico?.gMonoPadrao);
            GrupoMonofasicoSujeitoRetencao = new CBSIBSGrupoMonofasicoSujeitoRetencao(monofasicoSujeitoRetencao: monofasico?.gMonoReten);
            GrupoMonofasicoRetido = new CBSIBSGrupoMonofasicoRetido(monofasicoRetido: monofasico?.gMonoRet);
            GrupoMonofasicoDiferido = new CBSIBSGrupoMonofasicoDiferimento(monofasicoDiferimento: monofasico?.gMonoDif);

            ValorTotalMonofasicoCBS = monofasico.vTotCBSMonoItem;
            ValorTotalMonofasicoIBS = monofasico.vTotIBSMonoItem;
        }

        public CBSIBSGrupoMonofasicoTributado GrupoMonofasicoTributado { get; set; }
        public CBSIBSGrupoMonofasicoRetido GrupoMonofasicoRetido { get; set; }
        public CBSIBSGrupoMonofasicoSujeitoRetencao GrupoMonofasicoSujeitoRetencao { get; set; }
        public CBSIBSGrupoMonofasicoDiferimento GrupoMonofasicoDiferido { get; set; }

        /// <summary>vTotCBSMonoItem — Total de CBS Monofásico.</summary>
        public decimal ValorTotalMonofasicoCBS { get; set; }

        /// <summary>vTotIBSMonoItem — Total de IBS Monofásico.</summary>
        public decimal ValorTotalMonofasicoIBS { get; set; }
    }
}
