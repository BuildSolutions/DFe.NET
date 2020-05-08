using DFe.Classes.Entidades;
using DFe.Utils.Extensoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.BLL.Configuracao.Entidades.Produtos
{
    public class DadosCombustivel
    {
        public DadosCombustivel(
            string codigoANP,
            string descricaoANP,
            decimal? percentualGasNaturalNacional,
            decimal? percentualGasNaturalImportado,
            decimal? percentualGLPDerivadoPetroleo,
            decimal? valorPartida,
            Estado estadoConsumo)
        {
            CodigoANP = codigoANP.SanitizeString();
            DescricaoANP = descricaoANP.SanitizeString();
            PercentualGasNaturalNacional = percentualGasNaturalNacional;
            PercentualGasNaturalImportado = percentualGasNaturalImportado;
            PercentualGLPDerivadoPetroleo = percentualGLPDerivadoPetroleo;
            ValorPartida = valorPartida;
            EstadoConsumo = estadoConsumo;
        }

        public string CodigoANP { get; }

        public string DescricaoANP { get; }

        public decimal? PercentualGasNaturalNacional { get; }

        public decimal? PercentualGasNaturalImportado { get; }

        public decimal? PercentualGLPDerivadoPetroleo { get; }

        public decimal? ValorPartida { get; }

        public Estado EstadoConsumo { get; }
    }
}
