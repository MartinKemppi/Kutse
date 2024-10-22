using Kutse_App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kutsung.Models
{
    public class KylalinePeole
    {
        public int KylalinePeoleId { get; set; }
        public int KylalineId { get; set; }
        public int PeoId { get; set; }

        public virtual Kylaline Kylaline { get; set; }
        public virtual Peo Peo { get; set; }
    }
}