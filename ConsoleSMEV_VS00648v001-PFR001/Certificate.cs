using System;
using System.Security.Cryptography.X509Certificates;


namespace ConsoleSMEV_VS00648v001_PFR001
{
    static class Certificate
    {
        public static X509Certificate2 GetCert()
        {
            string SubjectKeyIdentifier = Parametrs.Get("Certificate:SubjectKeyIdentifier");
            X509Store certStore = new X509Store(
                                        StoreName.My,
                                        StoreLocation.CurrentUser
                                  );
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = certStore.Certificates.Find(
                                                    X509FindType.FindBySubjectKeyIdentifier,
                                                    SubjectKeyIdentifier,
                                                    false
                                                );
            if (certs.Count == 0)
            {
                throw new Exception($"Сертификат с идентификатором ключа субъекта \"{SubjectKeyIdentifier}\" не найден среди установленных.");
            }
            return certs[0];
        }
    }
}
