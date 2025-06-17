using Data.DataContexts.ModelForSourceDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts
{
    public class CMGContext : DbContext
    {
        public CMGContext(DbContextOptions<CMGContext> options)
       : base(options)
        {
        }
        public DbSet<ClientsNew> Clients { get; set; }
        public DbSet<Schools> Schools { get; set; }
        public DbSet<Postalcode> Postalcodes { get; set; }
        public DbSet<PortalStates> States { get; set; }
        public DbSet<Campus> Campuses { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer("Server=tcp:lcdotnet.database.windows.net,1433;Database=CMG;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;User Id=lc-dotnet2024;Password=wQ7emaILDcxfw7q;max pool size=5000;TransparentNetworkIPResolution=False;");
    }
}
