using System.IO;
using DFe.Utils;
using GNRE.BLL.Configuracao.ValueObjects;

namespace GNRE.BLL.Configuracao.Entidades
{
    public class Emitente
    {
        public Emitente(
            Pessoa pessoa,
            ConfiguracaoCertificado certificado,
            DirectoryInfo diretorioSalvarXML,
            DirectoryInfo diretorioSchemas)
        {
            Pessoa = pessoa;
            Certificado = certificado;
            DiretorioSalvarXML = diretorioSalvarXML?.FullName;
            DiretorioSchemas = diretorioSchemas?.FullName;
        }

        public Pessoa Pessoa { get; }

        public ConfiguracaoCertificado Certificado { get; }

        public string DiretorioSalvarXML { get; }

        public string DiretorioSchemas { get; }
    }
}