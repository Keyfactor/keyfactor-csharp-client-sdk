# Community supported 
We welcome contributions.
 
The Keyfactor C# Client SDK is open source and community supported, meaning that there is **no SLA** applicable for these tools.

To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

# Keyfactor C# Client SDK
Client SDK in C# for the Keyfactor Web API

The SDK includes the request/response structures and methods to make a web request to any endpoint supported by the Keyfactor Platform Web API. The entire source code is in [KeyfactorClientSDK/Client.cs](KeyfactorClientSDK/Client.cs); this is auto-generated by Visual Studio 2019 in conjunction with a Keyfactor-maintained tool. Usage examples are shown in the "Harness" project and below:

```
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
```
