using Dapr.Client;
using k8s;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<string> GetSecretsAsync()
        {

            //var client = await this.client.CreateCertificateSigningRequest();
           
            return "hh";
        }
    }
}
