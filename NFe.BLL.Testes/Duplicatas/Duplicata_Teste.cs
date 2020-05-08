using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFe.BLL.Configuracao.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFe.BLL.Testes.Duplicatas
{
    [TestClass]
    public class Duplicata_Teste
    {
        /// <summary>
        /// Método auxiliar para preencher as propriedades do objeto a ser testado
        /// </summary>
        private void PreenchePropriedades(
            string codigo, 
            DateTime dataVencimento, 
            decimal valor)
        {
            var duplicata = new Duplicata(codigo, dataVencimento, valor);
        }
    }
}
