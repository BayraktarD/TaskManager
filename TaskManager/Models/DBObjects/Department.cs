using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public Guid IdDepartment { get; set; }
        public string Department1 { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
