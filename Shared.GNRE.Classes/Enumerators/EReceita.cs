using System.ComponentModel;
using System.Xml.Serialization;

namespace GNRE.Classes.Enumerators
{
    public enum EReceita
    {
        [Description("ICMS Comunicação")]
        [XmlEnum("100013")]
        ICMSComunicacao = 100013,

        [Description("ICMS Energia Elétrica")]
        [XmlEnum("100021")]
        ICMSEnergiaEletrica = 100021,

        [Description("ICMS Transporte")]
        [XmlEnum("100030")]
        ICMSTransporte = 100030,

        [Description("ICMS Substituição Tributária por Apuração")]
        [XmlEnum("100048")]
        ICMSSubstituicaoTributariaApuracao = 100048,

        [Description("ICMS Importação")]
        [XmlEnum("100056")]
        ICMSImportacao = 100056,

        [Description("ICMS Autuação Fiscal")]
        [XmlEnum("100064")]
        ICMSAutuacaoFiscal = 100064,

        [Description("ICMS Parcelamento")]
        [XmlEnum("100072")]
        ICMSParcelamento = 100072,

        [Description("ICMS Recolhimentos Especiais")]
        [XmlEnum("100080")]
        ICMSRecolhimentosEspeciais = 100080,

        [Description("ICMS Substituição Tributária por Operação")]
        [XmlEnum("100099")]
        ICMSSubstituicaoTributariaOperacao = 100099,

        [Description("ICMS Consumidor Final Não Contribuinte Outra UF por Operação")]
        [XmlEnum("100102")]
        ICMSConsumidorFinalNaoContribuinteOutraUFOperacao = 100102,

        [Description("ICMS Consumidor Final Não Contribuinte Outra UF por Apuração")]
        [XmlEnum("100110")]
        ICMSConsumidorFinalNaoContribuinteOutraUFApuracao = 100110,

        [Description("ICMS Fundo Estadual de Combate à Pobreza por Operação")]
        [XmlEnum("100129")]
        ICMSFundoEstadualCombatePobrezaOperacao = 100129,

        [Description("ICMS Fundo Estadual de Combate à Pobreza por Apuração")]
        [XmlEnum("100137")]
        ICMSFundoEstadualCombatePobrezaApuracao = 100137,

        [Description("ICMS Dívida Ativa")]
        [XmlEnum("150010")]
        ICMSDividaAtiva = 150010,

        [Description("Multa por Infração à Obrigação Acessória")]
        [XmlEnum("500011")]
        MultaInfracaoObrigacaoAcessoria = 500011,
    }
}
