using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.BLL
{
    public class ConfiguracaoNFCe : ConfiguracaoApp
    {
        public ConfiguracaoCsc _configuracaoCsc { get; private set; }

        public ConfiguracaoNFCe(Emitente empresa, ConfiguracaoCsc configuracaoCsc) :
            base (empresa, DFe.Classes.Flags.ModeloDocumento.NFCe)
        {
            _configuracaoCsc = configuracaoCsc;
        }

        public override bool ValidarConfiguracaoApp(out string errors)
        {
            bool baseValidacao = base.ValidarConfiguracaoApp(out errors);
            var validator = new ConfiguracaoCscValidator();
            var resultado = validator.Validate(_configuracaoCsc);

            if (!resultado.IsValid)
            {
                errors += string.Join("\r\n", resultado.Errors.Select(err => err.ErrorMessage));
            }

            if (!baseValidacao)
            {
                return false;
            }

            return resultado.IsValid;
        }
    }
}
