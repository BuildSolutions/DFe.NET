using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using DFe.Utils.Extensoes;
using GNRE.Utils.Enderecos;

namespace GNRE.Utils
{
    public sealed class ConfiguracaoServico : INotifyPropertyChanged
    {
        private static volatile ConfiguracaoServico _instancia;
        private static readonly object SyncRoot = new object();
        private string _diretorioSchemas;
        private bool _salvarXmlServicos;
        private VersaoServico _versaoLayout;
        private Estado _cUf;
        private TipoAmbiente _tpAmb;

        public ConfiguracaoServico()
        {
            Certificado = new ConfiguracaoCertificado();

            cUF = Estado.AC;
        }

        static ConfiguracaoServico()
        {
        }

        /// <summary>
        ///     Configurações relativas ao Certificado Digital
        /// </summary>
        public ConfiguracaoCertificado Certificado { get; set; }

        /// <summary>
        ///     Tempo máximo de espera pela resposta do webservice, em milisegundos
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        ///     Estado de destino do webservice
        /// </summary>
        public Estado cUF
        {
            get { return _cUf; }
            set
            {
                if (value == _cUf) return;
                _cUf = value;
                OnPropertyChanged();
                AtualizaVersoes();
            }
        }

        /// <summary>
        ///     Tipo de ambiente do webservice (Produção, Homologação)
        /// </summary>
        public TipoAmbiente tpAmb
        {
            get { return _tpAmb; }
            set
            {
                if (value == _tpAmb) return;
                _tpAmb = value;
                OnPropertyChanged();
                AtualizaVersoes();
            }
        }

        public VersaoServico VersaoLayout
        {
            get { return _versaoLayout; }
            set
            {
                if (value == _versaoLayout) return;
                _versaoLayout = value;
                OnPropertyChanged();
                AtualizaVersoes();
            }
        }

        public SecurityProtocolType ProtocoloDeSeguranca { get; set; }

        /// <summary>
        ///     Diretório onde estão armazenados os schemas para validação
        /// </summary>
        public string DiretorioSchemas
        {
            get { return _diretorioSchemas; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Directory.Exists(value))
                    throw new Exception("Diretório " + value + " não encontrado!");
                _diretorioSchemas = value;
            }
        }

        /// <summary>
        ///     Informar se a biblioteca deve salvar o xml de envio e de retorno
        /// </summary>
        public bool SalvarXmlServicos
        {
            get { return _salvarXmlServicos; }
            set
            {
                if (!value)
                    DiretorioSalvarXml = "";
                _salvarXmlServicos = value;
            }
        }

        /// <summary>
        ///     Diretório onde os xmls de envio/retorno devem ser salvos
        /// </summary>
        public string DiretorioSalvarXml { get; set; }

        /// <summary>
        /// Determina se o cerificado do servidor deve ser verificado
        /// </summary>
        public bool ValidarCertificadoDoServidor { get; set; }


        /// <summary>
        ///     Instância do Singleton de ConfiguracaoServico
        /// </summary>
        public static ConfiguracaoServico Instancia
        {
            get
            {
                if (_instancia != null) return _instancia;
                lock (SyncRoot)
                {
                    if (_instancia != null) return _instancia;
                    _instancia = new ConfiguracaoServico();
                }

                return _instancia;
            }
        }

        public bool RemoverAcentos { get; set; }

        /// <summary>
        ///     Limpa a instancia atual caso exista
        /// </summary>
        public static void LimparInstancia()
        {
            _instancia = null;
        }

        /// <summary>
        /// Atualiza as versões dos serviços
        /// <para>Obs: As versões do serviços podem variar em função  da UF(<see cref="Estado"/>), do tipo de ambiente(<see cref="TipoAmbiente"/>), do modelo de documento(<see cref="DFe.Classes.Flags.ModeloDocumento"/>) e da forma de emissão(<see cref="TipoEmissao"/>)</para>
        /// </summary>
        private void AtualizaVersoes()
        {
            if(VersaoLayout.EValido() &&
                VersaoLayout != VersaoServico.Versao200)
            {
                throw new InvalidOperationException($"GNRE não configurada para a versão {VersaoLayout.XmlDescricao()}.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        [AttributeUsage(AttributeTargets.Method)]
        public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
        {
            public NotifyPropertyChangedInvocatorAttribute() { }
            public NotifyPropertyChangedInvocatorAttribute(string parameterName)
            {
                ParameterName = parameterName;
            }

            public string ParameterName { get; private set; }
        }
    }
}