using System.Linq;
using System.Net;
using DFe.Utils;
using GNRE.BLL.Configuracao.Entidades;
using GNRE.BLL.Validators;
using GNRE.Utils;

namespace GNRE.BLL
{
    public class ConfiguracaoApp
    {
        protected ConfiguracaoServico _cfgServico
        {
            get;
            private set;
        }

        public Emitente Emitente { get; }
        //private readonly Configuracao _configuracao;

        public ConfiguracaoApp(Emitente empresa)
        {
            Emitente = empresa;

            _cfgServico = ConfiguracaoServico.Instancia;
            _cfgServico.TimeOut = 900000;
            _cfgServico.cUF = Emitente.Pessoa.Endereco.MunicipioEstadoSigla;

            _cfgServico.tpAmb = DFe.Classes.Flags.TipoAmbiente.Producao;
            _cfgServico.VersaoLayout = DFe.Classes.Flags.VersaoServico.Versao200;
            _cfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls12;
            _cfgServico.SalvarXmlServicos = true;
            _cfgServico.DiretorioSalvarXml = Emitente.DiretorioSalvarXML;
            _cfgServico.DiretorioSchemas = Emitente.DiretorioSchemas;
            _cfgServico.Certificado = Emitente.Certificado;
        }

        public virtual bool ValidarConfiguracaoApp(out string errors)
        {
            errors = string.Empty;

            var validatorEmitente = new EmitenteValidator();
            var resultadoEmitente = validatorEmitente.Validate(Emitente);
            if (!resultadoEmitente.IsValid)
            {
                errors += string.Join("\r\n", resultadoEmitente.Errors.Select(err => err.ErrorMessage));
            }

            return string.IsNullOrEmpty(errors);
        }

        public ConfiguracaoServico CfgServico
        {
            get
            {
                ConfiguracaoServico.Instancia.CopiarPropriedades(_cfgServico);
                return _cfgServico;
            }
            set
            {
                _cfgServico = value;
                ConfiguracaoServico.Instancia.CopiarPropriedades(value);
            }
        }
    }
}
