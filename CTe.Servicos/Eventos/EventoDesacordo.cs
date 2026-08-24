using System.Threading.Tasks;
using CTe.Classes;
using CTe.Classes.Servicos.Evento;
using CTe.Classes.Servicos.Evento.Flags;
using CTe.Servicos.Factory;

namespace CTe.Servicos.Eventos
{
    public class EventoDesacordo
    {
        private readonly int _sequenciaEvento;
        private readonly string _cnpj;
        private readonly string _chave;
        private readonly string _indicadorDesacordo;
        private readonly string _observacao;

        public eventoCTe EventoEnviado { get; private set; }
        public retEventoCTe RetornoSefaz { get; private set; }

        public EventoDesacordo(int sequenciaEvento, string chave, string cnpj, string indicadorDesacordo, string observacao)
        {
            _chave = chave;
            _cnpj = cnpj;
            _sequenciaEvento = sequenciaEvento;
            _indicadorDesacordo = indicadorDesacordo;
            _observacao = observacao;
        }

        /// <summary>
        /// Gera o evento de desacordo de CTe
        /// </summary>
        /// <param name="configuracaoServico"></param>
        /// <param name="orgaoEmissor">Sempre considera a UF que gerou o xml. Então a empresa pode estar configurada para uma UF X e gerar o desacordo de um xml gerando na UF Y, sendo o evento, portando, enviado para UF Y</param>
        /// <returns></returns>
        public retEventoCTe Discordar(ConfiguracaoServico configuracaoServico = null, DFe.Classes.Entidades.Estado? orgaoEmissor = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;
            var eventoDiscordar = ClassesFactory.CriaEvPrestDesacordo(_indicadorDesacordo, _observacao);

            EventoEnviado = FactoryEvento.CriaEvento(CTeTipoEvento.Desacordo, _sequenciaEvento, _chave, _cnpj, eventoDiscordar, configServico, orgaoEmissor);
            RetornoSefaz = new ServicoController().Executar(CTeTipoEvento.Desacordo, _sequenciaEvento, _chave, _cnpj, eventoDiscordar, configServico, orgaoEmissor);
            return RetornoSefaz;
        }

        /// <summary>
        /// Gera o evento de cancelamento de desacordo de CTe
        /// </summary>
        /// <param name="configuracaoServico"></param>
        /// <param name="orgaoEmissor">Sempre considera a UF que gerou o xml. Então a empresa pode estar configurada para uma UF X e gerar o cancelmaento para um xml gerado na UF Y, sendo o evento, portando, enviado para UF Y</param>
        /// <returns></returns>
        public async Task<retEventoCTe> DiscordarAsync(ConfiguracaoServico configuracaoServico = null, DFe.Classes.Entidades.Estado? orgaoEmissor = null)
        {
            var configServico = configuracaoServico ?? ConfiguracaoServico.Instancia;
            var eventoDiscordar = ClassesFactory.CriaEvPrestDesacordo(_indicadorDesacordo, _observacao);

            EventoEnviado = FactoryEvento.CriaEvento(CTeTipoEvento.Desacordo, _sequenciaEvento, _chave, _cnpj, eventoDiscordar, configServico, orgaoEmissor);
            RetornoSefaz = await new ServicoController().ExecutarAsync(CTeTipoEvento.Desacordo, _sequenciaEvento, _chave, _cnpj, eventoDiscordar, configServico, orgaoEmissor);
            return RetornoSefaz;
        }
    }
}