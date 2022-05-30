using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum EConsultaLoteStatus
    {
        [Description("Processada com sucesso")]
        [XmlEnum("0")]
        ProcessadaComSucesso = 0,

        [Description("Invalidada pelo Portal")]
        [XmlEnum("1")]
        InvalidadaPeloPortal = 1,

        [Description("Invalidada pela UF ")]
        [XmlEnum("2")]
        InvalidadaPelaUF = 2,

        [Description("Erro de comunicação")]
        [XmlEnum("3")]
        ErroDeComunicacao = 3,
    }
}
