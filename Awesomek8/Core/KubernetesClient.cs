using Awesomek8.Dtos;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<V1Secret> CreateSecrets(Secret secret)
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
                        Name = secret.Name.ToLower(),
                        CreationTimestamp = DateTime.Now
                    },
                    Data = new Dictionary<string, byte[]>()
                    {
                        [secret.File.FileName] = GetFileContent(secret.File)
                    }
                };

                return await client.CreateNamespacedSecretAsync(vsecret, secret.Namespace);
            }
            catch (HttpOperationException ex)
            {
                var error = JsonConvert.DeserializeObject<KubernetesError>(ex.Response.Content);
                throw new Exception(error.Message);
            }
        }

        public async Task<V1Status> DeleteSecret(string secretName, string namespaceName)
        {
            try
            {
                V1APIGroup group = client.GetAPIGroup();

                V1DeleteOptions deleteOptions = new V1DeleteOptions()
                {
                    ApiVersion = group.ApiVersion,
                    Kind = "Secret"
                };

                return await client.DeleteNamespacedSecretAsync(secretName, namespaceName, deleteOptions);
            }
            catch (HttpOperationException ex)
            {
                var error = JsonConvert.DeserializeObject<KubernetesError>(ex.Response.Content);
                throw new Exception(error.Message);
            }
        }


        public async Task<V1Ingress> CreateIngress(Ingress ingress)
        {
            try
            {
                V1Ingress newIngress = new V1Ingress()
                {
                    ApiVersion = "networking.k8s.io/v1",
                    Kind = "Ingress",
                    Metadata = new V1ObjectMeta()
                    {
                        Name = ingress.IngressName.ToLower()
                    },
                    Spec = new V1IngressSpec()
                    {
                        //Tls = new List<V1Ingress>() 
                        //{  
                        //   new V1Ingress(){ se} 
                        //}
                        Rules = new List<V1IngressRule>
                        {
                            new V1IngressRule()
                            {
                                Host = ingress.Host,
                                Http = new V1HTTPIngressRuleValue()
                                {
                                    Paths = new List<V1HTTPIngressPath>()
                                    {
                                        new V1HTTPIngressPath()
                                        {
                                            PathType="Prefix",
                                            Path = "/" ,
                                            Backend = new V1IngressBackend()
                                            {
                                                Service = new V1IngressServiceBackend()
                                                {
                                                    Name = "awesome8-http",

                                                    Port = new V1ServiceBackendPort()
                                                    {
                                                        Name = "http",
                                                        Number = 80
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                return await client.CreateNamespacedIngressAsync(newIngress, ingress.Namespace);
            }
            catch (HttpOperationException ex)
            {
                var error = JsonConvert.DeserializeObject<KubernetesError>(ex.Response.Content);
                throw new Exception(error.Message);
            }
        }


        private byte[] GetFileContent(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            return null;
        }
    }
}
