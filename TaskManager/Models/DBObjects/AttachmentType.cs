using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class AttachmentType
    {
        public Guid IdAttachmentType { get; set; }
        public string Type { get; set; } = null!;
    }
}
