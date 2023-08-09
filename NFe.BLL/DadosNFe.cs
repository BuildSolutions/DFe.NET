using DFe.Classes.Extensoes;
using DFe.Classes.Flags;
using DFe.Utils;
using DFe.Utils.Extensoes;
using FluentValidation;
using FluentValidation.Internal;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Configuracao.Entidades.Produtos;
using NFe.BLL.Configuracao.Entidades.Produtos.Impostos;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var assinado = NFeXML.Assina();
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

            infNFe.det = GetProdutos();
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
            if(NotaFiscal.Emitente.AutorizadosXmlDocumentos == null
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
            if(!NotaFiscal.EIndicadorIntermediador.EValido()
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
                NFref = GetNFRef()
            };

            if(indIntermed.EValido())
            {
                ide.indIntermed = indIntermed;
            }

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

        private List<NFref> GetNFRef()
        {
            if (NotaFiscal.DocumentosReferenciados == null
                || NotaFiscal.DocumentosReferenciados.Count == 0)
            {
                return new List<NFref>();
            }

            var lstNFref = new List<NFref>();

            foreach (var item in NotaFiscal.DocumentosReferenciados)
            {
                lstNFref.Add(new NFref
                {
                    refNFe = item.TipoDocumento == EDocumentoReferenciado.NFe ? ((DocumentoReferenciadoNFe)item).ChaveAcesso : null,
                    refNF = item.TipoDocumento == EDocumentoReferenciado.NF ? GetNotaFiscalReferenciada((DocumentoReferenciadoNF)item) : null,
                    refECF = item.TipoDocumento == EDocumentoReferenciado.ECF ? GetCupomFiscalReferenciado((DocumentoReferenciadoECF)item) : null
                });
            }

            return lstNFref;
        }

        private refNF GetNotaFiscalReferenciada(DocumentoReferenciadoNF notaFiscalReferenciada)
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

        private refECF GetCupomFiscalReferenciado(DocumentoReferenciadoECF cupomFiscalReferenciada)
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
                impostoDevol = GetProdutoImpostoDevolucao(item.Impostos.IPI),
                infAdProd = GetProdutoIndAdProd(item),
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

        private imposto GetProdutoImposto(Produto item)
        {
            return new imposto
            {
                vTotTrib = GetImpostoVTotTrib(item),
                ICMS = GetProdutoICMS(item.Impostos.ICMS),
                ICMSUFDest = GetProdutoIcmsUfDest(item.Impostos.PartilhaICMS),
                IPI = GetProdutoIPI(item.Impostos.IPI),
                II = GetProdutoII(item.Impostos.ImpostoImportacao),
                PIS = GetProdutoPIS(item.Impostos.PIS),
                COFINS = GetProdutoCOFINS(item.Impostos.COFINS)
            };
        }

        private ICMS GetProdutoICMS(Configuracao.Entidades.Produtos.Impostos.ICMS.ICMS item)
        {
            return new ICMS
            {
                TipoICMS =
                    NotaFiscal.Emitente.ECRT == CRT.SimplesNacional 
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

            var pRedBCST = item.AliquotaReducaoBaseCalculoST;

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
                        pRedBC = null, // TODO: 
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

                    //default:
                    //    throw new ArgumentException(string.Format("CSOSN inválido ou não informado no cadastro do produto:{0}{0}({1}) {2}",
                    //        Environment.NewLine,
                    //        item.NfProd_REFERENCIA,
                    //        item.NfProd_PRODUTODESCRICAO));
            }

            return null;
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
                        
                        modBCST = (DeterminacaoBaseIcmsSt)modBCST,
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
                        qBCMonoRet = null,
                        vICMSMonoRet = 0,
                        adRemICMSRet = 0,
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
                    // TODO: 
                    return null;

                    //default:
                    //    throw new ArgumentException(string.Format("CSOSN inválido ou não informado no cadastro do produto:{0}{0}({1}) {2}",
                    //        Environment.NewLine,
                    //        item.NfProd_REFERENCIA,
                    //        item.NfProd_PRODUTODESCRICAO));
            }

            return null;
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

            if(!string.IsNullOrWhiteSpace(item.DadosAdicionais))
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
            if(!CodigoBarras.IsGtinValido(codigoBarras))
            {
                return "SEM GTIN";
            }

            return codigoBarras;
        }

        private total GetTotal()
        {
            var vICMSBC = NotaFiscal.Total.ICMSBaseCalculo;
            var vICMS = NotaFiscal.Total.ICMSTotal;
            var vICMSDeson = NotaFiscal.Total.ICMSDesonerado;
            var vBCST = NotaFiscal.Total.ICMSSTBaseCalculo;
            var vST = NotaFiscal.Total.ICMSSTTotal;
            var vProd = NotaFiscal.Total.ProdutosTotal;
            var vFrete = NotaFiscal.Total.Frete;
            var vSeg = NotaFiscal.Total.Seguro;
            var vDesc = NotaFiscal.Total.Desconto + NotaFiscal.Total.ICMSDesonerado;
            var vII = NotaFiscal.Total.ImpostoImportacao;
            var vIPI = NotaFiscal.Total.IPI;
            var vPIS = NotaFiscal.Total.PIS;
            var vCOFINS = NotaFiscal.Total.COFINS;
            var vOutro = NotaFiscal.Total.OutrasDespesasAcessorias;
            var vNF = NotaFiscal.Total.NFeValorTotal;
            var vTotTrib = NotaFiscal.Total.TributosIBPT;
            //var vTotTrib = !_notaFiscal.Emitente.HabilitarDetalhamentoImposto
            //    || _notaFiscal.Destinatario.EConsumidorFinal != ConsumidorFinal.cfConsumidorFinal 
            //    ? 0 : _notaFiscal.Total.TributosIBPT;

            var vICMSUFDest = NotaFiscal.Total.ICMSUFDestino;
            var vICMSUFRemet = NotaFiscal.Total.ICMSUFOrigem;
            var vFCPUFDest = NotaFiscal.Total.FCPUFDestino;


            var vFCP = 0; // TODO: * Valor Total do FCP (Fundo de Combate à Pobreza) - Corresponde ao total da soma dos campos  id:N17c
            var vFCPST = NotaFiscal.Total.FCPSubstituicaoTributaria; // * Valor Total do FCP (Fundo de Combate à Pobreza) retido por substituição tributária - Corresponde ao total da soma dos campos  id:N23d 
            var vFCPSTRet = 0; // TODO: * Valor Total do FCP retido anteriormente por Substituição Tributária - Corresponde ao total da soma dos campos  id:N27d

            decimal vIPIDevol = 0;
            if (NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnDevolucao)
            {
                vIPI = 0;
                vIPIDevol = NotaFiscal.Total.IPI;
            }

            var icmsTot = new ICMSTot
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
                vTotTrib = vTotTrib
            };

            return new total { ICMSTot = icmsTot };
        }

        private cobr GetCobranca()
        {
            if (_cfgApp.CfgServico.ModeloDocumento == ModeloDocumento.NFCe)
            {
                return null;
            }

            var vDup = NotaFiscal.Duplicatas?.Sum(duplicata => duplicata.Valor) ?? 0;
            var nFat = NotaFiscal.Numero.ToString();
            var vOrig = vDup > 0 ? vDup : NotaFiscal.Total.NFeValorTotal;
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
                    UFCons = NotaFiscal.Destinatario.Pessoa.Endereco.MunicipioEstadoSigla?.GetSiglaUfString()
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

            if (NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnAjuste
                || NotaFiscal.EFinalidadeNFe == FinalidadeNFe.fnDevolucao)
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
                if (totalPago > NotaFiscal.Total.NFeValorTotal)
                {
                    troco = totalPago - NotaFiscal.Total.NFeValorTotal;
                }
            }
            else if (NotaFiscal.Duplicatas == null
               || NotaFiscal.Duplicatas.Count == 0) // Cliente quer que NF-e seja sempre a vista
            {
                pagamentos.Add(new detPag
                {
                    indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                    tPag = FormaPagamento.fpOutro,
                    vPag = NotaFiscal.Total.NFeValorTotal,
                    xPag = "COMBINADO COM O CLIENTE"
                });
            }
            else
            {
                pagamentos.Add(new detPag
                {
                    indPag = IndicadorPagamentoDetalhePagamento.ipDetPgPrazo,
                    tPag = FormaPagamento.fpSemPagamento,
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
            if(pagamentoIntegracaoCartao == null)
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
