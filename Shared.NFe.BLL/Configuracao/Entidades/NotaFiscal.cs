using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.Entidades.Produtos;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using System;
using System.Collections.Generic;

namespace NFe.BLL.Configuracao.Entidades
{
    public class NotaFiscal
    {
        public NotaFiscal(
            int serie,
            long numero,
            TipoNFe eTipoNFe,
            string naturezaOperacaoDescricao,
            DateTime dataEmissao,
            DateTime? dataSaida,
            FinalidadeNFe eFinalidadeNFe,
            Emitente emitente,
            Destinatario destinatario,
            Transporte dadosTransporte,
            Totalizador total,
            IReadOnlyList<Produto> produtos,
            IReadOnlyList<Duplicata> duplicatas,
            string dadosAdicionaisFisco,
            string dadosAdicionaisContribuinte,
            PresencaComprador ePresencaComprador,
            IReadOnlyList<Pagamento> formasPagamento = null,
            IReadOnlyList<DocumentoReferenciado> documentosReferenciados = null,
            string numeroPedidoCompraB2B = null,
            bool isZonaFrancaManaus = false,
            Protocolo protocolo = null)
        {
            var emitenteUf = emitente?.Pessoa?.Endereco?.MunicipioEstadoSigla.GetValueOrDefault();
            var destinatarioUf = destinatario?.Pessoa?.Endereco?.MunicipioEstadoSigla.GetValueOrDefault();
            var isOperacaoInterna = ePresencaComprador == PresencaComprador.pcPresencial
                && destinatario?.EConsumidorFinal == ConsumidorFinal.cfConsumidorFinal
                && dadosTransporte?.ModalidadeFrete == Classes.Informacoes.Transporte.ModalidadeFrete.mfSemFrete;

            Serie = serie;
            Numero = numero;
            Emitente = emitente;
            NaturezaOperacaoDescricao = naturezaOperacaoDescricao.SanitizeString();
            DataEmissao = dataEmissao;
            DataSaida = dataSaida;
            ETipoNFe = eTipoNFe;
            Destinatario = destinatario;
            EPresencaComprador = ePresencaComprador;
            EFinalidadeNFe = eFinalidadeNFe;
            EDestinoOperacao = _obterDestinoOperacao(emitenteUf.GetValueOrDefault(), destinatarioUf, isOperacaoInterna);
            IsImportacao = destinatario != null && destinatarioUf.GetValueOrDefault() == Estado.EX && ETipoNFe == TipoNFe.tnEntrada;
            IsExportacao = destinatario != null && (destinatarioUf.GetValueOrDefault() == Estado.EX && ETipoNFe == TipoNFe.tnSaida);
            DadosTransporte = dadosTransporte;
            NumeroPedidoCompraB2B = numeroPedidoCompraB2B.SanitizeString();
            Total = total;
            Duplicatas = duplicatas;
            DadosAdicionaisFisco = dadosAdicionaisFisco.SanitizeString();
            DadosAdicionaisContribuinte = dadosAdicionaisContribuinte.SanitizeString();
            DocumentosReferenciados = documentosReferenciados;
            Produtos = produtos;
            FormasPagamento = formasPagamento;
            IsZonaFrancaManaus = isZonaFrancaManaus;
            Protocolo = protocolo;
            CNf = Protocolo?.ObterCNf() ?? new Random().Next(1, 99999999).ToString("00000000");
        }

