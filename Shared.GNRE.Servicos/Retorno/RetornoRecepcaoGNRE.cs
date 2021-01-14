using System;
using System.Collections.Generic;
using System.Text;
using GNRE.Classes.Servicos;
using GNRE.Classes.Servicos.Recepcao.Retorno;
using GNRE.Servicos.Retorno;

namespace GNRE.Servicos.Retorno
{
    public class RetornoRecepcaoGNRE : RetornoBasico
    {
        public RetornoRecepcaoGNRE(string envioStr, string retornoStr, string retornoCompletaStr, TretLote_GNRE retorno) : base(envioStr, retornoStr, retornoCompletaStr, retorno)
        {
            Retorno = retorno;
        }

        public new TretLote_GNRE Retorno { get; }
    }
}
