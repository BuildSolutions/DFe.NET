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
            string diretorioSalvarXML,
            string diretorioSchemas)
        {
            Pessoa = pessoa;
            Certificado = certificado;
            DiretorioSalvarXML = diretorioSalvarXML;
            DiretorioSchemas = diretorioSchemas;
        }

        public Pessoa Pessoa { get; }

        public ConfiguracaoCertificado Certificado { get; }

        public string DiretorioSalvarXML { get; }

        public string DiretorioSchemas { get; }
    }
}