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
            if (monofasico == null)
            {
                return;
            }

            if (monofasico.gMonoPadrao != null)
            {
                GrupoMonofasicoTributado = new CBSIBSGrupoMonofasicoTributado(monofasicoPadrao: monofasico?.gMonoPadrao);
            }

            if (monofasico.gMonoReten != null)
            {
                GrupoMonofasicoSujeitoRetencao = new CBSIBSGrupoMonofasicoSujeitoRetencao(monofasicoSujeitoRetencao: monofasico?.gMonoReten);
            }

            if (monofasico.gMonoRet != null)
            {
                GrupoMonofasicoRetido = new CBSIBSGrupoMonofasicoRetido(monofasicoRetido: monofasico?.gMonoRet);
            }

            if (monofasico.gMonoDif != null)
            {
                GrupoMonofasicoDiferido = new CBSIBSGrupoMonofasicoDiferimento(monofasicoDiferimento: monofasico?.gMonoDif);
            }

            ValorTotalMonofasicoCBS = monofasico?.vTotCBSMonoItem ?? 0;
            ValorTotalMonofasicoIBS = monofasico?.vTotIBSMonoItem ?? 0;
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
