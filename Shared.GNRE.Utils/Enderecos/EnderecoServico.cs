using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using GNRE.Classes.Enumerators;

namespace GNRE.Utils.Enderecos
{
    public class EnderecoServico
    {
        public EnderecoServico(EServicosGNRE servicoGNRE, VersaoServico versaoServico, TipoAmbiente tipoAmbiente, Estado estado, string url)
        {
            ServicoGNRE = servicoGNRE;
            VersaoServico = versaoServico;
            TipoAmbiente = tipoAmbiente;
            Estado = estado;
            Url = url;
        }

        public EServicosGNRE ServicoGNRE { get; }
        public VersaoServico VersaoServico { get; }
        public TipoAmbiente TipoAmbiente { get; }
        public Estado Estado { get; }
        public string Url { get; }
    }
}