        public NotaFiscal(Classes.nfeProc notafiscalProcessada)
        {
            var nfe = notafiscalProcessada.NFe;
            var duplicatas = new List<Duplicata>();
            var produtos = new List<Produto>();
            var pagamentos = new List<Pagamento>();
            Transporte transportadora = null;

            if (nfe.infNFe.transp != null)
            {
                transportadora = new Transporte(nfe.infNFe.transp);
            }

            nfe.infNFe.cobr?.dup.ForEach(item => duplicatas.Add(new Duplicata(item.nDup, item.dVenc.GetValueOrDefault(), item.vDup)));

            nfe.infNFe.det.ForEach(item => produtos.Add(new Produto(item)));

            nfe.infNFe.pag?.ForEach(item => item.detPag?.ForEach(pagamento => pagamentos.Add(new Pagamento(pagamento.tPag, pagamento.vPag))));

            Serie = nfe.infNFe.ide.serie;
            Numero = nfe.infNFe.ide.nNF;
            Emitente = new Emitente(nfe.infNFe.emit);
            NaturezaOperacaoDescricao = nfe.infNFe.ide.natOp;
            DataEmissao = nfe.infNFe.ide.dhEmi.DateTime;
            DataSaida = nfe.infNFe.ide.dhSaiEnt?.DateTime ?? nfe.infNFe.ide.dhEmi.DateTime;
            ETipoNFe = nfe.infNFe.ide.tpNF;
            Destinatario = new Destinatario(nfe.infNFe.dest);
            EPresencaComprador = nfe.infNFe.ide.indPres.GetValueOrDefault();
            EFinalidadeNFe = nfe.infNFe.ide.finNFe;
            EDestinoOperacao = nfe.infNFe.ide.idDest.GetValueOrDefault();
            IsImportacao = Destinatario?.Pessoa?.Endereco?.MunicipioEstadoSigla == Estado.EX && ETipoNFe == TipoNFe.tnEntrada;
            IsExportacao = Destinatario?.Pessoa?.Endereco?.MunicipioEstadoSigla == Estado.EX && ETipoNFe == TipoNFe.tnSaida;
            DadosTransporte = transportadora;
            NumeroPedidoCompraB2B = nfe.infNFe.compra?.xPed;
            Total = new Totalizador(nfe.infNFe.total.ICMSTot);
            Duplicatas = duplicatas;
            DadosAdicionaisFisco = nfe.infNFe.infAdic?.infAdFisco;
            DadosAdicionaisContribuinte = nfe.infNFe.infAdic?.infCpl;
            Produtos = produtos;
            Protocolo = new Protocolo(notafiscalProcessada.protNFe.infProt);
            FormasPagamento = pagamentos;
            CNf = Protocolo?.ObterCNf() ?? new Random().Next(1, 99999999).ToString("00000000");
        }

        public string CNf { get; private set; }

        //public string ChaveAcesso { get; private set; }

        public int Serie { get; private set; }

        public long Numero { get; private set; }

        public Emitente Emitente { get; private set; }

        public string NaturezaOperacaoDescricao { get; private set; }

        public DateTime DataEmissao { get; private set; }

        public DateTime? DataSaida { get; private set; }

        public TipoNFe ETipoNFe { get; private set; }

        public Destinatario Destinatario { get; private set; }

        public PresencaComprador EPresencaComprador { get; private set; }

        public FinalidadeNFe EFinalidadeNFe { get; private set; }

        public DestinoOperacao EDestinoOperacao { get; private set; }

        public bool IsImportacao { get; private set; }

        public bool IsExportacao { get; private set; }

        public Transporte DadosTransporte { get; private set; }

        public string NumeroPedidoCompraB2B { get; private set; }

        public Totalizador Total { get; private set; }

        public IReadOnlyList<Duplicata> Duplicatas { get; private set; }

        public string DadosAdicionaisFisco { get; private set; }

        public string DadosAdicionaisContribuinte { get; private set; }

        public IReadOnlyList<Pagamento> FormasPagamento { get; private set; }

        public IReadOnlyList<DocumentoReferenciado> DocumentosReferenciados { get; private set; }

        public IReadOnlyList<Produto> Produtos { get; private set; }

        public bool IsZonaFrancaManaus { get; private set; }

        public Protocolo Protocolo { get; private set; }

        private DestinoOperacao _obterDestinoOperacao(Estado estadoEmitente, Estado? estadoDestinatario, bool isOperacaoPresencial)
        {
            if(estadoDestinatario == null
                || estadoEmitente == estadoDestinatario
                || isOperacaoPresencial)
            {
                return DestinoOperacao.doInterna;
            }
            else if (estadoDestinatario == Estado.EX || estadoEmitente == Estado.EX)
            {
                return DestinoOperacao.doExterior;
            }
            else
            {
                return DestinoOperacao.doInterestadual;
            }
        }

        public void AtualizarChaveAcesso(string chaveAcesso)
        {
            if(Protocolo == null
                || string.IsNullOrEmpty(Protocolo?.ChaveAcesso))
            {
                Protocolo = new Protocolo(null, null, null, null);
            }

            Protocolo.AtualizarChaveAcesso(chaveAcesso);
        }

        public void AtualizarProtocolo(Classes.Protocolo.infProt protocolo)
        {
            if(protocolo != null)
            {
                Protocolo = new Protocolo(protocolo);
            }
        }
    }
}
