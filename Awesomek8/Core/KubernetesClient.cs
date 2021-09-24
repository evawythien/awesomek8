using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Awesomek8.Core
{
    public class KubernetesClient<TOptions> : IKubernetesClient where TOptions : KubernetesOptions
    {

        private readonly TOptions options;
        private readonly Kubernetes client;
        private readonly ILogger<KubernetesClient<KubernetesOptions>> logger;

        public KubernetesClient(TOptions options, Kubernetes client, ILogger<KubernetesClient<KubernetesOptions>> logger)
        {
            this.options = options;
            this.client = client;
            this.logger = logger;
        }

        public async Task<V1Secret> CreateSecrets(string secretName)
        {
            try
            {
                V1APIGroup group = client.GetAPIGroup();

                V1Secret vsecret = new V1Secret()
                {
                    ApiVersion = group.ApiVersion,

                    Kind = "Secret",
                    Metadata = new V1ObjectMeta()
                    {
                        Name = secretName.ToLower()
                    }
                };

                V1Secret secret = await client.CreateNamespacedSecretAsync(vsecret, "default");


                return secret;
            }
            catch (HttpOperationException e)
            {
                Console.WriteLine(e.Response.Content);

                var jsonString = JsonConvert.DeserializeObject<KubernetesError>(e.Response.Content);
                return null;
            }
        }


        public async Task<V1CertificateSigningRequest> CreateCertificate(byte[] archive)
        {
            try
            {
                V1APIGroup group = client.GetAPIGroup();

                V1CertificateSigningRequest certificate = new V1CertificateSigningRequest()
                {
                    ApiVersion = group.ApiVersion,

                    Kind = "Secret",
                    Metadata = new V1ObjectMeta()
                    {
                        Name = "awesomesecret"
                    },
                    Spec = new V1CertificateSigningRequestSpec()
                    {
                        Request = archive,                        
                    }
                };

                V1CertificateSigningRequest secret = await client.CreateCertificateSigningRequestAsync(certificate);


                return secret;
            }
            catch (HttpOperationException e)
            {
                Console.WriteLine(e.Response.Content);

                var jsonString = JsonConvert.DeserializeObject<KubernetesError>(e.Response.Content);
                return null;
            }
        }


        public async Task<V1CertificateSigningRequest> CreateIngress()
        {
            try
            {
                V1APIGroup group = client.GetAPIGroup();

                V1CertificateSigningRequest certificate = new V1CertificateSigningRequest()
                {
                    ApiVersion = group.ApiVersion,

                    Kind = "Secret",
                    Metadata = new V1ObjectMeta()
                    {
                        Name = "awesomesecret"
                    }

                };



                return certificate;
            }
            catch (HttpOperationException e)
            {
                Console.WriteLine(e.Response.Content);

                var jsonString = JsonConvert.DeserializeObject<KubernetesError>(e.Response.Content);
                return null;
            }
        }
    }
}
