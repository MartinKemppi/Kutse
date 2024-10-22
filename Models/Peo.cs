using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Kutsung.Models
{
    public class Peo
    {
        public int Id { get; set; }
        public string Pidu { get; set; }
        public DateTime Kuupaev { get; set; }
        public List<KylalinePeole> RegistreeritudPeod { get; set; }
    }
}