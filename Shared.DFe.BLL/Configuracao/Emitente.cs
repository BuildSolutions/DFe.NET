using DFe.Utils;

namespace DFe.BLL.Configuracao
{
    public class Emitente
    {
        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public long MunicipioCodigoIBGE { get; set; }

        public string MunicipioNome{ get; set; }

        public string MunicipioEstadoSigla { get; set; }

        public int PaisCodigo { get; set; } = 1058;

        public string PaisNome { get; set; } = "BRASIL";

        public string CEP { get; set; }

        public long Telefone { get; set; }

        public string CNPJ { get; set; }

        public string InscricaoEstadual { get; set; }

        public string InscricaoEstadualSubstituicaoTributaria { get; set; }

        public int CRT { get; set; }

        public ConfiguracaoCertificado Certificado { get; set; }

        public string DiretorioSalvarXML { get; set; }

        public string DiretorioSchemas { get; set; }
    }
}
