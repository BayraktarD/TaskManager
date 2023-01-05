using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class AttachmentTypeModel
    {
        [Display(Name = "Attachment Type Id")]
        public Guid IdAttachmentType { get; set; }

        [Display(Name = "Attachment Type")]
        public string Type { get; set; } = null!;

    }
}
