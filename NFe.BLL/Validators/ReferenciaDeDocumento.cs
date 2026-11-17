using System;
using DFe.Classes.Entidades;

namespace NFe.BLL.Validators
{
    /// <summary>
    /// Conferências que as formas compostas do grupo BA (ide/NFref) têm em comum: refNF
    /// (BA03), refNFP (BA10) e refECF (BA20).
    /// <para>Existe para as três entidades aplicarem a mesma régua. Até 11/11/2026 só o
    /// refNFP conferia alguma coisa, e refNF e refECF aceitavam CNPJ vazio, UF que não
    /// existe, série negativa e número zero — tudo isso só falhava na validação do XML,
    /// longe de quem construiu o objeto.</para>
    /// <para>Quem chama monta a própria mensagem: o texto cita o campo do leiaute, e isso
    /// é da entidade, não daqui.</para>
    /// </summary>
    public static class ReferenciaDeDocumento
    {
        /// <summary>
        /// Só as 27 unidades da federação (TCodUfIBGE). A faixa 11 a 53 tem buracos — 18,
        /// 30, 34 e 49 não existem — e o enum <see cref="Estado"/> ainda carrega ambiente
        /// nacional (AN 91), SVRS (92) e exterior (EX 99), que não são UF.
        /// </summary>
        public static bool UFValida(Estado uf)
        {
            return Enum.IsDefined(typeof(Estado), uf)
                && uf != Estado.AN
                && uf != Estado.SVRS
                && uf != Estado.EX;
        }

        /// <summary>Série do documento fiscal: 0 a 999, zero quando não utilizada.</summary>
        public static bool SerieValida(int serie)
        {
            return serie >= 0 && serie <= 999;
        }

        /// <summary>Número do documento fiscal: 1 a 999999999.</summary>
        public static bool NumeroValido(int numero)
        {
            return numero >= 1 && numero <= 999999999;
        }

        /// <summary>
        /// Só dígitos, no tamanho exato. O CNPJ alfanumérico da NT 2026.004 vale para a
        /// chave de acesso, não para os CNPJ e CPF do grupo BA, que o leiaute mantém como
        /// TCnpj e TCpf.
        /// </summary>
        public static bool SomenteDigitos(string valor, int tamanho)
        {
            return valor != null
                && valor.Length == tamanho
                && SomenteDigitos(valor);
        }

        /// <summary>Só dígitos, em qualquer tamanho.</summary>
        public static bool SomenteDigitos(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return false;
            }

            foreach (var caractere in valor)
            {
                if (caractere < '0' || caractere > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
