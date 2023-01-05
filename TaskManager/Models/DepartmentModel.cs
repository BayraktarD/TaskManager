using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskManager.Models
{
    public class DepartmentModel
    {
        [Display(Name = "Department Id")]
        public Guid IdDepartment { get; set; }

        [Display(Name = "Department Name")]
        public string Department1 { get; set; } = null!;

    }
}
