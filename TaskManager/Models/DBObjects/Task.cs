using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class Task
    {
        public Task()
        {
            TaskAttachments = new HashSet<TaskAttachment>();
        }

        public Guid IdTask { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool EditableStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool EditableEndDate { get; set; }
        public Guid? AssignedToId { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTime? FinishedDate { get; set; }
        public bool IsActive { get; set; }
        public string Details { get; set; } = null!;
        public bool HasAttachments { get; set; }

        public virtual User? AssignedTo { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
        public virtual User? ModifiedBy { get; set; }
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
    }
}
