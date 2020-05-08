using NFe.BLL.Enums;

namespace NFe.BLL.Configuracao.Entidades
{
    public class DocumentoReferenciadoECF : DocumentoReferenciado
    {
        public DocumentoReferenciadoECF(int numeroECF, int numeroCOO)
        {
            TipoDocumento = EDocumentoReferenciado.ECF;
            Modelo = "2D";
            NumeroCOO = numeroCOO;
            NumeroECF = numeroECF;

        }

        public string Modelo { get; }

        public int NumeroCOO { get; }

        public int NumeroECF { get; }
    }
}
