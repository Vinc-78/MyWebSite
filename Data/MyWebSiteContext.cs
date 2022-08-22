using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebSite.Models;

namespace MyWebSite.Data
{
    public class MyWebSiteContext : DbContext
    {
        public MyWebSiteContext (DbContextOptions<MyWebSiteContext> options)
            : base(options)
        {
        }

        public DbSet<MyWebSite.Models.Azienda> Azienda { get; set; } = default!;
        public DbSet<MyWebSite.Models.Intervento> Intervento { get; set; } = default!;
    }
}
