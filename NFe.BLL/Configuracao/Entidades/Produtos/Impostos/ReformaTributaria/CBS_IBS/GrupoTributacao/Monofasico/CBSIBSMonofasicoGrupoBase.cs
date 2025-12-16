using System;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico
{
    public abstract class CBSIBSMonofasicoGrupoBase : ICBSIBSGrupoMonofasico
    {
        public CBSIBSMonofasicoGrupoBase(decimal quantidade,
            decimal aliquotaAdRemCBS,
            decimal aliquotaAdRemIBS,
            decimal valorMonofasicoCBS,
            decimal valorMonofasicoIBS)
        {
            Quantidade = quantidade;
            AliquotaAdRemCBS = aliquotaAdRemCBS;
            AliquotaAdRemIBS = aliquotaAdRemIBS;
            ValorMonofasicoCBS = valorMonofasicoCBS;
            ValorMonofasicoIBS = valorMonofasicoIBS;
        }

        /// <summary>qBCMono — Quantidade tributada na monofasia.
        /// Informar a BC quantidade conforme unidade de medida estabelecida na legislação para o produto.</summary>
        public decimal Quantidade { get; set; }

        /// <summary>adRemCBS — Alíquota ad rem do CBS.</summary>
        public decimal AliquotaAdRemCBS { get; set; }

        /// <summary>adRemIBS — Alíquota ad rem do IBS.</summary>
        public decimal AliquotaAdRemIBS { get; set; }

        /// <summary>vCBSMono — Valor da CBS monofásica.
        /// O valor do imposto é obtido pela multiplicação da alíquota ad rem pela quantidade do produto conforme unidade de medida estabelecida na legislação.</summary>
        public decimal ValorMonofasicoCBS { get; private set; }

        /// <summary>vIBSMono — Valor da IBS monofásica.
        /// O valor do imposto é obtido pela multiplicação da alíquota ad rem pela quantidade do produto conforme unidade de medida estabelecida na legislação.</summary>
        public decimal ValorMonofasicoIBS { get; private set; }
    }
}
