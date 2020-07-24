using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.BLL.Enums;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Destinatario
    {
        public Destinatario(
            Pessoa pessoa,
            bool isMEIIsentoICMS,
            bool isProdutorRural,
            string suframaNumero,
            ConsumidorFinal eConsumidorFinal)
        {
            Pessoa = pessoa;
            IsMEIIsentoIncideICMS = isMEIIsentoICMS;
            IsProdutorRural = isProdutorRural;
            IsSuframa = !string.IsNullOrEmpty(suframaNumero);
            SuframaNumero = suframaNumero.SanitizeString();
            EConsumidorFinal = eConsumidorFinal;

            if (eConsumidorFinal == ConsumidorFinal.cfConsumidorFinal
                && pessoa?.PessoaTipo == ETipoPessoa.Fisica
                && !isProdutorRural)
            {
                this.Pessoa.InformarInscricaoEstadual(null);
            }
        }

        public Destinatario(dest destinatario)
        {
            Pessoa = new Pessoa(destinatario);
            IsMEIIsentoIncideICMS = false;
            IsProdutorRural = false;
            IsSuframa = !string.IsNullOrEmpty(destinatario.ISUF);
            SuframaNumero = destinatario.ISUF.SanitizeString();
            EConsumidorFinal = ConsumidorFinal.cfNao;

            if (EConsumidorFinal == ConsumidorFinal.cfConsumidorFinal
                && Pessoa?.PessoaTipo == ETipoPessoa.Fisica
                && !IsProdutorRural)
            {
                this.Pessoa.InformarInscricaoEstadual(null);
            }
        }

        public Pessoa Pessoa { get; }

        public bool IsMEIIsentoIncideICMS { get; }

        public bool IsProdutorRural { get; }

        public bool IsSuframa { get; }

        public string SuframaNumero { get; }

        public ConsumidorFinal EConsumidorFinal { get; }
    }
}
