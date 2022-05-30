using System;
using System.Text.RegularExpressions;

namespace GNRE.Classes.Informacoes.Emitente
{
    public class PessoaIdentificacao
    {
        private const string ErroCpfCnpjIEPreenchidos = "Somente preencher um dos campos: CNPJ ou CPF ou IE, para um objeto do tipo identificacao!";
        private string _cnpj;
        private string _cpf;
        private string _ie;

        public string CNPJ
        {
            get { return _cnpj; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (string.IsNullOrEmpty(_cpf)
                    && string.IsNullOrEmpty(_ie))
                {
                    _cnpj = Regex.Match(value, @"\d+").Value;
                }
                else
                {
                    throw new ArgumentException(ErroCpfCnpjIEPreenchidos);
                }
            }
        }

        public string CPF
        {
            get { return _cpf; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (string.IsNullOrEmpty(_cnpj)
                    && string.IsNullOrEmpty(_ie))
                {
                    _cpf = Regex.Match(value, @"\d+").Value;
                }
                else
                {
                    throw new ArgumentException(ErroCpfCnpjIEPreenchidos);
                }
            }
        }

        public string IE
        {
            get { return _ie; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (string.IsNullOrEmpty(_cnpj)
                    && string.IsNullOrEmpty(_cpf))
                {
                    _ie = Regex.Match(value, @"\d+").Value;
                }
                //else
                //{
                //    throw new ArgumentException(ErroCpfCnpjIEPreenchidos);
                //}
            }
        }
    }
}
