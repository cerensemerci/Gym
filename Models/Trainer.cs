using System.ComponentModel.DataAnnotations;

namespace Basics.Models
{
    public class Trainer
    {
        public int TrainerID { get; set; }
        
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}".Trim();

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        public string Specialty { get; set; } = string.Empty;

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<TrainerAvailability> TrainerAvailabilities { get; set; } = new List<TrainerAvailability>();
    }
}