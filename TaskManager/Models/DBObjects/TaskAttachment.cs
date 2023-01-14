using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class TaskAttachment
    {
        public Guid IdTask { get; set; }
        public Guid IdAttachment { get; set; }
        public byte[] Attachment { get; set; } = null!;
        public string AttachmentName { get; set; } = null!;

        public virtual Task IdTaskNavigation { get; set; } = null!;
    }
}
