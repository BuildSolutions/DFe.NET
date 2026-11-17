using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFe.Classes.Extensoes;
using DFe.Classes.Flags;
using DFe.Utils;
using DFe.Utils.Extensoes;
using FluentValidation;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Configuracao.Entidades.Produtos;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.CreditoPresumido;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.CreditoPresumido;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS.GrupoTributacao.Monofasico;
using NFe.BLL.Configuracao.Entidades.Totalizadores;
using NFe.BLL.Enums;
using NFe.BLL.Validators;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Cobranca;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using NFe.Utils;
using NFe.Utils.InformacoesSuplementares;
using NFe.Utils.NFe;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using Shared.NFe.Classes.Informacoes.Intermediador;
using COFINS = NFe.BLL.Configuracao.Entidades.Produtos.Impostos.COFINS;
using IPI = NFe.BLL.Configuracao.Entidades.Produtos.Impostos.IPI;

namespace NFe.BLL
{
    public class DadosNFe
    {
        public NotaFiscal NotaFiscal { get; private set; }
        public Classes.NFe NFeXML { get; private set; }

        protected readonly ConfiguracaoApp _cfgApp;

        public DadosNFe(NotaFiscal notaFiscal, ModeloDocumento modeloDocumento, ConfiguracaoCsc configuracaoCsc = null)
        {
            if (modeloDocumento == ModeloDocumento.NFe)
            {
                _cfgApp = new ConfiguracaoNFe(notaFiscal.Emitente);
            }
            else if (modeloDocumento == ModeloDocumento.NFCe)
            {
                _cfgApp = new ConfiguracaoNFCe(notaFiscal.Emitente, configuracaoCsc);
            }

            NotaFiscal = notaFiscal;
            GetNFe();
        }

        public DadosNFe(NotaFiscal notaFiscal, ConfiguracaoApp cfgApp)
        {
            _cfgApp = cfgApp;
            NotaFiscal = notaFiscal;
            GetNFe();
        }

        public DadosNFe(string notaFiscalXMLPath, ConfiguracaoApp cfgApp)
        {
            _cfgApp = cfgApp;
            var nFeProcessada = new nfeProc().CarregarDeArquivoXml(notaFiscalXMLPath);

            if (nFeProcessada != null)
            {
                NFeXML = nFeProcessada.NFe;
                NotaFiscal = new NotaFiscal(nFeProcessada);
            }
        }

        public Classes.NFe AssinaNFe()
        {
            // A configuracao vai explicita: sem ela Assina cai em
            // ConfiguracaoServico.Instancia e assinaria com o certificado do ultimo
            // emitente que montou configuracao no processo.
            var assinado = NFeXML.Assina(_cfgApp.CfgServico);
            NotaFiscal.AtualizarChaveAcesso(NFeXML?.infNFe?.Id?.Substring(3));
            return assinado;
        }

        public Classes.NFe ValidaNFeXml(out string erro)
        {
            erro = string.Empty;
            try
            {
                return NFeXML.Valida(_cfgApp.CfgServico);
            }
            catch (Exception ex)
            {
                erro = ex.ToString();
            }

            return null;
        }

        //private string ObterNomeArquivo(string nomeArquivo, DateTime dataGerado, string pasta)
        //{
        //    var caminhoXmlGerados = Path.Combine(NotaFiscal.Emitente.DiretorioSalvarXML, pasta);
        //    var ano = dataGerado.ToString("yyyy");
        //    var mes = dataGerado.ToString("MM_yyyy");

        //    var arquivoSalvarGerados = Path.Combine(caminhoXmlGerados,
        //        ano,
        //        mes,
        //        string.Concat(nomeArquivo, ".xml"));

        //    if (!Directory.Exists(Path.GetDirectoryName(arquivoSalvarGerados)))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(Path.GetDirectoryName(arquivoSalvarGerados));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ArgumentException("Erro ao salvar aquivo xml no disco", ex);
        //        }
        //    }

        //    return arquivoSalvarGerados;
        //}

        public bool ValidarDadosNFe(out string errors)
        {
            errors = string.Empty;
            string erroValidacao = string.Empty;
            var validator = new NotaFiscalValidator(_cfgApp.CfgServico.ModeloDocumento);
            var resultado = validator.Validate(NotaFiscal);

            if (!resultado.IsValid)
            {
                errors = string.Join("\r\n", resultado.Errors.Select(err => err.ErrorMessage));
                return false;
            }

            return true;
        }

        public bool ValidarDadosSuframa(Suframa suframa, out string errors)
        {
            errors = string.Empty;
            string erroValidacao = string.Empty;
            var validator = new SuframaValidator();
            var resultado = validator.Validate(suframa);

            if (!resultado.IsValid)
            {
                errors = string.Join("\r\n", resultado.Errors.Select(err => err.ErrorMessage));
                return false;
            }

            return true;
        }

        /// <summary>
        /// gCompraGov (ide) e gRed (item) sao dois niveis do XML que tem de concordar, e
        /// quem monta um nao enxerga o outro: o grupo do documento sai de GetIdentificacao
        /// e os impostos do item saem de GetProdutoCBSIBS. A conferencia fica aqui porque e
        /// o unico ponto onde ide e det ja existem.
        /// <para>Com gCompraGov informado, as UB26-20, UB45-20 e UB64-20 exigem o gRed nos
        /// tres grupos do item - rejeicoes 1033, 1074 e 1079 - mesmo com pRedAliq zero. Sem
        /// gCompraGov, a UB82a-30 proibe o gTribCompraGov no item (rejeicao 1144).</para>
        /// <para>Sem esta conferencia o erro so aparecia na resposta da SEFAZ, sem dizer
        /// qual item, e quem usa a biblioteca tinha de sincronizar os dois niveis na mao.</para>
        /// </summary>
        public static void ConferirCoerenciaDaCompraGovernamental(infNFe infNFe)
        {
            var temCompraGovernamental = infNFe.ide?.gCompraGov != null;

            foreach (var item in infNFe.det ?? new List<det>())
            {
                var grupo = item.imposto?.IBSCBS?.gIBSCBS;

                if (grupo == null)
                {
                    continue;
                }

                if (!temCompraGovernamental)
                {
                    if (grupo.gTribCompraGov != null)
                    {
                        throw new ArgumentException($"Item {item.nItem} informa o grupo de tributação em compra governamental (gTribCompraGov) e o documento não tem o grupo de compra governamental (ide/gCompraGov).");
                    }

                    continue;
                }

                if (grupo.gIBSUF?.gRed == null
                    || grupo.gIBSMun?.gRed == null
                    || grupo.gCBS?.gRed == null)
                {
                    throw new ArgumentException($"Item {item.nItem}: em compra governamental o grupo de redução de alíquota (gRed) é obrigatório no IBS da UF, no IBS do Município e na CBS, ainda que com pRedAliq zero.");
                }
            }
        }

        private infNFe GetNFe()
        {
            var infNFe = new infNFe
            {
                versao = _cfgApp.CfgServico.VersaoLayout.VersaoServicoParaString(),
                ide = GetIdentificacao(),
                emit = GetEmitente(),
                dest = GetDestinatario(),
                transp = GetTransporte(),
                compra = GetCompra()
            };

            infNFe.entrega = GetEnderecoEntrega();
            infNFe.retirada = GetEnderecoRetirada();
            infNFe.det = GetProdutos();

            ConferirCoerenciaDaCompraGovernamental(infNFe);
            infNFe.total = GetTotal();
            infNFe.cobr = GetCobranca();
            infNFe.pag = GetPagamento();
            infNFe.infAdic = GetInfAdic();
            infNFe.exporta = GetExporta();
            infNFe.autXML = GetAutorizadosXML();
            infNFe.infIntermed = GetInformacaoIntermediador();

            if (_cfgApp.Emitente.Pessoa.Endereco.MunicipioEstadoSigla == DFe.Classes.Entidades.Estado.SC
                || _cfgApp.Emitente.Pessoa.Endereco.MunicipioEstadoSigla == DFe.Classes.Entidades.Estado.PR)
            {
                infNFe.infRespTec = GetRespTec();
            }

            NFeXML = new Classes.NFe { infNFe = infNFe };
            NFeXML.infNFeSupl = GetNFCeQrCode();

            return infNFe;
        }

        private List<autXML> GetAutorizadosXML()
        {
            if (NotaFiscal.Emitente.AutorizadosXmlDocumentos == null
                || NotaFiscal.Emitente.AutorizadosXmlDocumentos.Count == 0)
            {
                return null;
            }

            var autorizadosXml = new List<autXML>();
            foreach (var documento in NotaFiscal.Emitente.AutorizadosXmlDocumentos)
            {
                if (documento.CpfCnpj.Length == 14)
                {
                    autorizadosXml.Add(new autXML()
                    {
                        CNPJ = documento.CpfCnpj
                    });
                }
                else
                {
                    autorizadosXml.Add(new autXML()
                    {
                        CPF = documento.CpfCnpj
                    });
                }
            }

            return autorizadosXml;
        }

        private infIntermed GetInformacaoIntermediador()
        {
            if (!NotaFiscal.EIndicadorIntermediador.EValido()
                || NotaFiscal.EIndicadorIntermediador == IndicadorIntermediador.iiSemIntermediador
                || NotaFiscal.DadosIntermediador == null)
            {
                return null;
            }

            return new infIntermed()
            {
                CNPJ = NotaFiscal.DadosIntermediador.CNPJ,
                idCadIntTran = NotaFiscal.DadosIntermediador.NomeUsuarioPlataforma
            };
        }

        private ide GetIdentificacao()
        {
            var nNF = NotaFiscal.Numero;
            var cUF = _cfgApp.Emitente.Pessoa.Endereco.MunicipioEstadoSigla.GetValueOrDefault();
            var cNF = NotaFiscal.CNf;
            var natOp = NotaFiscal.NaturezaOperacaoDescricao;
            var serie = NotaFiscal.Serie;
            DateTime dhEmi = NotaFiscal.DataEmissao;
            DateTime? dhSaiEnt = NotaFiscal.DataSaida;
            var tpNf = NotaFiscal.ETipoNFe;
            DestinoOperacao idDest = NotaFiscal.EDestinoOperacao;
            var cMunFG = _cfgApp.Emitente.Pessoa.Endereco.MunicipioCodigoIBGE;
            var tpImp = _cfgApp.CfgServico.ModeloDocumento == ModeloDocumento.NFCe ? TipoImpressao.tiNFCe : TipoImpressao.tiRetrato;
            var tpEmis = _cfgApp.CfgServico.tpEmis;
            var tpAmb = _cfgApp.CfgServico.tpAmb;
            FinalidadeNFe finNFe = NotaFiscal.EFinalidadeNFe;
            ConsumidorFinal indFinal = NotaFiscal.Destinatario?.EConsumidorFinal ?? ConsumidorFinal.cfConsumidorFinal;
            PresencaComprador indPres = NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnAjuste || NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnComplementar ? PresencaComprador.pcNao : NotaFiscal.EPresencaComprador;
            IndicadorIntermediador? indIntermed = NotaFiscal.EIndicadorIntermediador;
            var verProc = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); //Versão da DLL: PWNFe.BLL
            ModeloDocumento modeloDocumento = _cfgApp.CfgServico.ModeloDocumento;

            var ide = new ide
            {
                cUF = cUF,
                cNF = cNF,
                natOp = natOp,
                mod = modeloDocumento,
                serie = serie,
                nNF = nNF,
                dhEmi = dhEmi,
                dhSaiEnt = dhSaiEnt,
                tpNF = tpNf,
                idDest = idDest,
                cMunFG = cMunFG,
                tpImp = tpImp,
                tpEmis = tpEmis,
                tpAmb = tpAmb,
                finNFe = finNFe,
                indFinal = indFinal,
                indPres = indPres,
                procEmi = ProcessoEmissao.peAplicativoContribuinte,
                verProc = verProc,
                NFref = GetNFRef(NotaFiscal.DocumentosReferenciados)
            };

            // B25.1/B25.2. Sem o tipo, a nota de credito e a de debito levam rejeicao 1202 -
            // "cClassTrib incompativel com a finalidade da nota ou com o tipo de Nota de
            // Debito ou Credito". O lado vem do finNFe, e os dois enums numeram pelo leiaute,
            // igual a FinalidadeNFe: por isso e cast, nao switch.
            if (finNFe == FinalidadeNFe.fnNotaCredito)
            {
                ide.tpNFCredito = (TipoNFeCredito?)NotaFiscal.TipoNotaCreditoDebito;
            }
            else if (finNFe == FinalidadeNFe.fnNotaDebito)
            {
                ide.tpNFDebito = (TipoNFeDebito?)NotaFiscal.TipoNotaCreditoDebito;
            }

            if (indIntermed.EValido())
            {
                ide.indIntermed = indIntermed;
            }

            ide.gCompraGov = ObterCompraGovernamental(NotaFiscal.CompraGovernamental);

            if (ide.tpEmis != TipoEmissao.teNormal)
            {
                ide.dhCont = DateTime.Now; ;
                ide.xJust = "TESTE DE CONTIGÊNCIA PARA NFe/NFCe";
            }

            return ide;
        }

        //private IndicadorIntermediador? GetIndicadorIntermediador()
        //{
        //    IndicadorIntermediador? indicadorIntermediador = null;
        //    if (NotaFiscal.EIndicadorIntermediador.EValido())
        //    {
        //        indicadorIntermediador = NotaFiscal.EIndicadorIntermediador;
        //    }
        //    else if (NotaFiscal.IntermediadorObrigadorio.Contains(NotaFiscal.EPresencaComprador))
        //    {
        //        indicadorIntermediador = IndicadorIntermediador.iiSemIntermediador;
        //    }

        //    return indicadorIntermediador;
        //}

        private emit GetEmitente()
        {
            var emit = new emit
            {
                CPF = "",
                CNPJ = _cfgApp.Emitente.Pessoa.CPFCNPJ,
                xNome = _cfgApp.Emitente.Pessoa.NomeRazaoSocial,
                xFant = _cfgApp.Emitente.Pessoa.ApelidoFantasia,
                IE = _cfgApp.Emitente.Pessoa.RGInscricaoEstadual,
                IEST = _cfgApp.Emitente.InscricaoEstadualSubstituicaoTributaria,
                CRT = _cfgApp.Emitente.ECRT,
                enderEmit = GetEnderecoEmitente()
            };

            return emit;
        }

        private enderEmit GetEnderecoEmitente()
        {
            return new enderEmit
            {
                xLgr = _cfgApp.Emitente.Pessoa.Endereco.Logradouro,
                nro = _cfgApp.Emitente.Pessoa.Endereco.Numero,
                xCpl = _cfgApp.Emitente.Pessoa.Endereco.Complemento,
                xBairro = _cfgApp.Emitente.Pessoa.Endereco.Bairro,
                cMun = _cfgApp.Emitente.Pessoa.Endereco.MunicipioCodigoIBGE,
                xMun = _cfgApp.Emitente.Pessoa.Endereco.MunicipioNome,
                UF = _cfgApp.Emitente.Pessoa.Endereco.MunicipioEstadoSigla.GetValueOrDefault(),
                CEP = _cfgApp.Emitente.Pessoa.Endereco.CEP,
                cPais = _cfgApp.Emitente.Pessoa.Endereco.PaisCodigo,
                xPais = _cfgApp.Emitente.Pessoa.Endereco.PaisNome,
                fone = _cfgApp.Emitente.Pessoa.Telefone
            };
        }

