using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskManager.Models
{
    public class UserModel
    {
        [Display(Name = "User Id")]
        public Guid IdUser { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; } = null!;

        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        public Guid UserType { get; set; }

        [Display(Name = "User Type")]
        public string? UserTypeString { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Can Create Tasks")]
        public bool CanCreateTasks { get; set; }

        [Display(Name = "Can Assign Tasks")]
        public bool CanAssignTasks { get; set; }

        [Display(Name = "Can Unassign Tasks")]
        public bool CanUnassignTasks { get; set; }

        [Display(Name = "Can Delete Tasks")]
        public bool CanDeleteTasks { get; set; }

        [Display(Name = "Can Activate Profiles")]
        public bool CanActivateProfiles { get; set; }

        [Display(Name = "Can Deactivate Profiles")]
        public bool CanDeactivateProfiles { get; set; }

        [Display(Name = "Can Modify Tasks")]
        public bool CanModifyTasks { get; set; }

        public Guid JobTitle { get; set; }
        [Display(Name = "Job Title")]
        public string? JobTitleString { get; set; }

        public Guid Department { get; set; }
        [Display(Name = "Department")]
        public string? DepartmentString { get; set; }
    }
}
