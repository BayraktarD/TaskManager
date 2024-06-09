namespace TaskManager.EmailManagementApi.Entities
{
    public class IncomingEmailAttachment
    {
        public string FileName { get; set; }
        public List<byte> Attachment { get; set; }
    }
}
