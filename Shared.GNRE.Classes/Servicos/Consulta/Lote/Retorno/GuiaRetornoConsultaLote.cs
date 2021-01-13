using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Dados;

namespace GNRE.Classes.Servicos.Consulta.Lote.Retorno
{
    public class GuiaRetornoConsultaLote
    {
        public EConsultaLoteStatus situacaoGuia { get; set; }

        public TDadosGNRE TDadosGNRE { get; set; }

        public long? codigoBarras;

        public MotivoRejeicao motivosRejeicao { get; set; }

        public bool ShouldSerializecodigoBarras()
        {
            return codigoBarras.HasValue;
        }
    }
}
