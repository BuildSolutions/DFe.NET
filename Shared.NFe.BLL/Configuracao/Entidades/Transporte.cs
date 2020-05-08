using DFe.Utils.Extensoes;
using NFe.BLL.Configuracao.ValueObjects;
using NFe.Classes.Informacoes.Transporte;

namespace NFe.BLL.Configuracao.Entidades
{
    public class Transporte
    {
        public Transporte(
            ModalidadeFrete modalidadeFrete,
            Pessoa transportadora,
            int? quantidadeVolumes = null,
            string especieVolumeDescricao = null,
            decimal? pesoLiquido = null,
            decimal? pesoBruto = null)
        {
            ModalidadeFrete = modalidadeFrete;
            Transportadora = transportadora;
            QuantidadeVolumes = quantidadeVolumes;
            EspecieVolumeDescricao = especieVolumeDescricao.SanitizeString();
            PesoLiquido = pesoLiquido;
            PesoBruto = pesoBruto;
        }

        public ModalidadeFrete ModalidadeFrete { get; }

        public Pessoa Transportadora { get; }

        public int? QuantidadeVolumes { get; }

        public string EspecieVolumeDescricao { get; }

        public decimal? PesoLiquido { get; }

        public decimal? PesoBruto { get; }
    }
}
