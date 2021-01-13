using GNRE.Classes.Informacoes.Emitente;

namespace GNRE.Classes.ValueObjects
{
    public abstract class Pessoa
    {
        public PessoaIdentificacao identificacao { get; set; }

        public string razaoSocial { get; set; }
    }
}
