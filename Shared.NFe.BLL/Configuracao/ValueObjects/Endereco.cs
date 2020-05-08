using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using System;
using System.Collections.Generic;
using System.Text;

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
            Logradouro = logradouro.SanitizeString();
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
