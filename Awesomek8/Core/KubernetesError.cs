namespace Awesomek8.Core
{
    public class KubernetesError
    {
        public string Kind { get; set; }
        public string ApiVersion { get; set; }
        public object Metadata { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public KubernetesErrorDetail Details { get; set; }
        public string Code { get; set; }
    }

    public class KubernetesErrorDetail
    {
        public string Name { get; set; }
        public string Kind { get; set; }
    }
}
