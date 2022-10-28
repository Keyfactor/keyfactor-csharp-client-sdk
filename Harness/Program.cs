/******************************************************************************/
/* Copyright 2022 Keyfactor                                                   */
/* Licensed under the Apache License, Version 2.0 (the "License"); you may    */
/* not use this file except in compliance with the License.  You may obtain a */
/* copy of the License at http://www.apache.org/licenses/LICENSE-2.0.  Unless */
/* required by applicable law or agreed to in writing, software distributed   */
/* under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES   */
/* OR CONDITIONS OF ANY KIND, either express or implied. See the License for  */
/* the specific language governing permissions and limitations under the      */
/* License.                                                                   */
/******************************************************************************/
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
