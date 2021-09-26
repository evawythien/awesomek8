using Awesomek8.Dtos;
using k8s.Models;
using System.Threading.Tasks;

namespace Awesomek8.Core
{
    public interface IKubernetesClient
    {
        Task<V1Secret> CreateSecrets(Secret secret);
        Task<V1Status> DeleteSecret(string secretName, string namespaceName);
        Task<V1Ingress> CreateIngress(Ingress ingress);
    }
}
