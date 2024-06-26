﻿using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos
{
    public class PartilhaICMS
    {
        public PartilhaICMS(decimal baseCalculoICMSDestino, decimal aliquotaInternaUFDestino, decimal valorICMSUFDestino, decimal aliquotaInterestadual, decimal aliquotaICMSPartilha, decimal valorICMSUFOrigem, decimal valorFCP, decimal aliquotaFCP)
        {
            BaseCalculoICMSDestino = baseCalculoICMSDestino;
            AliquotaInternaUFDestino = aliquotaInternaUFDestino;
            ValorICMSUFDestino = valorICMSUFDestino;
            AliquotaInterestadual = aliquotaInterestadual;
            AliquotaICMSPartilha = aliquotaICMSPartilha;
            ValorICMSUFOrigem = valorICMSUFOrigem;
            BaseCalculoFCP = valorFCP > 0 ? baseCalculoICMSDestino : 0M;
            ValorFCP = valorFCP;
            AliquotaFCP = aliquotaFCP;
        }

        public PartilhaICMS(ICMSUFDest icmsUfDest)
        {
            BaseCalculoICMSDestino = icmsUfDest.vBCUFDest;
            AliquotaInternaUFDestino = icmsUfDest.pICMSUFDest;
            ValorICMSUFDestino = icmsUfDest.vICMSUFDest;
            AliquotaInterestadual = icmsUfDest.pICMSInter;
            AliquotaICMSPartilha = icmsUfDest.pICMSInterPart;
            ValorICMSUFOrigem = icmsUfDest.vICMSUFRemet;
            BaseCalculoFCP = icmsUfDest.vBCFCPUFDest ?? 0M;
            ValorFCP = icmsUfDest.vFCPUFDest ?? 0M;
            AliquotaFCP = icmsUfDest.pFCPUFDest ?? 0M;
        }

        public decimal BaseCalculoICMSDestino { get; }

        public decimal AliquotaInternaUFDestino { get; }

        public decimal ValorICMSUFDestino { get; }

        public decimal AliquotaInterestadual { get; }

        public decimal AliquotaICMSPartilha { get; }

        public decimal ValorICMSUFOrigem { get; }

        public decimal BaseCalculoFCP { get; }

        public decimal ValorFCP { get; }

        public decimal AliquotaFCP { get; }
    }
}
