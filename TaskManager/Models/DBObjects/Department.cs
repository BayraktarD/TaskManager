using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public Guid IdDepartment { get; set; }
        public string Department1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
