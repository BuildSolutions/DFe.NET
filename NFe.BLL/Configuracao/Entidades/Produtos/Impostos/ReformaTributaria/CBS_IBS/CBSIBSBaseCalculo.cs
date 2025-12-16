using System;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.CBS_IBS
{
    /// <summary>
    /// Valores usados para compor a Base de Cálculo do IBS/CBS.
    /// Regra NT: UB16-10.
    /// A Base de Cálculo NÃO está neste objeto – ela será calculada pela fórmula.
    /// </summary>
    public class CBSIBSBaseCalculo
    {
        private readonly decimal _valorProdutos;
        private readonly decimal _valorServicos;
        private readonly decimal _valorFrete;
        private readonly decimal _valorSeguro;
        private readonly decimal _outrasDespesas;
        private readonly decimal _impostoImportacao;
        private readonly decimal _descontos;
        private readonly decimal _valorPIS;
        private readonly decimal _valorCOFINS;
        private readonly decimal _valorICMS;
        private readonly decimal _valorICMSUFDest;
        private readonly decimal _fcp;
        private readonly decimal _fcpUFDestino;
        private readonly decimal _icmsMonofasico;
        private readonly decimal _iss;
        private readonly decimal _impostoSeletivo;

        public CBSIBSBaseCalculo(decimal valorProdutos,
            decimal valorServicos,
            decimal valorFrete,
            decimal valorSeguro,
            decimal outrasDespesas,
            decimal impostoImportacao,
            decimal descontos,
            decimal valorPIS,
            decimal valorCOFINS,
            decimal valorICMS,
            decimal valorICMSUFDest,
            decimal fcp,
            decimal fcpUFDestino,
            decimal icmsMonofasico,
            decimal iss,
            decimal impostoSeletivo)
        {
            _valorProdutos = valorProdutos;
            _valorServicos = valorServicos;
            _valorFrete = valorFrete;
            _valorSeguro = valorSeguro;
            _outrasDespesas = outrasDespesas;
            _impostoImportacao = impostoImportacao;
            _descontos = descontos;
            _valorPIS = valorPIS;
            _valorCOFINS = valorCOFINS;
            _valorICMS = valorICMS;
            _valorICMSUFDest = valorICMSUFDest;
            _fcp = fcp;
            _fcpUFDestino = fcpUFDestino;
            _icmsMonofasico = icmsMonofasico;
            _iss = iss;
            _impostoSeletivo = impostoSeletivo;
        }

        //public decimal BaseCalculo { get; set; }

        ///// <summary>vProd — Valor total dos produtos.</summary>
        //public decimal ValorProdutos { get; }

        ///// <summary>vServ — Valor de serviços.</summary>
        //public decimal ValorServicos { get; }

        ///// <summary>vFrete — Valor do frete.</summary>
        //public decimal ValorFrete { get; }

        ///// <summary>vSeg — Valor do seguro.</summary>
        //public decimal ValorSeguro { get; }

        ///// <summary>vOutro — Outras despesas acessórias.</summary>
        //public decimal OutrasDespesas { get; }

        ///// <summary>vII — Imposto de Importação.</summary>
        //public decimal ImpostoImportacao { get; }

        ///// <summary>vDesc — Valor total de descontos.</summary>
        //public decimal Descontos { get; }

        ///// <summary>vPIS — Valor do PIS a subtrair (exceto se indSomaPISST=1).</summary>
        //public decimal ValorPIS { get; }

        ///// <summary>vCOFINS — Valor do COFINS a subtrair (exceto se indSomaCOFINSST=1).</summary>
        //public decimal ValorCOFINS { get; }

        ///// <summary>vICMS — Valor do ICMS a subtrair.</summary>
        //public decimal ValorICMS { get; }

        ///// <summary>vICMSUFDest — DIFAL destino.</summary>
        //public decimal ICMSUFDestino { get; }

        ///// <summary>vFCP — Fundo de Combate à Pobreza (origem).</summary>
        //public decimal FCP { get; }

        ///// <summary>vFCPUFDest — FCP destino (DIFAL).</summary>
        //public decimal FCPUFDestino { get; }

        ///// <summary>vICMSMono — ICMS Monofásico.</summary>
        //public decimal ICMSMonofasico { get; }

        ///// <summary>vISSQN — Valor do ISSQN.</summary>
        //public decimal ISS { get; }

        ///// <summary>vIS — Imposto Seletivo (SOMA na base).</summary>
        //public decimal ImpostoSeletivo { get; }

        public decimal CalcularBaseCalculoCBSIBS()
        {
            return Math.Round(_valorProdutos
                + _valorServicos
            + _valorFrete
            + _valorSeguro
            + _outrasDespesas
            + _impostoImportacao
            + _impostoSeletivo
                - _descontos
                - _valorICMS
                - _valorICMSUFDest
                - _fcp
                - _valorPIS
                - _valorCOFINS
                - _fcpUFDestino
                - _icmsMonofasico
                - _iss, 2);
        }
    }
}
