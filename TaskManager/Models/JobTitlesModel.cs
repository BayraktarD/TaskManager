using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    [Display(Name ="Job Title Model")]
    public class JobTitlesModel
    {
        [Display(Name = "Job Title Id")]
        public Guid IdJobTitle { get; set; }

        [Display(Name ="Job Title")]
        public string JobTitle1 { get; set; } = null!;

    }
}
