using System.Collections;
using System.Numerics;
using System.Configuration;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.IO;
using System.Text;

namespace helpers.certificate
{
    public class electronicCertificate
    {
        public static void generate(string pass, string nya, out byte[] PFX, out byte[] CER)
        {
            var kpgen = new RsaKeyPairGenerator();

            SecureRandom rand = new SecureRandom();

            kpgen.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(rand, 1024));

            kpgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            var cerKp = kpgen.GenerateKeyPair();

            IDictionary attrsEmpresa = new Hashtable();
            attrsEmpresa[X509Name.DC] = "DocV Sign";
            attrsEmpresa[X509Name.CN] = "DocV Sign";
            attrsEmpresa[X509Name.O] = "DocV Sign";

            IDictionary attrsEmpleado = new Hashtable();
            attrsEmpleado[X509Name.DC] = nya;
            attrsEmpleado[X509Name.CN] = nya;
            attrsEmpleado[X509Name.O] = nya;

            IList ord = new ArrayList();
            ord.Add(X509Name.CN);
            ord.Add(X509Name.O);

            X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();

            certGen.SetSerialNumber(Org.BouncyCastle.Math.BigInteger.One);
            certGen.SetIssuerDN(new X509Name(ord, attrsEmpresa));
            certGen.SetNotBefore(DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)));
            certGen.SetNotAfter(DateTime.Today.AddDays(365));
            certGen.SetSubjectDN(new X509Name(ord, attrsEmpleado));
            certGen.SetPublicKey(cerKp.Public);
            certGen.SetSignatureAlgorithm("SHA1WithRSA");
            certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(false));
            certGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, true, new AuthorityKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(cerKp.Public)));
            X509Certificate x509 = certGen.Generate(cerKp.Private);

            System.Security.Cryptography.X509Certificates.X509Certificate x509_ = DotNetUtilities.ToX509Certificate(x509.CertificateStructure);
            System.Security.Cryptography.X509Certificates.X509Certificate2 x509__ = new System.Security.Cryptography.X509Certificates.X509Certificate2(x509_);

            MemoryStream p12Stream = new MemoryStream();
            Pkcs12Store p12 = new Pkcs12Store();
            p12.SetKeyEntry(Guid.NewGuid().ToString(), new AsymmetricKeyEntry(cerKp.Private), new X509CertificateEntry[] { new X509CertificateEntry(x509) });
            p12.Save(p12Stream, pass.ToCharArray(), rand);

            byte[] _arrPublicKey = x509_.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Cert);

            PFX = p12Stream.ToArray();
            CER = _arrPublicKey;

        }


    }
}