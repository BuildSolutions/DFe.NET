using DFe.Classes.Flags;

namespace GNRE.Classes.Servicos.Consulta.Lote
{
    public class TconsLote_GNRE
    {
        /// <summary>
        /// Identificação do Ambiente: 1=Produção/2=Homologação 
        /// </summary>
        public TipoAmbiente Ambiente { get; set; }

        /// <summary>
        /// Número do recibo de gerado pelo portal GNRE
        /// </summary>
        public int numeroRecibo { get; set; }
    }
}
