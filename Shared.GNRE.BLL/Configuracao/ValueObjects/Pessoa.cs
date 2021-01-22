using DFe.Utils.Extensoes;
using GNRE.BLL.Enums;

namespace GNRE.BLL.Configuracao.ValueObjects
{
    public class Pessoa
    {
        public Pessoa(
            ETipoDocumentoPessoa eTipoDocumento,
            string documento,
            string nomeRazaoSocial,
            Endereco endereco,
            long? telefone = null)
        {
            ETipoDocumento = eTipoDocumento;
            Documento = documento.RetornaNumeros();
            NomeRazaoSocial = nomeRazaoSocial.SanitizeString();
            Endereco = endereco;
            Telefone = telefone == 0 ? null : telefone;
        }

        //public Pessoa(ContribuinteEmitente emitente)
        //{
        //    if (!string.IsNullOrEmpty(emitente.identificacao.IE))
        //    {
        //        ETipoDocumento = ETipoDocumentoPessoa.InscricaoEstadual;
        //        Documento = emitente.identificacao.IE.RetornaNumeros();
        //    }
        //    else
        //    {
        //        NomeRazaoSocial = emitente.razaoSocial.SanitizeString().SubstringMaxLength(60);
        //        Endereco = new Endereco(cep: emitente.cep,
        //            emitente.endereco,
        //            municipioCodigoIBGE: emitente.municipio);

        //        long.TryParse(emitente.telefone, out long fone);
        //        Telefone = fone;

        //        if (!string.IsNullOrEmpty(emitente.identificacao.CPF))
        //        {
        //            ETipoDocumento = ETipoDocumentoPessoa.CPF;
        //            Documento = emitente.identificacao.CPF.RetornaNumeros();
        //        }
        //        else
        //        {
        //            ETipoDocumento = ETipoDocumentoPessoa.CNPJ;
        //            Documento = emitente.identificacao.CNPJ.RetornaNumeros();
        //        }
        //    }
        //}

        //public Pessoa(dest destinatario)
        //{
        //    PessoaTipo = ETipoPessoa.Juridica;
        //    NomeRazaoSocial = destinatario.xNome.SanitizeString().SubstringMaxLength(60);
        //    ApelidoFantasia = null;
        //    Endereco = new Endereco(destinatario.enderDest);
        //    Telefone = null;
        //    CPFCNPJ = destinatario.CPF ?? destinatario.CNPJ;
        //    RGInscricaoEstadual = destinatario.IE;
        //    Email = destinatario.email;

        //    InformarInscricaoEstadualIsento();
        //}

        public ETipoDocumentoPessoa ETipoDocumento { get; private set; }

        public string Documento { get; private set; }

        public string NomeRazaoSocial { get; private set; }

        public Endereco Endereco { get; private set; }

        public long? Telefone { get; private set; }
    }
}
