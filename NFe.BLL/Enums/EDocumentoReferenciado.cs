namespace NFe.BLL.Enums
{
    /// <summary>
    /// Qual das formas do grupo BA (ide/NFref) a referência usa. NFref é um xs:choice:
    /// cada documento referenciado preenche exatamente uma.
    /// <para>Os valores 0, 1 e 2 são persistidos; não renumerar.</para>
    /// </summary>
    public enum EDocumentoReferenciado
    {
        /// <summary>BA02 - refNFe, chave de acesso de NF-e ou NFC-e.</summary>
        NFe = 0,

        /// <summary>BA03 - refNF, NF modelo 1/1A em papel.</summary>
        NF = 1,

        /// <summary>BA20 - refECF, cupom fiscal.</summary>
        ECF = 2,

        /// <summary>BA02a - refNFeSig, chave de NF-e com o código numérico zerado.</summary>
        NFeSigilosa = 3,

        /// <summary>BA19 - refCTe, chave de acesso de CT-e.</summary>
        CTe = 4,

        /// <summary>BA10 - refNFP, nota fiscal de produtor rural.</summary>
        NFProdutorRural = 5
    }
}
