using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskManager.Models
{
    public class TaskAttachmentModel
    {
        [Display(Name = "Id Task")]
        public Guid IdTask { get; set; }

        [Display(Name = "Attachment Id")]
        public Guid IdAttachment { get; set; }

        [Display(Name = "Attachment")]
        public byte[] Attachment { get; set; } = null!;

        [Display(Name = "Attachment Type")]
        public Guid AttachmentType { get; set; }

    }
}
