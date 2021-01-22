using System;
using System.Collections.Generic;
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using GNRE.Classes.Enumerators;

namespace GNRE.BLL.Configuracao.Entidades
{
    public abstract class GNRE
    {
        public VersaoServico Versao { get; set; }

        public Estado UF { get; set; }

        public double NotaFiscalReferencia { get; set; }

        public Emitente Emitente { get; set; }

        public EReceita Receita { get; set; }

        public int? DetalhamentoReceita { get; set; }

        public ETipoDocumento? TipoDocumento { get; set; }

        public string DocumentoNumero { get; set; }

        public int? Produto { get; set; }

        public EPeriodoApuracao? EPeriodoApuracao { get; set; }

        public DateTime? PeriodoReferencia { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal ValorST { get; set; }

        public decimal? ValorFECP { get; set; }

        public string Convenio { get; set; }

        public Destinatario Destinatario { get; set; }

        public List<CampoExtra> camposExtras { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTime DataPagamento { get; set; }
    }
}
