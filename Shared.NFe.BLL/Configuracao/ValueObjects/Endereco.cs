using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Transporte;

namespace NFe.BLL.Configuracao.ValueObjects
{
    public class Endereco
    {
        public Endereco(
            string cep,
            string logradouro,
            string numero,
            string complemento,
            string bairro,
            long municipioCodigoIBGE,
            string municipioNome,
            Estado? municipioEstadoSigla,
            int paisCodigo = 1058,
            string paisNome = "BRASIL"
            )
        {
            Logradouro = logradouro.SanitizeString().SubstringMaxLength(60);
            Numero = numero.SanitizeString();
            Complemento = complemento.SanitizeString();
            Bairro = bairro.SanitizeString();
            MunicipioCodigoIBGE = municipioCodigoIBGE;
            MunicipioNome = municipioNome.SanitizeString();
            MunicipioEstadoSigla = municipioEstadoSigla.NuloSeInvalido();
            PaisCodigo = paisCodigo;
            PaisNome = paisNome.SanitizeString();
            CEP = cep.SanitizeString();
        }

        public Endereco(enderEmit endereco)
        {
            Logradouro = endereco.xLgr.SanitizeString().SubstringMaxLength(60);
            Numero = endereco.nro.SanitizeString();
            Complemento = endereco.xCpl.SanitizeString();
            Bairro = endereco.xBairro.SanitizeString();
            MunicipioCodigoIBGE = endereco.cMun;
            MunicipioNome = endereco.xMun.SanitizeString();
            MunicipioEstadoSigla = endereco.UF.NuloSeInvalido();
            PaisCodigo = endereco.cPais.GetValueOrDefault();
            PaisNome = endereco.xPais.SanitizeString();
            CEP = endereco.CEP.SanitizeString();
        }

        public Endereco(enderDest endereco)
        {
            Logradouro = endereco.xLgr.SanitizeString().SubstringMaxLength(60);
            Numero = endereco.nro.SanitizeString();
            Complemento = endereco.xCpl.SanitizeString();
            Bairro = endereco.xBairro.SanitizeString();
            MunicipioCodigoIBGE = endereco.cMun;
            MunicipioNome = endereco.xMun.SanitizeString();
            MunicipioEstadoSigla = (Estado?)System.Enum.Parse(typeof(Estado), endereco.UF);
            PaisCodigo = endereco.cPais.GetValueOrDefault();
            PaisNome = endereco.xPais.SanitizeString();
            CEP = endereco.CEP.SanitizeString();
        }

        public Endereco(transporta endereco)
        {
            System.Enum.TryParse(endereco.UF, out Estado estado);
            var enderecoCompleto = endereco.xEnder.SanitizeString();
            Logradouro = enderecoCompleto;

            if (enderecoCompleto.IndexOf(",") != -1)
            {
                Logradouro = enderecoCompleto.Substring(0, enderecoCompleto.IndexOf(",")).SanitizeString().SubstringMaxLength(60);

                if (enderecoCompleto.Substring(enderecoCompleto.IndexOf(",") + 1, enderecoCompleto.Length - enderecoCompleto.IndexOf(",") - 1).Length > 15)
                {
                    Numero = enderecoCompleto.Substring(enderecoCompleto.IndexOf(",") + 1, 15);
                }
                else
                {
                    Numero = enderecoCompleto.Substring(enderecoCompleto.IndexOf(",") + 1, enderecoCompleto.Length - enderecoCompleto.IndexOf(",") - 1);
                }
            }

            MunicipioNome = endereco.xMun.SanitizeString();
            MunicipioEstadoSigla = estado;
            PaisCodigo = 1058;
            PaisNome = "BRASIL";

            Complemento = null;
            Bairro = null;
            CEP = null;
            MunicipioCodigoIBGE = 0;
        }

        public string Logradouro { get; }

        public string Numero { get; }

        public string Complemento { get; }

        public string Bairro { get; }

        public long MunicipioCodigoIBGE { get; }

        public string MunicipioNome { get; }

        public Estado? MunicipioEstadoSigla { get; }

        public int PaisCodigo { get; }

        public string PaisNome { get; }

        public string CEP { get; }
    }
}
