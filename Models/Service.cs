using System.ComponentModel.DataAnnotations;

namespace Basics.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty; // fitness, yoga, pilates vb.

        [Required(ErrorMessage = "Süre zorunludur.")]
        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Ücret zorunludur.")]
        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; }
    }
}
