namespace Awesomek8.Dtos
{
    public class Ingress
    {
        public string Namespace { get; set; }
        public string IngressName { get; set; }
        public string SecretName { get; set; }
        public string Host { get; set; }
        public IngressService Service { get; set; }
    }

    public class IngressService
    {
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
