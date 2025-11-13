using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFe.BLL.Configuracao.Entidades
{
    public class EnderecoEntrega
    {
        public EnderecoEntrega(
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
            RGIE = rgie.SanitizeString();
            Fone = fone.SanitizeString();
            Email = email.SanitizeString();
        }

        public EnderecoEntrega(entrega entrega)
        {
            if (entrega != null)
            {
                Endereco = new Endereco(entrega);

                Nome = entrega.xNome;
                CPFCNPJ = !string.IsNullOrEmpty(entrega.CNPJ) ? entrega.CNPJ : entrega.CPF;
                RGIE = entrega.IE.SanitizeString();
                Fone = entrega.fone.SanitizeString();
                Email = entrega.email.SanitizeString();
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
