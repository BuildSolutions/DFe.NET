using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    public class TresultLote_GNRE : IRetornoServico
    {
        /// <summary>
        /// Identificação do Ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente Ambiente { get; set; }

        public SituacaoProcessamento situacaoProcess { get; set; }

        public Resultado resultado { get; set; }
    }
}
