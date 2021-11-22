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
        private IKubernetesClient Client { get; set; }

        public KubernetesController(IKubernetesClient client)
        {
            this.Client = client;
        }

        [HttpPost("secret")]
        public async Task<V1Secret> CreateSecrets([FromForm] Secret secret)
        {
            return await Client.CreateSecrets(secret);
        }

        [HttpPost("ingress")]
        public async Task<V1Ingress> CreateIngress(Ingress ingress)
        {
            return await Client.CreateIngress(ingress);
        }
    }
}
