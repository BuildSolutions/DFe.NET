using System;
using System.Text.RegularExpressions;
using DFe.Utils;
using DFe.Utils.Extensoes;

namespace NFe.BLL.Validators
{
    /// <summary>
    /// Chave de acesso de DF-e, do jeito que o leiaute exige: 44 posições no padrão TChNFe
    /// (tiposBasico_v4.00.xsd), que é o mesmo TChDFeRTC usado pelo refDFeAnt.
    /// <para>Existe porque duas entidades precisam da mesma regra — <see cref="Configuracao.Entidades.DocumentoReferenciadoNFe"/>
    /// e <see cref="Configuracao.Entidades.CompraGovernamental"/> — e até 11/11/2026 cada
    /// uma normalizava de um jeito: uma só sanitizava, a outra também passava para
    /// maiúsculas. Com CNPJ alfanumérico (NT 2026.004) a diferença é visível: a mesma chave
    /// em minúsculas passava numa e era recusada na outra.</para>
    /// <para>Quem chama monta a própria mensagem de erro: o texto cita o campo do leiaute,
    /// e isso é da entidade, não daqui.</para>
    /// </summary>
    public static class ChaveAcessoDFe
    {
        private const string Padrao = @"^[0-9]{6}[0-9A-Z]{12}[0-9]{26}$";

        /// <summary>
        /// Tira o lixo da string (<c>SanitizeString</c> já remove caractere inválido de XML
        /// e o espaço em volta) e sobe para maiúsculas, porque a chave com CNPJ
        /// alfanumérico só existe em maiúsculas.
        /// <para>Devolve <c>null</c> para entrada vazia: quem chama trata como chave
        /// ausente.</para>
        /// </summary>
        public static string Normalizar(string chaveAcesso)
        {
            return chaveAcesso.SanitizeString()?.ToUpperInvariant();
        }

        /// <summary>Confere só o formato: 44 posições no padrão do leiaute.</summary>
        public static bool FormatoValido(string chaveAcesso)
        {
            return !string.IsNullOrWhiteSpace(chaveAcesso)
                && Regex.IsMatch(chaveAcesso, Padrao);
        }

        /// <summary>
        /// Confere o dígito verificador pelo mesmo cálculo da emissão (já adequado ao CNPJ
        /// alfanumérico da NT 2026.004).
        /// <para>O cálculo lê mês e UF da própria chave: mês 00 ou 13 faz o construtor de
        /// <see cref="DateTime"/> lançar. Aqui isso significa chave errada, não falha de
        /// sistema.</para>
        /// </summary>
        public static bool DigitoVerificadorValido(string chaveAcesso)
        {
            try
            {
                return ChaveFiscal.ChaveValida(chaveAcesso);
            }
            catch
            {
                return false;
            }
        }
    }
}
