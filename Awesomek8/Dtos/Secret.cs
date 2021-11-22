using Microsoft.AspNetCore.Http;

namespace Awesomek8.Dtos
{
    public class Secret
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public IFormFile File { get; set; }
        public string CertificatePassword { get; set; }
    }
}
