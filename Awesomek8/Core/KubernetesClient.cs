using Dapr.Client;
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
        private readonly DaprClient client;
        private readonly ILogger<KubernetesClient<KubernetesOptions>> logger;

        public KubernetesClient(TOptions options, DaprClient client, ILogger<KubernetesClient<KubernetesOptions>> logger)
        {
            this.options = options;
            this.client = client;
            this.logger = logger;
        }

        public async Task<string> GetSecretsAsync()
        {

            var client = await this.client.GetSecretAsync(
                    "kubernetes", // Name of the Dapr Secret Store
                    "super-secret", // Name of the k8s secret
                    new Dictionary<string, string>() { { "namespace", "default" } }); // Namespace where the k8s secret is deployed


            return "hh";
        }
    }
}
