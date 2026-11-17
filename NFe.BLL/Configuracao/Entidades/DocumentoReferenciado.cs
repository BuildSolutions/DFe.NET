using NFe.BLL.Enums;

namespace NFe.BLL.Configuracao.Entidades
{
    /// <summary>
    /// Base das seis formas do grupo BA (ide/NFref). Cada classe derivada corresponde a uma
    /// delas e fixa o proprio <see cref="TipoDocumento"/>.
    /// </summary>
    public abstract class DocumentoReferenciado
    {
        /// <summary>
        /// Somente a classe derivada define. Setter publico deixava combinar, por exemplo,
        /// um <see cref="DocumentoReferenciadoNFe"/> com o tipo NF, e a montagem do XML
        /// faz o cast pelo tipo - viraria InvalidCastException no meio da emissao.
        /// </summary>
        public EDocumentoReferenciado TipoDocumento { get; protected set; }
    }
}
