using System.ComponentModel.DataAnnotations;

namespace Kutse_App.Models
{
    public class Guest
    {
        [Required(ErrorMessage = "Sisesta nimi")]
        public string Nimi { get; set; }

        [Required(ErrorMessage = "Sisesta email")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Valesti sisestatud email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Sisesta telefoni number")]
        [RegularExpression(@"\+372.+", ErrorMessage = "Numbri alguses peal olema +372")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Sisesta valik")]
        public bool? Tuleb { get; set; }
    }
}