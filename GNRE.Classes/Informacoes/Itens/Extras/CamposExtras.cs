namespace GNRE.Classes.Itens.Extras
{
    public class CamposExtras
    {
        public CamposExtras(CampoExtra campoExtra)
        {
            this.campoExtra = campoExtra;
        }

        internal CamposExtras()
        {

        }

        public CampoExtra campoExtra { get; set; }
    }

    public class CampoExtra
    {
        public CampoExtra(string codigo, string valor)
        {
            this.codigo = codigo;
            this.valor = valor;
        }

        internal CampoExtra()
        {

        }

        public string codigo { get; set; }

        public string valor { get; set; }
    }
}
