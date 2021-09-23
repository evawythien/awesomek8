using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awesomek8.Core
{
    public interface IKubernetesClient
    {
        Task<string> GetSecretsAsync();
    }
}
