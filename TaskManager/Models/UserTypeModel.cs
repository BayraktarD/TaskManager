using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskManager.Models
{
    public class UserTypeModel
    {
        [Display(Name = "User Type Id")]
        public Guid IdUserType { get; set; }

        [Display(Name = "User Type")]
        public string UserType1 { get; set; } = null!;

    }
}
