using k8s.Models;
using System.Threading.Tasks;

namespace Awesomek8.Core
{
    public interface IKubernetesClient
    {
        Task<V1Secret> CreateSecrets(string secretName);
    }
}
