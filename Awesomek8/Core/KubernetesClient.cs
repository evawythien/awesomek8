using Awesomek8.Dtos;
using Awesomek8.Exceptions;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Awesomek8.Core
{
    public class KubernetesClient : IKubernetesClient
    {
        private readonly Kubernetes client;
        private readonly ILogger<KubernetesClient> logger;

        public KubernetesClient(Kubernetes client, ILogger<KubernetesClient> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<V1Secret> CreateSecrets(Secret secret)
        {
            byte[] fileData = GetFileContent(secret.File);
            ValidateCertificate(fileData, secret.CertificatePassword);

            try
            {
                V1APIGroup group = client.GetAPIGroup();

                V1Secret vsecret = new()
                {
                    ApiVersion = group.ApiVersion,
                    Kind = "Secret",
                    Metadata = new V1ObjectMeta()
                    {
                        Name = secret.Name.ToLower()
                    },
                    Data = new Dictionary<string, byte[]>()
                    {
                        [secret.File.FileName] = fileData
                    }
                };

                return await client.CreateNamespacedSecretAsync(vsecret, secret.Namespace);
            }
            catch (HttpOperationException ex)
            {
                KubernetesError error = JsonConvert.DeserializeObject<KubernetesError>(ex.Response.Content);
                if (error != null)
                    throw new HttpOperationException(error?.Message);
                else
                    throw new HttpOperationException(ex.Message, ex);
            }
        }

        public async Task<V1Ingress> CreateIngress(Ingress ingress)
        {
            try
            {
                V1Ingress newIngress = new()
                {
                    ApiVersion = "networking.k8s.io/v1",
                    Kind = "Ingress",
                    Metadata = new V1ObjectMeta()
                    {
                        Name = ingress.IngressName.ToLower()
                    },
                    Spec = new V1IngressSpec()
                    {
                        Tls = new List<V1IngressTLS>()
                        {
                            new V1IngressTLS()
                            {
                                Hosts = new List<string>() { ingress.Host.ToLower() },
                                SecretName = ingress.SecretName.ToLower()
                            }
                        },
                        Rules = new List<V1IngressRule>
                        {
                            new V1IngressRule()
                            {
                                Host = ingress.Host.ToLower(),
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
                                                    Name = ingress.Service.Name,
                                                    Port = new V1ServiceBackendPort()
                                                    {
                                                        Number = ingress.Service.Port,
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
                KubernetesError error = JsonConvert.DeserializeObject<KubernetesError>(ex.Response.Content);
                if (error != null)
                    throw new HttpOperationException(error.Message, ex);
                else
                    throw new HttpOperationException(ex.Message, ex);
            }
        }

        private static void ValidateCertificate(byte[] fileData, string password)
        {
            X509Certificate2 certificate = new X509Certificate2(fileData, password); // If password is not valid, throw exception.

            bool verify = certificate.Verify();
            if (!verify)
                throw new CertificateException("Invalid certificate");

            if (certificate.NotAfter <= DateTime.Now)
                throw new CertificateException("Expired certificate");
        }

        private static byte[] GetFileContent(IFormFile file)
        {
            if (file.Length > 0)
            {
                using MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                return ms.ToArray();
            }
            return Array.Empty<byte>();
        }
    }
}
