using System;
using System.Collections.Generic;

namespace TaskManager.Models.DBObjects
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public Guid IdUserType { get; set; }
        public string UserType1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
