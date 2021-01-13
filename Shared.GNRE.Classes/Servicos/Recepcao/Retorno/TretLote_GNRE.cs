using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Recepcao.Retorno
{
    public class TretLote_GNRE : IRetornoServico
    {
        /// <summary>
        /// Identificação do Ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente Ambiente { get; set; }

        public SituacaoRecepcao situacaoRecepcao { get; set; }

        public Recibo recibo { get; set; }
    }
}
