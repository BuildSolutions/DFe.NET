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

        /// <summary>
        /// Leitura das duas gerações do leiaute. A NT 2025.002 v1.50 passou a envolver os
        /// subgrupos em gIBSMonoAdRem e gCBSMonoAdRem, e separou o que antes era um grupo
        /// só com IBS e CBS juntos em dois grupos espelhados, um por tributo. Documento
        /// autorizado antes disso traz gMonoPadrao, gMonoReten e gMonoRet direto no
        /// gIBSCBSMono.
        /// <para>Sem ler os invólucros novos, um XML emitido por esta mesma versão voltava
        /// só com os totais, e a reemissão saía com o gIBSCBSMono sem tributação.</para>
        /// <para>Ad valorem (gIBSMonoAdValorem e gCBSMonoAdValorem) fica de fora: estas
        /// entidades só guardam quantidade e alíquota ad rem, que é o que a emissão produz.
        /// Ler o que não se consegue escrever de volta daria a impressão de round-trip que
        /// não existe.</para>
        /// </summary>
        public CBSIBSGrupoMonofasico(gIBSCBSMono monofasico)
        {
            if (monofasico == null)
            {
                return;
            }

            if (monofasico.gIBSMonoAdRem != null
                || monofasico.gCBSMonoAdRem != null)
            {
                LerLeiauteAdRem(monofasico.gIBSMonoAdRem, monofasico.gCBSMonoAdRem);
            }
            else
            {
                LerLeiauteAnterior(monofasico);
            }

            ValorTotalMonofasicoCBS = monofasico.vTotCBSMonoItem;
            ValorTotalMonofasicoIBS = monofasico.vTotIBSMonoItem;
        }

        /// <summary>
        /// Leiaute da NT 2025.002 v1.50: cada tributo tem o seu invólucro, e os dois trazem
        /// o mesmo qBCMono. O grupo retido (gMonoRet) perdeu quantidade e alíquota no
        /// leiaute novo e carrega só o valor.
        /// </summary>
        private void LerLeiauteAdRem(gIBSMonoAdRem ibsAdRem, gCBSMonoAdRem cbsAdRem)
        {
            if (ibsAdRem?.gMonoPadrao != null || cbsAdRem?.gMonoPadrao != null)
            {
                GrupoMonofasicoTributado = new CBSIBSGrupoMonofasicoTributado(
                    quantidadeTributada: ibsAdRem?.gMonoPadrao?.qBCMono ?? cbsAdRem?.gMonoPadrao?.qBCMono ?? 0,
                    aliquotaAdRemCBS: cbsAdRem?.gMonoPadrao?.adRemCBS ?? 0,
                    aliquotaAdRemIBS: ibsAdRem?.gMonoPadrao?.adRemIBS ?? 0,
                    valorMonofasicoCBS: cbsAdRem?.gMonoPadrao?.vCBSMono ?? 0,
                    valorMonofasicoIBS: ibsAdRem?.gMonoPadrao?.vIBSMono ?? 0);
            }

            if (ibsAdRem?.gMonoReten != null || cbsAdRem?.gMonoReten != null)
            {
                GrupoMonofasicoSujeitoRetencao = new CBSIBSGrupoMonofasicoSujeitoRetencao(
                    quantidadeRetencao: ibsAdRem?.gMonoReten?.qBCMonoReten ?? cbsAdRem?.gMonoReten?.qBCMonoReten ?? 0,
                    aliquotaAdRemCBSRetencao: cbsAdRem?.gMonoReten?.adRemCBSReten ?? 0,
                    aliquotaAdRemIBSRetencao: ibsAdRem?.gMonoReten?.adRemIBSReten ?? 0,
                    valorMonofasicoCBSRetencao: cbsAdRem?.gMonoReten?.vCBSMonoReten ?? 0,
                    valorMonofasicoIBSRetencao: ibsAdRem?.gMonoReten?.vIBSMonoReten ?? 0);
            }

            if (ibsAdRem?.gMonoRet != null || cbsAdRem?.gMonoRet != null)
            {
                GrupoMonofasicoRetido = new CBSIBSGrupoMonofasicoRetido(
                    quantidadeRetida: 0,
                    aliquotaAdRemCBSRetido: 0,
                    aliquotaAdRemIBSRetido: 0,
                    valorMonofasicoCBSRetido: cbsAdRem?.gMonoRet?.vCBSMonoRet ?? 0,
                    valorMonofasicoIBSRetido: ibsAdRem?.gMonoRet?.vIBSMonoRet ?? 0);
            }
        }

        /// <summary>
        /// Leiaute anterior à NT 2025.002 v1.50, que deixou de ser aceito em 03/11/2026 e
        /// continua aqui só para ler documento já autorizado. O gMonoDif saiu do leiaute:
        /// é lido, e a emissão não o escreve de volta.
        /// </summary>
        private void LerLeiauteAnterior(gIBSCBSMono monofasico)
        {
            if (monofasico.gMonoPadrao != null)
            {
                GrupoMonofasicoTributado = new CBSIBSGrupoMonofasicoTributado(monofasicoPadrao: monofasico.gMonoPadrao);
            }

            if (monofasico.gMonoReten != null)
            {
                GrupoMonofasicoSujeitoRetencao = new CBSIBSGrupoMonofasicoSujeitoRetencao(monofasicoSujeitoRetencao: monofasico.gMonoReten);
            }

            if (monofasico.gMonoRet != null)
            {
                GrupoMonofasicoRetido = new CBSIBSGrupoMonofasicoRetido(monofasicoRetido: monofasico.gMonoRet);
            }

            if (monofasico.gMonoDif != null)
            {
                GrupoMonofasicoDiferido = new CBSIBSGrupoMonofasicoDiferimento(monofasicoDiferimento: monofasico.gMonoDif);
            }
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
