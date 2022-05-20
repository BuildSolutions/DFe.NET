using Shared.NFe.Classes.Informacoes.Intermediador;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Intermediador
    {
        public Intermediador(string cnpj, string nomeUsuarioPlataforma)
        {
            CNPJ = cnpj;
            NomeUsuarioPlataforma = nomeUsuarioPlataforma;
        }

        public Intermediador(infIntermed infIntermed)
        {
            CNPJ = infIntermed?.CNPJ;
            NomeUsuarioPlataforma = infIntermed?.idCadIntTran;
        }

        public string CNPJ { get; set; }

        public string NomeUsuarioPlataforma { get; set; }
    }
}
