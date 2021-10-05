using DFe.Utils;
using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.Classes.Informacoes.Emitente;
using System.Collections.Generic;
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
            string diretorioSalvarXML,
            string diretorioSchemas,
            IReadOnlyList<AutorizadosXml> autorizadosXmlDocumentos = null)
        {
            Pessoa = pessoa;
            InscricaoEstadualSubstituicaoTributaria = inscricaoEstadualSubstituicaoTributaria.RetornaNumeros();
            ECRT = crt;
            HabilitarDetalhamentoImposto = habilitarDetalhamentoImposto;
            Certificado = certificado;
            DiretorioSalvarXML = diretorioSalvarXML;
            DiretorioSchemas = diretorioSchemas;
            AutorizadosXmlDocumentos = autorizadosXmlDocumentos;
        }

        public Emitente(
            emit emitente)
        {
            Pessoa = new Pessoa(emitente);
            InscricaoEstadualSubstituicaoTributaria = emitente.IEST.RetornaNumeros();
            ECRT = emitente.CRT;
            HabilitarDetalhamentoImposto = true;
            Certificado = null;
            DiretorioSalvarXML = null;
            DiretorioSchemas = null;
        }

        public Pessoa Pessoa { get; }

        public string InscricaoEstadualSubstituicaoTributaria { get; }

        public CRT ECRT { get; }

        public bool HabilitarDetalhamentoImposto { get; }

        public ConfiguracaoCertificado Certificado { get; }

        public string DiretorioSalvarXML { get; }

        public string DiretorioSchemas { get; }

        public IReadOnlyList<AutorizadosXml> AutorizadosXmlDocumentos { get; private set; }
    }
}