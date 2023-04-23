using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LABB1.Models
{
    public class Leave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveId { get; set; }
        [Required]
        [DisplayName("Type of leave")]
        public string LeaveType { get; set; }
        [Required]
        [DisplayName("Leave starts")]
        public DateTime LeaveStart { get; set; }
        [Required]
        [DisplayName("Leave ends")]
        public DateTime LeaveStop { get; set; }
        [DisplayName("Leave apply date")]
        public DateTime LeaveApply { get; set; } = DateTime.Now;
        [ForeignKey("Employees")]
        [DisplayName("Employee ID")]
        public int FK_EmployeeId { get; set; }
        public virtual Employee? Employees { get; set; }
        [NotMapped]
        [DisplayName("How long the leave is in days")]
        public int NumDays => (LeaveStop - LeaveStart).Days;
    }
}
