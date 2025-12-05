using System.ComponentModel.DataAnnotations; 
namespace Basics.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad en az 2, en fazla 50 karakter olmalıdır.")]
        [Display(Name = "Adınız")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad en az 2, en fazla 50 karakter olmalıdır.")]
        [Display(Name = "Soyadınız")]
        public string LastName { get; set; } = string.Empty;


        [Display(Name = "Ad Soyad")]
        public string FullName => $"{FirstName} {LastName.ToUpper()}";

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta Adresi")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yaş alanı zorunludur.")]
        [Range(18, 65, ErrorMessage = "Yaş 18 ile 65 arasında olmalıdır.")]
        [Display(Name = "Yaş")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Uzmanlık alanı en az 2, en fazla 100 karakter olmalıdır.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string Specialty { get; set; } = string.Empty;
    }
}
    
        