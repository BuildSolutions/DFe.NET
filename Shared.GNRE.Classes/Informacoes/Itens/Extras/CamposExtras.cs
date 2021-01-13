using GNRE.Classes.Enumerators;

namespace GNRE.Classes.Itens.Extras
{
    public class CamposExtras
    {
        public CampoExtra campoExtra { get; set; }
    }

    public class CampoExtra
    {
        public ECampoExtra codigo { get; set; }

        public string valor { get; set; }
    }
}
