using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskModel
    {
        public Guid IdTask { get; set; }
        public Guid CreatedById { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public bool EditableStartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool? EditableEndDate { get; set; }
        public Guid? AssignedToId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ModificationDate { get; set; }
        public Guid? ModifiedById { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? FinishedDate { get; set; }
        public bool IsActive { get; set; }

        [StringLength(1000, ErrorMessage = "String too long(max. 1000 chars)")]
        public string Details { get; set; } = null!;
        public bool HasAttachments { get; set; }

    }
}
