using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFe.BLL.Configuracao.Entidades
{
    public class EnderecoRetirada
    {
        public EnderecoRetirada(
            Endereco endereco,
            string nome,
            string cpfcnpj,
            string rgie,
            string fone,
            string email)
        {
            Endereco = endereco;
            Nome = nome;
            CPFCNPJ = cpfcnpj;
            RGIE = rgie;
            Fone = fone;
            Email = email;
        }

        public EnderecoRetirada(retirada retirada)
        {
            if (retirada != null)
            {
                Endereco = new Endereco(retirada);

                Nome = retirada.xNome;
                CPFCNPJ = !string.IsNullOrEmpty(retirada.CNPJ) ? retirada.CNPJ : retirada.CPF;
                RGIE = retirada.IE;
                Fone = retirada.fone;
                Email = retirada.email;
            }
        }

        public Endereco Endereco { get; }
        public string Nome { get; }
        public string CPFCNPJ { get; }
        public string RGIE { get; }
        public string Fone { get; }
        public string Email { get; }

    }
}
