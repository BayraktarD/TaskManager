namespace TaskManager.Models
{
    public class TaskAttachmentModel
    {
        public Guid IdTask { get; set; }
        public Guid IdAttachment { get; set; }
        public byte[] Attachment { get; set; } = null!;
        public Guid AttachmentType { get; set; }

    }
}
