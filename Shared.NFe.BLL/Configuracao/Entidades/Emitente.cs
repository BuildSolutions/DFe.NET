using DFe.Utils;
using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.Classes.Informacoes.Emitente;
using System.IO;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Emitente
    {
        public Emitente(
            Pessoa pessoa,
            string inscricaoEstadualSubstituicaoTributaria,
            CRT crt,
            bool habilitarDetalhamentoImposto,
            ConfiguracaoCertificado certificado,
            DirectoryInfo diretorioSalvarXML,
            DirectoryInfo diretorioSchemas)
        {
            Pessoa = pessoa;
            InscricaoEstadualSubstituicaoTributaria = inscricaoEstadualSubstituicaoTributaria.RetornaNumeros();
            ECRT = crt;
            HabilitarDetalhamentoImposto = habilitarDetalhamentoImposto;
            Certificado = certificado;
            DiretorioSalvarXML = diretorioSalvarXML?.FullName;
            DiretorioSchemas = diretorioSchemas?.FullName;
        }

        public Pessoa Pessoa { get; }

        public string InscricaoEstadualSubstituicaoTributaria { get; }

        public CRT ECRT { get; }

        public bool HabilitarDetalhamentoImposto { get; }

        public ConfiguracaoCertificado Certificado { get; }

        public string DiretorioSalvarXML { get; }

        public string DiretorioSchemas { get; }
    }
}