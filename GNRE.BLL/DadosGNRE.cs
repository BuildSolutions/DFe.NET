using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFe.Classes.Extensoes;
using DFe.Utils.Extensoes;
using GNRE.BLL.Validators;
using GNRE.Classes;
using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Dados;
using GNRE.Classes.Informacoes.Destinatario;
using GNRE.Classes.Informacoes.Emitente;
using GNRE.Classes.Informacoes.Itens;
using GNRE.Classes.Informacoes.Itens.PeriodoReferencia;
using GNRE.Classes.Itens.Extras;
using GNRE.Classes.Servicos.Recepcao;
using GNRE.Utils.Recepcao;

namespace GNRE.BLL
{
    public class DadosGNRE
    {
        private List<Configuracao.Entidades.GNRE> _gnres;
        public IReadOnlyList<Configuracao.Entidades.GNRE> GNREs { get => _gnres; }
        public TLote_GNRE LoteGNRE { get; private set; }

        protected readonly ConfiguracaoApp _cfgApp;

        public DadosGNRE(List<Configuracao.Entidades.GNRE> gnres)
        {
            _cfgApp = new ConfiguracaoApp(gnres[0].Emitente);

            _gnres =gnres;
            GetDadosGNRE();
        }

        public DadosGNRE(List<Configuracao.Entidades.GNRE> gnres, ConfiguracaoApp cfgApp)
        {
            _cfgApp = cfgApp;
            _gnres = gnres;
            GetDadosGNRE();
        }

        public TLote_GNRE ValidaNFeXml(out string erro)
        {
            erro = string.Empty;
            try
            {
                return LoteGNRE.Valida(_cfgApp.CfgServico);
            }
            catch (Exception ex)
            {
                erro = ex.ToString();
            }

            return null;
        }

        public bool ValidarDadosGNRE(out string errors)
        {
            errors = string.Empty;
            string erroValidacao = string.Empty;
            var validator = new GNREValidator();

            bool hasError = false;
            var sbError = new StringBuilder();
            foreach (var gnre in _gnres)
            {
                var resultado = validator.Validate(gnre);

                if (!resultado.IsValid)
                {
                    if (sbError?.Length > 0)
                    {
                        sbError.AppendLine();
                    }

                    if (gnre.NotaFiscalReferencia > 0)
                    {
                        sbError.AppendLine($"GNRE referente a NFe: {gnre.NotaFiscalReferencia}");
                    }

                    sbError.AppendLine(string.Join("\r\n", resultado.Errors.Select(err => err.ErrorMessage)));
                    errors = sbError.ToString();
                    hasError = true;
                }
            }

            return !hasError;
        }

        private List<TDadosGNRE> GetDadosGNRE()
        {
            var guiasGNRE = new List<TDadosGNRE>();

            foreach (var gnre in GNREs)
            {
                guiasGNRE.Add(new TDadosGNRE(versao: _cfgApp.CfgServico.VersaoLayout.GetVersaoString(),
                    ufFavorecida: gnre.UF,
                    tipoGnre: ETipoGNRE.GuiaSimples,
                    contribuinteEmitente: GetEmitente(),
                    itensGNRE: GetItemsGNRE(gnre),
                    valorGNRE: gnre.ValorTotal,
                    dataPagamento: gnre.DataPagamento)
                );
            }

            LoteGNRE = new TLote_GNRE(new GuiasGNRE(guiasGNRE));

            return guiasGNRE;
        }

        private ItensGNRE GetItemsGNRE(Configuracao.Entidades.GNRE guiaGNRE)
        {
            return new ItensGNRE()
            {
                item = GetItemGNRE(guiaGNRE)
            };
        }

        private Item GetItemGNRE(Configuracao.Entidades.GNRE guiaGNRE)
        {
            return new Item(
                receita: guiaGNRE.Receita,
                contribuinteDestinatario: GetDestinatario(guiaGNRE),
                documentoOrigem: GetDocumentoOrigem(guiaGNRE),
                detalhamentoReceita: guiaGNRE.DetalhamentoReceita,
                produto: guiaGNRE.Produto,
                referencia: GetPeriodoApuracao(guiaGNRE),
                dataVencimento: guiaGNRE.DataVencimento,
                valor: GetValor(guiaGNRE),
                convenio: guiaGNRE.Convenio,
                camposExtras: GetCamposExtras(guiaGNRE));
        }

