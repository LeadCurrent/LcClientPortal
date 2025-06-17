using Data.DataContexts.ModelForSourceDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts
{
    public class PROMKTContext : DbContext
    {
        public PROMKTContext(DbContextOptions<PROMKTContext> options)
      : base(options)
        {
        }
        public DbSet<ClientsNew> Clients { get; set; }
        public DbSet<Schools> Schools { get; set; }
        public DbSet<Postalcode> Postalcodes { get; set; }
        public DbSet<PortalStates> States { get; set; }
        public DbSet<Campus> Campuses { get; set; }
    }
}
