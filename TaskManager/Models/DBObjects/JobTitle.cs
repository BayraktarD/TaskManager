using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class JobTitle
    {
        public JobTitle()
        {
            Employees = new HashSet<Employee>();
        }

        public Guid IdJobTitle { get; set; }
        public string JobTitle1 { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
