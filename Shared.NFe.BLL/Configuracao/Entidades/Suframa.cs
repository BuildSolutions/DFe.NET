using System;
using System.Collections.Generic;
using DFe.Classes.Entidades;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Suframa
    {
        public Suframa(long numeroLoteSuframa,
            DateTime dataEmissao,
            string destinatarioCNPJ,
            string transportadoraCNPJ,
            string destinatarioInscricaoSuframa,
            Estado ufOrigem,
            Estado ufDestino,
            List<string> chavesAcessoNFes)
        {
            NumeroLoteSuframa = long.Parse($"{numeroLoteSuframa:00000}{DateTime.Now:MMyy}");
            DataEmissao = dataEmissao;
            DestinatarioCNPJ = destinatarioCNPJ;
            TransportadoraCNPJ = transportadoraCNPJ;
            DestinatarioInscricaoSuframa = destinatarioInscricaoSuframa;
            UfOrigem = ufOrigem;
            UfDestino = ufDestino;
            ChavesAcessoNFes = chavesAcessoNFes;
        }

        public long NumeroLoteSuframa { get; }

        public string Versao { get => "6.0"; }

        public DateTime DataEmissao { get; }

        public string DestinatarioCNPJ { get; }

        public string TransportadoraCNPJ { get; }

        public string DestinatarioInscricaoSuframa { get; }

        public Estado UfDestino { get; }

        public Estado UfOrigem { get; }

        public long QuantidadeNotasFiscais { get => (ChavesAcessoNFes?.Count) ?? 0; }

        public List<string> ChavesAcessoNFes { get; }
    }
}
