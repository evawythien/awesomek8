using Awesomek8.Core;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public async Task<V1Secret> GetSecretsAsync(string secretName)
        {
            return await client.CreateSecrets(secretName);
        }
    }
}
