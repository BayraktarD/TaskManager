using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class JobTitle
    {
        public JobTitle()
        {
            Users = new HashSet<User>();
        }

        public Guid IdJobTitle { get; set; }
        public string JobTitle1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
