using System.ComponentModel.DataAnnotations;

namespace Basics.Models
{
    public class Gym
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        [Display(Name = "Salon Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres zorunludur.")]
        [Display(Name = "Adres")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Çalışma saatleri zorunludur.")]
        [Display(Name = "Çalışma Saatleri")]
        public string WorkingHours { get; set; } = "08:00 - 22:00";
    }
}
