﻿using System;
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
        public Guid AssignedToId { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTime? FinishedDate { get; set; }
        public bool IsActive { get; set; }
        public string TaskDetails { get; set; } = null!;
        public bool HasAttachments { get; set; }
        public string? SolutionDetails { get; set; }
        public string? TaskName { get; set; }

        public virtual Employee AssignedTo { get; set; } = null!;
        public virtual Employee CreatedBy { get; set; } = null!;
        public virtual Employee? ModifiedBy { get; set; }
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
    }
}