        private List<Valor> GetValor(Configuracao.Entidades.GNRE guiaGNRE)
        {
            var valoresGNRE = new List<Valor>();

            if(guiaGNRE.ValorST > 0)
            {
                valoresGNRE.Add(new Valor(ETipoValor.PrincipalICMS, guiaGNRE.ValorST.ParaDecimalString()));
            }

            if (guiaGNRE.ValorFECP > 0)
            {
                valoresGNRE.Add(new Valor(ETipoValor.PrincipalFECP, guiaGNRE.ValorFECP.ParaDecimalString()));
            }

            return valoresGNRE;
        }

        private List<CamposExtras> GetCamposExtras(Configuracao.Entidades.GNRE guiaGNRE)
        {
            if(guiaGNRE.camposExtras == null
                || guiaGNRE.camposExtras.Count == 0)
            {
                return null;
            }

            var camposExtras = new List<CamposExtras>();
            foreach (var campoExtra in guiaGNRE.camposExtras)
            {
                camposExtras.Add(new CamposExtras( new CampoExtra(campoExtra.Codigo, campoExtra.Valor)));
            }

            return camposExtras;
        }

        private Referencia GetPeriodoApuracao(Configuracao.Entidades.GNRE guiaGNRE)
        {
            if(!guiaGNRE.PeriodoReferencia.HasValue)
            {
                return null;
            }

            return new Referencia(guiaGNRE.PeriodoReferencia.Value.Month.ToString("00"), guiaGNRE.PeriodoReferencia.Value.Year, guiaGNRE.EPeriodoApuracao.HasValue ? guiaGNRE.EPeriodoApuracao.ToString() : null);
        }

        private DocumentoOrigem GetDocumentoOrigem(Configuracao.Entidades.GNRE guiaGNRE)
        {
            if (!guiaGNRE.TipoDocumento.HasValue)
            {
                return null;
            }

            return new DocumentoOrigem(guiaGNRE.TipoDocumento.GetValueOrDefault(), guiaGNRE.DocumentoNumero);
        }

        private ContribuinteDestinatario GetDestinatario(Configuracao.Entidades.GNRE guiaGNRE)
        {
            if(guiaGNRE.Destinatario == null)
            {
                return null;
            }

            var identificacaoDestinatario = new PessoaIdentificacao
            {
                IE = guiaGNRE.Destinatario.ETipoDocumento == Enums.ETipoDocumentoPessoa.InscricaoEstadual ? guiaGNRE.Destinatario.Documento : null,
                CNPJ = guiaGNRE.Destinatario.ETipoDocumento == Enums.ETipoDocumentoPessoa.CNPJ ? guiaGNRE.Destinatario.Documento : null,
                CPF = guiaGNRE.Destinatario.ETipoDocumento == Enums.ETipoDocumentoPessoa.CPF ? guiaGNRE.Destinatario.Documento : null
            };

            return new ContribuinteDestinatario()
            {
                razaoSocial = guiaGNRE.Destinatario.NomeRazaoSocial,
                identificacao = identificacaoDestinatario,
                municipio = guiaGNRE.Destinatario.MunicipioCodigoIBGE
            };
        }

        private ContribuinteEmitente GetEmitente()
        {
            return new ContribuinteEmitente
            {
                razaoSocial = _cfgApp.Emitente.Pessoa.NomeRazaoSocial,
                identificacao = new PessoaIdentificacao() { CNPJ = _cfgApp.Emitente.Pessoa.Documento},
                cep = _cfgApp.Emitente.Pessoa.Endereco.CEP,
                endereco = _cfgApp.Emitente.Pessoa.Endereco.EnderecoCompleto,
                municipio = _cfgApp.Emitente.Pessoa.Endereco.MunicipioCodigoIBGE.ToString(),
                uf = _cfgApp.Emitente.Pessoa.Endereco.MunicipioEstadoSigla,
                telefone = _cfgApp.Emitente.Pessoa.Telefone.NuloSeZero()
            };
        }
    }
}
