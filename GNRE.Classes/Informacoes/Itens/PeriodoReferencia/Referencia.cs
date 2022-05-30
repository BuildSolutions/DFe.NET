namespace GNRE.Classes.Informacoes.Itens.PeriodoReferencia
{
    public class Referencia
    {
        public Referencia( string mes, int? ano, string periodo = null, string parcela = null)
        {
            this.periodo = periodo;
            this.mes = mes;
            this.ano = ano;
            this.parcela = parcela;
        }

        internal Referencia()
        {

        }

        public string periodo { get; set; }

        public string mes { get; set; }

        public int? ano { get; set; }

        public string parcela { get; set; }

        public bool ShouldSerializeano()
        {
            return ano.HasValue;
        }
    }
}
