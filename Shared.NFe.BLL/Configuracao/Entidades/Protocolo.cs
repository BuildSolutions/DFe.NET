using NFe.Classes.Protocolo;
using System;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Protocolo
    {
        public Protocolo(string chaveAcesso, DateTime? dataRecebimento, string numeroProtocolo, string digestValue)
        {
            ChaveAcesso = chaveAcesso;
            DataRecebimento = dataRecebimento;
            NumeroProtocolo = numeroProtocolo;
            DigestValue = digestValue;
        }

        public Protocolo(infProt protocolo)
        {
            ChaveAcesso = protocolo.chNFe;
            DataRecebimento = protocolo.dhRecbto.DateTime;
            NumeroProtocolo = protocolo.nProt;
            DigestValue = protocolo.digVal;
        }

        public string ChaveAcesso { get; private set; }

        public DateTime? DataRecebimento { get; }

        public string NumeroProtocolo { get; }

        public string DigestValue { get; }

        public void AtualizarChaveAcesso(string chaveAcesso)
        {
            ChaveAcesso = chaveAcesso;
        }
    }
}
