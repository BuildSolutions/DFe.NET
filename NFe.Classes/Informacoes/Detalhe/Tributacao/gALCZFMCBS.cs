using System.Xml.Serialization;

namespace NFe.Classes.Informacoes.Detalhe.Tributacao
{
    /// <summary>
    /// UB66a - Grupo de operações em áreas incentivadas (ALC/ZFM) amparadas por alíquota
    /// zero da CBS, nos termos dos artigos 451 e 466 da LC 214/25.
    /// </summary>
    public class gALCZFMCBS
    {
        private decimal _pAliqEfetRegCbs;
        private decimal _vTribRegCbs;

        /// <summary>
        /// UB66b - Tipo de aplicação da alíquota zero da CBS.
        /// </summary>
        [XmlElement(Order = 1)]
        public tpALCZFMCBS tpALCZFMCBS { get; set; }

        /// <summary>
        /// UB66c - Número do processo na Suframa para o item comercializado, de 8 a 12
        /// caracteres. Obrigatório quando tpALCZFMCBS = 2 (rejeição 1192).
        /// </summary>
        [XmlElement(Order = 2)]
        public string nProcSuframa { get; set; }

        /// <summary>
        /// UB66d - Alíquota efetiva da CBS que incidiria fora da área incentivada.
        /// </summary>
        [XmlElement(Order = 3)]
        public decimal pAliqEfetRegCBS
        {
            get => _pAliqEfetRegCbs.Arredondar(4);
            set => _pAliqEfetRegCbs = value.Arredondar(4);
        }

        /// <summary>
        /// UB66e - Valor da CBS que seria devido fora da área ou do regime incentivado
        /// (rejeição 1218 quando divergir do calculado).
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal vTribRegCBS
        {
            get => _vTribRegCbs.Arredondar(2);
            set => _vTribRegCbs = value.Arredondar(2);
        }

        public bool ShouldSerializenProcSuframa()
        {
            return !string.IsNullOrWhiteSpace(nProcSuframa);
        }
    }
}
