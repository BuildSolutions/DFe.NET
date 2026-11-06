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

            // Instancia propria, e nao ConfiguracaoServico.Instancia: o singleton e do
            // processo inteiro, entao dois emitentes (ou duas threads) montando nota ao
            // mesmo tempo sobrescreviam certificado, UF e diretorios um do outro, e a
            // nota saia assinada com o certificado do emitente errado.
            _cfgServico = new ConfiguracaoServico();
            _cfgServico.TimeOut = 45000;
            _cfgServico.cUF = Emitente.Pessoa.Endereco.MunicipioEstadoSigla.GetValueOrDefault();

#if DEBUG
            _cfgServico.tpAmb = TipoAmbiente.Homologacao;
#else
                        
            _cfgServico.tpAmb = TipoAmbiente.Producao;
#endif
            //_cfgServico.tpAmb = TipoAmbiente.Producao;
            _cfgServico.tpEmis = TipoEmissao.teNormal;
            _cfgServico.ModeloDocumento = modeloDocumento;
            _cfgServico.VersaoLayout = VersaoServico.Versao400;
            _cfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls12;
            _cfgServico.SalvarXmlServicos = true;
            _cfgServico.DiretorioSalvarXml = Emitente.DiretorioSalvarXML;
            //_cfgServico.CarregaVersoes400();
            _cfgServico.DiretorioSchemas = Emitente.DiretorioSchemas;
            _cfgServico.Certificado = Emitente.Certificado;

            PublicarNoSingleton();
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
                return _cfgServico;
            }
            set
            {
                _cfgServico = value;
                PublicarNoSingleton();
            }
        }

        /// <summary>
        /// Copia esta configuração para <see cref="ConfiguracaoServico.Instancia"/>. A cópia
        /// é só de saída: nada dentro desta dll lê o singleton, todas as chamadas às outras
        /// dlls recebem a configuração explícita. Existe apenas para o chamador que ainda
        /// depende do singleton.
        /// <para>Com mais de um emitente o singleton não é fonte confiável: ele guarda a
        /// última configuração publicada e, com publicações concorrentes, pode guardar uma
        /// mistura das duas, porque a cópia é feita propriedade a propriedade. Alteração
        /// feita pelo chamador direto na instância devolvida por <see cref="CfgServico"/>
        /// também não chega aqui.</para>
        /// </summary>
        // TODO: espelho global; apagar quando o app chamador receber CfgServico explicitamente.
        private void PublicarNoSingleton()
        {
            ConfiguracaoServico.Instancia.CopiarPropriedades(_cfgServico);
        }
    }
}
