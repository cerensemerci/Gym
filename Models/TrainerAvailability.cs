using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basics.Models
{
    public class TrainerAvailability
    {
        [Key]
        public int AvailabilityID { get; set; }

        [Required]
        public int TrainerID { get; set; }
        
        [ForeignKey("TrainerID")]
        public Trainer Trainer { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string Status { get; set; } = "Available"; // Available, Booked, Approved

        public int? ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        public decimal Price { get; set; }
        public int Duration { get; set; }

        public int? EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}
