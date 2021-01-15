using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Dados;
using GNRE.Classes.Servicos.Consulta.Lote.Retorno;

namespace GNRE.Classes
{
    public class GuiasGNRE : TDadosGNRE
    {
        public EConsultaLoteStatus situacaoGuia { get; set; }

        public long? codigoBarras;

        public MotivoRejeicao motivosRejeicao { get; set; }

        public bool ShouldSerializecodigoBarras()
        {
            return codigoBarras.HasValue;
        }
    }
}
