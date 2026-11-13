using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.Entidades.Produtos;
using NFe.BLL.Configuracao.Entidades.Totalizadores;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Identificacao;
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
            EnderecoEntrega enderecoEntrega,
            EnderecoRetirada enderecoRetirada,
            Totalizador total,
            IReadOnlyList<Produto> produtos,
            IReadOnlyList<Duplicata> duplicatas,
            string dadosAdicionaisFisco,
            string dadosAdicionaisContribuinte,
            PresencaComprador ePresencaComprador,
            IndicadorIntermediador? eIndicadorIntermediador,
            IReadOnlyList<Pagamento> formasPagamento = null,
            IReadOnlyList<DocumentoReferenciado> documentosReferenciados = null,
            string numeroPedidoCompraB2B = null,
            bool isZonaFrancaManaus = false,
            Protocolo protocolo = null,
            Intermediador intermediador = null,
            bool deveValidarValorTotalDaNfeEDuplicatas = true,
            bool deveValidarDataEmissaoAtrasada = true,
            CompraGovernamental compraGovernamental = null
            )
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
            EnderecoEntrega = enderecoEntrega;
            EnderecoRetirada = enderecoRetirada;
            EPresencaComprador = ePresencaComprador;
            EIndicadorIntermediador = eIndicadorIntermediador;
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
            DadosIntermediador = intermediador;
            CNf = Protocolo?.ObterCNf() ?? GerarCNf();
            
            DeveValidarValorTotalDaNfeEDuplicatas = deveValidarValorTotalDaNfeEDuplicatas;
            DeveValidarDataEmissaoAtrasada = deveValidarDataEmissaoAtrasada;
            CompraGovernamental = compraGovernamental;
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

            nfe.infNFe.cobr?.dup?.ForEach(item => duplicatas.Add(new Duplicata(item.nDup, item.dVenc.GetValueOrDefault(), item.vDup)));

            // A chave e o nItem de origem seguem com o produto para permitir o
            // referenciamento por item na devolucao (grupo VC01 DFeReferenciado).
            // So este construtor (leitura de XML de arquivo) preenche os dois: o
            // item nao guarda a chave da nota em que ele esta, e depois do
            // SelectMany dos itens de varias notas de origem nao ha mais como
            // saber de qual XML cada item veio.
            // A chave sai do protocolo de proposito (e nao de infNFe/@Id):
            // devolucao referencia nota autorizada, e protNFe e a garantia de que
            // ela foi. XML sem protocolo deixa o item sem origem, que e o
            // comportamento correto.
            var chaveAcessoOrigem = notafiscalProcessada.protNFe?.infProt?.chNFe;

            nfe.infNFe.det.ForEach(item => produtos.Add(new Produto(item, chaveAcessoOrigem)));

            nfe.infNFe.pag?.ForEach(item => item.detPag?.ForEach(pagamento => pagamentos.Add(new Pagamento(pagamento.tPag, pagamento.vPag))));

            bool isConsumidorFinal = false;
            if(nfe.infNFe.ide.indFinal.HasValue
                && nfe.infNFe.ide.indFinal.Value == ConsumidorFinal.cfConsumidorFinal)
            {
                isConsumidorFinal = true;
            }

            Destinatario destinatario = null;
            EnderecoEntrega enderecoEntrega = null;
            EnderecoRetirada enderecoRetirada = null;
            Intermediador intermediador = null;

            if (nfe.infNFe.dest != null)
            {
                destinatario = new Destinatario(nfe.infNFe.dest, isConsumidorFinal);
            }

            if (nfe.infNFe.entrega != null)
            {
                enderecoEntrega = new EnderecoEntrega(nfe.infNFe.entrega);
            }

            if (nfe.infNFe.retirada != null)
            {
                enderecoRetirada = new EnderecoRetirada(nfe.infNFe.retirada);
            }

            if (nfe.infNFe.infIntermed != null)
            {
                intermediador = new Intermediador(nfe.infNFe.infIntermed);
            }

            EModelo = nfe.infNFe.ide.mod;
            Serie = nfe.infNFe.ide.serie;
            Numero = nfe.infNFe.ide.nNF;
            Emitente = new Emitente(nfe.infNFe.emit);
            NaturezaOperacaoDescricao = nfe.infNFe.ide.natOp;
            DataEmissao = nfe.infNFe.ide.dhEmi.DateTime;
            DataSaida = nfe.infNFe.ide.dhSaiEnt?.DateTime ?? nfe.infNFe.ide.dhEmi.DateTime;
            ETipoNFe = nfe.infNFe.ide.tpNF;
            Destinatario = destinatario;
            EnderecoEntrega = enderecoEntrega;
            EnderecoRetirada = enderecoRetirada;
            EPresencaComprador = nfe.infNFe.ide.indPres.GetValueOrDefault();
            EIndicadorIntermediador = nfe.infNFe.ide.indIntermed;
            EFinalidadeNFe = nfe.infNFe.ide.finNFe;
            EDestinoOperacao = nfe.infNFe.ide.idDest.GetValueOrDefault();
            IsImportacao = Destinatario?.Pessoa?.Endereco?.MunicipioEstadoSigla == Estado.EX && ETipoNFe == TipoNFe.tnEntrada;
            IsExportacao = Destinatario?.Pessoa?.Endereco?.MunicipioEstadoSigla == Estado.EX && ETipoNFe == TipoNFe.tnSaida;
            DadosTransporte = transportadora;
            NumeroPedidoCompraB2B = nfe.infNFe.compra?.xPed;
            Total = new Totalizador(nfe.infNFe.total);
            Duplicatas = duplicatas;
            DadosAdicionaisFisco = nfe.infNFe.infAdic?.infAdFisco;
            DadosAdicionaisContribuinte = nfe.infNFe.infAdic?.infCpl;
            Produtos = produtos;
            // XML sem protocolo é caso válido, como diz o comentário de chaveAcessoOrigem
            // acima: a nota fica sem protocolo em vez de estourar.
            Protocolo = notafiscalProcessada.protNFe?.infProt == null
                ? null
                : new Protocolo(notafiscalProcessada.protNFe.infProt);
            FormasPagamento = pagamentos;
            DadosIntermediador = intermediador;

            if (nfe.infNFe.ide?.gCompraGov != null)
            {
                CompraGovernamental = new CompraGovernamental(nfe.infNFe.ide.gCompraGov);
            }

            DocumentosReferenciados = ObterDocumentosReferenciados(nfe.infNFe.ide?.NFref);
            CNf = Protocolo?.ObterCNf() ?? GerarCNf();
            DeveValidarValorTotalDaNfeEDuplicatas = true;
            DeveValidarDataEmissaoAtrasada = true;
        }

        public string CNf { get; private set; }

        //public string ChaveAcesso { get; private set; }
        public ModeloDocumento EModelo { get; set; }

        public int Serie { get; private set; }

        public long Numero { get; private set; }

        public Emitente Emitente { get; private set; }

        public string NaturezaOperacaoDescricao { get; private set; }

        public DateTime DataEmissao { get; private set; }

        public DateTime? DataSaida { get; private set; }

        public TipoNFe ETipoNFe { get; private set; }

        public Destinatario Destinatario { get; private set; }

        /// <summary>gCompraGov — Grupo de compras governamentais (BB01), no nível do documento.</summary>
        public CompraGovernamental CompraGovernamental { get; private set; }
        
        public EnderecoEntrega EnderecoEntrega { get; private set; }
        
        public EnderecoRetirada EnderecoRetirada { get; private set; }

        public PresencaComprador EPresencaComprador { get; private set; }

        public IndicadorIntermediador? EIndicadorIntermediador { get; private set; }

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

        public Intermediador DadosIntermediador { get; private set; }

        public bool DeveValidarValorTotalDaNfeEDuplicatas { get; private set; }

        public bool DeveValidarDataEmissaoAtrasada { get; private set; }

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

        private static readonly Random _geradorCNf = new Random();

        /// <summary>
        /// Gera o cNF quando a nota ainda não tem chave de acesso. O gerador é único e com
        /// lock: <see cref="Random"/> não é thread-safe e, sem semente explícita, instâncias
        /// criadas no mesmo tick repetem a sequência — duas notas montadas juntas sairiam
        /// com o mesmo cNF.
        /// </summary>
        private static string GerarCNf()
        {
            lock (_geradorCNf)
            {
                return _geradorCNf.Next(1, 99999999).ToString("00000000");
            }
        }

        public static ISet<PresencaComprador> IntermediadorObrigadorio = new HashSet<PresencaComprador>()
        {
             PresencaComprador.pcInternet, PresencaComprador.pcTeleatendimento, PresencaComprador.pcEntregaDomicilio, PresencaComprador.pcOutros
        };

        /// <summary>
        /// Le ide/NFref de volta para as entidades. Antes o construtor de leitura nao
        /// preenchia DocumentosReferenciados: a referencia entrava por um caminho e nao
        /// voltava pelo outro, logo quem lia um XML autorizado perdia o grupo BA.
        /// <para><b>Ler tolera, emitir recusa.</b> A referencia que nao monta e PULADA, e nao
        /// estoura: este construtor tambem le XML de TERCEIRO - NFeEntradaOperacoes usa ele
        /// na importacao de NF-e de entrada, na devolucao, na transferencia de estoque e na
        /// complementar por XML - e derrubar a importacao por causa de uma referencia que
        /// ninguem consome seria trocar um dado inutil por uma tela que nao abre. Quem
        /// recusa chave invalida e o construtor da entidade, no caminho da emissao, que e
        /// onde o XML e NOSSO.</para>
        /// </summary>
        private static List<DocumentoReferenciado> ObterDocumentosReferenciados(List<Classes.Informacoes.Identificacao.NFref> nFref)
        {
            var documentosReferenciados = new List<DocumentoReferenciado>();

            if (nFref == null)
            {
                return documentosReferenciados;
            }

            foreach (var referencia in nFref)
            {
                if (!string.IsNullOrWhiteSpace(referencia.refNFe))
                {
                    AdicionarReferenciaPorChave(documentosReferenciados, referencia.refNFe,
                        Enums.EDocumentoReferenciado.NFe);
                }
                else if (!string.IsNullOrWhiteSpace(referencia.refNFeSig))
                {
                    AdicionarReferenciaPorChave(documentosReferenciados, referencia.refNFeSig,
                        Enums.EDocumentoReferenciado.NFeSigilosa);
                }
                else if (!string.IsNullOrWhiteSpace(referencia.refCTe))
                {
                    AdicionarReferenciaPorChave(documentosReferenciados, referencia.refCTe,
                        Enums.EDocumentoReferenciado.CTe);
                }
                else if (referencia.refNF != null)
                {
                    var dataEmissao = ObterDataDeEmissaoReferenciada(referencia.refNF.AAMM);

                    if (dataEmissao.HasValue)
                    {
                        documentosReferenciados.Add(new DocumentoReferenciadoNF(dataEmissao: dataEmissao.Value,
                            emitenteCNPJ: referencia.refNF.CNPJ,
                            emitenteUF: referencia.refNF.cUF,
                            serie: referencia.refNF.serie,
                            numeroNotaFiscal: referencia.refNF.nNF));
                    }
                }
                else if (referencia.refECF != null)
                {
                    documentosReferenciados.Add(new DocumentoReferenciadoECF(
                        numeroECF: referencia.refECF.nECF,
                        numeroCOO: referencia.refECF.nCOO));
                }
                else if (referencia.refNFP != null)
                {
                    var dataEmissao = ObterDataDeEmissaoReferenciada(referencia.refNFP.AAMM);

                    if (dataEmissao.HasValue)
                    {
                        AdicionarReferenciaDeProdutorRural(documentosReferenciados, referencia.refNFP, dataEmissao.Value);
                    }
                }
            }

            return documentosReferenciados;
        }

        /// <summary>
        /// refNF e refNFP guardam so AAMM (BA04 e BA12); a entidade pede DateTime, entao o
        /// dia vira o 1o. AAMM fora do padrao devolve null e a referencia e DESCARTADA:
        /// fabricar 01/01/0001 - que volta como "0101" na reescrita - trocaria um dado
        /// ausente por um dado errado, que e pior.
        /// </summary>
        private static DateTime? ObterDataDeEmissaoReferenciada(string anoMes)
        {
            if (DateTime.TryParseExact(anoMes, "yyMM",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime dataEmissao))
            {
                return dataEmissao;
            }

            return null;
        }

        /// <summary>
        /// refNFP exige CNPJ ou CPF, e so um dos dois. XML de terceiro com os dois campos
        /// vazios, ou com os dois preenchidos, e descartado na LEITURA pelo mesmo motivo da
        /// chave invalida: ler tolera, emitir recusa.
        /// </summary>
        private static void AdicionarReferenciaDeProdutorRural(List<DocumentoReferenciado> documentosReferenciados,
            refNFP referencia,
            DateTime dataEmissao)
        {
            try
            {
                documentosReferenciados.Add(new DocumentoReferenciadoNFP(dataEmissao: dataEmissao,
                    emitenteUF: referencia.cUF,
                    emitenteInscricaoEstadual: referencia.IE,
                    modelo: referencia.mod,
                    serie: referencia.serie,
                    numeroNotaFiscal: referencia.nNF,
                    emitenteCNPJ: referencia.CNPJ,
                    emitenteCPF: referencia.CPF));
            }
            catch (ArgumentException)
            {
            }
        }

        /// <summary>
        /// Chave que a entidade recusa - fora do padrao TChNFe ou com digito verificador
        /// errado - e descartada na LEITURA. Ver a observacao em
        /// <see cref="ObterDocumentosReferenciados"/>.
        /// </summary>
        private static void AdicionarReferenciaPorChave(List<DocumentoReferenciado> documentosReferenciados,
            string chaveAcesso,
            Enums.EDocumentoReferenciado tipoDocumento)
        {
            try
            {
                documentosReferenciados.Add(new DocumentoReferenciadoNFe(chaveAcesso, tipoDocumento));
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
