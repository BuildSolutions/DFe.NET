using DFe.Utils.Extensoes;
using System;


namespace NFe.BLL.Configuracao.Entidades
{
    public class Duplicata
    {
        public Duplicata(string codigo, DateTime dataVencimento, decimal valor)
        {
            Codigo = codigo.SanitizeString();
            DataVencimento = dataVencimento;
            Valor = valor;
        }

        public string Codigo { get; }

        public DateTime DataVencimento { get; }

        public decimal Valor { get; }
    }
}
