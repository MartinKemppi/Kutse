using Kutsung.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kutse_App.Models
{
    public class Kylaline
    {
        public int Id { get; set; }

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
        public List<KylalinePeole> RegistreeritudPeod { get; set; }
    }
}