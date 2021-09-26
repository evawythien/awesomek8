using Awesomek8.Core;
using Awesomek8.Dtos;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Awesomek8.Controller
{
    [Route("[controller]")]
    [Controller]
    [ApiController]
    public class KubernetesController : ControllerBase
    {
        private IKubernetesClient client { get; set; }

        public KubernetesController(IKubernetesClient client)
        {
            this.client = client;
        }

        [HttpPost("secret")]
        public async Task<V1Secret> CreateSecrets([FromForm] Secret secret)
        {
            return await client.CreateSecrets(secret);
        }

        [HttpDelete("secret")]
        public async Task<V1Status> DeleteSecret(string secretName, string namespaceName)
        {
            return await client.DeleteSecret(secretName, namespaceName);
        }

        [HttpPost("ingress")]
        public async Task<V1Ingress> CreateIngress(Ingress ingress)
        {
            return await client.CreateIngress(ingress);
        }
    }
}
