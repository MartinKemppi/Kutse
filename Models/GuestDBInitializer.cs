using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Kutse_App.Models;

namespace Kutsung.Models
{
    public class GuestDBInitializer : CreateDatabaseIfNotExists<GuestContext> //DropCreateDatabaseAlways<GuestContext>
    {
        protected override void Seed(GuestContext context)
        {
            base.Seed(context);
        }
    }
}