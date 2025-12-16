using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Interfaces;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao
{
    /// <summary>
    ///Grupo de Informações do IBS
    /// </summary>
    public class IBSGrupo : IIBSGrupo
    {
        private readonly decimal? _valorCreditoPresumidoASerDeduzido;

        public IBSGrupo(IBSUFGrupo ibsUFGrupo,
            IBSMunicipioGrupo ibsMunicipioGrupo,
            decimal valorTotal)
        {
            IBSUFGrupo = ibsUFGrupo;
            IBSMunicipioGrupo = ibsMunicipioGrupo;
            ValorTotal = valorTotal;
        }

        /// <summary>gIBSUF - Grupo de Informações do IBS para a UF (UB18)</summary>
        public IBSUFGrupo IBSUFGrupo { get; }

        /// <summary>gIBSMun - Grupo de Informações do IBS para o município (UB36)</summary>
        public IBSMunicipioGrupo IBSMunicipioGrupo { get; }

        /// <summary>vIBS — Valor do IBS (soma de vIBSUF e vIBSMun) (UB54a)
        /// Quando houver crédito presumido com indicador “IndDeduzCredPres=1”, o vCredPres deve ser abatido desse valor.</summary>
        public decimal ValorTotal { get; protected set; }
    }
}
