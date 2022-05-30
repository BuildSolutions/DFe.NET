using GNRE.Classes.Enumerators;
using GNRE.Classes.Informacoes.Dados;
using GNRE.Classes.Servicos.Consulta.Lote.Retorno;

namespace GNRE.Classes
{
    public class GuiasGNRERetorno : TDadosGNRE
    {
        public EConsultaLoteStatus situacaoGuia { get; set; }

        public string codigoBarras;

        public MotivoRejeicao motivosRejeicao { get; set; }
    }
}
