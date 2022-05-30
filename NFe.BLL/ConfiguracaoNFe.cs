using NFe.BLL.Configuracao.Entidades;

namespace NFe.BLL
{
    public class ConfiguracaoNFe : ConfiguracaoApp
    {
        public ConfiguracaoNFe(Emitente empresa) :
            base (empresa, DFe.Classes.Flags.ModeloDocumento.NFe)
        {

        }
    }
}
