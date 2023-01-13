using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskManager.Models
{
    public class EmployeeModel
    {
        [Display(Name = "Employee Id")]
        public Guid IdEmployee { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Surname")]
        public string Surname { get; set; } = null!;

        [Display(Name = "User Id")]
        public string? UserId { get; set; }

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
