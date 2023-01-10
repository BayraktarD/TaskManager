using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class Employee
    {
        public Employee()
        {
            TaskAssignedTos = new HashSet<Task>();
            TaskCreatedBies = new HashSet<Task>();
            TaskModifiedBies = new HashSet<Task>();
        }

        public Guid IdEmployee { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public bool IsActive { get; set; }
        public string Email { get; set; } = null!;
        public bool CanCreateTasks { get; set; }
        public bool CanAssignTasks { get; set; }
        public bool CanUnassignTasks { get; set; }
        public bool CanDeleteTasks { get; set; }
        public bool CanActivateProfiles { get; set; }
        public bool CanDeactivateProfiles { get; set; }
        public bool CanModifyTasks { get; set; }
        public Guid JobTitle { get; set; }
        public Guid Department { get; set; }

        public virtual Department DepartmentNavigation { get; set; } = null!;
        public virtual JobTitle JobTitleNavigation { get; set; } = null!;
        public virtual ICollection<Task> TaskAssignedTos { get; set; }
        public virtual ICollection<Task> TaskCreatedBies { get; set; }
        public virtual ICollection<Task> TaskModifiedBies { get; set; }
    }
}
