using DFe.Utils.Extensoes;
using GNRE.BLL.Enums;

namespace GNRE.BLL.Configuracao.Entidades
{
    public class Destinatario
    {
        public Destinatario(ETipoDocumentoPessoa eTipoDocumento,
            string documento,
            string nomeRazaoSocial,
            long municipioCodigoIBGE)
        {
            ETipoDocumento = eTipoDocumento;
            Documento = documento.RetornaNumeros();
            NomeRazaoSocial = nomeRazaoSocial.SanitizeString().SubstringMaxLength(60);

            var municipioCodigoIBGEString = municipioCodigoIBGE.ToString();
            MunicipioCodigoIBGE = municipioCodigoIBGEString;

            if (municipioCodigoIBGEString?.Length == 7)
            {
                MunicipioCodigoIBGE = municipioCodigoIBGEString.Substring(2);
            }
        }

        public ETipoDocumentoPessoa ETipoDocumento { get; private set; }

        public string Documento { get; private set; }

        public string NomeRazaoSocial { get; private set; }

        public string MunicipioCodigoIBGE { get; private set; }
    }
}