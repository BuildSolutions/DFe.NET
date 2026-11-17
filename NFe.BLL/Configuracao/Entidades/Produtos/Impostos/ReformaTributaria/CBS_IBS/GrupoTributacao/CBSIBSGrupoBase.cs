using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    /// Base dos grupos de IBS/CBS do <b>mapeamento para o XML</b>: recebe valores já
    /// calculados e apenas os carrega até as classes de NFe.Classes.
    /// <para>Não confundir com PwFiscal.ReformaTributaria.Calculo (projeto
    /// simplesolution), que é o motor de cálculo: aquela recebe base e alíquota e deriva
    /// os valores. Esta aqui não calcula nada.</para>
    /// </summary>
    public abstract class CBSIBSGrupoBase : ICBSIBSGrupo
    {
        protected CBSIBSGrupoBase(decimal? aliquota,
            decimal? valor,

            decimal? percentualDiferimento = null,
            decimal? valorDiferimento = null,

            decimal? valorDevolucaoTributos = null,
            decimal? percentualDevolucaoTributos = null,

            decimal? percentualReducaoAliquota = null,
            decimal? aliquotaEfetiva = null,

            bool exigeGrupoReducao = false)
        {
            AliquotaPercentual = aliquota ?? 0;
            ValorTotal = valor ?? 0;
            //if (aliquota.HasValue && aliquota.Value > 0)
            //{
            //    GrupoTributacaoNormal = new CBSIBSGrupoTributacaoNormal(aliquota ?? 0, valor ?? 0);
            //}

            //gRed sai tambem com pRedAliq zero: a UB26-20 (rejeicao 1033), a UB45-20
            //(1074) e a UB64-20 (1079) exigem o grupo sempre que gCompraGov e informado,
            //porque o pRedutor do Art. 473 reduz a aliquota efetiva sem passar pelo
            //pRedAliq. Sem este grupo o pAliqEfet nao sai e o valor passa a ser conferido
            //contra a aliquota nominal. Testar a aliquota efetiva no lugar disto nao
            //resolve: em 2026 o pIBSMun e zero, logo a efetiva do municipio tambem e, e o
            //gRed do gIBSMun ficaria de fora. Mesma condicao do motor de calculo
            //(PwFiscal CBSIBSGrupoBaseCalculo).
            if (percentualReducaoAliquota.GetValueOrDefault() > 0 || exigeGrupoReducao)
            {
                GrupoReducao = new CBSIBSGrupoReducao(percentualReducaoAliquota ?? 0, aliquotaEfetiva ?? 0);
            }

            if (percentualDiferimento.HasValue && percentualDiferimento.Value > 0)
            {
                GrupoDiferimento = new CBSIBSGrupoDiferimento(percentualDiferimento ?? 0, valorDiferimento ?? 0);
            }

            if (valorDevolucaoTributos.HasValue && valorDevolucaoTributos.Value > 0)
            {
                GrupoDevolucaoTributo = new CBSIBSGrupoDevolucaoTributo(valorDevolucaoTributos ?? 0,
                    percentualDevolucaoTributos);
            }
        }
        //public CBSIBSGrupoTributacaoNormal GrupoTributacaoNormal { get; protected set; }
        public CBSIBSGrupoDiferimento GrupoDiferimento { get; protected set; }
        public CBSIBSGrupoDevolucaoTributo GrupoDevolucaoTributo { get; protected set; }
        public CBSIBSGrupoReducao GrupoReducao { get; protected set; }
        /// <summary>pIBS/pCBS — Alíquota em percentual (ex.: 1.5 = 1,5%).</summary>
        public decimal AliquotaPercentual { get; }
        public decimal ValorTotal { get; protected set; }
    }
}
