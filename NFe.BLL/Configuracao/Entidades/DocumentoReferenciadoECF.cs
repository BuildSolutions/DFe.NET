using System;
using NFe.BLL.Enums;

namespace NFe.BLL.Configuracao.Entidades
{
    /// <summary>
    /// BA20 - refECF, cupom fiscal emitido por ECF. O modelo é fixo em "2D" (cupom emitido
    /// por ECF); "2B" e "2C" são máquina registradora e PDV, que esta BLL não emite.
    /// </summary>
    public class DocumentoReferenciadoECF : DocumentoReferenciado
    {
        public DocumentoReferenciadoECF(int numeroECF, int numeroCOO)
        {
            Conferir(numeroECF, numeroCOO);

            TipoDocumento = EDocumentoReferenciado.ECF;
            Modelo = "2D";
            NumeroCOO = numeroCOO;
            NumeroECF = numeroECF;
        }

        /// <summary>
        /// nECF (BA22) tem 3 posições e nCOO (BA23) tem 6: são contadores, logo não podem
        /// ser negativos nem estourar o tamanho do campo.
        /// </summary>
        private static void Conferir(int numeroECF, int numeroCOO)
        {
            if (numeroECF < 0 || numeroECF > 999)
            {
                throw new ArgumentException($"Número do ECF inválido no cupom fiscal referenciado (refECF): {numeroECF}. O leiaute aceita de 0 a 999.");
            }

            if (numeroCOO < 0 || numeroCOO > 999999)
            {
                throw new ArgumentException($"Número do COO inválido no cupom fiscal referenciado (refECF): {numeroCOO}. O leiaute aceita de 0 a 999999.");
            }
        }

        public string Modelo { get; }

        public int NumeroCOO { get; }

        public int NumeroECF { get; }
    }
}