        private dest GetDestinatario()
        {
            if (NotaFiscal.Destinatario == null)
            {
                return null;
            }

            var dest = new dest(_cfgApp.CfgServico.VersaoLayout);

            indIEDest indIEDest = indIEDest.NaoContribuinte;
            string idEstrangeiro = null;
            string ie = InformarDestIE();
            var cpfCnpj = NotaFiscal.Destinatario.Pessoa.CPFCNPJ;
            var cnpj = string.Empty;
            var cpf = string.Empty;
            var blnClienteMEIIncidenciaICMS = NotaFiscal.Destinatario.IsMEIIsentoIncideICMS;
            var blnProdutorRural = NotaFiscal.Destinatario.IsProdutorRural;
            var isIsentoIE = NotaFiscal.Destinatario.Pessoa.InscricaoEstadualIsento;
            var xNome = NotaFiscal.Destinatario.Pessoa.NomeRazaoSocial;

            var email = string.IsNullOrEmpty(NotaFiscal.Destinatario.Pessoa.Email) ? null : NotaFiscal.Destinatario.Pessoa.Email;
            if (email?.Contains(";") == true)
            {
                email = email.Substring(0, email.IndexOf(';'));
            }

            if (NotaFiscal.IsImportacao)
            {
                idEstrangeiro = ie ?? string.Empty;

                // Destinatario no exterior: o identificador vai em idEstrangeiro e indIEDest fica 9.
                // Deixando a mesma string em IE o grupo dest sairia com <IE>, que a SEFAZ rejeita
                // quando o indicador de IE do destinatario e 9.
                ie = null;
            }
            else if (NotaFiscal.IsExportacao)
            {
                idEstrangeiro = string.Empty;
            }
            else
            {
                if (NotaFiscal.Destinatario.Pessoa.PessoaTipo == ETipoPessoa.Juridica)
                {
                    cnpj = cpfCnpj;

                    if (!string.IsNullOrEmpty(ie)
                        && ie != "ISENTO")
                    {
                        indIEDest = indIEDest.ContribuinteICMS; // 1=Contribuinte ICMS (informar a IE do destinatário);
                    }
                    else if (blnClienteMEIIncidenciaICMS)
                    {
                        indIEDest = indIEDest.Isento; // 2
                        ie = null;
                    }
                    else
                    //_infNFe.Dest.indIEDest = 2; //comentado dia 02/12 pois nao validava nfe de consumidor final com 18%
                    {
                        indIEDest = indIEDest.NaoContribuinte; //9=Não Contribuinte, que pode ou não possuir Inscrição Estadual no Cadastro de Contribuintes do ICMS;
                        ie = null;
                    }
                }
                else if (NotaFiscal.Destinatario.Pessoa.PessoaTipo == ETipoPessoa.Fisica && blnProdutorRural)
                {
                    cpf = cpfCnpj;
                    indIEDest = indIEDest.ContribuinteICMS; //1=Contribuinte ICMS (informar a IE do destinatário);
                }
                else
                {
                    cpf = cpfCnpj;
                    ie = null;
                    indIEDest = indIEDest.NaoContribuinte; //9=Não Contribuinte, que pode ou não possuir Inscrição Estadual no Cadastro de Contribuintes do ICMS;
                }
            }

            if (_cfgApp.CfgServico.tpAmb == TipoAmbiente.Homologacao)
            {
                xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            return new dest(_cfgApp.CfgServico.VersaoLayout)
            {
                CNPJ = cnpj,
                CPF = cpf,
                idEstrangeiro = idEstrangeiro,
                xNome = xNome,
                enderDest = GetEnderecoDestinatarioCliente(),
                indIEDest = indIEDest,
                IE = ie,
                ISUF = GetISUF(),
                email = email,
            };
        }

        private string InformarDestIE()
        {
            bool isProdutorRural = NotaFiscal.Destinatario.IsProdutorRural;
            bool isClienteMEIIncidenciaICMS = NotaFiscal.Destinatario.IsMEIIsentoIncideICMS;

            if (NotaFiscal.Destinatario.EConsumidorFinal == ConsumidorFinal.cfConsumidorFinal
                && NotaFiscal.Destinatario.Pessoa.PessoaTipo == ETipoPessoa.Fisica
                && !isProdutorRural)
            {
                return null;
            }

            if ((!NotaFiscal.Destinatario.Pessoa.InscricaoEstadualIsento || isProdutorRural)
                && !NotaFiscal.IsExportacao
                && !isClienteMEIIncidenciaICMS)
            {
                return NotaFiscal.Destinatario.Pessoa.RGInscricaoEstadual.RetornaAlfanumericos();
            }

            return null;
        }

        private enderDest GetEnderecoDestinatarioCliente()
        {
            if (NotaFiscal.Destinatario == null
                || NotaFiscal.Destinatario.Pessoa == null
                || NotaFiscal.Destinatario.Pessoa.Endereco == null)
            {
                return null;
            }

            var xLgr = NotaFiscal.Destinatario.Pessoa.Endereco.Logradouro;
            var nro = NotaFiscal.Destinatario.Pessoa.Endereco.Numero;
            var xCpl = NotaFiscal.Destinatario.Pessoa.Endereco.Complemento;
            var xBairro = NotaFiscal.Destinatario.Pessoa.Endereco.Bairro;
            var cMun = Convert.ToInt64(NotaFiscal.Destinatario.Pessoa.Endereco.MunicipioCodigoIBGE);
            var xMun = NotaFiscal.Destinatario.Pessoa.Endereco.MunicipioNome;
            var uf = NotaFiscal.Destinatario.Pessoa.Endereco.MunicipioEstadoSigla;
            var cep = NotaFiscal.Destinatario.Pessoa.Endereco.CEP;
            var cPais = NotaFiscal.Destinatario.Pessoa.Endereco.PaisCodigo;
            var xPais = NotaFiscal.Destinatario.Pessoa.Endereco.PaisNome;
            var fone = NotaFiscal.Destinatario.Pessoa.Telefone;

            return new enderDest
            {
                xLgr = xLgr,
                nro = nro,
                xCpl = xCpl,
                xBairro = xBairro,
                cMun = cMun,
                xMun = xMun,
                UF = uf?.GetSiglaUfString(),
                CEP = cep,
                cPais = cPais,
                xPais = xPais,
                fone = fone
            };
        }
        private entrega GetEnderecoEntrega()
        {
            if (NotaFiscal.EnderecoEntrega == null
                || NotaFiscal.EnderecoEntrega.Endereco == null
                || string.IsNullOrEmpty(NotaFiscal.EnderecoEntrega.Endereco.Logradouro))
            {
                return null;
            }

            var xLgr = NotaFiscal.EnderecoEntrega.Endereco.Logradouro;
            var nro = NotaFiscal.EnderecoEntrega.Endereco.Numero;
            string xCpl = !string.IsNullOrEmpty(NotaFiscal.EnderecoEntrega.Endereco.Complemento) ? NotaFiscal.EnderecoEntrega.Endereco.Complemento : null;
            var xBairro = NotaFiscal.EnderecoEntrega.Endereco.Bairro;
            var cMun = Convert.ToInt64(NotaFiscal.EnderecoEntrega.Endereco.MunicipioCodigoIBGE);

            var xMun = NotaFiscal.EnderecoEntrega.Endereco.MunicipioNome;
            var uf = NotaFiscal.EnderecoEntrega.Endereco.MunicipioEstadoSigla;
            var cep = NotaFiscal.EnderecoEntrega.Endereco.CEP;
            var cPais = NotaFiscal.EnderecoEntrega.Endereco.PaisCodigo;
            var xPais = NotaFiscal.EnderecoEntrega.Endereco.PaisNome;
            string fone = !string.IsNullOrEmpty(NotaFiscal.EnderecoEntrega.Fone) ? NotaFiscal.EnderecoEntrega.Fone : null;
            // TODO: entrega/retirada aceitam email (NT 2020.006) e ele nao e emitido aqui.

            string nome = !string.IsNullOrEmpty(NotaFiscal.EnderecoEntrega.Nome) ? NotaFiscal.EnderecoEntrega.Nome : null;
            string cnpj = null;
            string cpf = null;
            string ie = null;
            if (NotaFiscal.EnderecoEntrega.CPFCNPJ?.Length == 14)
            {
                cnpj = NotaFiscal.EnderecoEntrega.CPFCNPJ;
                ie = NotaFiscal.EnderecoEntrega.RGIE;
            }
            else if (NotaFiscal.EnderecoEntrega.CPFCNPJ?.Length == 11)
            {
                cpf = NotaFiscal.EnderecoEntrega.CPFCNPJ;
            }

            // Extrai os digitos antes de converter: CEP guardado como "13000-000" passa no
            // EnderecoValidator (que compara RetornaNumeros) mas iria para o XML como 0.
            long.TryParse(cep.RetornaNumeros(), out long lCEP);

            return new entrega
            {
                xLgr = xLgr,
                nro = nro,
                xCpl = xCpl,
                xBairro = xBairro,
                cMun = cMun,
                xMun = xMun,
                UF = uf?.GetSiglaUfString(),
                CEP = lCEP,
                cPais = cPais,
                xPais = xPais,
                fone = fone,
                xNome = nome,
                CNPJ = cnpj,
                CPF = cpf,
                IE = ie
            };
        }
        private retirada GetEnderecoRetirada()
        {
            if (NotaFiscal.EnderecoRetirada == null
                || NotaFiscal.EnderecoRetirada.Endereco == null
                || string.IsNullOrEmpty(NotaFiscal.EnderecoRetirada.Endereco.Logradouro))
            {
                return null;
            }

            var xLgr = NotaFiscal.EnderecoRetirada.Endereco.Logradouro;
            var nro = NotaFiscal.EnderecoRetirada.Endereco.Numero;
            string xCpl = !string.IsNullOrEmpty(NotaFiscal.EnderecoRetirada.Endereco.Complemento) ? NotaFiscal.EnderecoRetirada.Endereco.Complemento : null;
            var xBairro = NotaFiscal.EnderecoRetirada.Endereco.Bairro;
            var cMun = Convert.ToInt64(NotaFiscal.EnderecoRetirada.Endereco.MunicipioCodigoIBGE);

            var xMun = NotaFiscal.EnderecoRetirada.Endereco.MunicipioNome;
            var uf = NotaFiscal.EnderecoRetirada.Endereco.MunicipioEstadoSigla;
            var cep = NotaFiscal.EnderecoRetirada.Endereco.CEP;
            var cPais = NotaFiscal.EnderecoRetirada.Endereco.PaisCodigo;
            var xPais = NotaFiscal.EnderecoRetirada.Endereco.PaisNome;
            string fone = !string.IsNullOrEmpty(NotaFiscal.EnderecoRetirada.Fone) ? NotaFiscal.EnderecoRetirada.Fone : null;
            // TODO: entrega/retirada aceitam email (NT 2020.006) e ele nao e emitido aqui.

            string nome = !string.IsNullOrEmpty(NotaFiscal.EnderecoRetirada.Nome) ? NotaFiscal.EnderecoRetirada.Nome : null;
            string cnpj = null;
            string cpf = null;
            string ie = null;
            if (NotaFiscal.EnderecoRetirada.CPFCNPJ?.Length == 14)
            {
                cnpj = NotaFiscal.EnderecoRetirada.CPFCNPJ;
                ie = NotaFiscal.EnderecoRetirada.RGIE;
            }
            else if (NotaFiscal.EnderecoRetirada.CPFCNPJ?.Length == 11)
            {
                cpf = NotaFiscal.EnderecoRetirada.CPFCNPJ;
            }

            long.TryParse(cep.RetornaNumeros(), out long lCEP);

            return new retirada
            {
                xLgr = xLgr,
                nro = nro,
                xCpl = xCpl,
                xBairro = xBairro,
                cMun = cMun,
                xMun = xMun,
                UF = uf?.GetSiglaUfString(),
                CEP = lCEP,
                cPais = cPais,
                xPais = xPais,
                fone = fone,
                xNome = nome,
                CNPJ = cnpj,
                CPF = cpf,
                IE = ie
            };
        }

        private string GetISUF()
        {
            if (NotaFiscal.Destinatario.Pessoa.PessoaTipo == ETipoPessoa.Fisica
                || !NotaFiscal.Destinatario.IsSuframa)
            {
                return null;
            }

            if (string.IsNullOrEmpty(NotaFiscal.Destinatario.SuframaNumero))
            {
                throw new Exception($"\r\nNúmero do SUFRAMA do Destinatário (Cliente: {NotaFiscal.Destinatario.Pessoa.NomeRazaoSocial} não cadastrado/Inválido!");
            }

            return NotaFiscal.Destinatario.SuframaNumero;
        }

        private transp GetTransporte()
        {
            if (NotaFiscal.DadosTransporte == null)
            {
                return null;
            }

            var t = new transp
            {
                modFrete = NotaFiscal.DadosTransporte.ModalidadeFrete,
                transporta = GetTransportadora(),
                vol = GetVolumes()
            };

            return t;
        }

        private transporta GetTransportadora()
        {
            if (NotaFiscal.DadosTransporte.Transportadora == null
                || string.IsNullOrEmpty(NotaFiscal.DadosTransporte.Transportadora.NomeRazaoSocial))
            {
                return null;
            }

            var cpf = NotaFiscal.DadosTransporte.Transportadora.PessoaTipo == ETipoPessoa.Juridica ? null : NotaFiscal.DadosTransporte.Transportadora.CPFCNPJ;
            var cnpj = NotaFiscal.DadosTransporte.Transportadora.PessoaTipo == ETipoPessoa.Juridica ? NotaFiscal.DadosTransporte.Transportadora.CPFCNPJ : null;
            var xNome = NotaFiscal.DadosTransporte.Transportadora.NomeRazaoSocial;
            var ie = string.IsNullOrEmpty(NotaFiscal.DadosTransporte.Transportadora.RGInscricaoEstadual) ? null : NotaFiscal.DadosTransporte.Transportadora.RGInscricaoEstadual;

            var xEnder = string.Join(", ", new string[] { NotaFiscal.DadosTransporte.Transportadora.Endereco.Logradouro, NotaFiscal.DadosTransporte.Transportadora.Endereco.Numero }.Where(c => !string.IsNullOrEmpty(c))).SanitizeString().SubstringMaxLength(60);
            //if (!string.IsNullOrEmpty(xEnder) && xEnder.Length > 60)
            //{
            //    xEnder = xEnder.Substring(0, 60);
            //}

            var xMun = string.IsNullOrEmpty(NotaFiscal.DadosTransporte.Transportadora.Endereco.MunicipioNome) ? null : NotaFiscal.DadosTransporte.Transportadora.Endereco.MunicipioNome;
            var uf = NotaFiscal.DadosTransporte.Transportadora.Endereco.MunicipioEstadoSigla?.GetSiglaUfString(); //TODO

            return new transporta
            {
                CPF = cpf,
                CNPJ = cnpj,
                xNome = xNome,
                IE = ie,
                xEnder = xEnder,
                xMun = xMun,
                UF = uf
            };
        }

        private List<vol> GetVolumes()
        {
            //Volumes
            if (NotaFiscal.DadosTransporte.QuantidadeVolumes == 0)
            {
                return null;
            }
            var qVol = NotaFiscal.DadosTransporte.QuantidadeVolumes;
            var esp = string.IsNullOrEmpty(NotaFiscal.DadosTransporte.EspecieVolumeDescricao) ? null : NotaFiscal.DadosTransporte.EspecieVolumeDescricao;
            decimal? pesoL = NotaFiscal.DadosTransporte.PesoLiquido == 0 ? null : NotaFiscal.DadosTransporte.PesoLiquido;
            decimal? pesoB = NotaFiscal.DadosTransporte.PesoBruto == 0 ? null : NotaFiscal.DadosTransporte.PesoBruto;

            return new List<vol>
            {
                new vol
                {
                    qVol = qVol,
                    esp = esp,
                    pesoL = pesoL,
                    pesoB = pesoB
                }
            };
        }

        private compra GetCompra()
        {
            if (string.IsNullOrEmpty(NotaFiscal.NumeroPedidoCompraB2B))
            {
                return null;
            }

            return new compra
            {
                xPed = NotaFiscal.NumeroPedidoCompraB2B
            };
        }

        /// <summary>
        /// Funcao pura: depende so da lista recebida, por isso e estatica e publica -
        /// permite testar o mapeamento de NFref sem montar a NFe inteira.
        /// </summary>
        public static List<NFref> GetNFRef(IReadOnlyList<DocumentoReferenciado> documentosReferenciados)
        {
            if (documentosReferenciados == null
                || documentosReferenciados.Count == 0)
            {
                return new List<NFref>();
            }

            var lstNFref = new List<NFref>();

            // Chave repetida no NFref e rejeicao. Acontece de verdade na NF-e de CFOP
            // 5929/6929: a lista chega de duas fontes - o que o operador cadastrou e o que
            // vem da venda (SAT/NFC-e) - e nada impede a mesma chave nas duas. A guarda
            // fica aqui porque este e o unico caminho para o grupo BA.
            var chavesJaIncluidas = new HashSet<string>();

            // NFref e um xs:choice de seis formas: cada documento referenciado preenche
            // exatamente uma. O switch deixa a exclusividade explicita - a versao anterior
            // montava as tres opcoes com ternarios lado a lado, o que sugeria o contrario.
            foreach (var item in documentosReferenciados)
            {
                if (item == null)
                {
                    continue;
                }

                var porChave = item as DocumentoReferenciadoNFe;

                if (porChave != null
                    && !chavesJaIncluidas.Add($"{(int)item.TipoDocumento}|{porChave.ChaveAcesso}"))
                {
                    continue;
                }

                var nFref = new NFref();

                switch (item.TipoDocumento)
                {
                    case EDocumentoReferenciado.NFe:
                        nFref.refNFe = ((DocumentoReferenciadoNFe)item).ChaveAcesso;
                        break;

                    case EDocumentoReferenciado.NFeSigilosa:
                        nFref.refNFeSig = ((DocumentoReferenciadoNFe)item).ChaveAcesso;
                        break;

                    case EDocumentoReferenciado.CTe:
                        nFref.refCTe = ((DocumentoReferenciadoNFe)item).ChaveAcesso;
                        break;

                    case EDocumentoReferenciado.NF:
                        nFref.refNF = GetNotaFiscalReferenciada((DocumentoReferenciadoNF)item);
                        break;

                    case EDocumentoReferenciado.ECF:
                        nFref.refECF = GetCupomFiscalReferenciado((DocumentoReferenciadoECF)item);
                        break;

                    case EDocumentoReferenciado.NFProdutorRural:
                        nFref.refNFP = GetNotaFiscalProdutorRuralReferenciada((DocumentoReferenciadoNFP)item);
                        break;

                    default:
                        throw new ArgumentException($"Documento referenciado de tipo não tratado: {item.TipoDocumento}.");
                }

                lstNFref.Add(nFref);
            }

            // ide/NFref e maxOccurs=999 (leiauteNFe_v4.00.xsd). A contagem e DEPOIS da
            // deduplicacao: chave repetida nao ocupa ocorrencia.
            if (lstNFref.Count > 999)
            {
                throw new ArgumentException($"A nota informa {lstNFref.Count} documentos referenciados (ide/NFref); o leiaute aceita no máximo 999.");
            }

            return lstNFref;
        }

        private static refNF GetNotaFiscalReferenciada(DocumentoReferenciadoNF notaFiscalReferenciada)
        {
            return new refNF
            {
                AAMM = notaFiscalReferenciada.DataEmissao,
                CNPJ = notaFiscalReferenciada.EmitenteCNPJ,
                cUF = notaFiscalReferenciada.EmitenteCodigoUF,
                mod = notaFiscalReferenciada.Modelo, // 1
                nNF = notaFiscalReferenciada.Numero,
                serie = notaFiscalReferenciada.Serie
            };
        }

        /// <summary>
        /// BA10 - refNFP. CNPJ e CPF sao um xs:choice: a entidade ja garante que so um dos
        /// dois esta preenchido, e o outro vai nulo para nao sair no XML.
        /// </summary>
        private static refNFP GetNotaFiscalProdutorRuralReferenciada(DocumentoReferenciadoNFP notaProdutorRural)
        {
            return new refNFP
            {
                AAMM = notaProdutorRural.DataEmissao,
                cUF = notaProdutorRural.EmitenteCodigoUF,
                CNPJ = notaProdutorRural.EmitenteCNPJ,
                CPF = notaProdutorRural.EmitenteCPF,
                IE = notaProdutorRural.EmitenteInscricaoEstadual,
                mod = notaProdutorRural.Modelo,
                serie = notaProdutorRural.Serie,
                nNF = notaProdutorRural.Numero
            };
        }

        private static refECF GetCupomFiscalReferenciado(DocumentoReferenciadoECF cupomFiscalReferenciada)
        {
            return new refECF
            {
                mod = cupomFiscalReferenciada.Modelo,
                nECF = cupomFiscalReferenciada.NumeroECF,
                nCOO = cupomFiscalReferenciada.NumeroCOO
            };
        }

        private List<det> GetProdutos()
        {
            var produtos = new List<det>();

            int i = 1;
            foreach (var produto in NotaFiscal.Produtos)
            {
                produtos.Add(GetDetalhe(i++, produto));
            }

            return produtos;
        }

        private det GetDetalhe(int nItem, Produto item)
        {
            return new det
            {
                nItem = nItem,
                prod = GetProduto(item),
                imposto = GetProdutoImposto(item),
                impostoDevol = _levaImpostoDoRegimeAntigo()
                    ? GetProdutoImpostoDevolucao(item.Impostos.IPI)
                    : null,
                infAdProd = GetProdutoIndAdProd(item),
                DFeReferenciado = GetDFeReferenciado(item),
            };
        }

        /// <summary>
        /// VC01 - Referenciamento do item de outro DFe. Na NF-e de devolucao a
        /// referencia deixou de ser so no cabecalho (refNFe) e passou a ser por
        /// item, chave mais nItem de origem (regra VC02-14).
        /// </summary>
        private DFeReferenciado GetDFeReferenciado(Produto item)
        {
            if (string.IsNullOrEmpty(item.DocumentoReferenciadoChaveAcesso))
            {
                return null;
            }

            return new DFeReferenciado
            {
                chaveAcesso = item.DocumentoReferenciadoChaveAcesso,
                nItem = item.DocumentoReferenciadoItem
            };
        }

        private prod GetProduto(Produto item)
        {
            //ValidaProduto(item);

            var cProd = item.Referencia;
            var cEAN = GetProdutoEAN(item.CodigoBarras);
            var xProd = item.Descricao;
            var ncm = item.NCM;
            var cfop = item.CFOP;
            var uCom = item.UnidadeCompra;
            var qCom = item.Quantidade;
            var vUnCom = item.ValorUnitario;
            var vProd = item.ValorTotal;
            var vDesc = item.Desconto + item.Impostos.ICMS.ValorICMSDesonerado.GetValueOrDefault();
            var uTrib = !string.IsNullOrEmpty(item.UnidadeTributacao) ? item.UnidadeTributacao : item.UnidadeCompra;
            var cBenef = item.CodigoBeneficioFiscal;

            // TODO: Se unidade comercial for diferente de unidade tributada, criar conversão. Eg. pwNFE2.XML.NFeXML._gerarXMLNFe:1332
            var qTrib = item.QuantidadeTributacao > 0 ? item.QuantidadeTributacao : item.Quantidade;
            var vUnTrib = item.ValorUnitarioTributacao > 0 ? item.ValorUnitarioTributacao : item.ValorUnitario;
            var vFrete = item.Frete;
            var vOutro = item.OutrasDespesasAcessorias;
            IndicadorTotal indTot = GetProdutoIndicadorTotal(item.ValorTotal);

            if (_cfgApp.CfgServico.tpAmb == TipoAmbiente.Homologacao)
            {
                xProd = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            var p = new prod
            {
                cProd = cProd,
                cEAN = cEAN,
                xProd = xProd,
                NCM = ncm,
                CFOP = cfop,
                uCom = uCom,
                qCom = qCom,
                vUnCom = vUnCom,
                vProd = vProd,
                vDesc = vDesc,
                cEANTrib = cEAN,
                uTrib = uTrib,
                qTrib = qTrib,
                vUnTrib = vUnTrib,
                vFrete = vFrete,
                vOutro = vOutro,
                indTot = indTot,
                xPed = item.PedidoCompraNumero,
                nItemPed = item.PedidoCompraItem,
                ProdutoEspecifico = GetProdutoCombustivel(item),
                DI = GetProdutoDI(item.DeclaracaoImportacao),
                //NVE = {"AA0001", "AB0002", "AC0002"},
                CEST = item.CEST.RetornaNumeros(),
                cBenef = cBenef

                //ProdutoEspecifico = new arma
                //{
                //    tpArma = TipoArma.UsoPermitido,
                //    nSerie = "123456",
                //    nCano = "123456",
                //    descr = "TESTE DE ARMA"
                //}
            };
            return p;
        }

        private IndicadorTotal GetProdutoIndicadorTotal(decimal valorTotalItem)
        {
            //0 – o valor do item (vProd) não compõe o valor total da NF-e (vProd)
            //1 – o valor do item (vProd) compõe o valor total da NF-e (vProd) (v2.0)
            if ((NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnAjuste || NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnComplementar)
                && valorTotalItem == 0)
            {
                return IndicadorTotal.ValorDoItemNaoCompoeTotalNF;
            }

            return IndicadorTotal.ValorDoItemCompoeTotalNF;
        }

        //private void ValidaProduto(Produto produto)
        //{
        //    if (string.IsNullOrEmpty(produto.NCM))
        //    {
        //        throw new Exception($"Nota fiscal Nº: {_notaFiscal.Numero}\r\n\r\nNCM não cadastrado no produto: ({produto.Referencia}) {produto.Descricao}");
        //    }

        //    if ((_notaFiscal.EFinalidadeNFe != FinalidadeNFe.fnComplementar && _notaFiscal.EFinalidadeNFe != FinalidadeNFe.fnAjuste)
        //        && produto.ValorTotal == 0)
        //    {
        //        throw new Exception($"Nota fiscal Nº: {_notaFiscal.Numero}\r\n\r\nValor total do produto: ({produto.Referencia}) {produto.Descricao} é inválido");
        //    }

        //    if (produto.CFOP == 0)
        //    {
        //        throw new Exception($"Nota fiscal Nº: {_notaFiscal.Numero}\r\n\r\nCFOP não informado para o produto: ({produto.Referencia}) {produto.Descricao}");
        //    }
        //}

        private decimal? GetImpostoVTotTrib(Produto item)
        {
            //if (!_notaFiscal.Emitente.HabilitarDetalhamentoImposto
            //    || _notaFiscal.Destinatario.EConsumidorFinal != ConsumidorFinal.cfConsumidorFinal)
            //{
            //    return null;
            //}

            return item.IBPTValor;
        }

        /// <summary>
        /// NF-e de credito (<c>finNFe = 5</c>) e de debito (<c>6</c>) <b>nao levam os grupos de
        /// imposto do regime antigo</b> - ICMS, ICMSUFDest, IPI, II, PIS, COFINS e o
        /// <c>impostoDevol</c>. Regra <b>B25-80</b>, rejeicao <b>1001</b>: o grupo e recusado
        /// mesmo zerado, entao tem de ficar FORA do XML, nao ir com zero.
        /// <para>O <c>IBSCBS</c> continua: e o motivo da nota. O <c>cClassTrib</c> 800xxx
        /// (transferencia de credito) e 811xxx (ajustes) mora nele, e e ele que a rejeicao
        /// <b>1202</b> cobra junto do <c>tpNFCredito</c>/<c>tpNFDebito</c>.</para>
        /// <para><b>Quatro tipos sao excecao</b>, porque neles a mercadoria CIRCULA: credito
        /// <c>03</c> (retorno por recusa total), <c>04</c> (reducao de valores) e <c>06</c>
        /// (retorno por recusa parcial), e debito <c>07</c> (perda em estoque). Nesses o imposto
        /// antigo continua saindo - por isso a decisao depende do TIPO, nao so da finalidade.</para>
        /// <para>Esta e a resposta unica a pergunta: os grupos do item, o <c>impostoDevol</c> e
        /// os totais do regime antigo no <c>ICMSTot</c> perguntam todos aqui. Governar so o item
        /// deixaria total sem item correspondente, e o total tem de bater com a soma dos itens.</para>
        /// <para>O <c>vTotTrib</c> sai junto, por ser valor de imposto e a regra falar em nao
        /// levar imposto. Se algum dia a SEFAZ cobrar o campo nestas duas finalidades, a
        /// rejeicao nomeia o campo - diferente da 1001, que so diz que o grupo nao e admitido.</para>
        /// </summary>
        /// <summary>
        /// Nota de credito (<c>finNFe = 5</c>) ou de debito (<c>6</c>). Tres decisoes dependem
        /// disto e precisam concordar: os grupos de imposto do item e os totais
        /// (<see cref="FinalidadeLevaImpostoDoRegimeAntigo"/>), o <c>pag</c> e o <c>cobr</c>.
        /// </summary>
        public static bool EhNotaDeCreditoOuDebito(FinalidadeNFe finNFe)
        {
            return finNFe == FinalidadeNFe.fnNotaCredito
                || finNFe == FinalidadeNFe.fnNotaDebito;
        }

        public static bool FinalidadeLevaImpostoDoRegimeAntigo(FinalidadeNFe finNFe, int? tipoNotaCreditoDebito)
        {
            if (finNFe == FinalidadeNFe.fnNotaCredito)
            {
                return tipoNotaCreditoDebito == (int)TipoNFeCredito.tcRetornoRecusaNaoLocalizacaoEntrega
                    || tipoNotaCreditoDebito == (int)TipoNFeCredito.tcReducaoValores
                    || tipoNotaCreditoDebito == (int)TipoNFeCredito.tcRetornoRecusaParcialEntrega;
            }

            if (finNFe == FinalidadeNFe.fnNotaDebito)
            {
                return tipoNotaCreditoDebito == (int)TipoNFeDebito.tdPerdaEmEstoque;
            }

            return true;
        }

        /// <summary>
        /// Atalho para quem ja tem a nota em mao - o <see cref="NotaFiscal"/> desta instancia.
        /// </summary>
        private bool _levaImpostoDoRegimeAntigo()
        {
            return FinalidadeLevaImpostoDoRegimeAntigo(NotaFiscal.EFinalidadeNFe,
                NotaFiscal.TipoNotaCreditoDebito);
        }

        /// <summary>
        /// O <c>vNF</c> da nota que nao leva imposto do regime antigo. Ele vinha do total
        /// calculado no banco, que SOMA os impostos zerados pela B25-80: a <b>W16-10</b> refaz a
        /// conta com o que esta no XML, e sobra rejeicao <b>610</b> - "Total da NF-e difere do
        /// somatorio dos valores que compoem o valor total". Exemplo: produto 100 + IPI 10 grava
        /// <c>vNF</c> 110 com <c>vIPI</c> 0 no XML.
        /// <para>E a conta da propria W16-10, com as parcelas zeradas valendo zero. O
        /// <c>vICMSDeson</c> nao aparece porque o <c>vDesc</c> montado aqui ja o inclui.</para>
        /// </summary>
        public static decimal TotalDaNotaSemImpostoDoRegimeAntigo(decimal vProd, decimal vDesc,
            decimal vFrete, decimal vSeg, decimal vOutro)
        {
            return vProd - vDesc + vFrete + vSeg + vOutro;
        }

        private imposto GetProdutoImposto(Produto item)
        {
            if (!_levaImpostoDoRegimeAntigo())
            {
                return new imposto
                {
                    IBSCBS = GetProdutoCBSIBS(item.Impostos.CBSIBS)
                };
            }

            return new imposto
            {
                vTotTrib = GetImpostoVTotTrib(item),
                ICMS = GetProdutoICMS(item.Impostos.ICMS),
                ICMSUFDest = GetProdutoIcmsUfDest(item.Impostos.PartilhaICMS),
                IPI = GetProdutoIPI(item.Impostos.IPI),
                II = GetProdutoII(item.Impostos.ImpostoImportacao),
                PIS = GetProdutoPIS(item.Impostos.PIS),
                COFINS = GetProdutoCOFINS(item.Impostos.COFINS),
                IBSCBS = GetProdutoCBSIBS(item.Impostos.CBSIBS)
                //IS = GetProdutoIS(item.I),
            };
        }

        /// <summary>
        /// gCompraGov e do documento, nao do item: o redutor entra na aliquota efetiva de
        /// todos os itens. Emitir gTribCompraGov no item sem este grupo gera rejeicao 1033.
        /// </summary>
        private static gCompraGov ObterCompraGovernamental(CompraGovernamental compraGovernamental)
        {
            if (compraGovernamental == null)
            {
                return null;
            }

            return new gCompraGov()
            {
                tpEnteGov = compraGovernamental.TipoEnteGovernamental,
                pRedutor = compraGovernamental.PercentualRedutor,
                tpOperGov = compraGovernamental.TipoOperacaoGovernamental,
                //Lista nova: o DTO e serializado e pode ser mexido por quem recebe, e a
                //entidade guarda as chaves ja conferidas.
                refDFeAnt = compraGovernamental.ChavesDocumentosAnteriores?.Count > 0
                    ? new List<string>(compraGovernamental.ChavesDocumentosAnteriores)
                    : null
            };
        }

        public static IBSCBS GetProdutoCBSIBS(CBSIBSImposto cbiIbsImposto)
        {
            if(cbiIbsImposto == null)
            {
                return null;
            }

            // gIBSCBS, gIBSCBSMono, gTransfCred e gAjusteCompet sao as quatro opcoes do
            // primeiro xs:choice de TTribNFe. Em item monofasico, de transferencia ou de
            // ajuste o grupo normal chega como casca - so a base de calculo, porque gCBS,
            // gIBSUF e gIBSMun dependem de CST que exige CBS/IBS - e cede lugar ao irmao.
            var grupoMonofasico = ObterCbsIbsGrupoMonofasico(cbiIbsImposto.CbsIbs?.GrupoTributacaoMonofasico);
            var grupoTransferenciaCredito = ObterCbsIbsGrupoTransferenciaCredito(cbiIbsImposto.CbsIbs?.GrupoTransferenciaCredito);
            var grupoAjusteCompetencia = ObterCbsIbsGrupoAjusteCompetencia(cbiIbsImposto.CbsIbs?.GrupoAjusteCompetencia);
            gIBSCBS grupoNormal = null;

            if (grupoMonofasico != null
                || grupoTransferenciaCredito != null
                || grupoAjusteCompetencia != null)
            {
                // A conferencia vem antes de montar o grupo normal de proposito: item
                // monofasico com tributacao normal pela metade tem de acusar o conflito de
                // grupos, nao "tributacao incompleta".
                if (TemConteudoProprio(cbiIbsImposto.CbsIbs))
                {
                    throw new ArgumentException("O item informa tributação normal de IBS/CBS junto com grupo monofásico, de transferência de crédito ou de ajuste de competência: o leiaute aceita somente um desses grupos.");
                }
            }
            else
            {
                grupoNormal = ObterCbsIbsGrupo(cbiIbsImposto.CbsIbs);
            }

            var ibsCbs = new IBSCBS()
            {

                CST = cbiIbsImposto.Cst,
                cClassTrib = cbiIbsImposto.ClassificacaoTributaria,

                gIBSCBS = grupoNormal,
                gTransfCred = grupoTransferenciaCredito,
                gIBSCBSMono = grupoMonofasico,
                gCredPresOper = ObterCbsIbsGrupoCreditoPresumidoOperacao(cbiIbsImposto.CbsIbs?.GrupoCreditoPresumido),
                gCredPresIBSZFM = ObterCbsIbsGrupoCreditoPresumidoZFM(cbiIbsImposto.CbsIbs?.GrupoCreditoPresumido?.IBSGrupoCreditoPresumidoZFM),
                gAjusteCompet = grupoAjusteCompetencia,
                gEstornoCred = ObterCbsIbsGrupoEstornoCredito(cbiIbsImposto.GrupoEstornoCredito)
            };

            ConferirExclusividadeDosGruposCbsIbs(ibsCbs);

            if (cbiIbsImposto.IsDoacao == true)
            {
                ibsCbs.indDoacao = "1";
            }

            return ibsCbs;
        }

        /// <summary>
        /// O grupo normal de IBS/CBS tem conteudo proprio, e nao e so a base de calculo que
        /// acompanha qualquer item.
        /// </summary>
        private static bool TemConteudoProprio(CBSIBSGrupo grupoCbsIbs)
        {
            return grupoCbsIbs?.CBSGrupo != null
                || grupoCbsIbs?.IBSGrupo != null
                || grupoCbsIbs?.GrupoTributacaoRegular != null
                || grupoCbsIbs?.GrupoTributacaoCompraGoverno != null;
        }

        /// <summary>
        /// TTribNFe tem dois xs:choice (DFeTiposBasicos_v1.00.xsd): um entre gIBSCBS,
        /// gIBSCBSMono, gTransfCred e gAjusteCompet, outro entre gCredPresOper e
        /// gCredPresIBSZFM. gEstornoCred nao entra em nenhum dos dois - e irmao opcional do
        /// primeiro choice. Mais de uma opcao preenchida gera XML que fere o schema, e a
        /// rejeicao chega como erro generico de validacao sem apontar a causa.
        /// </summary>
        private static void ConferirExclusividadeDosGruposCbsIbs(IBSCBS ibsCbs)
        {
            var gruposDaOperacao = new List<string>();
            if (ibsCbs.gIBSCBS != null) gruposDaOperacao.Add(nameof(IBSCBS.gIBSCBS));
            if (ibsCbs.gIBSCBSMono != null) gruposDaOperacao.Add(nameof(IBSCBS.gIBSCBSMono));
            if (ibsCbs.gTransfCred != null) gruposDaOperacao.Add(nameof(IBSCBS.gTransfCred));
            if (ibsCbs.gAjusteCompet != null) gruposDaOperacao.Add(nameof(IBSCBS.gAjusteCompet));

            if (gruposDaOperacao.Count > 1)
            {
                throw new ArgumentException($"O item informa mais de um grupo de IBS/CBS exclusivos entre si ({string.Join(", ", gruposDaOperacao)}): o leiaute aceita somente um deles.");
            }

            if (ibsCbs.gCredPresOper != null
                && ibsCbs.gCredPresIBSZFM != null)
            {
                throw new ArgumentException("O item informa gCredPresOper e gCredPresIBSZFM: o leiaute aceita somente um dos dois.");
            }
        }

        private static gIBSCBS ObterCbsIbsGrupo(CBSIBSGrupo grupoCbsIbs)
        {
            if (grupoCbsIbs == null)
            {
                return null;
            }

            var ibsUF = ObterCbsIbsGrupoIbsUF(grupoCbsIbs.IBSGrupo?.IBSUFGrupo);
            var ibsMunicipio = ObterCbsIbsGrupoIbsMunicipio(grupoCbsIbs.IBSGrupo?.IBSMunicipioGrupo);
            var cbs = ObterCbsIbsGrupoCbs(grupoCbsIbs.CBSGrupo);

            // TCIBS_NFe (DFeTiposBasicos_v1.00.xsd) exige vBC, gIBSUF, gIBSMun, vIBS e
            // gCBS juntos: o grupo e completo ou nao existe. Sem nenhuma parte o item nao
            // tem tributacao normal de IBS/CBS - caso do monofasico e da transferencia de
            // credito, que ocupam outra opcao do mesmo xs:choice.
            if (ibsUF == null
                && ibsMunicipio == null
                && cbs == null
                && grupoCbsIbs.GrupoTributacaoRegular == null
                && grupoCbsIbs.GrupoTributacaoCompraGoverno == null)
            {
                return null;
            }

            // Parte presente e parte ausente nao da XML valido nem pode ser descartado em
            // silencio: e dado incompleto no cadastro do item.
            if (ibsUF == null
                || ibsMunicipio == null
                || cbs == null)
            {
                throw new ArgumentException("Tributação de IBS/CBS incompleta no item: o grupo gIBSCBS exige o IBS da UF, o IBS do município e a CBS juntos.");
            }

            return new gIBSCBS()
            {
                vBC = grupoCbsIbs.BaseCalculo,
                gCBS = cbs,
                gIBSUF = ibsUF,
                gIBSMun = ibsMunicipio,
                gTribCompraGov = ObterCbsIbsGrupoCompraGovernamental(grupoCbsIbs.GrupoTributacaoCompraGoverno),
                gTribRegular = ObterCbsIbsGrupoTributacaoRegular(grupoCbsIbs.GrupoTributacaoRegular),
                vIBS = grupoCbsIbs.IBSGrupo?.ValorTotal,
            };
        }

        private static gCBS ObterCbsIbsGrupoCbs(CBSGrupo cbs)
        {
            if (cbs == null)
            {
                return null;
            }

            return new gCBS()
            {
                pCBS = cbs.AliquotaPercentual,
                
                gDif = ObterCbsIbsGrupoDiferimento(cbs?.GrupoDiferimento),
                gDevTrib = ObterCbsIbsGrupoDevolucaoTributo(cbs?.GrupoDevolucaoTributo),
                gRed = ObterCbsIbsGrupoReducaoAliquota(cbs?.GrupoReducao),
                gALCZFMCBS = ObterCbsIbsGrupoAreaIncentivada(cbs?.GrupoAreaIncentivada),
                vCBS = cbs.ValorTotal
            };
        }

        /// <summary>
        /// gALCZFMCBS (UB66a). A entidade ja conferiu o nProcSuframa exigido pelo tipo 2
        /// (rejeicao 1192), entao aqui e so copia.
        /// </summary>
        private static gALCZFMCBS ObterCbsIbsGrupoAreaIncentivada(CBSGrupoAreaIncentivada areaIncentivada)
        {
            if (areaIncentivada == null)
            {
                return null;
            }

            return new gALCZFMCBS()
            {
                tpALCZFMCBS = areaIncentivada.TipoAplicacao,
                nProcSuframa = areaIncentivada.NumeroProcessoSuframa,
                pAliqEfetRegCBS = areaIncentivada.AliquotaEfetivaRegular,
                vTribRegCBS = areaIncentivada.ValorTributoRegular
            };
        }

        private static gIBSUF ObterCbsIbsGrupoIbsUF(IBSUFGrupo ibsUF)
        {
            if (ibsUF == null)
            {
                return null;
            }

            return new gIBSUF()
            {
                pIBSUF = ibsUF.AliquotaPercentual,

                gDif = ObterCbsIbsGrupoDiferimento(ibsUF?.GrupoDiferimento),
                gDevTrib = ObterCbsIbsGrupoDevolucaoTributo(ibsUF?.GrupoDevolucaoTributo),
                gRed = ObterCbsIbsGrupoReducaoAliquota(ibsUF?.GrupoReducao),
                vIBSUF = ibsUF.ValorTotal
            };
        }

        private static gIBSMun ObterCbsIbsGrupoIbsMunicipio(IBSMunicipioGrupo ibsMunicipio)
        {
            if (ibsMunicipio == null)
            {
                return null;
            }

            return new gIBSMun()
            {
                pIBSMun = ibsMunicipio.AliquotaPercentual,

                gDif = ObterCbsIbsGrupoDiferimento(ibsMunicipio?.GrupoDiferimento),
                gDevTrib = ObterCbsIbsGrupoDevolucaoTributo(ibsMunicipio?.GrupoDevolucaoTributo),
                gRed = ObterCbsIbsGrupoReducaoAliquota(ibsMunicipio?.GrupoReducao),
                vIBSMun = ibsMunicipio.ValorTotal
            };
        }

        private static gDif ObterCbsIbsGrupoDiferimento(CBSIBSGrupoDiferimento diferimento)
        {
            if (diferimento == null)
            {
                return null;
            }

            return new gDif()
            {
                pDif = diferimento.PercentualDiferimento,
                vDif = diferimento.ValorDiferimento
            };
        }

        private static gDevTrib ObterCbsIbsGrupoDevolucaoTributo(CBSIBSGrupoDevolucaoTributo devolucaoTributo)
        {
            if (devolucaoTributo == null)
            {
                return null;
            }

            return new gDevTrib()
            {
                pDevTrib = devolucaoTributo.PercentualDevolucao,
                vDevTrib = devolucaoTributo.ValorDevolvido
            };
        }

        private static gRed ObterCbsIbsGrupoReducaoAliquota(CBSIBSGrupoReducao reducao)
        {
            if (reducao == null)
            {
                return null;
            }

            return new gRed()
            {
                pRedAliq = reducao.PercentualReducao,
                pAliqEfet = reducao.AliquotaEfetiva
            };
        }

        private static gTribCompraGov ObterCbsIbsGrupoCompraGovernamental(CBSIBSGrupoTributacaoCompraGoverno compraGovernamental)
        {
            if (compraGovernamental == null)
            {
                return null;
            }

            return new gTribCompraGov()
            {
                pAliqCBS = compraGovernamental.AliquotaPercentualCBS,
                vTribCBS = compraGovernamental.ValorTributoCBS,
                pAliqIBSUF = compraGovernamental.AliquotaPercentualIBSUF,
                vTribIBSUF = compraGovernamental.ValorTributoIBSUF,
                pAliqIBSMun = compraGovernamental.AliquotaPercentualIBSMunicipio,
                vTribIBSMun = compraGovernamental.ValorTributoIBSMunicipio,
            };
        }

        private static gTribRegular ObterCbsIbsGrupoTributacaoRegular(CBSIBSGrupoTributacaoRegular tributacaoRegular)
        {
            if (tributacaoRegular == null)
            {
                return null;
            }

            return new gTribRegular()
            {
                CSTReg = tributacaoRegular.CSTRegular,
                cClassTribReg = tributacaoRegular.CClassTribRegular,
                pAliqEfetRegIBSUF = tributacaoRegular.AliquotaPercentualRegularIBSUF,
                vTribRegIBSUF = tributacaoRegular.ValorTributoRegularIBSUF,
                pAliqEfetRegIBSMun = tributacaoRegular.AliquotaPercentualRegularIBSMunicipio,
                vTribRegIBSMun = tributacaoRegular.ValorTributoRegularIBSMunicipio,
                pAliqEfetRegCBS = tributacaoRegular.AliquotaPercentualRegularCBS,
                vTribRegCBS = tributacaoRegular.ValorTributoRegularCBS
            };
        }

        private static gCredPresOper ObterCbsIbsGrupoCreditoPresumidoOperacao(CBSIBSGrupoCreditoPresumido creditoPresumido)
        {
            // O que diz se a operacao existe e o cCredPres, nao os subgrupos: gIBSCredPres
            // e gCBSCredPres sao minOccurs=0 em TCredPresOper, logo operacao sem nenhum dos
            // dois e valida. Item que so tem credito presumido de ZFM nao informa cCredPres
            // e sai apenas no gCredPresIBSZFM, que e a outra opcao do mesmo xs:choice.
            if (creditoPresumido?.CodigoClassificacaoCreditoPresumido == null)
            {
                return null;
            }

            return new gCredPresOper()
            {
                vBCCredPres = creditoPresumido.BaseCalculoCreditoPresumido,
                cCredPres = creditoPresumido.CodigoClassificacaoCreditoPresumido.Value,
                gCBSCredPres = ObterCbsIbsGrupoCreditoPresumido(creditoPresumido.CBSGrupoCreditoPresumido, "da CBS"),
                gIBSCredPres = ObterCbsIbsGrupoCreditoPresumido(creditoPresumido.IBSGrupoCreditoPresumido, "do IBS"),
            };
        }

        private static gIBSCredPres ObterCbsIbsGrupoCreditoPresumido(GrupoCreditoPresumidoBase creditoPresumido, string tributo)
        {
            if (creditoPresumido == null)
            {
                return null;
            }

            // TCredPres tem um xs:choice entre vCredPres e vCredPresCondSus: o credito ou
            // esta aproveitado, ou esta em condicao suspensiva, nunca os dois. Quem decide
            // e a presenca do valor, nao o valor ser diferente de zero - credito suspensivo
            // de valor zero existe no XML e tem de voltar no mesmo braco do choice.
            if (creditoPresumido.ValorCredito.HasValue
                && creditoPresumido.ValorCreditoEmCondicaoSuspensiva.HasValue)
            {
                throw new ArgumentException($"Crédito presumido {tributo} informa valor aproveitado e valor em condição suspensiva ao mesmo tempo: o leiaute aceita somente um dos dois.");
            }

            return new gIBSCredPres()
            {
                pCredPres = creditoPresumido.PercentualCredito,
                vCredPres = creditoPresumido.ValorCredito ?? 0,
                vCredPresCondSus = creditoPresumido.ValorCreditoEmCondicaoSuspensiva
            };
        }

        private static gCredPresIBSZFM ObterCbsIbsGrupoCreditoPresumidoZFM(IBSGrupoCreditoPresumidoZFM creditoPresumidoZFM)
        {
            if (creditoPresumidoZFM == null)
            {
                return null;
            }

            return new gCredPresIBSZFM()
            {
                competApur = creditoPresumidoZFM.AnoMesCompetenciaApuracao,
                tpCredPresIBSZFM = creditoPresumidoZFM.TipoCreditoPresumidoIBSZFM,
                vCredPresIBSZFM = creditoPresumidoZFM.ValorCreditoPresumidoIBSZFM
            };
        }

        /// <summary>
        /// gAjusteCompet e uma das opcoes do xs:choice de TTribNFe: quando sai, nem gIBSCBS
        /// nem gIBSCBSMono nem gTransfCred podem sair.
        /// </summary>
        private static gAjusteCompet ObterCbsIbsGrupoAjusteCompetencia(CBSIBSGrupoAjusteCompetencia ajusteCompetencia)
        {
            if (ajusteCompetencia == null)
            {
                return null;
            }

            return new gAjusteCompet()
            {
                competApur = ajusteCompetencia.AnoMesCompetenciaApuracao,
                vIBS = ajusteCompetencia.ValorAjusteIBS,
                vCBS = ajusteCompetencia.ValorAjusteCBS
            };
        }

        /// <summary>
        /// gEstornoCred nao participa de xs:choice nenhum: convive com qualquer grupo da
        /// operacao. vIBSEstCred e vCBSEstCred sao obrigatorios dentro do grupo, por isso
        /// grupo com os dois valores zerados nao e emitido.
        /// </summary>
        private static gEstornoCred ObterCbsIbsGrupoEstornoCredito(CBSIBSGrupoEstornoCredito estornoCredito)
        {
            if (estornoCredito == null
                || (estornoCredito.ValorEstornoIBS == 0
                    && estornoCredito.ValorEstornoCBS == 0))
            {
                return null;
            }

            return new gEstornoCred()
            {
                vIBSEstCred = estornoCredito.ValorEstornoIBS,
                vCBSEstCred = estornoCredito.ValorEstornoCBS
            };
        }

        /// <summary>
        /// TMonofasia do PL_010f: o IBS vai em gIBSMonoAdRem OU gIBSMonoAdValorem, a CBS
        /// em gCBSMonoAdRem OU gCBSMonoAdValorem, e cada invólucro carrega gMonoPadrao,
        /// gMonoReten, gMonoRet e gpBioDiferenca. As entidades de monofasia desta BLL só
        /// guardam quantidade e alíquota ad rem, por isso o mapeamento usa os dois
        /// invólucros ad rem. Ad valorem e gpBioDiferenca exigem campos que as entidades
        /// não têm e ficam de fora.
        /// <para>O grupo de diferimento da monofasia saiu do leiaute e por isso deixou de
        /// ser emitido. A entidade continua lendo o grupo de documentos já autorizados.</para>
        /// </summary>
        private static gIBSCBSMono ObterCbsIbsGrupoMonofasico(CBSIBSGrupoMonofasico grupoMonofasico)
        {
            if (grupoMonofasico == null)
            {
                return null;
            }

            // TMonofasia deixa as duas sequencias como minOccurs=0, mas vTotIBSMonoItem
            // e vTotCBSMonoItem sao obrigatorios: grupo so com os totais e valido no
            // schema (conferido contra o PL_010f) e e o que sai ao reemitir documento
            // antigo cuja unica monofasia era o gMonoDif, que saiu do leiaute.
            return new gIBSCBSMono()
            {
                gIBSMonoAdRem = ObterCbsIbsMonofasicoIbsAdRem(grupoMonofasico),
                gCBSMonoAdRem = ObterCbsIbsMonofasicoCbsAdRem(grupoMonofasico),

                vTotCBSMonoItem = grupoMonofasico.ValorTotalMonofasicoCBS,
                vTotIBSMonoItem = grupoMonofasico.ValorTotalMonofasicoIBS
            };
        }

        private static gIBSMonoAdRem ObterCbsIbsMonofasicoIbsAdRem(CBSIBSGrupoMonofasico grupoMonofasico)
        {
            var padrao = ObterCbsIbsMonofasicoIbsPadrao(grupoMonofasico.GrupoMonofasicoTributado);
            var retencao = ObterCbsIbsMonofasicoIbsRetencao(grupoMonofasico.GrupoMonofasicoSujeitoRetencao);
            var retido = ObterCbsIbsMonofasicoIbsRetido(grupoMonofasico.GrupoMonofasicoRetido);

            if (padrao == null && retencao == null && retido == null)
            {
                return null;
            }

            return new gIBSMonoAdRem()
            {
                gMonoPadrao = padrao,
                gMonoReten = retencao,
                gMonoRet = retido
            };
        }

        private static gCBSMonoAdRem ObterCbsIbsMonofasicoCbsAdRem(CBSIBSGrupoMonofasico grupoMonofasico)
        {
            var padrao = ObterCbsIbsMonofasicoCbsPadrao(grupoMonofasico.GrupoMonofasicoTributado);
            var retencao = ObterCbsIbsMonofasicoCbsRetencao(grupoMonofasico.GrupoMonofasicoSujeitoRetencao);
            var retido = ObterCbsIbsMonofasicoCbsRetido(grupoMonofasico.GrupoMonofasicoRetido);

            if (padrao == null && retencao == null && retido == null)
            {
                return null;
            }

            return new gCBSMonoAdRem()
            {
                gMonoPadrao = padrao,
                gMonoReten = retencao,
                gMonoRet = retido
            };
        }

        /// <summary>
        /// A entidade guarda IBS e CBS no mesmo objeto, mas no leiaute cada tributo tem o
        /// seu invólucro e os dois são independentes. Por isso a guarda é pelo dado DAQUELE
        /// tributo: com a guarda só na quantidade (que é compartilhada), ler um XML que
        /// trouxesse apenas o IBS fazia a reescrita fabricar um grupo de CBS zerado.
        /// <para>A quantidade é uma só nos dois grupos: é a mesma quantidade física de
        /// combustível, e o leiaute repete o qBCMono em cada invólucro.</para>
        /// </summary>
        private static gMonoPadraoIBSAdRem ObterCbsIbsMonofasicoIbsPadrao(CBSIBSGrupoMonofasicoTributado grupoMonofasicoTributado)
        {
            if (grupoMonofasicoTributado == null
                || grupoMonofasicoTributado.Quantidade == 0
                || (grupoMonofasicoTributado.AliquotaAdRemIBS == 0
                    && grupoMonofasicoTributado.ValorMonofasicoIBS == 0))
            {
                return null;
            }

            return new gMonoPadraoIBSAdRem()
            {
                qBCMono = grupoMonofasicoTributado.Quantidade,
                adRemIBS = grupoMonofasicoTributado.AliquotaAdRemIBS,
                vIBSMono = grupoMonofasicoTributado.ValorMonofasicoIBS
            };
        }

        private static gMonoPadraoCBSAdRem ObterCbsIbsMonofasicoCbsPadrao(CBSIBSGrupoMonofasicoTributado grupoMonofasicoTributado)
        {
            if (grupoMonofasicoTributado == null
                || grupoMonofasicoTributado.Quantidade == 0
                || (grupoMonofasicoTributado.AliquotaAdRemCBS == 0
                    && grupoMonofasicoTributado.ValorMonofasicoCBS == 0))
            {
                return null;
            }

            return new gMonoPadraoCBSAdRem()
            {
                qBCMono = grupoMonofasicoTributado.Quantidade,
                adRemCBS = grupoMonofasicoTributado.AliquotaAdRemCBS,
                vCBSMono = grupoMonofasicoTributado.ValorMonofasicoCBS
            };
        }

        private static gMonoRetenIBSAdRem ObterCbsIbsMonofasicoIbsRetencao(CBSIBSGrupoMonofasicoSujeitoRetencao grupoMonofasicoSujeitoRetencao)
        {
            if (grupoMonofasicoSujeitoRetencao == null
                || grupoMonofasicoSujeitoRetencao.Quantidade == 0
                || (grupoMonofasicoSujeitoRetencao.AliquotaAdRemIBS == 0
                    && grupoMonofasicoSujeitoRetencao.ValorMonofasicoIBS == 0))
            {
                return null;
            }

            return new gMonoRetenIBSAdRem()
            {
                qBCMonoReten = grupoMonofasicoSujeitoRetencao.Quantidade,
                adRemIBSReten = grupoMonofasicoSujeitoRetencao.AliquotaAdRemIBS,
                vIBSMonoReten = grupoMonofasicoSujeitoRetencao.ValorMonofasicoIBS
            };
        }

        private static gMonoRetenCBSAdRem ObterCbsIbsMonofasicoCbsRetencao(CBSIBSGrupoMonofasicoSujeitoRetencao grupoMonofasicoSujeitoRetencao)
        {
            if (grupoMonofasicoSujeitoRetencao == null
                || grupoMonofasicoSujeitoRetencao.Quantidade == 0
                || (grupoMonofasicoSujeitoRetencao.AliquotaAdRemCBS == 0
                    && grupoMonofasicoSujeitoRetencao.ValorMonofasicoCBS == 0))
            {
                return null;
            }

            return new gMonoRetenCBSAdRem()
            {
                qBCMonoReten = grupoMonofasicoSujeitoRetencao.Quantidade,
                adRemCBSReten = grupoMonofasicoSujeitoRetencao.AliquotaAdRemCBS,
                vCBSMonoReten = grupoMonofasicoSujeitoRetencao.ValorMonofasicoCBS
            };
        }

        /// <summary>
        /// gMonoRet só tem o valor retido no leiaute atual: quantidade e alíquota do grupo
        /// retido não têm para onde ir, por isso a guarda é pelo valor.
        /// </summary>
        private static gMonoRetIBS ObterCbsIbsMonofasicoIbsRetido(CBSIBSMonofasicoGrupoBase grupoMonofasicoRetido)
        {
            if (grupoMonofasicoRetido == null
                || grupoMonofasicoRetido.ValorMonofasicoIBS == 0)
            {
                return null;
            }

            return new gMonoRetIBS()
            {
                vIBSMonoRet = grupoMonofasicoRetido.ValorMonofasicoIBS
            };
        }

        private static gMonoRetCBS ObterCbsIbsMonofasicoCbsRetido(CBSIBSMonofasicoGrupoBase grupoMonofasicoRetido)
        {
            if (grupoMonofasicoRetido == null
                || grupoMonofasicoRetido.ValorMonofasicoCBS == 0)
            {
                return null;
            }

            return new gMonoRetCBS()
            {
                vCBSMonoRet = grupoMonofasicoRetido.ValorMonofasicoCBS
            };
        }

        private static gTransfCred ObterCbsIbsGrupoTransferenciaCredito(CBSIBSGrupoTransferenciaCredito grupoTransferenciaCredito)
        {
            if (grupoTransferenciaCredito == null
                || (grupoTransferenciaCredito.ValorASerTransferidoIBS == 0
                    && grupoTransferenciaCredito.ValorASerTransferidoCBS == 0))
            {
                return null;
            }

            return new gTransfCred()
            {
                vCBS = grupoTransferenciaCredito.ValorASerTransferidoCBS,
                vIBS = grupoTransferenciaCredito.ValorASerTransferidoIBS,
            };
        }

        private ICMS GetProdutoICMS(Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS item)
        {
            return new ICMS
            {
                TipoICMS =
                    NotaFiscal.Emitente.ECRT == CRT.SimplesNacional
                    || NotaFiscal.Emitente.ECRT == CRT.SimplesNacionalMei
                        ? InformarCSOSN(item)
                        : InformarICMS(item)
            };
        }

        private ICMSBasico InformarCSOSN(Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS item)
        {
            var CSOSN = item.CSOSN.GetValueOrDefault();
            var origemMercadoria = item.Origem;

            var pCredSN = item.RepasseCreditoAliquota;
            var vCredICMSSN = item.RepasseCreditoValor;

            var modBCST = item.ModalidadeCalculoST;

            var pMVAST = item.AliquotaMVAST;
            var vBCST = item.BaseCalculoST;
            var pICMSST = item.AliquotaST;
            var vICMSST = item.ValorTotalST;
            var vBC = item.BaseCalculo;
            var pICMS = item.Aliquota;
            var vICMS = item.ValorTotal;

            var vBCFCPST = item.BaseCaluloFCP;
            var pFCPST = item.AliquotaFCP;
            var vFCPST = item.ValorTotalFCP;

            var pRedBC = item.AliquotaReducaoBaseCalculo;
            var pRedBCST = item.AliquotaReducaoBaseCalculoST;

            var qBCMonoRet = item.BaseCalculoICMSRetido;
            var vICMSMonoRet = item.ValorICMSRetido;
            var adRemICMSRet = item.AliquotaAdRemICMSRetido;

            switch (CSOSN)
            {
                case Csosnicms.Csosn101:
                    return new ICMSSN101
                    {
                        orig = origemMercadoria,
                        CSOSN = Csosnicms.Csosn101,
                        pCredSN = pCredSN.GetValueOrDefault(),
                        vCredICMSSN = vCredICMSSN.GetValueOrDefault()
                    };
                case Csosnicms.Csosn102:
                case Csosnicms.Csosn103:
                case Csosnicms.Csosn300:
                case Csosnicms.Csosn400:
                    return new ICMSSN102
                    {
                        orig = origemMercadoria,
                        CSOSN = CSOSN
                    };
                case Csosnicms.Csosn201:
                    // TODO: pág 40 - NT_2016_002_v1_60

                    return new ICMSSN201
                    {
                        orig = origemMercadoria,
                        CSOSN = Csosnicms.Csosn201,
                        modBCST = modBCST.GetValueOrDefault(),
                        pMVAST = pMVAST,
                        pRedBCST = pRedBCST,
                        vBCST = vBCST,
                        pICMSST = pICMSST,
                        vICMSST = vICMSST,
                        pCredSN = 0M, // TODO:
                        vCredICMSSN = 0m, // TODO:
                        vBCFCPST = vBCFCPST,
                        pFCPST = pFCPST,
                        vFCPST = vFCPST
                    };

                case Csosnicms.Csosn202:
                    // TODO: pág 42 - NT_2016_002_v1_60
                    return new ICMSSN202
                    {
                        orig = origemMercadoria,
                        CSOSN = Csosnicms.Csosn202,
                        modBCST = modBCST.GetValueOrDefault(),
                        pMVAST = pMVAST,
                        pRedBCST = pRedBCST,
                        vBCST = vBCST,
                        pICMSST = pICMSST,
                        vICMSST = vICMSST,
                        vBCFCPST = vBCFCPST,
                        pFCPST = pFCPST,
                        vFCPST = vFCPST
                    };
                case Csosnicms.Csosn500:
                    return new ICMSSN500
                    {
                        orig = origemMercadoria,
                        CSOSN = Csosnicms.Csosn500,
                        vBCSTRet = 0, // TODO: prod_ADQUIRIDOCOMSTBC (campo existe) implementar para serializar na nfe_prod (se a legislação exigir tornar obrigatório
                        vICMSSTRet = 0, // TODO: prod_ADQUIRIDOCOMSTVR
                        pST = 0,
                        vICMSSubstituto = 0
                    };
                case Csosnicms.Csosn900:

                    return new ICMSSN900
                    {
                        orig = origemMercadoria,
                        CSOSN = Csosnicms.Csosn900,
                        modBC = DeterminacaoBaseIcms.DbiValorOperacao, // 3
                        vBC = vBC,
                        pRedBC = pRedBC,
                        pICMS = pICMS,
                        vICMS = vICMS,
                        modBCST = vBCST == 0 || vICMSST == 0 ? null : modBCST,
                        pMVAST = vBCST == 0 || vICMSST == 0 ? null : pMVAST,
                        pRedBCST = null, // TODO:
                        vBCST = vBCST == 0 || vICMSST == 0 ? (decimal?)null : vBCST,
                        pICMSST = vBCST == 0 || vICMSST == 0 ? (decimal?)null : pICMSST,
                        vICMSST = vBCST == 0 || vICMSST == 0 ? (decimal?)null : vICMSST,
                        pCredSN = null, // TODO:
                        vCredICMSSN = null // TODO:
                    };

                case Csosnicms.Cst02: // Tributada integralmente

                    return new ICMS02
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst02,
                        adRemICMS = 0,
                        qBCMono = null,
                        vICMSMono = 0
                    };

                case Csosnicms.Cst53: // Tributação com Diferimento (a exigência do preenchimento das informações do ICMS diferido fica a critério de cada UF). 

                    return new ICMS53
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst53,
                        adRemICMS = null,
                        adRemICMSDif = null,
                        pDif = null,
                        qBCMono = null,
                        qBCMonoDif = null,
                        vICMSMono = null,
                        vICMSMonoDif = null,
                        vICMSMonoOp = null
                    };

                case Csosnicms.Cst61: // Tributação monofásica sobre combustíveis cobrada anteriormente

                    return new ICMS61
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst61,
                        qBCMonoRet = qBCMonoRet,
                        vICMSMonoRet = vICMSMonoRet,
                        adRemICMSRet = adRemICMSRet,
                    };

            }

