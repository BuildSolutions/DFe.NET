using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;

namespace GNRE.BLL.Configuracao.ValueObjects
{
    public class Endereco
    {
        public Endereco(
            string cep,
            string logradouro,
            string numero,
            string bairro,
            long municipioCodigoIBGE,
            string municipioNome,
            Estado municipioEstadoSigla
            )
        {
            SetEnderecoCompleto(logradouro, numero, bairro);

            MunicipioNome = municipioNome.SanitizeString();
            MunicipioEstadoSigla = municipioEstadoSigla.NuloSeInvalido();
            CEP = cep.SanitizeString();

            var municipioCodigoIBGEString = municipioCodigoIBGE.ToString();
            MunicipioCodigoIBGE = municipioCodigoIBGEString;

            if (municipioCodigoIBGEString?.Length == 7)
            {
                MunicipioCodigoIBGE = municipioCodigoIBGEString.Substring(2);
            }
        }

        public Endereco(
            string cep,
            string enderecoCompleto,
            long municipioCodigoIBGE,
            string municipioNome,
            Estado municipioEstadoSigla
            )
        {
            EnderecoCompleto = enderecoCompleto;
            MunicipioNome = municipioNome.SanitizeString();
            MunicipioEstadoSigla = municipioEstadoSigla.NuloSeInvalido();
            CEP = cep.SanitizeString();

            var municipioCodigoIBGEString = municipioCodigoIBGE.ToString();
            MunicipioCodigoIBGE = municipioCodigoIBGEString;

            if (municipioCodigoIBGEString?.Length == 7)
            {
                MunicipioCodigoIBGE = municipioCodigoIBGEString.Substring(2);
            }
        }

        /// <summary>
        /// Endereço completo (Logradouro, Numero, Bairro)
        /// No caso da identificação do contribuinte ser por inscrição estadual, esse campo e sua tag poderão ser omitidos.
        /// </summary>
        public string EnderecoCompleto { get; private set; }

        /// <summary>
        /// O IBGE informará o código do município no formato "EEmmmmd". Tirar os 2 primeiros números que indicam o número do Estado e só colocar os 5 números restantes, ficando no formato "mmmmd".
        /// Exemplo:
        /// A cidade Recife tem o código 2611606, você tirará os dígitos "26" e colocará no arquivo de lote apenas os dígitos "11606".
        /// No caso da identificação do contribuinte ser por inscrição estadual, esse campo e sua tag poderão ser omitidos.
        /// </summary>
        public string MunicipioCodigoIBGE { get; }

        /// <summary>
        /// Nome do Município
        /// No caso da identificação do contribuinte ser por inscrição estadual, esse campo e sua tag poderão ser omitidos.
        /// </summary>
        public string MunicipioNome { get; }

        /// <summary>
        /// Contém a UF de localização do contribuinte emitente. Campo com 2 dígitos.
        /// No caso da identificação do contribuinte ser por inscrição estadual, esse campo e sua tag poderão ser omitidos.
        /// </summary>
        public Estado MunicipioEstadoSigla { get; }

        /// <summary>
        /// Contém o CEP do contribuinte emitente com 8 dígitos. Digitar apenas números. Esse campo não é obrigatório.
        /// No caso da identificação do contribuinte ser por inscrição estadual, esse campo e sua tag poderão ser omitidos.
        /// </summary>
        public string CEP { get; }

        private void SetEnderecoCompleto(string logradouro,
            string numero,
            string bairro)
        {
            EnderecoCompleto = $"{logradouro}, {numero}".SanitizeString();

            if(!string.IsNullOrEmpty(bairro))
            {
                EnderecoCompleto += $" - {bairro}".SanitizeString();
            }
        }
    }
}
