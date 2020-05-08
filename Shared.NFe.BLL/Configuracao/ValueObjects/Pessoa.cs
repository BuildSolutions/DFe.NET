using DFe.Utils.Extensoes;
using NFe.BLL.Enums;
using System;

namespace NFe.BLL.Configuracao.ValueObjects
{
    public class Pessoa
    {
        public Pessoa(
            ETipoPessoa pessoaTipo,
            string nomeRazaoSocial,
            string apelidoFantasia,
            Endereco endereco,
            string cpfCNPJ,
            string rgInscricaoEstadual,
            long? telefone = null,
            string email = "")
        {
            PessoaTipo = pessoaTipo;
            NomeRazaoSocial = nomeRazaoSocial.SanitizeString() ?? "CONSUMIDOR";
            ApelidoFantasia = apelidoFantasia.SanitizeString();
            Endereco = endereco;
            Telefone = telefone == 0 ? null : telefone;
            CPFCNPJ = cpfCNPJ.RetornaNumeros();
            RGInscricaoEstadual = rgInscricaoEstadual.RetornaAlfanumericos();
            Email = email.SanitizeString();

            InformarInscricaoEstadualIsento();
        }

        public ETipoPessoa PessoaTipo { get; private set; }

        public string NomeRazaoSocial { get; private set; }

        public string ApelidoFantasia { get; private set; }

        public Endereco Endereco { get; private set; }

        public long? Telefone { get; private set; }

        public string CPFCNPJ { get; private set; }

        public string RGInscricaoEstadual { get; private set; }

        public bool InscricaoEstadualIsento { get; private set; }

        public string Email { get; private set; }

        public void InformarInscricaoEstadual(string rgInscricaoEstadual)
        {
            RGInscricaoEstadual = rgInscricaoEstadual;
            InformarInscricaoEstadualIsento();
        }

        private void InformarInscricaoEstadualIsento()
        {
            InscricaoEstadualIsento = RGInscricaoEstadual?.Equals("ISENTO", StringComparison.CurrentCultureIgnoreCase) != false;
        }
    }
}
