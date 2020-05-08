using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using NFe.Classes.Informacoes.Detalhe;
using System;

namespace NFe.BLL.Configuracao.Entidades.Produtos
{
    public class DeclaracaoImportacao
    {
        public DeclaracaoImportacao(string numeroDI,
            DateTime dataRegistroDI,
            string localDesembaraco,
            DateTime dataDesembaraco,
            Estado uFDesembaraco,
            TipoTransporteInternacional eTipoTransporteInternacional,
            decimal? valorAFRMM,
            string codigoExportador,
            int numeroAdicao,
            int sequencialItem,
            string codigoFabricante)
        {
            NumeroDI = numeroDI;
            DataRegistroDI = dataRegistroDI;
            LocalDesembaraco = localDesembaraco.SanitizeString();
            DataDesembaraco = dataDesembaraco;
            UFDesembaraco = uFDesembaraco;
            ETipoTransporteInternacional = eTipoTransporteInternacional;
            ValorAFRMM = valorAFRMM;
            ETipoIntermediacao = TipoIntermediacao.ContaPropria;
            CodigoExportador = codigoExportador.SanitizeString();
            NumeroAdicao = numeroAdicao;
            SequencialItem = sequencialItem;
            CodigoFabricante = codigoFabricante.SanitizeString();
        }

        public string NumeroDI { get; }

        public DateTime DataRegistroDI { get; }

        public string LocalDesembaraco { get; }

        public DateTime DataDesembaraco { get; }

        public Estado UFDesembaraco { get; }

        public TipoTransporteInternacional ETipoTransporteInternacional { get; }

        public decimal? ValorAFRMM { get; }

        public TipoIntermediacao ETipoIntermediacao { get; }

        public string CodigoExportador { get; }

        public int NumeroAdicao { get; set; }

        public int SequencialItem { get; set; }

        public string CodigoFabricante { get; set; }
    }
}
