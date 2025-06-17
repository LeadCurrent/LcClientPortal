using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts
{
    public class TargetDbContext : DbContext
    {
        public TargetDbContext(DbContextOptions<TargetDbContext> options)
       : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Scholls> Schools { get; set; }
        public DbSet<Postalcode> Postalcodes { get; set; }
        public DbSet<PortalStates> PortalStates { get; set; }
        public DbSet<Campus> Campuses { get; set; }

        public DbSet<ClientIdMap> ClientIdMap { get; set; }
        public DbSet<SchoolIdMap> SchoolIdMap { get; set; }
        public DbSet<StateIdMap> StateIdMap { get; set; }
        public DbSet<PostalCodeIdMap> PostalcodeIdMap { get; set; }
        public DbSet<CampusIdMap> CampusIdMap { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //=> options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
    }

}
