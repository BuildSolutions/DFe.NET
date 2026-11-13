using System;
using System.Text.RegularExpressions;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria
{
    /// <summary>
    /// competApur é xs:gYearMonth com mínimo 2025-01 (TCompetApur,
    /// DFeTiposBasicos_v1.00.xsd): o formato é AAAA-MM, e não MM/AAAA. Formato errado vira
    /// rejeição de schema sem indicar a causa, por isso os dois grupos que carregam o campo
    /// (gCredPresIBSZFM e gAjusteCompet) conferem pela mesma regra.
    /// </summary>
    internal static class CompetenciaDeApuracao
    {
        private const string Minima = "2025-01";

        internal static void Conferir(string competencia, string nomeDoGrupo)
        {
            if (string.IsNullOrEmpty(competencia))
            {
                return;
            }

            //[0-9] e nao \d: em .NET o \d casa algarismo Unicode de qualquer alfabeto, e o
            //XSD aceita so 0-9. Competencia escrita com digito de largura total passaria
            //aqui e so falharia no schema.
            if (!Regex.IsMatch(competencia, @"^[0-9]{4}-(0[1-9]|1[0-2])$")
                || string.Compare(competencia, Minima, StringComparison.Ordinal) < 0)
            {
                throw new ArgumentException($"Competência de apuração {nomeDoGrupo} inválida: \"{competencia}\". O leiaute exige AAAA-MM a partir de {Minima}.");
            }
        }
    }
}
