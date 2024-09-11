namespace CapstoneProject.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        string Test { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
