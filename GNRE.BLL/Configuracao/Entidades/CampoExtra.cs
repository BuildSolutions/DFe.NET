namespace GNRE.BLL.Configuracao.Entidades
{
    public class CampoExtra
    {
        public CampoExtra(int codigo, string descricao, string valor)
        {
            _codigo = codigo;
            Descricao = descricao;
            Valor = valor;
        }

        private int _codigo;
        public string Codigo
        { 
            get => _codigo.ToString().PadLeft(2, '0');
            set { int.TryParse(value, out int intAux); _codigo = intAux; } 
        }

        public string Descricao { get; set; }

        public string Valor { get; set; }
    }
}
