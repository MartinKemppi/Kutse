using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Kutse_App.Models;

namespace Kutsung.Models
{
    public class GuestContext : DbContext
    {
        public DbSet<Kylaline> Kylalised { get; set; }
        public DbSet<Peo> Peod { get; set; }
        public DbSet<KylalinePeole> KylalisedPeole { get; set; }
    }
}