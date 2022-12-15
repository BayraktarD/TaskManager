namespace TaskManager.Models
{
    public class UserModel
    {
        public Guid IdUser { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid UserType { get; set; }
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

    }
}
