namespace GNRE.Classes.Informacoes.Itens.PeriodoReferencia
{
    public class Referencia
    {
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
