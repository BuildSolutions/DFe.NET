using DFe.Utils.Extensoes;
using NFe.BLL.Enums;

namespace NFe.BLL.Configuracao.Entidades
{
    public class DocumentoReferenciadoNFe : DocumentoReferenciado
    {
        public DocumentoReferenciadoNFe(string chaveAcesso)
        {
            TipoDocumento = EDocumentoReferenciado.NFe;
            ChaveAcesso = chaveAcesso.SanitizeString();
        }

        public string ChaveAcesso { get; }
    }
}
