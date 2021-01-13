using GNRE.Classes.ValueObjects;

namespace GNRE.Classes.Informacoes.Emitente
{
    public class ContribuinteEmitente : Pessoa
    {
        public string endereco { get; set; }

        public string municipio { get; set; }

        public string uf { get; set; }

        public string cep { get; set; }

        public string telefone { get; set; }
    }
}
