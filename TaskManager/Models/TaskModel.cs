using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskModel
    {
        [Display(Name = "Task Id")]
        public Guid IdTask { get; set; }

        [Display(Name = "Created By")]
        public Guid CreatedById { get; set; }

        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Editable Start Date")]
        public bool EditableStartDate { get; set; }


        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Editable End Date")]
        public bool EditableEndDate { get; set; }

        public Guid? AssignedToId { get; set; }

        [Display(Name = "Assigned To")]
        public string? AssignedToString { get; set; }

        [Display(Name = "Modified At")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ModificationDate { get; set; }

        [Display(Name = "Modified By")]
        public Guid? ModifiedById { get; set; }

        [Display(Name = "Finished Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FinishedDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Details")]
        [StringLength(1000, ErrorMessage = "String too long(max. 1000 chars)")]
        public string Details { get; set; } = null!;

        [Display(Name = "Has Attachments")]
        public bool HasAttachments { get; set; }

        //public IEnumerable<SelectListItem> Users { get; set; }

    }
}
