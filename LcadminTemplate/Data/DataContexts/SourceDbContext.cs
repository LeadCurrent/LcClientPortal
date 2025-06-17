using Data.DataContexts.ModelForOldSourceDB;
using Data.DataContexts.ModelForSourceDB;
using Data.EntityModelsAndLibraries.TblConfigEducationLevel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataContexts
{
    public class SourceDbContext : DbContext
    {
        private readonly string _connectionString;

        public SourceDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<ClientsNew> Clients { get; set; }
        public DbSet<Schools> Schools { get; set; }
        public DbSet<OffersNew> Offers { get; set; }
        public DbSet<PostalcodesNew> Postalcodes { get; set; }
        public DbSet<PortalStatesNew> States { get; set; }
        public DbSet<CampusesNew> Campuses { get; set; }
        public DbSet<LevelsNew> Levels { get; set; }
        public DbSet<ProgramNew> Programs { get; set; }
        public DbSet<DegreeprogramNew> degreeprograms { get; set; }
        public DbSet<CampusdegreeNew> campusdegrees { get; set; }
        public DbSet<SourceNew> sources { get; set; }
        public DbSet<AllocationNew> Allocations { get; set; }
        public DbSet<CampuspostalcodeNew> campuspostalcodes { get; set; }
        public DbSet<DownSellOfferNew> DownSellOffers { get; set; }
        public DbSet<DownSellOfferPostalCodeNew> DownSellOfferPostalCodes { get; set; }
        public DbSet<MasterSchoolNew> master_schools { get; set; }
        public DbSet<MasterSchoolMappingNew> master_school_mappings { get; set; }
        public DbSet<AreaNew> Areas { get; set; }
        public DbSet<ProgramareaNew> programareas { get; set; }
        public DbSet<InterestNew> interests { get; set; }
        public DbSet<PrograminterestNew> programinterests { get; set; }
        public DbSet<GroupNew> groups { get; set; }
        public DbSet<SchoolgroupNew> schoolgroups { get; set; }
        public DbSet<ExtrarequirededucationIdMap> extrarequirededucation { get; set; }
        public DbSet<LeadpostsNew> leadposts { get; set; }
        public DbSet<OffertargetingNew> offertargeting { get; set; }
        public DbSet<PingcacheNew> ping_cache { get; set; }
        public DbSet<PortaltargetingNew> portaltargeting { get; set; }
        public DbSet<SearchportalsNew> searchportals { get; set; }
        public DbSet<TblConfigEducationLevelsNew> tblConfigEducationLevels { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
         
        //    optionsBuilder.UseSqlServer(_connectionString,sql => sql.MaxBatchSize(500));
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, sql =>
            {
                sql.CommandTimeout(600);  // 10 minutes for heavy reads
            });

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }

}
