using NFe.BLL.Configuracao.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

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
