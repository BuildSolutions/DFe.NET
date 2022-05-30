namespace NFe.BLL.Configuracao.Entidades
{
    public class AutorizadosXml
    {
        public AutorizadosXml(string cpfCnpj)
        {
            CpfCnpj = cpfCnpj;
        }

        public string CpfCnpj { get; }
    }
}
