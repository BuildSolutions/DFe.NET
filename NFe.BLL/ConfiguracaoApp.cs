using DFe.Classes.Flags;
using DFe.Utils;
using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Validators;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Utils;
using System.Linq;
using System.Net;

namespace NFe.BLL
{
    public abstract class ConfiguracaoApp
    {
        protected ConfiguracaoServico _cfgServico
        {
            get;
            private set;
        }

        public Emitente Emitente { get; }
        //private readonly Configuracao _configuracao;

        protected ConfiguracaoApp(Emitente empresa, ModeloDocumento modeloDocumento)
        {
            Emitente = empresa;

            _cfgServico = ConfiguracaoServico.Instancia;
            _cfgServico.TimeOut = 45000;
            _cfgServico.cUF = Emitente.Pessoa.Endereco.MunicipioEstadoSigla.GetValueOrDefault();

//#if DEBUG
            //            _cfgServico.tpAmb = TipoAmbiente.Homologacao;
            //#else
            //            _cfgServico.tpAmb = TipoAmbiente.Homologacao;
            //            //_cfgServico.tpAmb = TipoAmbiente.Producao;
            //#endif
            _cfgServico.tpAmb = TipoAmbiente.Producao;
            _cfgServico.tpEmis = TipoEmissao.teNormal;
            _cfgServico.ModeloDocumento = modeloDocumento;
            _cfgServico.VersaoLayout = VersaoServico.Versao400;
            _cfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls12;
            _cfgServico.SalvarXmlServicos = true;
            _cfgServico.DiretorioSalvarXml = Emitente.DiretorioSalvarXML;
            //_cfgServico.CarregaVersoes400();
            _cfgServico.DiretorioSchemas = Emitente.DiretorioSchemas;
            _cfgServico.Certificado = Emitente.Certificado;

            var xNome = Emitente.Pessoa.NomeRazaoSocial;
            if (xNome.Length > 60)
            {
                xNome = xNome.Substring(0, 60);
            }

            var xFant = string.IsNullOrEmpty(Emitente.Pessoa.ApelidoFantasia) ? null : Emitente.Pessoa.ApelidoFantasia;
            if (!string.IsNullOrEmpty(xFant) && xFant.Length > 60)
            {
                xFant = xFant.Substring(0, 60);
            }

            var xLgr = Emitente.Pessoa.Endereco.Logradouro;
            if (xLgr?.Length > 60)
            {
                xLgr = xLgr.Substring(0, 60);
            }
        }

        public virtual bool ValidarConfiguracaoApp(out string errors)
        {
            errors = string.Empty;
            //var validatorCfg = new ConfiguracaoServicoValidator();
            //var resultadoCfg = validatorCfg.Validate(CfgServico);

            var validatorEmitente = new EmitenteValidator();
            var resultadoEmitente = validatorEmitente.Validate(Emitente);
            if (!resultadoEmitente.IsValid)
            {
                errors += string.Join("\r\n", resultadoEmitente.Errors.Select(err => err.ErrorMessage));
            }

            //if (!resultadoCfg.IsValid)
            //{
            //    errors += string.Join("\r\n", resultadoCfg.Errors.Select(err => err.ErrorMessage));
            //}

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
