using Data.EntityModelsAndLibraries.Allocation.Models;
using Data.EntityModelsAndLibraries.Area.Models;
using Data.EntityModelsAndLibraries.Campus.Models;
using Data.EntityModelsAndLibraries.Degreeprogram.Models;
using Data.EntityModelsAndLibraries.DownSellOffer.Models;
using Data.EntityModelsAndLibraries.Group.Models;
using Data.EntityModelsAndLibraries.Interest.Models;
using Data.EntityModelsAndLibraries.Leadpost.Models;
using Data.EntityModelsAndLibraries.Level.Models;
using Data.EntityModelsAndLibraries.MasterSchool.Models;
using Data.EntityModelsAndLibraries.Offer.Models;
using Data.EntityModelsAndLibraries.PrepingLog.Models;
using Data.EntityModelsAndLibraries.Program.Models;
using Data.EntityModelsAndLibraries.School.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Data.DataContexts
{
    public class MigrationService
    {
        private readonly CMGContext _cmgsource;
        private readonly ADHEREContext _adheresource;
        private readonly CMContext _cmsource;
        private readonly MSContext _mssource;
        private readonly ACMEDIAContext _acmidiasource;
        private readonly PROMKTContext _promktsource;
        //private readonly TargetDbContext _target;
        public DataContext _target { get; }//current context
        private readonly IConfiguration _configuration;


        public MigrationService(IConfiguration configuration, CMGContext cMGContext, ADHEREContext ADHEREContext, CMContext cMContext, MSContext mSContext, ACMEDIAContext aCMEDIAContext, PROMKTContext pROMKTContext, DataContext Context)
        {
            //_source = source;
            //_target = target;
            _cmgsource = cMGContext;
            _adheresource = ADHEREContext;
            _acmidiasource = aCMEDIAContext;
            _cmsource = cMContext;
            _mssource = mSContext;
            _promktsource = pROMKTContext;

            _target = Context;
            _configuration = configuration;
        }

        public void MigrateAll()
        {
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            var companies = _target.Company.AsNoTracking().ToList();

            var globalClients = new Dictionary<string, Client>();
            var globalSchools = new Dictionary<string, Scholls>();

            Parallel.ForEach(sourceConnections, kvp =>
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    Console.WriteLine($"Company not found for DB: {dbName}");
                    return;
                }

                var companyId = company.Id;

                try
                {
                    // Always create a fresh DbContext per thread
                    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                    optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TargetDb"));
                    using var targetContext = new DataContext(optionsBuilder.Options);

                    using var source = new SourceDbContext(connectionString);

                    Console.WriteLine($"Starting migration for {dbName}");

                    var localMigrationService = new MigrationService(
                        _configuration, _cmgsource, _adheresource, _cmsource, _mssource, _acmidiasource, _promktsource, targetContext
                    );

                    //localMigrationService.MigrateClients(source, companyId, globalClients);
                    //localMigrationService.MigrateSchools(source, companyId, globalSchools);
                    //localMigrationService.MigrateOffers(source, companyId);
                    //localMigrationService.MigrateCampuses(source, companyId);

                    Console.WriteLine($"Finished migrating: {dbName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Migration failed for {dbName}: {ex.Message}");
                }
            });

            // Run once after all parallel tasks
            //MigrateStates();
            //MigratePostalCodes();
            //MigrateLevelsProgramsAndDegreePrograms();
            //MigrateCampusDegrees();
            //MigrateSources();
            //MigrateAllocations();
            //MigrateCampusPostalCodes();
            MigrateDownSellOffers();
            MigrateDownSellOfferPostalCodes();
            //MigrateMasterSchools();
            //MigrateMasterSchoolMappings();
            //MigrateAreas();
            //MigrateProgramAreas();
            //MigrateInterests();
            //MigrateProgramInterests();
            //MigrateGroups();
            //MigrateSchoolGroups();
            //MigrateExtraRequiredEducation();
            //MigrateLeadPosts();
            //MigrateOfferTargeting();
            //MigratePingCache();
            //MigratePortalTargeting();
            //MigrateSearchPortals();
            //MigrateConfigEducationLevels();
            Console.WriteLine("🎉 Full migration complete.");
        }

        #region completed
        private void MigrateClients(SourceDbContext source, int companyId, Dictionary<string, Client> globalClients)
        {
            foreach (var src in source.Clients.ToList())
            {
                var key = src.Name?.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(key)) continue;

                if (!globalClients.TryGetValue(key, out var existing))
                {
                    var newClient = new Client
                    {
                        Name = src.Name,
                        Active = src.Active,
                        Direct = src.Direct,
                        oldId = src.Id
                    };
                    _target.Clients.Add(newClient);
                    _target.SaveChanges();
                    globalClients[key] = newClient;
                    existing = newClient;
                }

                _target.ClientIdMap.Add(new ClientIdMap
                {
                    OldId = src.Id,
                    NewId = existing.Id,
                    CompanyId = companyId
                });
            }
            _target.SaveChanges();
        }

        private void MigrateSchools(SourceDbContext source, int companyId, Dictionary<string, Scholls> globalSchools)
        {
            foreach (var src in source.Schools.ToList())
            {
                var key = src.Name?.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(key)) continue;

                if (!globalSchools.TryGetValue(key, out var existing))
                {
                    var newSchool = new Scholls
                    {
                        Name = src.Name,
                        Abbr = src.Abbr,
                        Website = src.Website,
                        Logo100 = src.Logo100,
                        Maxage = src.Maxage,
                        Minage = src.Minage,
                        Minhs = src.Minhs,
                        Maxhs = src.Maxhs,
                        Notes = src.Notes,
                        Shortcopy = src.Shortcopy,
                        Targeting = src.Targeting,
                        Accreditation = src.Accreditation,
                        Highlights = src.Highlights,
                        Alert = src.Alert,
                        Startdate = src.Startdate,
                        Scoreadjustment = src.Scoreadjustment,
                        Militaryfriendly = src.Militaryfriendly,
                        Disclosure = src.Disclosure,
                        Schoolgroup = src.Schoolgroup,
                        TcpaText = src.Tcpa_Text,
                        oldId = src.Id
                    };
                    _target.Schools.Add(newSchool);
                    _target.SaveChanges();
                    globalSchools[key] = newSchool;
                    existing = newSchool;
                }

                _target.SchoolIdMap.Add(new SchoolIdMap
                {
                    OldId = src.Id,
                    NewId = existing.Id,
                    CompanyId = companyId
                });
            }
            _target.SaveChanges();
        }

        private void MigrateOffers(SourceDbContext source, int companyId)
        {
            var schoolMap = _target.SchoolIdMap.Where(x => x.CompanyId == companyId).ToList();
            var clientMap = _target.ClientIdMap.Where(x => x.CompanyId == companyId).ToList();

            foreach (var src in source.Offers.ToList())
            {
                var newSchoolId = schoolMap.FirstOrDefault(m => m.OldId == src.Schoolid)?.NewId;
                var newClientId = clientMap.FirstOrDefault(m => m.OldId == src.Clientid)?.NewId;

                if (newSchoolId == null || newClientId == null) continue;

                var newOffer = new Offer
                {
                    Schoolid = newSchoolId.Value,
                    Clientid = newClientId.Value,
                    Url = src.Url,
                    Active = src.Active,
                    Rpl = src.Rpl,
                    Dcap = src.Dcap,
                    Dcapamt = src.Dcapamt,
                    Mcap = src.Mcap,
                    Mcapamt = src.Mcapamt,
                    Wcap = src.Wcap,
                    Wcapamt = src.Wcapamt,
                    Type = src.Type,
                    Militaryonly = src.Militaryonly,
                    Nomilitary = src.Nomilitary,
                    Transferphone = src.Transferphone,
                    Lccampaignid = src.Lccampaignid,
                    Archive = src.Archive,
                    EndClient = src.End_Client,
                    CecRplA = src.cec_rplA,
                    CecRplB = src.cec_rplB,
                    CecRplC = src.cec_rplC,
                    CecRplD = src.cec_rplD,
                    CecRplE = src.cec_rplE,
                    CecRplF = src.cec_rplF,
                    CecRplG = src.cec_rplG,
                    DeliveryIdentifier = src.Delivery_Identifier,
                    DeliveryName = src.Delivery_Name,
                    CompanyId = companyId,
                    oldId = src.Id
                };

                _target.Offers.Add(newOffer);
                _target.SaveChanges();

                _target.OfferIdMap.Add(new OfferIdMap
                {
                    OldId = src.Id,
                    NewId = newOffer.Id,
                    CompanyId = companyId
                });
            }
            _target.SaveChanges();
        }

        public void MigrateStates()
        {
            // Step 1: Get all companies from the target DB
            var companies = _target.Company.ToList();

            // Step 2: Get source connection strings (excluding the target)
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                // Step 3: Match the DB name to a company
                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {

                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceStates = source.States.ToList();

                foreach (var sourceState in sourceStates)
                {
                    var newState = new PortalStates
                    {
                        Name = sourceState.Name,
                        Abbr = sourceState.Abbr,
                        Country = sourceState.Country,
                        Timezone = sourceState.Timezone,
                        oldId = sourceState.Id,

                    };

                    _target.PortalStates.Add(newState);
                    _target.SaveChanges();

                    _target.StateIdMap.Add(new StateIdMap
                    {
                        OldId = sourceState.Id,
                        NewId = newState.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                }


            }


        }

        public void MigratePostalCodes()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            try
            {
                foreach (var kvp in sourceConnections)
                {
                    var dbName = kvp.Key;
                    var connectionString = kvp.Value;

                    var company = companies.FirstOrDefault(c => c.Name == dbName);
                    if (company == null)
                    {

                        continue;
                    }

                    var companyId = company.Id;

                    using var source = new SourceDbContext(connectionString);
                    //var sourcePostalCodes = source.Postalcodes.ToList();
                    var sourcePostalCodes = source.Postalcodes.AsNoTracking().ToList();
                    int count = 0;
                    foreach (var sourcePostalCode in sourcePostalCodes)
                    {
                        // Lookup new state ID from StateIdMap
                        var newStateId = _target.StateIdMap
                            .FirstOrDefault(x => x.OldId == sourcePostalCode.Stateid && x.CompanyId == companyId)
                            ?.NewId;

                        if (newStateId == null)
                        {

                            continue;
                        }

                        var newPostalCode = new Postalcode
                        {
                            Code = sourcePostalCode.Code,
                            City = sourcePostalCode.City,
                            Stateid = newStateId.Value,  // Remapped FK
                            Latitude = sourcePostalCode.Latitude,
                            Longitude = sourcePostalCode.Longitude,
                            oldId = sourcePostalCode.Id,
                        };

                        _target.Postalcodes.Add(newPostalCode);
                        _target.SaveChanges();

                        _target.PostalCodeIdMap.Add(new PostalCodeIdMap
                        {
                            OldId = sourcePostalCode.Id,
                            NewId = newPostalCode.Id,
                            CompanyId = companyId
                        });

                        _target.SaveChanges();
                        _target.ChangeTracker.Clear();



                        _target.SaveChanges();
                    }


                }

            }
            catch (Exception ex)
            {

                throw;
            }



        }

        private void MigrateCampuses(SourceDbContext source, int companyId)
        {
            var schoolMap = _target.SchoolIdMap.Where(x => x.CompanyId == companyId).ToList();
            var stateMap = _target.StateIdMap.Where(x => x.CompanyId == companyId).ToList();
            var postalMap = _target.PostalCodeIdMap.Where(x => x.CompanyId == companyId).ToList();

            var sourceCampuses = source.Campuses.AsNoTracking().ToList();

            foreach (var sourceCampus in sourceCampuses)
            {
                try
                {
                    var newSchoolId = schoolMap.FirstOrDefault(x => x.OldId == sourceCampus.Schoolid)?.NewId;
                    var newStateId = stateMap.FirstOrDefault(x => x.OldId == sourceCampus.Stateid)?.NewId;
                    var newPostalcodeId = postalMap.FirstOrDefault(x => x.OldId == sourceCampus.Postalcodeid)?.NewId;

                    if (newSchoolId == null || newPostalcodeId == null)
                    {
                        // We skip if mandatory foreign keys are missing
                        continue;
                    }

                    var newCampus = new Campus
                    {
                        Schoolid = newSchoolId.Value,
                        Name = sourceCampus.Name,
                        Address = sourceCampus.Address,
                        City = sourceCampus.City,
                        PortalStatesid = newStateId, // Nullable
                        Postalcodeid = newPostalcodeId.Value,
                        Campustype = sourceCampus.Campustype,
                        Active = sourceCampus.Active,
                        Copy = sourceCampus.Copy,
                        Clientid = sourceCampus.Clientid,
                        CompanyId = companyId,
                        oldId = sourceCampus.Id
                    };

                    _target.Campuses.Add(newCampus);
                    _target.SaveChanges();

                    _target.CampusIdMap.Add(new CampusIdMap
                    {
                        OldId = sourceCampus.Id,
                        NewId = newCampus.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                }
                catch (Exception ex)
                {
                    // Optionally log error per record
                    Console.WriteLine($"Campus migration error (ID: {sourceCampus.Id}): {ex.Message}");
                }
            }
        }

        public void MigrateLevelsProgramsAndDegreePrograms()
        {
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            var companies = _target.Company.ToList();
            var globalLevels = new Dictionary<string, Level>();
            var globalPrograms = new Dictionary<string, Program>();

            foreach (var (dbName, connectionString) in sourceConnections)
            {
                // Run only for CMG database
                if (!dbName.Equals("CMG", StringComparison.OrdinalIgnoreCase)) continue;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);

                // === Migrate Levels ===
                foreach (var srcLevel in source.Levels.AsNoTracking())
                {
                    var key = srcLevel.Name?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(key)) continue;

                    if (!globalLevels.TryGetValue(key, out var existingLevel))
                    {
                        var newLevel = new Level
                        {
                            Name = srcLevel.Name,
                            Copy = srcLevel.Copy,
                            oldId = srcLevel.Id
                        };
                        _target.Levels.Add(newLevel);
                        _target.SaveChanges();

                        globalLevels[key] = newLevel;
                        existingLevel = newLevel;
                    }

                    _target.LevelsIdMap.Add(new LevelsIdMap
                    {
                        OldId = srcLevel.Id,
                        NewId = existingLevel.Id
                    });
                }
                _target.SaveChanges();

                // === Migrate Programs ===
                foreach (var srcProgram in source.Programs.AsNoTracking())
                {
                    var key = srcProgram.Name?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(key)) continue;

                    if (!globalPrograms.TryGetValue(key, out var existingProgram))
                    {
                        var newProgram = new Program
                        {
                            Name = srcProgram.Name,
                            Copy = srcProgram.Copy,
                            oldId = srcProgram.Id
                        };
                        _target.Programs.Add(newProgram);
                        _target.SaveChanges();

                        globalPrograms[key] = newProgram;
                        existingProgram = newProgram;
                    }

                    _target.ProgramsIdMap.Add(new ProgramsIdMap
                    {
                        OldId = srcProgram.Id,
                        NewId = existingProgram.Id
                    });
                }
                _target.SaveChanges();

                // === Migrate DegreePrograms ===
                var levelMap = _target.LevelsIdMap.ToList();
                var programMap = _target.ProgramsIdMap.ToList();

                foreach (var srcDP in source.degreeprograms.AsNoTracking())
                {
                    var newLevelId = levelMap.FirstOrDefault(x => x.OldId == srcDP.Levelid)?.NewId;
                    var newProgramId = programMap.FirstOrDefault(x => x.OldId == srcDP.Programid)?.NewId;

                    if (newLevelId == null || newProgramId == null)
                    {
                        Console.WriteLine($"Skipping DegreeProgram {srcDP.Id} — missing FK mapping.");
                        continue;
                    }

                    var newDP = new Degreeprogram
                    {
                        Levelid = newLevelId.Value,
                        Programid = newProgramId.Value,
                        Copy = srcDP.Copy,
                        CompanyId = companyId,
                        oldId = srcDP.Id
                    };

                    _target.Degreeprograms.Add(newDP);
                    _target.SaveChanges();

                    _target.DegreeprogramsIdMap.Add(new DegreeprogramsIdMap
                    {
                        OldId = srcDP.Id,
                        NewId = newDP.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                }

                Console.WriteLine("Completed Levels, Programs, DegreePrograms migration for CMG.");
            }
        }

        public void MigrateCampusDegrees()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceCampusDegrees = source.campusdegrees.AsNoTracking().ToList();

                foreach (var sourceCD in sourceCampusDegrees)
                {
                    // FK remapping
                    var newCampusId = _target.CampusIdMap.FirstOrDefault(x => x.OldId == sourceCD.Campusid && x.CompanyId == companyId)?.NewId;

                    var newDegreeProgramId = _target.DegreeprogramsIdMap.FirstOrDefault(x => x.OldId == sourceCD.Degreeid && x.CompanyId == companyId)?.NewId;

                    if (newCampusId == null || newDegreeProgramId == null)
                    {
                        continue;
                    }

                    var newCampusDegree = new Campusdegree
                    {
                        Campusid = newCampusId.Value,
                        Name = sourceCD.Name,
                        Copy = sourceCD.Copy,
                        Active = sourceCD.Active,
                        Clientid = sourceCD.Clientid,
                        Degreeid = newDegreeProgramId.Value,
                        CompanyId = companyId,
                        oldId = sourceCD.Id
                    };

                    _target.Campusdegrees.Add(newCampusDegree);
                    _target.SaveChanges();

                    _target.CampusdegreeIdMap.Add(new CampusdegreeIdMap
                    {
                        OldId = sourceCD.Id,
                        NewId = newCampusDegree.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateSources()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceSources = source.sources.AsNoTracking().ToList();

                foreach (var sourceSource in sourceSources)
                {
                    var newSource = new Source
                    {
                        Name = sourceSource.Name,
                        Active = sourceSource.Active,
                        Apikey = sourceSource.Apikey,
                        Lcsiteid = sourceSource.Lcsiteid,
                        Lcsourceid = sourceSource.Lcsourceid,
                        Accesskey = sourceSource.Accesskey,
                        CompanyId = companyId,
                        oldId = sourceSource.Id
                    };

                    _target.Sources.Add(newSource);
                    _target.SaveChanges();

                    _target.SourceIdMap.Add(new SourceIdMap
                    {
                        OldId = sourceSource.Id,
                        NewId = newSource.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateAllocations()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceAllocations = source.Allocations.AsNoTracking().ToList();

                foreach (var sourceAllocation in sourceAllocations)
                {
                    var newOfferId = _target.OfferIdMap
                        .FirstOrDefault(x => x.OldId == sourceAllocation.Offerid && x.CompanyId == companyId)
                        ?.NewId;

                    var newSourceId = _target.SourceIdMap
                        .FirstOrDefault(x => x.OldId == sourceAllocation.Sourceid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newOfferId == null || newSourceId == null)
                    {
                        continue;
                    }

                    var newAllocation = new Allocation
                    {
                        Offerid = newOfferId.Value,
                        Sourceid = newSourceId.Value,
                        Active = sourceAllocation.Active,
                        Identifier = sourceAllocation.Identifier,
                        Cpl = sourceAllocation.Cpl,
                        Dcap = sourceAllocation.Dcap,
                        Dcapamt = sourceAllocation.Dcapamt,
                        Mcap = sourceAllocation.Mcap,
                        Mcapamt = sourceAllocation.Mcapamt,
                        Wcap = sourceAllocation.Wcap,
                        Wcapamt = sourceAllocation.Wcapamt,
                        Transferphone = sourceAllocation.Transferphone,
                        CecIncludeA = sourceAllocation.Cec_IncludeA,
                        CecIncludeB = sourceAllocation.Cec_IncludeB,
                        CecIncludeC = sourceAllocation.Cec_IncludeC,
                        CecIncludeD = sourceAllocation.Cec_IncludeD,
                        CecIncludeE = sourceAllocation.Cec_IncludeE,
                        CecIncludeF = sourceAllocation.Cec_IncludeF,
                        CecIncludeG = sourceAllocation.Cec_IncludeG,
                        CecCplA = sourceAllocation.Cec_CplA,
                        CecCplB = sourceAllocation.Cec_CplB,
                        CecCplC = sourceAllocation.Cec_CplC,
                        CecCplD = sourceAllocation.Cec_CplD,
                        CecCplE = sourceAllocation.Cec_CplE,
                        CecCplF = sourceAllocation.Cec_CplE,
                        CecCplG = sourceAllocation.Cec_CplG,

                        CompanyId = companyId,
                        oldId = sourceAllocation.Id
                    };

                    _target.Allocations.Add(newAllocation);
                    _target.SaveChanges();

                    _target.AllocationsIdMap.Add(new AllocationsIdMap
                    {
                        OldId = sourceAllocation.Id,
                        NewId = newAllocation.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateCampusPostalCodes()
        {
            const int batchSize = 20000;

            var companies = _target.Company.AsNoTracking().ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);

                int totalRecords = source.campuspostalcodes.Count();
                Console.WriteLine($"{dbName}: Found {totalRecords} campuspostalcode records.");

                for (int offset = 0; offset < totalRecords; offset += batchSize)
                {
                    var batch = source.campuspostalcodes
                        .AsNoTracking()
                        .OrderBy(cp => cp.Id)
                        .Skip(offset)
                        .Take(batchSize)
                        .ToList();

                    var newCampusPostals = new List<Campuspostalcode>();
                    var newMaps = new List<CampuspostalcodesIdMap>();

                    var campusMap = _target.CampusIdMap
                        .Where(x => x.CompanyId == companyId)
                        .ToDictionary(x => x.OldId, x => x.NewId);

                    var postalMap = _target.PostalCodeIdMap
                        .Where(x => x.CompanyId == companyId)
                        .ToDictionary(x => x.OldId, x => x.NewId);

                    foreach (var sourceCP in batch)
                    {
                        if (!campusMap.TryGetValue(sourceCP.Campusid, out var newCampusId)) continue;
                        if (!postalMap.TryGetValue(sourceCP.Postalcodeid, out var newPostalCodeId)) continue;

                        var newCP = new Campuspostalcode
                        {
                            Campusid = newCampusId,
                            Postalcodeid = newPostalCodeId,
                            CompanyId = companyId,
                            oldId = sourceCP.Id
                        };

                        newCampusPostals.Add(newCP);
                    }

                    // Save new campus postal codes
                    _target.Campuspostalcodes.AddRange(newCampusPostals);
                    _target.SaveChanges();

                    foreach (var cp in newCampusPostals)
                    {
                        newMaps.Add(new CampuspostalcodesIdMap
                        {
                            OldId = cp.oldId ?? 0,
                            NewId = cp.Id,
                            CompanyId = companyId
                        });
                    }

                    _target.CampuspostalcodesIdMap.AddRange(newMaps);
                    _target.SaveChanges();

                    _target.ChangeTracker.Clear();

                    Console.WriteLine($"✅ {dbName}: Migrated batch {offset}-{offset + batch.Count}");
                }

                Console.WriteLine($"🎯 {dbName}: Completed MigrateCampusPostalCodes");
            }
        }


        #endregion


        //public void MigrateCampusPostalCodes()
        //{
        //    var companies = _target.Company.ToList();

        //    var sourceConnections = _configuration
        //        .GetSection("ConnectionStrings")
        //        .GetChildren()
        //        .Where(cs => cs.Key != "TargetDb")
        //        .ToDictionary(cs => cs.Key, cs => cs.Value);

        //    foreach (var kvp in sourceConnections)
        //    {
        //        var dbName = kvp.Key;
        //        var connectionString = kvp.Value;

        //        var company = companies.FirstOrDefault(c => c.Name == dbName);
        //        if (company == null)
        //        {
        //            continue;
        //        }

        //        var companyId = company.Id;

        //        using var source = new SourceDbContext(connectionString);

        //        var sourceCPs = source.campuspostalcodes.AsNoTracking().ToList();

        //        foreach (var sourceCP in sourceCPs)
        //        {
        //            var newCampusId = _target.CampusIdMap
        //                .FirstOrDefault(x => x.OldId == sourceCP.Campusid && x.CompanyId == companyId)
        //                ?.NewId;

        //            var newPostalCodeId = _target.PostalCodeIdMap
        //                .FirstOrDefault(x => x.OldId == sourceCP.Postalcodeid && x.CompanyId == companyId)
        //                ?.NewId;

        //            if (newCampusId == null || newPostalCodeId == null)
        //            {
        //                continue;
        //            }

        //            var newCP = new Campuspostalcode
        //            {
        //                Campusid = newCampusId.Value,
        //                Postalcodeid = newPostalCodeId.Value,
        //                CompanyId = companyId,
        //                oldId = sourceCP.Id
        //            };

        //            _target.Campuspostalcodes.Add(newCP);
        //            _target.SaveChanges();

        //            _target.CampuspostalcodesIdMap.Add(new CampuspostalcodesIdMap
        //            {
        //                OldId = sourceCP.Id,
        //                NewId = newCP.Id,
        //                CompanyId = companyId
        //            });

        //            _target.SaveChanges();
        //            _target.ChangeTracker.Clear();

        //            _target.SaveChanges();
        //        }

        //    }

        //}

        //public void MigrateDownSellOffers()
        //{
        //    var companies = _target.Company.ToList();

        //    var sourceConnections = _configuration
        //        .GetSection("ConnectionStrings")
        //        .GetChildren()
        //        .Where(cs => cs.Key != "TargetDb")
        //        .ToDictionary(cs => cs.Key, cs => cs.Value);

        //    foreach (var kvp in sourceConnections)
        //    {
        //        var dbName = kvp.Key;
        //        var connectionString = kvp.Value;

        //        var company = companies.FirstOrDefault(c => c.Name == dbName);
        //        if (company == null)
        //        {
        //            continue;
        //        }

        //        var companyId = company.Id;

        //        using var source = new SourceDbContext(connectionString);
        //        var sourceDownSells = source.DownSellOffers.AsNoTracking().ToList();

        //        foreach (var sourceDownSell in sourceDownSells)
        //        {
        //            var newClientid = _target.ClientIdMap
        //                .FirstOrDefault(x => x.OldId == sourceDownSell.Clientid && x.CompanyId == companyId)
        //                ?.NewId;

        //            if (newClientid == null)
        //            {
        //                continue;
        //            }

        //            var newDownSell = new DownSellOffer
        //            {
        //                Clientid = newClientid.Value,
        //                Formurl = sourceDownSell.Formurl,
        //                Priority = sourceDownSell.Priority,
        //                Dcap = sourceDownSell.Dcap,
        //                Active = sourceDownSell.Active,
        //                Dcapamt = sourceDownSell.Dcapamt,
        //                Mcap = sourceDownSell.Mcap,
        //                Mcapamt = sourceDownSell.Mcapamt,
        //                Wcap = sourceDownSell.Wcap,
        //                Wcapamt = sourceDownSell.Wcapamt,
        //                Type = sourceDownSell.Type,
        //                Transferphone = sourceDownSell.Transferphone,
        //                IncludeUscitizens = sourceDownSell.IncludeUscitizens,
        //                IncludePermanentResidents = sourceDownSell.IncludePermanentResidents,
        //                IncludeGreenCardHolders = sourceDownSell.IncludeGreenCardHolders,
        //                IncludeNonCitizens = sourceDownSell.IncludeNonCitizens,
        //                IncludeInternet = sourceDownSell.IncludeInternet,
        //                IncludeNoInternet = sourceDownSell.IncludeNoInternet,
        //                IncludeMilitary = sourceDownSell.IncludeMilitary,
        //                IncludeNonMilitary = sourceDownSell.IncludeNonMilitary,
        //                Name = sourceDownSell.Name,
        //                Description = sourceDownSell.Description,
        //                MondayActive = sourceDownSell.MondayActive,
        //                MondayStartTime = sourceDownSell.MondayStartTime,
        //                MondayEndTime = sourceDownSell.MondayEndTime,
        //                TuesdayActive = sourceDownSell.TuesdayActive,
        //                TuesdayStartTime = sourceDownSell.TuesdayStartTime,
        //                TuesdayEndTime = sourceDownSell.TuesdayEndTime,
        //                WednesdayActive = sourceDownSell.WednesdayActive,
        //                WednesdayStartTime = sourceDownSell.WednesdayStartTime,
        //                WednesdayEndTime = sourceDownSell.WednesdayEndTime,
        //                ThursdayActive = sourceDownSell.ThursdayActive,
        //                ThursdayStartTime = sourceDownSell.ThursdayEndTime,
        //                FridayActive = sourceDownSell.FridayActive,
        //                FridayStartTime = sourceDownSell.FridayStartTime,
        //                FridayEndTime = sourceDownSell.FridayEndTime,
        //                SaturdayActive = sourceDownSell.SaturdayActive,
        //                SaturdayStartTime = sourceDownSell.SaturdayStartTime,
        //                SaturdayEndTime = sourceDownSell.SaturdayEndTime,
        //                SundayActive = sourceDownSell.SundayActive,
        //                SundayStartTime = sourceDownSell.SundayStartTime,
        //                SundayEndTime = sourceDownSell.SundayEndTime,
        //                Identifier = sourceDownSell.Identifier,
        //                Maxage = sourceDownSell.Maxage,
        //                Minage = sourceDownSell.Minage,

        //                CompanyId = companyId,
        //                oldId = sourceDownSell.Id
        //            };

        //            _target.DownSellOffers.Add(newDownSell);
        //            _target.SaveChanges();

        //            _target.DownSellOffersIdMap.Add(new DownSellOffersIdMap
        //            {
        //                OldId = sourceDownSell.Id,
        //                NewId = newDownSell.Id,
        //                CompanyId = companyId
        //            });

        //            _target.SaveChanges();
        //            _target.ChangeTracker.Clear();

        //            _target.SaveChanges();
        //        }

        //    }

        //}

        //public void MigrateDownSellOfferPostalCodes()
        //{
        //    var companies = _target.Company.ToList();

        //    var sourceConnections = _configuration
        //        .GetSection("ConnectionStrings")
        //        .GetChildren()
        //        .Where(cs => cs.Key != "TargetDb")
        //        .ToDictionary(cs => cs.Key, cs => cs.Value);

        //    foreach (var kvp in sourceConnections)
        //    {
        //        var dbName = kvp.Key;
        //        var connectionString = kvp.Value;

        //        var company = companies.FirstOrDefault(c => c.Name == dbName);
        //        if (company == null)
        //        {
        //            continue;
        //        }

        //        var companyId = company.Id;

        //        using var source = new SourceDbContext(connectionString);
        //        var sourceRows = source.DownSellOfferPostalCodes.AsNoTracking().ToList();

        //        foreach (var row in sourceRows)
        //        {
        //            var newDownSellOfferId = _target.DownSellOffersIdMap.FirstOrDefault(x => x.OldId == row.DownSellOfferId && x.CompanyId == companyId)
        //                ?.NewId;

        //            var newPostalCodeId = _target.PostalCodeIdMap.FirstOrDefault(x => x.OldId == row.Postalcodeid && x.CompanyId == companyId)
        //                ?.NewId;

        //            if (newDownSellOfferId == null || newPostalCodeId == null)
        //            {
        //                continue;
        //            }

        //            var newEntry = new DownSellOfferPostalCode
        //            {
        //                DownSellOfferId = newDownSellOfferId.Value,
        //                Postalcodeid = newPostalCodeId.Value,
        //                CompanyId = companyId,
        //                oldId = row.Id
        //            };

        //            _target.DownSellOfferPostalCodes.Add(newEntry);
        //            _target.SaveChanges();

        //            _target.DownSellOfferPostalCodesIdMap.Add(new DownSellOfferPostalCodesIdMap
        //            {
        //                OldId = row.Id,
        //                NewId = newEntry.Id,
        //                CompanyId = companyId
        //            });

        //            _target.SaveChanges();
        //            _target.ChangeTracker.Clear();

        //            _target.SaveChanges();
        //        }

        //    }

        //}
        public void MigrateDownSellOffers()
        {
            const int batchSize = 5000;

            var companies = _target.Company.AsNoTracking().ToList();
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Global: uniqueKey => DownSellOffer (with ID set post-save)
            var globalUniqueKeyToEntity = new Dictionary<string, DownSellOffer>();

            // Final map collection
            var allDownSellOffersIdMaps = new List<DownSellOffersIdMap>();

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;
                using var source = new SourceDbContext(connectionString);

                var clientMap = _target.ClientIdMap
                    .Where(x => x.CompanyId == companyId)
                    .ToDictionary(x => x.OldId, x => x.NewId);

                int total = source.DownSellOffers.Count();

                for (int offset = 0; offset < total; offset += batchSize)
                {
                    var batch = source.DownSellOffers
                        .AsNoTracking()
                        .OrderBy(x => x.Id)
                        .Skip(offset)
                        .Take(batchSize)
                        .ToList();

                    var newEntities = new List<DownSellOffer>();
                    var mapsForThisCompany = new List<DownSellOffersIdMap>();
                    var pendingKeys = new List<string>();

                    foreach (var src in batch)
                    {
                        if (!clientMap.TryGetValue(src.Clientid, out var newClientid))
                            continue;

                        string key = $"{src.Name?.Trim().ToLower()}|{newClientid}";

                        if (!globalUniqueKeyToEntity.TryGetValue(key, out var existingEntity))
                        {
                            // New global entry, insert later
                            var entity = new DownSellOffer
                            {
                                Clientid = newClientid,
                                Formurl = src.Formurl,
                                Priority = src.Priority,
                                Dcap = src.Dcap,
                                Active = src.Active,
                                Dcapamt = src.Dcapamt,
                                Mcap = src.Mcap,
                                Mcapamt = src.Mcapamt,
                                Wcap = src.Wcap,
                                Wcapamt = src.Wcapamt,
                                Type = src.Type,
                                Transferphone = src.Transferphone,
                                IncludeUscitizens = src.IncludeUscitizens,
                                IncludePermanentResidents = src.IncludePermanentResidents,
                                IncludeGreenCardHolders = src.IncludeGreenCardHolders,
                                IncludeNonCitizens = src.IncludeNonCitizens,
                                IncludeInternet = src.IncludeInternet,
                                IncludeNoInternet = src.IncludeNoInternet,
                                IncludeMilitary = src.IncludeMilitary,
                                IncludeNonMilitary = src.IncludeNonMilitary,
                                Name = src.Name,
                                Description = src.Description,
                                MondayActive = src.MondayActive,
                                MondayStartTime = src.MondayStartTime,
                                MondayEndTime = src.MondayEndTime,
                                TuesdayActive = src.TuesdayActive,
                                TuesdayStartTime = src.TuesdayStartTime,
                                TuesdayEndTime = src.TuesdayEndTime,
                                WednesdayActive = src.WednesdayActive,
                                WednesdayStartTime = src.WednesdayStartTime,
                                WednesdayEndTime = src.WednesdayEndTime,
                                ThursdayActive = src.ThursdayActive,
                                ThursdayStartTime = src.ThursdayStartTime,
                                ThursdayEndTime = src.ThursdayEndTime,
                                FridayActive = src.FridayActive,
                                FridayStartTime = src.FridayStartTime,
                                FridayEndTime = src.FridayEndTime,
                                SaturdayActive = src.SaturdayActive,
                                SaturdayStartTime = src.SaturdayStartTime,
                                SaturdayEndTime = src.SaturdayEndTime,
                                SundayActive = src.SundayActive,
                                SundayStartTime = src.SundayStartTime,
                                SundayEndTime = src.SundayEndTime,
                                Identifier = src.Identifier,
                                Maxage = src.Maxage,
                                Minage = src.Minage,
                                CompanyId = companyId,
                                oldId = src.Id
                            };

                            newEntities.Add(entity);
                            globalUniqueKeyToEntity[key] = entity; // Store reference
                            pendingKeys.Add(key); // Remember to update ID later

                            mapsForThisCompany.Add(new DownSellOffersIdMap
                            {
                                OldId = src.Id,
                                NewId = -1, // placeholder
                                CompanyId = companyId
                            });
                        }
                        else
                        {
                            // Use existing mapped entity ID
                            mapsForThisCompany.Add(new DownSellOffersIdMap
                            {
                                OldId = src.Id,
                                NewId = existingEntity.Id,
                                CompanyId = companyId
                            });
                        }
                    }

                    // Save only new ones
                    _target.DownSellOffers.AddRange(newEntities);
                    _target.SaveChanges();

                    // Update NewId in global dict
                    foreach (var key in pendingKeys)
                    {
                        var ent = globalUniqueKeyToEntity[key];
                        if (ent.Id > 0)
                        {
                            // Backfill NewId into current batch maps
                            var mapsToFix = mapsForThisCompany.Where(x => x.NewId == -1 && x.OldId == ent.oldId && x.CompanyId == companyId);
                            foreach (var map in mapsToFix)
                                map.NewId = ent.Id;
                        }
                    }

                    allDownSellOffersIdMaps.AddRange(mapsForThisCompany);
                    _target.ChangeTracker.Clear();
                }

                Console.WriteLine($"✅ Company {companyId} finished DownSellOffers.");
            }

            // Save all IdMap records
            _target.DownSellOffersIdMap.AddRange(allDownSellOffersIdMaps);
            _target.SaveChanges();

            Console.WriteLine($"🎯 Inserted {allDownSellOffersIdMaps.Count} rows into DownSellOffersIdMap.");
        }






        public void MigrateDownSellOfferPostalCodes()
        {
            var companies = _target.Company.AsNoTracking().ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                    continue;

                var companyId = company.Id;
                using var source = new SourceDbContext(connectionString);

                var sourceRows = source.DownSellOfferPostalCodes.AsNoTracking().ToList();
                if (!sourceRows.Any())
                    continue;

                var offerMap = _target.DownSellOffersIdMap
                    .Where(m => m.CompanyId == companyId)
                    .ToDictionary(m => m.OldId, m => m.NewId);

                var postalMap = _target.PostalCodeIdMap
                    .Where(m => m.CompanyId == companyId)
                    .ToDictionary(m => m.OldId, m => m.NewId);

                var existingEntries = _target.DownSellOfferPostalCodes
                    .Where(e => e.CompanyId == companyId)
                    .Select(e => new { e.DownSellOfferId, e.Postalcodeid })
                    .ToHashSet();

                var newEntries = new List<DownSellOfferPostalCode>();
                var newMaps = new List<DownSellOfferPostalCodesIdMap>();

                foreach (var row in sourceRows)
                {
                    if (!offerMap.TryGetValue(row.DownSellOfferId, out var newOfferId) ||
                        !postalMap.TryGetValue(row.Postalcodeid, out var newPostalId))
                        continue;

                    if (existingEntries.Contains(new { DownSellOfferId = newOfferId, Postalcodeid = newPostalId }))
                        continue;

                    var entry = new DownSellOfferPostalCode
                    {
                        DownSellOfferId = newOfferId,
                        Postalcodeid = newPostalId,
                        CompanyId = companyId,
                        oldId = row.Id
                    };
                    newEntries.Add(entry);
                }

                // Insert and generate ID map
                _target.DownSellOfferPostalCodes.AddRange(newEntries);
                _target.SaveChanges();

                foreach (var e in newEntries)
                {
                    newMaps.Add(new DownSellOfferPostalCodesIdMap
                    {
                        OldId = e.oldId ?? 0,
                        NewId = e.Id,
                        CompanyId = companyId
                    });
                }

                _target.DownSellOfferPostalCodesIdMap.AddRange(newMaps);
                _target.SaveChanges();
                _target.ChangeTracker.Clear();

                Console.WriteLine($"✅ {dbName}: Migrated {newEntries.Count} DownSellOfferPostalCodes.");
            }
        }
        public void MigrateMasterSchools()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.master_schools.AsNoTracking().ToList();

                foreach (var sourceItem in sourceRows)
                {
                    var newItem = new MasterSchool
                    {
                        Name = sourceItem.Name,                        
                        CompanyId = companyId,
                        oldId = sourceItem.Id
                    };

                    _target.MasterSchools.Add(newItem);
                    _target.SaveChanges();

                    _target.Master_schoolsIdMap.Add(new Master_schoolsIdMap
                    {
                        OldId = sourceItem.Id,
                        NewId = newItem.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateMasterSchoolMappings()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.master_school_mappings.AsNoTracking().ToList();

                foreach (var sourceRow in sourceRows)
                {
                    var newMasterSchoolId = _target.Master_schoolsIdMap
                        .FirstOrDefault(x => x.OldId == sourceRow.master_schools_id && x.CompanyId == companyId)?.NewId;

                   

                    if (newMasterSchoolId == null)
                    {
                        continue;
                    }

                    var newMapping = new MasterSchoolMapping
                    {
                        MasterSchoolsId = newMasterSchoolId.Value,
                        Identifier = sourceRow.Identifier,
                        CompanyId = companyId,
                        oldId = sourceRow.Id
                    };

                    _target.MasterSchoolMappings.Add(newMapping);
                    _target.SaveChanges();

                    _target.Master_school_mappingsIdMap.Add(new Master_school_mappingsIdMap
                    {
                        OldId = sourceRow.Id,
                        NewId = newMapping.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateAreas()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceAreas = source.Areas.AsNoTracking().ToList();

                foreach (var sourceArea in sourceAreas)
                {
                    var newArea = new Area
                    {
                        Name = sourceArea.Name,                        
                        Copy = sourceArea.Copy,
                        CompanyId = companyId,
                        oldId = sourceArea.Id
                    };

                    _target.Areas.Add(newArea);
                    _target.SaveChanges();

                    _target.AreasIdMap.Add(new AreasIdMap
                    {
                        OldId = sourceArea.Id,
                        NewId = newArea.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }

        public void MigrateProgramAreas()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.programareas.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newProgramId = _target.ProgramsIdMap
                        .FirstOrDefault(x => x.OldId == row.Programid)
                        ?.NewId;

                    var newAreaId = _target.AreasIdMap
                        .FirstOrDefault(x => x.OldId == row.Areaid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newProgramId == null || newAreaId == null)
                    {
                        continue;
                    }

                    var newEntry = new Programarea
                    {
                        Programid = newProgramId.Value,
                        Areaid = newAreaId.Value,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.Programareas.Add(newEntry);
                    _target.SaveChanges();

                    _target.ProgramareasIdMap.Add(new ProgramareasIdMap
                    {
                        OldId = row.Id,
                        NewId = newEntry.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }
        public void MigrateInterests()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceInterests = source.interests.AsNoTracking().ToList();

                foreach (var interest in sourceInterests)
                {
                    var newInterest = new Interest
                    {
                        Name = interest.Name,
                        Copy = interest.Copy,
                        CompanyId = companyId,
                        oldId = interest.Id
                    };

                    _target.Interests.Add(newInterest);
                    _target.SaveChanges();

                    _target.InterestsIdMap.Add(new InterestsIdMap
                    {
                        OldId = interest.Id,
                        NewId = newInterest.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }

        public void MigrateProgramInterests()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.programinterests.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newProgramId = _target.ProgramsIdMap
                        .FirstOrDefault(x => x.OldId == row.Programid)
                        ?.NewId;

                    var newInterestId = _target.InterestsIdMap
                        .FirstOrDefault(x => x.OldId == row.Interestid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newProgramId == null || newInterestId == null)
                    {
                        continue;
                    }

                    var newEntry = new Programinterest
                    {
                        Programid = newProgramId.Value,
                        Interestid = newInterestId.Value,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.Programinterests.Add(newEntry);
                    _target.SaveChanges();

                    _target.PrograminterestsIdMap.Add(new PrograminterestsIdMap
                    {
                        OldId = row.Id,
                        NewId = newEntry.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }

        public void MigrateGroups()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;
                using var source = new SourceDbContext(connectionString);
                var sourceGroups = source.groups.AsNoTracking().ToList();

                foreach (var group in sourceGroups)
                {
                    var newGroup = new Group
                    {
                        Name = group.Name,
                        Copy = group.Copy,
                        CompanyId = companyId,
                        oldId = group.Id
                    };

                    _target.Groups.Add(newGroup);
                    _target.SaveChanges();

                    _target.GroupIdMap.Add(new GroupIdMap
                    {
                        OldId = group.Id,
                        NewId = newGroup.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }
        public void MigrateSchoolGroups()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.schoolgroups.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newSchoolId = _target.SchoolIdMap
                        .FirstOrDefault(x => x.OldId == row.Schoolid && x.CompanyId == companyId)
                        ?.NewId;

                    var newGroupId = _target.GroupIdMap
                        .FirstOrDefault(x => x.OldId == row.Groupid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newSchoolId == null || newGroupId == null)
                    {
                        continue;
                    }

                    var newEntry = new Schoolgroup
                    {
                        Schoolid = newSchoolId.Value,
                        Groupid = newGroupId.Value,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.Schoolgroups.Add(newEntry);
                    _target.SaveChanges();

                    _target.SchoolGroupsIdMap.Add(new SchoolGroupsIdMap
                    {
                        OldId = row.Id,
                        NewId = newEntry.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateExtraRequiredEducation()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)//run only for one company becouse the data is same
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.extrarequirededucation.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    //var newDegreeId = _target.DegreeprogramsIdMap
                    //    .FirstOrDefault(x => x.OldId == row.Degreeid && x.CompanyId == companyId)
                    //    ?.NewId;

                    //var newCampusId = _target.CampusIdMap
                    //    .FirstOrDefault(x => x.OldId == row.Campusid && x.CompanyId == companyId)
                    //    ?.NewId;                    



                    var newEntry = new Extrarequirededucation
                    {
                        Degreeid = row.Degreeid,
                        Campusid = row.Campusid,
                        Value = row.Value,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.Extrarequirededucations.Add(newEntry);
                    _target.SaveChanges();


                }
                break;//run only for one company becouse the data is same

            }

        }

        public void MigrateLeadPosts()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.leadposts.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newSchoolId = _target.SchoolIdMap
                        .FirstOrDefault(x => x.OldId == row.Schoolid && x.CompanyId == companyId)
                        ?.NewId ?? row.Schoolid;//add same when null only for this tbl

                    var newSourceId = _target.SourceIdMap
                        .FirstOrDefault(x => x.OldId == row.Sourceid && x.CompanyId == companyId)
                        ?.NewId ?? row.Sourceid;//add same when null only for this tbl

                    var newOfferId = _target.OfferIdMap
                       .FirstOrDefault(x => x.OldId == row.Offerid && x.CompanyId == companyId)
                       ?.NewId ?? row.Offerid;//add same when null only for this tbl

                    var newProgramId = _target.ProgramsIdMap
                       .FirstOrDefault(x => x.OldId == row.Programid)
                       ?.NewId ?? row.Programid;//add same when null only for this tbl

                    var newCampusId = _target.CampusIdMap
                       .FirstOrDefault(x => x.OldId == row.Campusid && x.CompanyId == companyId)
                       ?.NewId ?? row.Campusid;//add same when null only for this tbl


                    if (newSchoolId == null || newSourceId == null)
                    {
                        continue;
                    }

                    var newEntry = new Leadpost
                    {
                        Schoolid = newSchoolId.Value,
                        Sourceid = newSourceId.Value,
                        Offerid = newOfferId.Value,
                        Programid = newProgramId.Value,
                        Campusid = newCampusId.Value,

                        Parameterstring = row.Parameterstring,
                        Serverresponse = row.Serverresponse,
                        Testflag = row.Testflag,
                        Testparameter = row.Testparameter,
                        Success = row.Success,
                        Ipaddress = row.Ipaddress,
                        Date = row.Date,
                        Vamidentifier = row.Vamidentifier,
                        Zip = row.Zip,
                        Campusname = row.Campusname,
                        Programname = row.Programname,
                        Clientname = row.Clientname,
                        Offername = row.Offername,
                        Agent = row.Agent,


                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.Leadposts.Add(newEntry);
                    _target.SaveChanges();

                    _target.LeadpostsIdMap.Add(new LeadpostsIdMap
                    {
                        OldId = row.Id,
                        NewId = newEntry.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }
        public void MigrateOfferTargeting()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.offertargeting.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newOfferId = _target.OfferIdMap
                        .FirstOrDefault(x => x.OldId == row.Offerid && x.CompanyId == companyId)
                        ?.NewId ?? row.Offerid; // fallback if mapping not found

                    var newEntry = new Offertargeting
                    {
                        Offerid = newOfferId,
                        CitizenIncludeUscitizens = row.Citizen_IncludeUscitizens,
                        CitizenIncludePermanentResidents = row.Citizen_IncludePermanentResidents,
                        CitizenIncludeGreenCardHolders = row.Citizen_IncludeGreenCardHolders,
                        CitizenIncludeOther = row.Citizen_IncludeOther,
                        InternetIncludeInternet = row.Internet_IncludeInternet,
                        InternetIncludeNoInternet = row.Internet_IncludeNoInternet,
                        MilitaryIncludeMilitary = row.Military_IncludeMilitary,
                        MilitaryIncludeNonMilitary = row.Military_IncludeNonMilitary,
                        StudentMinHighSchoolGradYear = row.Student_MinHighSchoolGradYear,
                        StudentMaxHighSchoolGradYear = row.Student_MaxHighSchoolGradYear,
                        StudentMinAge = row.Student_MinAge,
                        StudentMaxAge = row.Student_MaxAge,
                        LeadIpAddressRequired = row.Lead_IpAddressRequired,
                        MondayActive = row.Monday_Active,
                        TuesdayActive = row.Tuesday_Active,
                        WednesdayActive = row.Wednesday_Active,
                        ThursdayActive = row.Thursday_Active,
                        FridayActive = row.Friday_Active,
                        SaturdayActive = row.Saturday_Active,
                        SundayActive = row.Sunday_Active,
                        MondayStart = row.Monday_Start,
                        TuesdayStart = row.Tuesday_Start,
                        WednesdayStart = row.Wednesday_Start,
                        ThursdayStart = row.Thursday_Start,
                        FridayStart = row.Friday_Start,
                        SaturdayStart = row.Saturday_Start,
                        SundayStart = row.Sunday_Start,
                        MondayEnd = row.Monday_End,
                        TuesdayEnd = row.Tuesday_End,
                        WednesdayEnd = row.Wednesday_End,
                        ThursdayEnd = row.Thursday_End,
                        FridayEnd = row.Friday_End,
                        SaturdayEnd = row.Saturday_End,
                        SundayEnd = row.Sunday_End,
                        CecIncludeA = row.Cec_IncludeA,
                        CecIncludeB = row.Cec_IncludeB,
                        CecIncludeC = row.Cec_IncludeC,
                        CecIncludeD = row.Cec_IncludeD,
                        CecIncludeE = row.Cec_IncludeE,
                        CecIncludeF = row.Cec_IncludeF,
                        CecIncludeG = row.Cec_IncludeG,
                        CecCplA = row.Cec_CplA,
                        CecCplB = row.Cec_CplB,
                        CecCplC = row.Cec_CplC,
                        CecCplD = row.Cec_CplD,
                        CecCplE = row.Cec_CplE,
                        CecCplF = row.Cec_CplF,
                        CecCplG = row.Cec_CplG,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.Offertargetings.Add(newEntry);
                    _target.SaveChanges();

                    _target.OffertargetingIdMap.Add(new OffertargetingIdMap
                    {
                        OldId = row.Id,
                        NewId = newEntry.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }

        public void MigratePingCache()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.ping_cache.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newOfferId = _target.SourceIdMap
                        .FirstOrDefault(x => x.OldId == row.Source_Id && x.CompanyId == companyId)
                        ?.NewId;

                    if (newOfferId == null)
                    {
                        continue;
                    }

                    var newEntry = new PingCache
                    {
                        SourceId = newOfferId.Value,
                        PingSignature = row.Ping_Signature,
                        PingResponse = row.Ping_Response,
                        Allowed = row.Allowed,
                        Date = row.Date,
                        Email = row.Email,
                        Phone = row.Phone,

                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.PingCaches.Add(newEntry);
                    _target.SaveChanges();

                    _target.Ping_cacheIdMap.Add(new Ping_cacheIdMap
                    {
                        OldId = row.Id,
                        NewId = newEntry.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }

            }

        }

        public void MigratePortalTargeting()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.portaltargeting.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {


                    var newEntry = new Portaltargeting
                    {
                        Portalid = row.Portalid,
                        CitizenIncludeUscitizens = row.Citizen_IncludeUscitizens,
                        CitizenIncludePermanentResidents = row.Citizen_IncludePermanentResidents,
                        CitizenIncludeGreenCardHolders = row.Citizen_IncludeGreenCardHolders,
                        CitizenIncludeOther = row.Citizen_IncludeOther,
                        InternetIncludeInternet = row.Internet_IncludeInternet,
                        InternetIncludeNoInternet = row.Internet_IncludeNoInternet,
                        MilitaryIncludeMilitary = row.Military_IncludeMilitary,
                        MilitaryIncludeNonMilitary = row.Military_IncludeNonMilitary,
                        StudentMinHighSchoolGradYear = row.Student_MinHighSchoolGradYear,
                        StudentMaxHighSchoolGradYear = row.Student_MaxHighSchoolGradYear,
                        StudentMinAge = row.Student_MinAge,
                        StudentMaxAge = row.Student_MaxAge,
                        LeadIpAddressRequired = row.Lead_IpAddressRequired,
                        MondayActive = row.Monday_Active,
                        MondayStart = row.Monday_Start,
                        MondayEnd = row.Monday_End,
                        TuesdayActive = row.Tuesday_Active,
                        TuesdayStart = row.Tuesday_Start,
                        TuesdayEnd = row.Tuesday_End,
                        WednesdayEnd = row.Wednesday_End,
                        WednesdayActive = row.Wednesday_Active,
                        WednesdayStart = row.Wednesday_Start,
                        ThursdayActive = row.Thursday_Active,
                        ThursdayEnd = row.Thursday_End,
                        ThursdayStart = row.Thursday_Start,
                        FridayActive = row.Friday_Active,
                        FridayStart = row.Friday_Start,
                        FridayEnd = row.Friday_End,
                        SaturdayActive = row.Saturday_Active,
                        SaturdayStart = row.Saturday_Start,
                        SaturdayEnd = row.Saturday_End,
                        SundayActive = row.Sunday_Active,
                        SundayStart = row.Sunday_Start,
                        SundayEnd = row.Sunday_End
                    };

                    _target.Portaltargetings.Add(newEntry);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }
                break;//run only for one company becouse the data is same
            }

        }
        public void MigrateSearchPortals()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.searchportals.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newEntry = new Searchportal
                    {
                        Name = row.Name,
                        Url = row.Url,
                        Active = row.Active,
                        Transfers = row.Transfers,
                        Leads = row.Leads,
                        Rank = row.Rank,

                    };

                    _target.Searchportals.Add(newEntry);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();

                }
                break;//run only for one company becouse the data is same

            }

        }

        public void MigrateConfigEducationLevels()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    continue;
                }

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.tblConfigEducationLevels.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newEntry = new TblConfigEducationLevel
                    {
                        Identifier = row.Identifier,
                        Value = row.Value,
                        Label = row.Label,
                    };

                    _target.TblConfigEducationLevels.Add(newEntry);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();
                    _target.SaveChanges();
                }
                break;//run only for one company becouse the data is same
            }


        }

    }

}
