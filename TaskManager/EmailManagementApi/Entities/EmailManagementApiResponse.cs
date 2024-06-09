using System.Net;

namespace TaskManager.EmailManagementApi.Entities
{
    public class EmailManagementApiResponse
    {
        public int SentEmailId { get; set; } = default!;
        public string? EventTypeString { get; set; }
        public string MessageId { get; set; } = default!;
        public HttpStatusCode? StatusCode { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
        public string EmailManagementApiKey { get; set; } = default!;
    }
}
