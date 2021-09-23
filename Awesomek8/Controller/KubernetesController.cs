using Awesomek8.Core;
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
        public async Task<string> GetSecretsAsync()
        {
            var secretValues = await client.GetSecretsAsync();

            // Get the secret value
            //var secretValue = secretValues["super-secret"];

            //context.Response.ContentType = "application/json";
            /// var ff = await JsonSerializer.SerializeAsync(context.Response.Body, secretValue);
            return "hola";
        }
    }
}
