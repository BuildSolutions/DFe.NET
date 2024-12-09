using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Shared.DFe.Utils;
using SignatureZeus = DFe.Classes.Assinatura.Signature;

namespace DFe.Utils.Assinatura
{
    public class AssinaturaDigital
    {
        public static SignatureZeus Assina<T>(T objeto, string id, X509Certificate2 certificado) where T : class
        {
            //https://github.com/ZeusAutomacao/DFe.NET/issues/673
            //Consegui descobrir o q era.É que eu uso o framework 4.7.1, e a partir desta versão o valor default da propriedade DigestMethod passou a ser SHA256, causando o erro.
            //Basta adicionar uma configuração no app.config, forçando tal propriedade a ser SHA1. Segue o link para maiores informações:
            //https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/retargeting/4.7-4.7.1

            return Assina(objeto, id, certificado, cfgServicoRemoverAcentos: false);
        }

        public static SignatureZeus Assina<T>(T objeto, string id, X509Certificate2 certificado,
            string signatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1",
            string digestMethod = "http://www.w3.org/2000/09/xmldsig#sha1",
            bool cfgServicoRemoverAcentos = false) where T : class
        {
            var objetoLocal = objeto;
            if (id == null)
                throw new Exception("Não é possível assinar um objeto evento sem sua respectiva Id!");

            var documento = new XmlDocument { PreserveWhitespace = true };

            documento.LoadXml(cfgServicoRemoverAcentos
                ? FuncoesXml.ClasseParaXmlString(objetoLocal).RemoverAcentos()
                : FuncoesXml.ClasseParaXmlString(objetoLocal));

            var docXml = new SignedXml(documento) { SigningKey = certificado.PrivateKey };

            docXml.SignedInfo.SignatureMethod = signatureMethod;
            var reference = new Reference { Uri = "#" + id, DigestMethod = digestMethod };

            // adicionando EnvelopedSignatureTransform a referencia
            var envelopedSigntature = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(envelopedSigntature);

            var c14Transform = new XmlDsigC14NTransform();
            reference.AddTransform(c14Transform);

            docXml.AddReference(reference);

            // carrega o certificado em KeyInfoX509Data para adicionar a KeyInfo
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));

            docXml.KeyInfo = keyInfo;
            docXml.ComputeSignature();

            //// recuperando a representacao do XML assinado
            var xmlDigitalSignature = docXml.GetXml();
            var assinatura = FuncoesXml.XmlStringParaClasse<Classes.Assinatura.Signature>(xmlDigitalSignature.OuterXml);
            return assinatura;
        }
    }
}