            throw new ArgumentException($"CSOSN do ICMS inválido ou não implementado no sistema: {CSOSN}.");
        }

        private ICMSBasico InformarICMS(Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS item)
        {
            Csticms CST = item.CST.GetValueOrDefault();
            OrigemMercadoria origemMercadoria = item.Origem;

            var modBC = item.ModalidadeCalculo.GetValueOrDefault();
            var vBC = item.BaseCalculo;
            var pICMS = item.Aliquota;
            var vICMS = item.ValorTotal;

            DeterminacaoBaseIcmsSt? modBCST = item.ModalidadeCalculoST;

            var vBCST = item.BaseCalculoST;
            var pICMSST = item.AliquotaST;
            var vICMSST = item.ValorTotalST;
            var pMVAST = item.AliquotaMVAST;

            var vBCFCPST = item.BaseCaluloFCP;
            var pFCPST = item.AliquotaFCP;
            var vFCPST = item.ValorTotalFCP;

            var pRedBC = item.AliquotaReducaoBaseCalculo;
            var pRedBCST = item.AliquotaReducaoBaseCalculoST;

            var vICMSDeson = item.ValorICMSDesonerado;
            var motDesICMS = item.MotivoDesoneracao;

            var qBCMonoRet = item.BaseCalculoICMSRetido;
            var vICMSMonoRet = item.ValorICMSRetido;
            var adRemICMSRet = item.AliquotaAdRemICMSRetido;

            switch (CST)
            {
                case Csticms.Cst00: // Tributada integralmente

                    return new ICMS00
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst00,
                        modBC = modBC,
                        vBC = vBC,
                        pICMS = pICMS,
                        vICMS = vICMS
                    };

                case Csticms.Cst02: // Tributada integralmente

                    return new ICMS02
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst02,
                        adRemICMS = 0,
                        qBCMono = null,
                        vICMSMono = 0
                    };

                case Csticms.Cst10: // Tributada e com cobrança do ICMS por substituição tributária

                    return new ICMS10
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst10,
                        modBC = modBC,
                        vBC = vBC,
                        pICMS = pICMS,
                        vICMS = vICMS,

                        modBCST = modBCST.GetValueOrDefault(),
                        vBCST = vBCST,
                        pICMSST = pICMSST,
                        vICMSST = vICMSST,
                        pMVAST = pMVAST,

                        vBCFCPST = vBCFCPST,
                        pFCPST = pFCPST,
                        vFCPST = vFCPST
                    };

                case Csticms.Cst20: // Tributação com redução de base de cálculo

                    return new ICMS20
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst20,
                        modBC = modBC,
                        vBC = vBC,
                        pICMS = pICMS,
                        vICMS = vICMS,
                        pRedBC = pRedBC.GetValueOrDefault(),
                        vICMSDeson = null,
                        motDesICMS = null,
                    };

                case Csticms.Cst30: // Tributação Isenta ou não tributada e com cobrança do ICMS por substituição tributária 

                    return new ICMS30
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst30,
                        modBCST = modBCST.GetValueOrDefault(),
                        pMVAST = pMVAST,
                        pRedBCST = null,
                        vBCST = vBCST,
                        pICMSST = pICMSST,
                        vICMSST = vICMSST,
                        vICMSDeson = vICMSDeson,
                        motDesICMS = motDesICMS.GetValueOrDefault()
                    };

                case Csticms.Cst40: // Tributação Isenta
                case Csticms.Cst41: // Tributação Não tributada
                case Csticms.Cst50: // Tributação Suspensão

                    return new ICMS40
                    {
                        orig = origemMercadoria,
                        CST = CST,
                        vICMSDeson = vICMSDeson,
                        motDesICMS = motDesICMS
                    };

                case Csticms.Cst51: // Tributação com Diferimento (a exigência do preenchimento das informações do ICMS diferido fica a critério de cada UF). 

                    return new ICMS51
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst51,
                        modBC = null,
                        pRedBC = null,
                        vBC = null,
                        pICMS = null,
                        vICMSOp = null,
                        pDif = null,
                        vICMSDif = null,
                        vICMS = null
                    };

                case Csticms.Cst53: // Tributação com Diferimento (a exigência do preenchimento das informações do ICMS diferido fica a critério de cada UF). 

                    return new ICMS53
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst53,
                        adRemICMS = null,
                        adRemICMSDif = null,
                        pDif = null,
                        qBCMono = null,
                        qBCMonoDif = null,
                        vICMSMono = null,
                        vICMSMonoDif = null,
                        vICMSMonoOp = null
                    };

                case Csticms.Cst60: // Tributação ICMS cobrado anteriormente por substituição tributária

                    return new ICMS60
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst60,
                        vBCSTRet = 0,
                        vICMSSTRet = 0,
                        pST = 0,
                        vICMSSubstituto = 0
                    };

                case Csticms.Cst61: // Tributação monofásica sobre combustíveis cobrada anteriormente

                    return new ICMS61
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst61,
                        qBCMonoRet = qBCMonoRet,
                        vICMSMonoRet = vICMSMonoRet,
                        adRemICMSRet = adRemICMSRet,
                    };

                case Csticms.Cst70: // Tributação ICMS com redução de base de cálculo e cobrança do ICMS por substituição tributária 

                    return new ICMS70
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst70,
                        modBC = modBC,
                        pRedBC = pRedBC.GetValueOrDefault(),
                        vBC = vBC,
                        pICMS = pICMS,
                        vICMS = vICMS,
                        modBCST = modBCST.GetValueOrDefault(),
                        pMVAST = pMVAST,
                        pRedBCST = pRedBCST,
                        vBCST = vBCST,
                        pICMSST = pICMSST,
                        vICMSST = vICMSST,

                        vBCFCPST = vBCFCPST,
                        pFCPST = pFCPST,
                        vFCPST = vFCPST
                    };

                case Csticms.Cst90: // Tributação ICMS: Outros 

                    return new ICMS90
                    {
                        orig = origemMercadoria,
                        CST = Csticms.Cst90,
                        modBC = modBC,
                        vBC = vBC,
                        pICMS = pICMS,
                        vICMS = vICMS
                    };

                case Csticms.CstPart90:
                case Csticms.CstPart10:
                case Csticms.CstRep41:
                    // Partilha (ICMSPart) e repasse (ICMSST) nao sao montados: a entidade ICMS
                    // nao tem os campos do grupo (pBCOp, UFST, vBCSTRet, vBCSTDest). Devolver
                    // null deixava o item com <ICMS> vazio, e a rejeicao chegava como erro
                    // generico de schema, sem apontar a causa.
                    throw new ArgumentException($"CST {CST} do ICMS (partilha/repasse) não implementado no sistema.");
            }

            throw new ArgumentException($"CST do ICMS inválido ou não implementado no sistema: {CST}.");
        }

        private impostoDevol GetProdutoImpostoDevolucao(IPI item)
        {
            if (NotaFiscal.EFinalidadeNFe != FinalidadeNFe.fnDevolucao
                || item == null
                || item.ValorTotal == 0)
            {
                return null;
            }

            return new impostoDevol
            {
                pDevol = 100,
                IPI = new IPIDevolvido
                {
                    vIPIDevol = item.ValorTotal
                }
            };
        }

        private II GetProdutoII(ImpostoImportacao item)
        {
            if (!NotaFiscal.IsImportacao)
            {
                return null;
            }

            return new II
            {
                vBC = item.ValorBaseCalculo,
                vDespAdu = item.ValorDespesaAduaneira,
                vII = item.ValorImpostoImportacao,
                vIOF = item.ValorIOF
            };
        }

        private Classes.Informacoes.Detalhe.Tributacao.Federal.COFINS GetProdutoCOFINS(COFINS item)
        {
            return new Classes.Informacoes.Detalhe.Tributacao.Federal.COFINS
            {
                TipoCOFINS = InformarCOFINS(item)
            };
        }

        private COFINSBasico InformarCOFINS(COFINS item)
        {
            CSTCOFINS cstCOFINS = item.CST;

            var vBC = item.BaseCalculo;
            var pCOFINS = item.Aliquota;

            decimal vCOFINS = item.ValorTotal;
            if (vCOFINS < 0.01M)
            {
                vCOFINS = 0.01M;
            }

            switch (cstCOFINS)
            {
                case CSTCOFINS.cofins01:
                case CSTCOFINS.cofins02:
                case CSTCOFINS.cofins50:
                    return new COFINSAliq
                    {
                        CST = cstCOFINS,
                        vBC = vBC,
                        pCOFINS = pCOFINS,
                        vCOFINS = vCOFINS
                    };

                case CSTCOFINS.cofins03:
                    //return new COFINSQtde
                    //{
                    //    CST = cstCOFINS,
                    //    qBCProd = 0M,
                    //    vAliqProd = 0M,
                    //    vCOFINS = 0M
                    //};
                    throw new ArgumentException("CST COFINS 03 - não implementado no sistema");

                case CSTCOFINS.cofins04:
                case CSTCOFINS.cofins05:
                case CSTCOFINS.cofins06:
                case CSTCOFINS.cofins07:
                case CSTCOFINS.cofins08:
                case CSTCOFINS.cofins09:
                    return new COFINSNT
                    {
                        CST = cstCOFINS
                    };

                case CSTCOFINS.cofins49:
                case CSTCOFINS.cofins51:
                case CSTCOFINS.cofins52:
                case CSTCOFINS.cofins53:
                case CSTCOFINS.cofins54:
                case CSTCOFINS.cofins55:
                case CSTCOFINS.cofins56:
                case CSTCOFINS.cofins60:
                case CSTCOFINS.cofins61:
                case CSTCOFINS.cofins62:
                case CSTCOFINS.cofins63:
                case CSTCOFINS.cofins64:
                case CSTCOFINS.cofins65:
                case CSTCOFINS.cofins66:
                case CSTCOFINS.cofins67:
                case CSTCOFINS.cofins70:
                case CSTCOFINS.cofins71:
                case CSTCOFINS.cofins72:
                case CSTCOFINS.cofins73:
                case CSTCOFINS.cofins74:
                case CSTCOFINS.cofins75:
                case CSTCOFINS.cofins98:
                case CSTCOFINS.cofins99:

                    // TODO: -x- S06.1 - Informar os campos S07(vBC) e S08(pCOFINS) para cálculo da COFINS em percentual. 
                    // vBC = null;
                    // pCOFINS = null;
                    // - OU -
                    // TODO: -x- S08.1 - Informar os campos S09(qBCProd) e S10(vAliqProd) para cálculo da COFINS em valor.
                    // vAliqProd = null;

                    vCOFINS = 0;
                    vBC = 0;

                    return new COFINSOutr
                    {
                        CST = cstCOFINS,
                        // -x-
                        vBC = vBC,
                        pCOFINS = pCOFINS,
                        // -x-
                        // qBCProd = qBCProd,
                        // vAliqProd = vAliqProd,

                        vCOFINS = vCOFINS
                    };

                default:
                    throw new ArgumentException("CST COFINS inválido ou não informado no cadastro do produto");
            }
        }

        private Classes.Informacoes.Detalhe.Tributacao.Federal.PIS GetProdutoPIS(Configuracao.Entidades.Produtos.Impostos.PIS item)
        {
            return new Classes.Informacoes.Detalhe.Tributacao.Federal.PIS
            {
                TipoPIS = InformarPIS(item)
            };
        }

        private PISBasico InformarPIS(Configuracao.Entidades.Produtos.Impostos.PIS item)
        {
            CSTPIS cstPIS = item.CST;

            var vBC = item.BaseCalculo;
            var pPIS = item.Aliquota;

            decimal vPIS = item.ValorTotal;
            if (vPIS < 0.01M)
            {
                vPIS = 0.01M;
            }

            switch (cstPIS)
            {
                case CSTPIS.pis01:
                case CSTPIS.pis02:
                case CSTPIS.pis50:
                    return new PISAliq
                    {
                        CST = cstPIS,
                        vBC = vBC,
                        pPIS = pPIS,
                        vPIS = vPIS
                    };

                case CSTPIS.pis03:

                    //return new PISQtde
                    //{
                    //    CST = cstPIS,
                    //    qBCProd = 0M,
                    //    vAliqProd = 0M,
                    //    vPIS  = 0M
                    //};

                    throw new ArgumentException("CST PIS 03 - não implementado no sistema");

                case CSTPIS.pis04:
                case CSTPIS.pis05:
                case CSTPIS.pis06:
                case CSTPIS.pis07:
                case CSTPIS.pis08:
                case CSTPIS.pis09:
                    return new PISNT
                    {
                        CST = cstPIS
                    };

                case CSTPIS.pis49:
                case CSTPIS.pis51:
                case CSTPIS.pis52:
                case CSTPIS.pis53:
                case CSTPIS.pis54:
                case CSTPIS.pis55:
                case CSTPIS.pis56:
                case CSTPIS.pis60:
                case CSTPIS.pis61:
                case CSTPIS.pis62:
                case CSTPIS.pis63:
                case CSTPIS.pis64:
                case CSTPIS.pis65:
                case CSTPIS.pis66:
                case CSTPIS.pis67:
                case CSTPIS.pis70:
                case CSTPIS.pis71:
                case CSTPIS.pis72:
                case CSTPIS.pis73:
                case CSTPIS.pis74:
                case CSTPIS.pis75:
                case CSTPIS.pis98:
                case CSTPIS.pis99:

                    // TODO: -x- Q06.1 - Informar os campos Q07 (vBC) e Q08 (pPIS) se o cálculo do PIS em percentual.
                    //vBC = null;
                    //pPIS = null;
                    // - OU -
                    // TODO: -x- Q08.1 - Informar os campos Q10 (qBCProd) e Q11(vAliqProd) se o cálculo do PIS for em valor.
                    // vAliqProd = null;

                    vPIS = 0;
                    vBC = 0;

                    return new PISOutr
                    {
                        CST = cstPIS,
                        // -x-
                        vBC = vBC,
                        pPIS = pPIS,
                        // -x-
                        // qBCProd = qBCProd,
                        // vAliqProd = vAliqProd,

                        vPIS = vPIS
                    };

                default:
                    throw new ArgumentException("CST PIS inválido ou não informado no cadastro do produto");
            }
        }

        private Classes.Informacoes.Detalhe.Tributacao.Federal.IPI GetProdutoIPI(IPI item)
        {
            //Quando não há CST de IPI
            //então realmente não se incluirá esta chave
            if (item == null)
            {
                return null;
            }

            return new Classes.Informacoes.Detalhe.Tributacao.Federal.IPI()
            {
                cEnq = item.CodigoEnquadramento,
                TipoIPI = InformarIPI(item)
            };
        }

        private IPIBasico InformarIPI(IPI item)
        {
            CSTIPI cstIPI = item.CST;
            decimal vBC = 0;
            decimal pIPI = 0;
            decimal vIPI = 0;

            if (NotaFiscal.EFinalidadeNFe != FinalidadeNFe.fnDevolucao)
            {
                vBC = item.BaseCalculo;
                pIPI = item.Aliquota;
                vIPI = item.ValorTotal;
            }

            switch (cstIPI)
            {
                case CSTIPI.ipi00:
                case CSTIPI.ipi49:
                case CSTIPI.ipi50:
                case CSTIPI.ipi99:
                    return new IPITrib()
                    {
                        CST = cstIPI, // Preencher com o valor selecionado pelo switch
                        vBC = vBC,
                        pIPI = pIPI,
                        vIPI = vIPI
                    };

                case CSTIPI.ipi01:
                case CSTIPI.ipi02:
                case CSTIPI.ipi03:
                case CSTIPI.ipi04:
                case CSTIPI.ipi51:
                case CSTIPI.ipi52:
                case CSTIPI.ipi53:
                case CSTIPI.ipi54:
                case CSTIPI.ipi55:
                case CSTIPI.ipi05:
                    return new IPINT()
                    {
                        CST = cstIPI
                    };

                default:
                    throw new ArgumentException("CST IPI inválido ou não informado no cadastro do produto");
            }
        }

        private ICMSUFDest GetProdutoIcmsUfDest(PartilhaICMS item)
        {
            if (item == null
                || item.AliquotaICMSPartilha == 0
                || NotaFiscal.IsExportacao
                || NotaFiscal.ETipoNFe == TipoNFe.tnEntrada) //Exportação
            {
                return null;
            }

            return new ICMSUFDest
            {
                vBCUFDest = item.BaseCalculoICMSDestino,
                vBCFCPUFDest = item.BaseCalculoFCP == 0 && item.ValorFCP > 0 ? item.BaseCalculoICMSDestino : item.BaseCalculoFCP, // TODO: Valor da Base de Cálculo do FCP na UF de destino
                pFCPUFDest = item.AliquotaFCP,
                pICMSUFDest = item.AliquotaInternaUFDestino,
                pICMSInter = item.AliquotaInterestadual,
                pICMSInterPart = item.AliquotaICMSPartilha,
                vFCPUFDest = item.ValorFCP,
                vICMSUFDest = item.ValorICMSUFDestino,
                vICMSUFRemet = item.ValorICMSUFOrigem
            };
        }

        private string GetProdutoIndAdProd(Produto item)
        {
            //TODO::
            StringBuilder sbInfAdProd = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(item.DadosAdicionais))
            {
                sbInfAdProd.AppendLine(item.DadosAdicionais + "; ");
            }

            if (NotaFiscal.Emitente.HabilitarDetalhamentoImposto
                && item.Impostos.TributosIBPT?.ValorNacional > 0
                && (_cfgApp.CfgServico.ModeloDocumento == ModeloDocumento.NFCe
                || NotaFiscal.Destinatario?.EConsumidorFinal == ConsumidorFinal.cfConsumidorFinal))
            {
                sbInfAdProd.Append("Valor aproximado dos tributos: ");

                sbInfAdProd.AppendFormat("R$ {0:##,##0.00} ({1:0.##}%) Federal ",
                    item.Impostos.TributosIBPT.ValorNacional,
                    item.Impostos.TributosIBPT.AliquotaNacional);

                sbInfAdProd.AppendFormat("R$ {0:##,##0.00} ({1:0.##}%) Estadual ",
                    item.Impostos.TributosIBPT.ValorEstadual,
                    item.Impostos.TributosIBPT.AliquotaEstadual);

                sbInfAdProd.Append("R$ 0,00 (0%) Municipal. Fonte:IBPT; ");
            }

            if (NotaFiscal.IsZonaFrancaManaus
                && item.Impostos.ICMS.ValorICMSDesonerado > 0)
            {
                sbInfAdProd.AppendFormat("Valor do ICMS abatido: {0:##,##0.00} ",
                    item.Impostos.ICMS.ValorICMSDesonerado);

                //sbInfAdProd.AppendFormat("({0:##,##0.00}% sobre R$ {1:##,##0.00}); ",
                //    item.NfProd_ICMSDESONERADOALIQ,
                //    (item.ValorTotal - item.Desconto));

                if (item.Desconto > 0)
                {
                    sbInfAdProd.AppendFormat("Valor do desconto comercial: R$ {0:##,##0.00}; ",
                        item.Desconto);
                }
            }

            if (item.Impostos.ICMS.BaseCalculoST > 0
                && item.Impostos.ICMS.AliquotaFCP.GetValueOrDefault() > 0)
            {
                sbInfAdProd.AppendFormat("Base FCP ST = {0:##,##0.00} / Percentual FCP ST = {1:##,##0.00} / Valor FCP ST = {2:##,##0.00##}; ",
                    item.Impostos.ICMS.BaseCalculoST,
                    item.Impostos.ICMS.AliquotaFCP.GetValueOrDefault(),
                    item.Impostos.ICMS.ValorTotalFCP.GetValueOrDefault());
            }

            string strInfAdProd = sbInfAdProd
                .ToString()
                .Trim()
                .SanitizeString();

            return string.IsNullOrEmpty(strInfAdProd) ?
                null :
                strInfAdProd;
        }

        private List<Classes.Informacoes.Detalhe.DeclaracaoImportacao.DI> GetProdutoDI(DeclaracaoImportacao item)
        {
            if (!NotaFiscal.IsImportacao)
            {
                return null;
            }

            //if (item.NfProd_DDESEMB.ToString("dd/MM/yyyy") == "01/01/0001")
            //    throw new ArgumentException(string.Format("Data do Desembaraço Aduaneiro inválida ou não informada para o produto:{0}{0}({1}) {2}",
            //        Environment.NewLine,
            //        item.NfProd_REFERENCIA,
            //        item.NfProd_PRODUTODESCRICAO));

            return new List<Classes.Informacoes.Detalhe.DeclaracaoImportacao.DI>
            {
                new Classes.Informacoes.Detalhe.DeclaracaoImportacao.DI
                {
                    nDI = item.NumeroDI,
                    dDI = item.DataRegistroDI,
                    xLocDesemb = item.LocalDesembaraco,
                    UFDesemb = item.UFDesembaraco.GetSiglaUfString(),
                    dDesemb = item.DataDesembaraco,
                    tpViaTransp = item.ETipoTransporteInternacional,
                    vAFRMM = item.ValorAFRMM,
                    tpIntermedio = item.ETipoIntermediacao,
                    cExportador = item.CodigoExportador,
                    adi = InformarAdi(item)
                }
            };
        }

        private List<NFe.Classes.Informacoes.Detalhe.DeclaracaoImportacao.adi> InformarAdi(DeclaracaoImportacao item)
        {
            return new List<Classes.Informacoes.Detalhe.DeclaracaoImportacao.adi>
            {
                new Classes.Informacoes.Detalhe.DeclaracaoImportacao.adi
                {
                    nAdicao = item.NumeroAdicao,
                    nSeqAdic = item.SequencialItem,
                    cFabricante = item.CodigoFabricante,
                }
            };
        }

        private static string GetProdutoEAN(string codigoBarras)
        {
            if (!CodigoBarras.IsGtinValido(codigoBarras))
            {
                return "SEM GTIN";
            }

            return codigoBarras;
        }

        private total GetTotal()
        {
            var total = new total 
            { 
                ICMSTot = GetIcmsTotal(),
                retTrib = GetRetTribTotal(),
                IBSCBSTot = GetCbsIbsTotal(NotaFiscal.Total.TotalizadorCBSIBS)
            };

            return total;
        }

        private IBSCBSTot GetCbsIbsTotal(TotalizadorCBSIBS totalizador)
        {
            if(totalizador == null)
            {
                return null;
            }

            return new IBSCBSTot()
            {
                vBCIBSCBS = totalizador?.CBSIBSBaseCalculo ?? 0,
                gCBS = GetTotalizadorCBS(totalizador?.TotalizadorCBS),
                gIBS = GetTotalizadorIBS(totalizador?.TotalizadorIBS),
                gEstornoCred = GetTotalizadorEstornoCredito(totalizador?.TotalizadorCBSIBSEstornoCredito),
                gMono = GetTotalizadorMonofasico(totalizador?.TotalizadorCBSIBSMonofasico)
            };
        }

        private gCBSTotal GetTotalizadorCBS(TotalizadorCBS totalizador)
        {
            if (totalizador == null)
            {
                return null;
            }

            return new gCBSTotal()
            {
                vCBS = totalizador.ValorTotal,
                vDevTrib = totalizador.DevolucaoTributosValorTotal,
                vDif = totalizador.DiferimentoValorTotal,
                vCredPres = totalizador.CreditoPresumidoValorTotal,
                vCredPresCondSus = totalizador.CreditoPresumidoEmCondicaoSuspensivaValorTotal
            };
        }

        private gIBSUFTotal GetTotalizadorIBSUF(TotalizadorIBSUF totalizador)
        {
            if (totalizador == null)
            {
                return null;
            }

            return new gIBSUFTotal()
            {
                vIBSUF = totalizador.ValorTotal,
                vDevTrib = totalizador.DevolucaoTributosValorTotal,
                vDif = totalizador.DiferimentoValorTotal,
            };
        }

        private gIBSMunTotal GetTotalizadorIBSMunicipio(TotalizadorIBSMunicipio totalizador)
        {
            if (totalizador == null)
            {
                return null;
            }

            return new gIBSMunTotal()
            {
                vIBSMun = totalizador.ValorTotal,
                vDevTrib = totalizador.DevolucaoTributosValorTotal,
                vDif = totalizador.DiferimentoValorTotal,
            };
        }

        private gIBSTotal GetTotalizadorIBS(TotalizadorIBS totalizador)
        {
            if (totalizador == null)
            {
                return null;
            }

            return new gIBSTotal()
            {
                vIBS = totalizador.ValorTotal,
                gIBSUF = GetTotalizadorIBSUF(totalizador.TotalizadorIBSUF),
                gIBSMun = GetTotalizadorIBSMunicipio(totalizador.TotalizadorIBSMunicipio),
                vCredPres = totalizador.CreditoPresumidoValorTotal,
                vCredPresCondSus = totalizador.CreditoPresumidoEmCondicaoSuspensivaValorTotal
            };
        }

        private gEstornoCredTotal GetTotalizadorEstornoCredito(TotalizadorCBSIBSEstornoCredito totalizador)
        {
            if (totalizador == null)
            {
                return null;
            }

            return new gEstornoCredTotal()
            {
                vCBSEstCred = totalizador.CBSValorEstornado,
                vIBSEstCred = totalizador.IBSValorEstornado
            };
        }

        private gMonoTotal GetTotalizadorMonofasico(TotalizadorCBSIBSMonofasico totalizador)
        {
            if (totalizador == null)
            {
                return null;
            }

            return new gMonoTotal()
            {
                vCBSMono = totalizador.CBSMonofasicoValorTotal,
                vIBSMono = totalizador.IBSMonofasicoValorTotal,
                vCBSMonoReten = totalizador.CBSMonofasicoSujeitoRetencaoValorTotal,
                vIBSMonoReten = totalizador.IBSMonofasicoSujeitoRetencaoValorTotal,
                vCBSMonoRet = totalizador.CBSMonofasicoRetidoValorTotal,
                vIBSMonoRet = totalizador.IBSMonofasicoRetidoValorTotal,
            };
        }

        private retTrib GetRetTribTotal()
        {
            if(NotaFiscal.Total.TotalizadorICMS.ValorIRRetido == 0)
            {
                return null;
            }

            return new retTrib
            {
                vBCIRRF = NotaFiscal.Total.TotalizadorICMS.ProdutosTotal,
                vIRRF = NotaFiscal.Total.TotalizadorICMS.ValorIRRetido
            };
        }

        private ICMSTot GetIcmsTotal()
        {
            var vICMSBC = NotaFiscal.Total.TotalizadorICMS.ICMSBaseCalculo;
            var vICMS = NotaFiscal.Total.TotalizadorICMS.ICMSTotal;
            var vICMSDeson = NotaFiscal.Total.TotalizadorICMS.ICMSDesonerado;
            var vBCST = NotaFiscal.Total.TotalizadorICMS.ICMSSTBaseCalculo;
            var vST = NotaFiscal.Total.TotalizadorICMS.ICMSSTTotal;
            var vFrete = NotaFiscal.Total.TotalizadorICMS.Frete;
            var vProd = NotaFiscal.Total.TotalizadorICMS.ProdutosTotal;
            var vSeg = NotaFiscal.Total.TotalizadorICMS.Seguro;
            var vDesc = NotaFiscal.Total.TotalizadorICMS.Desconto + NotaFiscal.Total.TotalizadorICMS.ICMSDesonerado;
            var vII = NotaFiscal.Total.TotalizadorICMS.ImpostoImportacao;
            var vIPI = NotaFiscal.Total.TotalizadorICMS.IPI;
            var vPIS = NotaFiscal.Total.TotalizadorICMS.PIS;
            var vCOFINS = NotaFiscal.Total.TotalizadorICMS.COFINS;
            var vOutro = NotaFiscal.Total.TotalizadorICMS.OutrasDespesasAcessorias;
            var vNF = NotaFiscal.Total.TotalizadorICMS.NFeValorTotal;
            var vTotTrib = NotaFiscal.Total.TotalizadorICMS.TributosIBPT;
            //var vTotTrib = !_notaFiscal.Emitente.HabilitarDetalhamentoImposto
            //    || _notaFiscal.Destinatario.EConsumidorFinal != ConsumidorFinal.cfConsumidorFinal 
            //    ? 0 : _notaFiscal.Total.TotalizadorICMS.TributosIBPT;

            var vICMSUFDest = NotaFiscal.Total.TotalizadorICMS.ICMSUFDestino;
            var vICMSUFRemet = NotaFiscal.Total.TotalizadorICMS.ICMSUFOrigem;
            var vFCPUFDest = NotaFiscal.Total.TotalizadorICMS.FCPUFDestino;


            var vFCP = 0; // TODO: * Valor Total do FCP (Fundo de Combate à Pobreza) - Corresponde ao total da soma dos campos  id:N17c
            var vFCPST = NotaFiscal.Total.TotalizadorICMS.FCPSubstituicaoTributaria; // * Valor Total do FCP (Fundo de Combate à Pobreza) retido por substituição tributária - Corresponde ao total da soma dos campos  id:N23d 
            var vFCPSTRet = 0; // TODO: * Valor Total do FCP retido anteriormente por Substituição Tributária - Corresponde ao total da soma dos campos  id:N27d

            var qBCMonoRet = NotaFiscal.Total.TotalizadorICMS.QuantidadeBaseCalculoMonofasicoRetido;
            var vICMSMonoRet = NotaFiscal.Total.TotalizadorICMS.ValorICMSMonofasicoRetido;

            decimal vIPIDevol = 0;
            if (NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnDevolucao)
            {
                vIPI = 0;
                vIPIDevol = NotaFiscal.Total.TotalizadorICMS.IPI;
            }

            // Os totais acompanham os ITENS. Com os grupos do regime antigo fora do item
            // (B25-80, rejeicao 1001), um vICMS ou vPIS diferente de zero aqui seria total
            // sem item que o componha - e o total tem de bater com a soma dos itens.
            //
            // ZERA, nao omite: o ICMSTot e obrigatorio no leiaute e os campos dele tambem.
            // Os valores do DOCUMENTO ficam: vProd, vFrete, vSeg, vDesc e vOutro nao sao
            // imposto. O vNF e recalculado logo abaixo, porque ele SOMA imposto.
            if (!_levaImpostoDoRegimeAntigo())
            {
                vICMSBC = 0;
                vICMS = 0;
                vICMSDeson = 0;
                vBCST = 0;
                vST = 0;
                vFCPST = 0;
                vICMSUFDest = 0;
                vICMSUFRemet = 0;
                vFCPUFDest = 0;
                vII = 0;
                vIPI = 0;
                vIPIDevol = 0;
                vPIS = 0;
                vCOFINS = 0;
                vTotTrib = 0;
                qBCMonoRet = 0;
                vICMSMonoRet = 0;

                vNF = TotalDaNotaSemImpostoDoRegimeAntigo(vProd, vDesc, vFrete, vSeg, vOutro);
            }

            return new ICMSTot
            {
                vBC = vICMSBC,
                vICMS = vICMS,
                vICMSDeson = vICMSDeson,
                vFCPUFDest = vFCPUFDest,
                vICMSUFDest = vICMSUFDest,
                vICMSUFRemet = vICMSUFRemet,
                vFCP = vFCP,
                vBCST = vBCST,
                vST = vST,
                vFCPST = vFCPST,
                vFCPSTRet = vFCPSTRet,
                vProd = vProd,
                vFrete = vFrete,
                vSeg = vSeg,
                vDesc = vDesc,
                vII = vII,
                vIPI = vIPI,
                vIPIDevol = vIPIDevol,
                vPIS = vPIS,
                vCOFINS = vCOFINS,
                vOutro = vOutro,
                vNF = vNF,
                vTotTrib = vTotTrib,
                qBCMonoRet = qBCMonoRet,
                vICMSMonoRet = vICMSMonoRet
            };
        }

        private cobr GetCobranca()
        {
            if (_cfgApp.CfgServico.ModeloDocumento == ModeloDocumento.NFCe)
            {
                return null;
            }

            // Nota de credito e de debito saem SEM cobranca. O pag delas ja vai com
            // fpSemPagamento e vPag = 0, e o vNF e recalculado sem os impostos do regime
            // antigo: a fatura montada do total do banco declararia valor que a nota nao tem -
            // produto 100 + IPI 10 dava vNF 100, vPag 0 e vLiq 110.
            //
            // Ajuste e devolucao continuam com cobr: o vNF deles nao e recalculado, entao a
            // fatura fecha com o total.
            if (EhNotaDeCreditoOuDebito(NotaFiscal.EFinalidadeNFe))
            {
                return null;
            }

            var vDup = NotaFiscal.Duplicatas?.Sum(duplicata => duplicata.Valor) ?? 0;
            var nFat = NotaFiscal.Numero.ToString();
            var vOrig = vDup > 0 ? vDup : NotaFiscal.Total.TotalizadorICMS.NFeValorTotal;
            var vDesc = 0M;

            var fat = new fat
            {
                nFat = nFat,
                vOrig = vOrig,
                vDesc = vDesc,
                vLiq = vOrig
            };

            return new cobr
            {
                fat = fat,
                dup = InformarParcelas()
            };
        }

        private List<ProdutoEspecifico> GetProdutoCombustivel(Produto item)
        {
            // Combustível
            if (item.DadosCombustivel == null
                || string.IsNullOrEmpty(item.DadosCombustivel.CodigoANP))
            {
                return null;
            }

            var uf = _cfgApp.CfgServico.ModeloDocumento == ModeloDocumento.NFCe
                ? NotaFiscal.Emitente.Pessoa.Endereco.MunicipioEstadoSigla?.GetSiglaUfString()
                : NotaFiscal.Destinatario.Pessoa.Endereco.MunicipioEstadoSigla?.GetSiglaUfString();

                return new List<ProdutoEspecifico>()
            {
                new comb
                {
                    cProdANP = item.DadosCombustivel.CodigoANP,
                    descANP = item.DadosCombustivel.DescricaoANP,
                    pGLP = item.DadosCombustivel.PercentualGLPDerivadoPetroleo == 0 ? null : item.DadosCombustivel.PercentualGLPDerivadoPetroleo,
                    pGNn = item.DadosCombustivel.PercentualGasNaturalNacional == 0 ? null : item.DadosCombustivel.PercentualGasNaturalNacional,
                    pGNi = item.DadosCombustivel.PercentualGasNaturalImportado == 0 ? null : item.DadosCombustivel.PercentualGasNaturalImportado,
                    vPart = item.DadosCombustivel.ValorPartida == 0 ? null : item.DadosCombustivel.ValorPartida,
                    UFCons = uf
                }
            };
        }

        private List<dup> InformarParcelas()
        {
            if (NotaFiscal.Duplicatas == null
                || NotaFiscal.Duplicatas.Count == 0)
            {
                return null;
            }

            //ValidaDuplicadas();

            // TODO: parcelas pagas - com data de pagamento preenchida - retornar como paga

            var listaDuplicadas = new List<dup>();

            int numeroDuplicata = 1;
            foreach (var dup in NotaFiscal.Duplicatas)
            {
                // Obrigatória informação do número de parcelas com 3 algarismos, sequenciais e consecutivos. 
                var nDup = (numeroDuplicata++).ToString("000"); // dup.Codigo; - 

                var dVenc = dup.DataVencimento;
                var vDup = dup.Valor;

                listaDuplicadas.Add(new dup
                {
                    nDup = nDup,
                    dVenc = dVenc,
                    vDup = vDup,
                });
            }

            return listaDuplicadas;
        }

        //private void ValidaDuplicadas()
        //{
        //    if (_notaFiscal.Duplicatas == null
        //       || _notaFiscal.Duplicatas.Count == 0)
        //    {
        //        return;
        //    }

        //    decimal totalDuplicadas = _notaFiscal.Duplicatas.Sum(p => p.Valor);

        //    if ((_notaFiscal.Total.NFeValorTotal) != totalDuplicadas)
        //    {
        //        throw new ArgumentException("Valor Total da NF-e não corresponde com o Somatório Total das duplicatas.");
        //    }

        //    foreach (var dup in _notaFiscal.Duplicatas)
        //    {
        //        if (string.IsNullOrEmpty(dup.Codigo))
        //        {
        //            throw new ArgumentException($"Código da duplicata da NF-e: {_notaFiscal.Numero:000.000.000} é inválido!");
        //        }

        //        if (dup.DataVencimento.ToString("dd/MM/yyyy") == "01/01/0001")
        //        {
        //            throw new ArgumentException(string.Format("Data de Vencimento da Duplicata: {0} é inválida!", dup.Codigo));
        //        }

        //        if (dup.DataVencimento.Date < _dataAtual.Date)
        //        {
        //            throw new ArgumentException(string.Format("Data de Vencimento da Duplicata: {0} é menor do que a Data Atual do Servidor: {1:dd/MM/yyyy}",
        //                dup.Codigo,
        //                _dataAtual.Date));
        //        }

        //        if (dup.Valor == 0)
        //        {
        //            throw new ArgumentException(string.Format("Valor da Duplicata: {0} é inválido!", dup.Codigo));
        //        }
        //    }
        //}

        private List<pag> GetPagamento()
        {
            List<detPag> pagamentos = new List<detPag>();
            decimal? troco = null;

            // Credito e debito entram aqui junto com ajuste e devolucao: sao documentos de
            // acerto, nao tem pagamento. E o vPag precisa ser ZERO, nao o total do banco: o
            // vNF dessas notas e recalculado sem os impostos do regime antigo
            // (TotalDaNotaSemImpostoDoRegimeAntigo), e o ramo de baixo copiaria o total
            // antigo para o vPag, que e justamente a divergencia que se quer evitar.
            if (NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnAjuste
                || NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnDevolucao
                || EhNotaDeCreditoOuDebito(NotaFiscal.EFinalidadeNFe))
            {
                pagamentos.Add(new detPag
                {
                    indPag = null,
                    tPag = FormaPagamento.fpSemPagamento,
                    vPag = 0
                });
            }
            else if (NotaFiscal.FormasPagamento?.Count > 0)
            {
                foreach (var pagamento in NotaFiscal.FormasPagamento)
                {
                    pagamentos.Add(new detPag
                    {
                        indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                        tPag = pagamento.FormaPagamento,
                        vPag = pagamento.Valor,
                        card = GetDadosPagamentoCartao(pagamento.pagamentoIntegracaoCartao),
                        xPag = pagamento.FormaPagamento == FormaPagamento.fpOutro ? "DIRETO COM O CLIENTE" : null
                    });
                }
                var totalPago = NotaFiscal.FormasPagamento.Sum(pag => pag.Valor);
                if (totalPago > NotaFiscal.Total.TotalizadorICMS.NFeValorTotal)
                {
                    troco = totalPago - NotaFiscal.Total.TotalizadorICMS.NFeValorTotal;
                }
            }
            else if (//_cfgApp.CfgServico.ModeloDocumento == ModeloDocumento.NFe &&
                (NotaFiscal.Duplicatas == null
                    || NotaFiscal.Duplicatas.Count == 0)) // Cliente quer que NF-e seja sempre a vista
            {
                pagamentos.Add(new detPag
                {
                    indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                    tPag = FormaPagamento.fpOutro,
                    vPag = NotaFiscal.Total.TotalizadorICMS.NFeValorTotal,
                    xPag = "DIRETO COM O CLIENTE"
                });
            }
            else
            {
                pagamentos.Add(new detPag
                {
                    indPag = IndicadorPagamentoDetalhePagamento.ipDetPgPrazo,
                    //tPag = FormaPagamento.fpSemPagamento,
                    tPag = FormaPagamento.fpPagamentoPosterior,
                    vPag = 0
                });
            }

            return new List<pag>
            {
                new pag
                {
                    detPag = pagamentos,
                    vTroco = troco
                }
            };
        }

        private card GetDadosPagamentoCartao(PagamentoIntegracaoCartao pagamentoIntegracaoCartao)
        {
            if (pagamentoIntegracaoCartao == null)
            {
                return null;
            }

            return new card()
            {
                tpIntegra = pagamentoIntegracaoCartao.ETipoIntegracaoPagamento,
                CNPJ = pagamentoIntegracaoCartao.CNPJCredenciadoraCartao,
                tBand = pagamentoIntegracaoCartao.BandeiraCartao,
                cAut = pagamentoIntegracaoCartao.CodigoAutorizacaoTransacao
            };
        }

        private infAdic GetInfAdic()
        {
            return new infAdic
            {
                infAdFisco = NotaFiscal.DadosAdicionaisFisco,
                infCpl = NotaFiscal.DadosAdicionaisContribuinte
            };
        }

        private exporta GetExporta()
        {
            if (!NotaFiscal.IsExportacao)
            {
                return null;
            }

            var UFSaidaPais = NotaFiscal.Emitente.Pessoa.Endereco.MunicipioEstadoSigla;
            var xLocExporta = NotaFiscal.Emitente.Pessoa.Endereco.MunicipioNome;
            var xLocDespacho = NotaFiscal.Emitente.Pessoa.Endereco.MunicipioNome;

            return new exporta
            {
                UFSaidaPais = UFSaidaPais?.GetSiglaUfString(),
                xLocExporta = xLocExporta,
                xLocDespacho = xLocDespacho
            };
        }

        private infNFeSupl GetNFCeQrCode()
        {
            if (_cfgApp.CfgServico.ModeloDocumento != ModeloDocumento.NFCe)
            {
                return null;
            }

            AssinaNFe();
            var qrCodeDados = new infNFeSupl();
            qrCodeDados.urlChave = qrCodeDados.ObterUrlConsulta(NFeXML, VersaoQrCode.QrCodeVersao2);
            qrCodeDados.qrCode = qrCodeDados.ObterUrlQrCode(NFeXML, VersaoQrCode.QrCodeVersao2, ((ConfiguracaoNFCe)_cfgApp)._configuracaoCsc.CIdToken.ToString(), ((ConfiguracaoNFCe)_cfgApp)._configuracaoCsc.Csc);

            return qrCodeDados;
        }

        private infRespTec GetRespTec()
        {
            return new infRespTec()
            {
                CNPJ = "12566641000197",
                xContato = "PEDRO WILLIAM BAIARDI DE MORAES",
                fone = "1938522979",
                email = "suporte@buildsolutions.com.br"
            };
        }
    }
}
