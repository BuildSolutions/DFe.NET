using System.IO;
using DFe.Classes.Entidades;
using DFe.Utils;
using NFe.BLL;
using NFe.BLL.Configuracao.Entidades;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Emitente;
using Xunit;

namespace NFe.BLL.Testes
{
    /// <summary>
    /// ConfiguracaoApp já tomou ConfiguracaoServico.Instancia para si e mutou o
    /// singleton do processo: o segundo emitente sobrescrevia certificado, UF e
    /// diretórios do primeiro, e a nota saía assinada com o certificado errado.
    /// </summary>
    public class ConfiguracaoAppTestes
    {
        private static string DiretorioDe(string nome)
        {
            // Nenhum diretorio e criado: ConfiguracaoApp so guarda o caminho. O temporario
            // serve para o teste nao depender de uma raiz do Windows.
            return Path.Combine(Path.GetTempPath(), "nfe-bll-testes", nome);
        }

        private static Emitente ObterEmitente(Estado uf, string serialCertificado, string diretorio)
        {
            var endereco = new Endereco(cep: "13000000",
                logradouro: "RUA UM",
                numero: "1",
                complemento: null,
                bairro: "CENTRO",
                municipioCodigoIBGE: 3509502,
                municipioNome: "CAMPINAS",
                municipioEstadoSigla: uf);

            var pessoa = new Pessoa(pessoaTipo: ETipoPessoa.Juridica,
                nomeRazaoSocial: "EMITENTE " + uf,
                apelidoFantasia: null,
                endereco: endereco,
                cpfCNPJ: "12345678000195",
                rgInscricaoEstadual: "123456789");

            return new Emitente(pessoa: pessoa,
                inscricaoEstadualSubstituicaoTributaria: null,
                crt: CRT.RegimeNormal,
                habilitarDetalhamentoImposto: false,
                certificado: new ConfiguracaoCertificado { Serial = serialCertificado },
                diretorioSalvarXML: diretorio,
                diretorioSchemas: diretorio);
        }

        [Fact]
        public void DoisEmitentes_NaoCompartilhamConfiguracaoDeServico()
        {
            var primeiro = new ConfiguracaoNFe(ObterEmitente(Estado.SP, "SERIAL-SP", DiretorioDe("sp")));
            var segundo = new ConfiguracaoNFe(ObterEmitente(Estado.MG, "SERIAL-MG", DiretorioDe("mg")));

            Assert.NotSame(primeiro.CfgServico, segundo.CfgServico);

            Assert.Equal(Estado.SP, primeiro.CfgServico.cUF);
            Assert.Equal("SERIAL-SP", primeiro.CfgServico.Certificado.Serial);
            Assert.Equal(DiretorioDe("sp"), primeiro.CfgServico.DiretorioSchemas);

            Assert.Equal(Estado.MG, segundo.CfgServico.cUF);
            Assert.Equal("SERIAL-MG", segundo.CfgServico.Certificado.Serial);
            Assert.Equal(DiretorioDe("mg"), segundo.CfgServico.DiretorioSchemas);
        }

        [Fact]
        public void LerCfgServico_NaoAlteraAConfiguracao()
        {
            var configuracao = new ConfiguracaoNFe(ObterEmitente(Estado.SP, "SERIAL-SP", DiretorioDe("sp")));

            var primeiraLeitura = configuracao.CfgServico;
            var segundaLeitura = configuracao.CfgServico;

            Assert.Same(primeiraLeitura, segundaLeitura);
            Assert.Equal(Estado.SP, segundaLeitura.cUF);
        }
    }
}
