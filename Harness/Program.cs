using Keyfactor;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Harness
{
    class Program
    {
        static void Main(string[] args)
        {
            Client c = new Client("http://192.168.78.139/KeyfactorAPI", "RE9NQUlOXHVzZXI6UGFzc3dvcmQ=");

            PFXEnrollmentResponse pfxResp = c.PostPFXEnrollAsync("PFX", new PFXEnrollmentRequest()
            {
                Subject="CN=CSharpTest",
                Timestamp = DateTime.UtcNow,
                Template = "WebServer",
                IncludeChain = false,
                PopulateMissingValuesFromAD = false
            }).Result;
            Console.WriteLine(pfxResp.CertificateInformation.Thumbprint);

            c.UpdateMetadataAsync(null, new MetadataUpdateRequest()
            {
                Id = pfxResp.CertificateInformation.KeyfactorId.Value,
                Metadata = new Dictionary<string, string>() { { "Email-Contact", "keyfactor@example.com" } }
            });

            CertificateRetrievalResponse resp = c.GetCertificateAsync(pfxResp.CertificateInformation.KeyfactorId.Value, null, null, null, 4).Result;
            Console.WriteLine(Convert.ToBase64String(resp.ContentBytes));
            Console.WriteLine(resp.Metadata["Email-Contact"]);
        }
    }
}
