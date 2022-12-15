using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class AttachmentType
    {
        public AttachmentType()
        {
            TaskAttachments = new HashSet<TaskAttachment>();
        }

        public Guid IdAttachmentType { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
    }
}
