// MigrationService

using Data.DataContexts.ModelForSourceDB;
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
            var globalGroups = new Dictionary<string, Group>();
            //MigrateClients();
            //MigrateSchools2();
            //MigrateStates();
            //MigratePostalCodes();
            //MigrateCampuses();

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

                    //localMigrationService.MigrateClients(source, companyId, globalClients); // Don't Use this method created a new one below
                    //localMigrationService.MigrateSchools(source, companyId, globalSchools);  // Completed
                    //localMigrationService.MigrateOffers(source, companyId);  // Completed
                    //localMigrationService.MigrateGroups(source, companyId, globalGroups); // Completed
                    //localMigrationService.MigrateSchoolGroups(source, companyId); // Completed
                    //localMigrationService.MigrateCampuses2(source, companyId); // Don't Use this method created a new one above

                    Console.WriteLine($"Finished migrating: {dbName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Migration failed for {dbName}: {ex.Message}");
                }
            });

            // Run once after all parallel tasks

            #region Validated Tables
            //MigrateLevelsProgramsAndDegreePrograms();
            //MigrateCampusDegrees();
            //MigrateSources();
            //MigrateAllocations();
            //MigrateCampusPostalCodes();
            //MigrateDownSellOffers();
            //MigrateDownSellOfferPostalCodes();
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

            #endregion

            Console.WriteLine("🎉 Full migration complete.");
        }



        #region completed
        public void MigrateClients()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Step 1: Track unique clients by name (lowercased)
            var globalClients = new Dictionary<string, Client>();

            // Step 2: Index existing clients from target DB
            var existingClientsByName = _target.Clients
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .ToList()
                .GroupBy(c => c.Name.Trim().ToLower())
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceClients = source.Clients.ToList(); // ClientsNew class

                foreach (var src in sourceClients)
                {
                    var key = src.Name?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(key)) continue;

                    Client matchedClient;

                    // Step 3: Check in global memory first
                    if (!globalClients.TryGetValue(key, out matchedClient))
                    {
                        // Step 4: Check in DB-level map
                        if (!existingClientsByName.TryGetValue(key, out matchedClient))
                        {
                            // Step 5: Create and save new Client
                            matchedClient = new Client
                            {
                                Name = src.Name,
                                Active = src.Active,
                                Direct = src.Direct,
                                oldId = src.Id
                            };

                            _target.Clients.Add(matchedClient);
                            _target.SaveChanges();

                            existingClientsByName[key] = matchedClient;
                        }

                        globalClients[key] = matchedClient;
                    }

                    // Step 6: Add ClientIdMap if not already mapped
                    bool alreadyMapped = _target.ClientIdMap
                        .Any(m => m.OldId == src.Id && m.CompanyId == companyId);

                    if (!alreadyMapped)
                    {
                        _target.ClientIdMap.Add(new ClientIdMap
                        {
                            OldId = src.Id,
                            NewId = matchedClient.Id,
                            CompanyId = companyId
                        });
                    }
                }

                _target.SaveChanges();
            }

            Console.WriteLine("✅ Client migration completed from all sources.");
        }
        public void MigrateSchools2()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Track global schools by normalized name
            var globalSchools = new Dictionary<string, Scholls>();

            // Index existing schools in target
            var existingSchoolsByName = _target.Schools
                .AsNoTracking()
                .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                .ToList()
                .GroupBy(s => s.Name.Trim().ToLower())
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceSchools = source.Schools.ToList();

                foreach (var src in sourceSchools)
                {
                    var key = src.Name?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(key)) continue;

                    Scholls matchedSchool;

                    if (!globalSchools.TryGetValue(key, out matchedSchool))
                    {
                        if (!existingSchoolsByName.TryGetValue(key, out matchedSchool))
                        {
                            matchedSchool = new Scholls
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

                            _target.Schools.Add(matchedSchool);
                            _target.SaveChanges();

                            existingSchoolsByName[key] = matchedSchool;
                        }

                        globalSchools[key] = matchedSchool;
                    }

                    // ✅ Corrected mapping to SchoolIdMap instead of ClientIdMap
                    bool alreadyMapped = _target.SchoolIdMap
                        .Any(m => m.OldId == src.Id && m.CompanyId == companyId);

                    if (!alreadyMapped)
                    {
                        _target.SchoolIdMap.Add(new SchoolIdMap
                        {
                            OldId = src.Id,
                            NewId = matchedSchool.Id,
                            CompanyId = companyId
                        });
                    }
                }

                _target.SaveChanges();
            }

            Console.WriteLine("✅ School migration completed from all sources.");
        }
        private void MigrateSchools(SourceDbContext source, int companyId, Dictionary<string, Scholls> globalSchools)
        {
            // Load existing schools by normalized name
            var existingSchoolsByName = _target.Schools
                .AsNoTracking()
                .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                .ToList()
                .GroupBy(s => s.Name.Trim().ToLower())
                .ToDictionary(g => g.Key, g => g.First());

            // Load existing mappings to prevent duplicate mappings
            var existingMappings = _target.SchoolIdMap
                .Where(m => m.CompanyId == companyId)
                .Select(m => m.OldId)
                .ToHashSet();

            // Step 1: Loop through source schools
            foreach (var src in source.Schools.AsNoTracking().ToList())
            {
                var key = src.Name?.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(key)) continue;

                // Step 2: Skip if already mapped
                if (existingMappings.Contains(src.Id)) continue;

                Scholls matchedSchool;

                // Step 3: Try global memory
                if (!globalSchools.TryGetValue(key, out matchedSchool))
                {
                    // Step 4: Try DB-level memory
                    if (!existingSchoolsByName.TryGetValue(key, out matchedSchool))
                    {
                        // Step 5: Insert new
                        matchedSchool = new Scholls
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

                        _target.Schools.Add(matchedSchool);
                        _target.SaveChanges();

                        existingSchoolsByName[key] = matchedSchool;
                    }

                    // Cache globally
                    globalSchools[key] = matchedSchool;
                }

                // Step 6: Add map only if not already mapped
                if (!_target.SchoolIdMap.Any(m => m.OldId == src.Id && m.CompanyId == companyId))
                {
                    _target.SchoolIdMap.Add(new SchoolIdMap
                    {
                        OldId = src.Id,
                        NewId = matchedSchool.Id,
                        CompanyId = companyId
                    });
                }
            }

            _target.SaveChanges();
        }

        private static readonly object _groupLock = new object();
        public void MigrateGroups(SourceDbContext source, int companyId, Dictionary<string, Group> globalGroups)
        {
            var sourceGroups = source.groups.AsNoTracking().ToList();
            var newGroupIdMaps = new List<GroupIdMap>();

            foreach (var src in sourceGroups)
            {
                var key = src.Name?.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(key)) continue;

                Group existing;

                lock (_groupLock)
                {
                    // Double-check inside the lock to prevent duplicates
                    if (!globalGroups.TryGetValue(key, out existing))
                    {
                        existing = new Group
                        {
                            Name = src.Name,
                            Copy = src.Copy,
                            oldId = src.Id
                        };

                        _target.Groups.Add(existing);
                        _target.SaveChanges();

                        globalGroups[key] = existing;
                    }
                }

                // Add mapping
                newGroupIdMaps.Add(new GroupIdMap
                {
                    OldId = src.Id,
                    NewId = existing.Id,
                    CompanyId = companyId
                });
            }

            if (newGroupIdMaps.Any())
            {
                _target.GroupIdMap.AddRange(newGroupIdMaps);
                _target.SaveChanges();
            }
        }
        public void MigrateSchoolGroups(SourceDbContext source, int companyId)
        {
            var schoolMap = _target.SchoolIdMap
                .Where(x => x.CompanyId == companyId)
                .ToDictionary(x => x.OldId, x => x.NewId);

            var groupMap = _target.GroupIdMap
                .Where(x => x.CompanyId == companyId)
                .ToDictionary(x => x.OldId, x => x.NewId);

            var sourceGroups = source.schoolgroups.AsNoTracking().ToList();

            var newSchoolGroups = new List<Schoolgroup>();
            var schoolGroupIdMappings = new List<SchoolGroupsIdMap>(); // Use dedicated IdMap

            foreach (var src in sourceGroups)
            {
                if (!schoolMap.TryGetValue(src.Schoolid, out var newSchoolId)) continue;
                if (!groupMap.TryGetValue(src.Groupid, out var newGroupId)) continue;

                var newSchoolGroup = new Schoolgroup
                {
                    Schoolid = newSchoolId,
                    Groupid = newGroupId,
                    CompanyId = companyId,
                    oldId = src.Id
                };

                newSchoolGroups.Add(newSchoolGroup);
            }

            if (newSchoolGroups.Any())
            {
                _target.Schoolgroups.AddRange(newSchoolGroups);
                _target.SaveChanges();

                // After saving, IDs are populated
                foreach (var sg in newSchoolGroups)
                {
                    schoolGroupIdMappings.Add(new SchoolGroupsIdMap
                    {
                        OldId = sg.oldId ?? 0,
                        NewId = sg.Id,
                        CompanyId = companyId
                    });
                }

                _target.SchoolGroupsIdMap.AddRange(schoolGroupIdMappings); // Use correct map
                _target.SaveChanges();
                _target.ChangeTracker.Clear();
            }

            Console.WriteLine($"✅ Migrated {newSchoolGroups.Count} Schoolgroups for CompanyId {companyId}");
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
            // Get all companies
            var companies = _target.Company.ToList();

            // Get all source DB connections except target
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Build existing state map (name -> PortalStates.Id)
            var existingStateMap = _target.PortalStates
                .ToDictionary(s => s.Name.ToLower(), s => s.Id);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                    continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceStates = source.States.AsNoTracking().ToList();

                foreach (var sourceState in sourceStates)
                {
                    var nameKey = sourceState.Name.ToLower();

                    int portalStateId;

                    // If not already inserted, add to PortalStates
                    if (!existingStateMap.ContainsKey(nameKey))
                    {
                        var newState = new PortalStates
                        {
                            Name = sourceState.Name,
                            Abbr = sourceState.Abbr,
                            Country = sourceState.Country,
                            Timezone = sourceState.Timezone,
                            oldId = sourceState.Id
                        };

                        _target.PortalStates.Add(newState);
                        _target.SaveChanges();

                        portalStateId = newState.Id;

                        // Track this new name -> ID
                        existingStateMap[nameKey] = portalStateId;
                    }
                    else
                    {
                        // Use existing PortalState ID
                        portalStateId = existingStateMap[nameKey];
                    }

                    // Add to StateIdMap regardless of duplicate or not
                    _target.StateIdMap.Add(new StateIdMap
                    {
                        OldId = sourceState.Id,
                        NewId = portalStateId,
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

            const int batchSize = 5000;

            try
            {
                foreach (var kvp in sourceConnections)
                {
                    var dbName = kvp.Key;
                    var connectionString = kvp.Value;

                    var company = companies.FirstOrDefault(c => c.Name == dbName);
                    if (company == null) continue;

                    var companyId = company.Id;

                    using var source = new SourceDbContext(connectionString);
                    var sourcePostalCodes = source.Postalcodes.AsNoTracking().ToList();

                    var existingPostalCodes = _target.Postalcodes
                        .AsNoTracking()
                        .ToList()
                        .ToDictionary(
                            p => $"{p.Code.Trim().ToLower()}|{p.City.Trim().ToLower()}|{p.Stateid}",
                            p => p
                        );

                    var existingMappings = _target.PostalCodeIdMap
                        .Where(m => m.CompanyId == companyId)
                        .Select(m => m.OldId)
                        .ToHashSet();

                    var newPostalCodes = new List<Postalcode>();
                    var newPostalCodeMaps = new List<PostalCodeIdMap>();

                    foreach (var sourcePostalCode in sourcePostalCodes)
                    {
                        if (existingMappings.Contains(sourcePostalCode.Id))
                            continue;

                        var newStateId = _target.StateIdMap
                            .FirstOrDefault(x => x.OldId == sourcePostalCode.Stateid && x.CompanyId == companyId)
                            ?.NewId;

                        if (newStateId == null)
                            continue;

                        var key = $"{sourcePostalCode.Code.Trim().ToLower()}|{sourcePostalCode.City.Trim().ToLower()}|{newStateId}";

                        if (!existingPostalCodes.TryGetValue(key, out var matchedPostalCode))
                        {
                            matchedPostalCode = new Postalcode
                            {
                                Code = sourcePostalCode.Code,
                                City = sourcePostalCode.City,
                                Stateid = newStateId.Value,
                                Latitude = sourcePostalCode.Latitude,
                                Longitude = sourcePostalCode.Longitude,
                                oldId = sourcePostalCode.Id
                            };

                            newPostalCodes.Add(matchedPostalCode);
                            existingPostalCodes[key] = matchedPostalCode; // update in memory
                        }

                        newPostalCodeMaps.Add(new PostalCodeIdMap
                        {
                            OldId = sourcePostalCode.Id,
                            NewId = matchedPostalCode.Id, // this will be 0 initially but gets tracked
                            CompanyId = companyId
                        });

                        // When batch size is hit, save and clear
                        if (newPostalCodes.Count >= batchSize)
                        {
                            _target.Postalcodes.AddRange(newPostalCodes);
                            _target.SaveChanges();
                            newPostalCodes.Clear();
                        }

                        if (newPostalCodeMaps.Count >= batchSize)
                        {
                            // Fix: Set NewId now that Postalcodes have been saved
                            foreach (var map in newPostalCodeMaps)
                            {
                                if (map.NewId == 0)
                                {
                                    var code = sourcePostalCodes.First(p => p.Id == map.OldId);
                                    var stateId = _target.StateIdMap
                                        .FirstOrDefault(x => x.OldId == code.Stateid && x.CompanyId == companyId)
                                        ?.NewId ?? 0;

                                    var key2 = $"{code.Code.Trim().ToLower()}|{code.City.Trim().ToLower()}|{stateId}";
                                    if (existingPostalCodes.TryGetValue(key2, out var post))
                                    {
                                        map.NewId = post.Id;
                                    }
                                }
                            }

                            _target.PostalCodeIdMap.AddRange(newPostalCodeMaps);
                            _target.SaveChanges();
                            newPostalCodeMaps.Clear();
                        }
                    }

                    // Final remaining batch
                    if (newPostalCodes.Any())
                    {
                        _target.Postalcodes.AddRange(newPostalCodes);
                        _target.SaveChanges();
                    }

                    if (newPostalCodeMaps.Any())
                    {
                        foreach (var map in newPostalCodeMaps)
                        {
                            if (map.NewId == 0)
                            {
                                var code = sourcePostalCodes.First(p => p.Id == map.OldId);
                                var stateId = _target.StateIdMap
                                    .FirstOrDefault(x => x.OldId == code.Stateid && x.CompanyId == companyId)
                                    ?.NewId ?? 0;

                                var key2 = $"{code.Code.Trim().ToLower()}|{code.City.Trim().ToLower()}|{stateId}";
                                if (existingPostalCodes.TryGetValue(key2, out var post))
                                {
                                    map.NewId = post.Id;
                                }
                            }
                        }

                        _target.PostalCodeIdMap.AddRange(newPostalCodeMaps);
                        _target.SaveChanges();
                    }

                    _target.ChangeTracker.Clear();
                    Console.WriteLine($"✅ {dbName}: PostalCodes & Maps migrated.");
                }

                Console.WriteLine("🎯 All PostalCodes migration completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                throw;
            }
        }
        public void MigrateCampuses()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Step 1: Prepare in-memory maps
            var globalCampusMap = new Dictionary<string, Campus>();

            // Step 2: Load all existing campuses from target
            var existingCampuses = _target.Campuses
                .AsNoTracking()
                .ToList();

            foreach (var campus in existingCampuses)
            {
                var key = $"{campus.Schoolid}|{campus.Name?.Trim().ToLower()}|{campus.Address?.Trim().ToLower()}|{campus.City?.Trim().ToLower()}|{campus.PortalStatesid}|{campus.Postalcodeid}|{campus.Campustype}|{campus.Active}|{campus.Copy?.Trim().ToLower()}|{campus.Clientid}";
                if (!string.IsNullOrWhiteSpace(key))
                    globalCampusMap[key] = campus;
            }

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceCampuses = source.Campuses.AsNoTracking().ToList();

                var schoolMap = _target.SchoolIdMap.Where(x => x.CompanyId == companyId).ToList();
                var stateMap = _target.StateIdMap.Where(x => x.CompanyId == companyId).ToList();
                var postalMap = _target.PostalCodeIdMap.Where(x => x.CompanyId == companyId).ToList();

                foreach (var src in sourceCampuses)
                {
                    try
                    {
                        var newSchoolId = schoolMap.FirstOrDefault(x => x.OldId == src.Schoolid)?.NewId;
                        var newStateId = stateMap.FirstOrDefault(x => x.OldId == src.Stateid)?.NewId;
                        var newPostalcodeId = postalMap.FirstOrDefault(x => x.OldId == src.Postalcodeid)?.NewId;

                        if (newSchoolId == null || newPostalcodeId == null)
                            continue;

                        var key = $"{newSchoolId}|{src.Name?.Trim().ToLower()}|{src.Address?.Trim().ToLower()}|{src.City?.Trim().ToLower()}|{newStateId}|{newPostalcodeId}|{src.Campustype}|{src.Active}|{src.Copy?.Trim().ToLower()}|{src.Clientid}";
                        if (string.IsNullOrWhiteSpace(key)) continue;

                        Campus matchedCampus;

                        // Step 3: Check in global in-memory dictionary
                        if (!globalCampusMap.TryGetValue(key, out matchedCampus))
                        {
                            // Step 4: Insert new unique campus
                            matchedCampus = new Campus
                            {
                                Schoolid = newSchoolId.Value,
                                Name = src.Name,
                                Address = src.Address,
                                City = src.City,
                                PortalStatesid = newStateId,
                                Postalcodeid = newPostalcodeId.Value,
                                Campustype = src.Campustype,
                                Active = src.Active,
                                Copy = src.Copy,
                                Clientid = src.Clientid,
                                oldId = src.Id
                            };

                            _target.Campuses.Add(matchedCampus);
                            _target.SaveChanges();

                            globalCampusMap[key] = matchedCampus;
                        }

                        // Step 5: Always map OldId to NewId per company
                        bool alreadyMapped = _target.CampusIdMap
                            .Any(m => m.OldId == src.Id && m.CompanyId == companyId);

                        if (!alreadyMapped)
                        {
                            _target.CampusIdMap.Add(new CampusIdMap
                            {
                                OldId = src.Id,
                                NewId = matchedCampus.Id,
                                CompanyId = companyId
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Campus migration error (Company: {dbName}, Campus ID: {src.Id}): {ex.Message}");
                    }
                }

                _target.SaveChanges();
                _target.ChangeTracker.Clear();

                Console.WriteLine($"✅ Campuses migrated from: {dbName}");
            }

            Console.WriteLine("🎉 Campus migration completed from all sources.");
        }
        public void MigrateLevelsProgramsAndDegreePrograms()
        {
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            var companies = _target.Company.ToList();
            var globalLevels = new Dictionary<string, Level>(StringComparer.OrdinalIgnoreCase);
            var globalPrograms = new Dictionary<string, Program>(StringComparer.OrdinalIgnoreCase);

            foreach (var (dbName, connectionString) in sourceConnections)
            {
                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;
                using var source = new SourceDbContext(connectionString);

                // === LEVELS ===
                var existingLevelIds = _target.LevelsIdMap.Select(x => x.OldId).ToHashSet();
                foreach (var srcLevel in source.Levels.AsNoTracking())
                {
                    if (existingLevelIds.Contains(srcLevel.Id)) continue;

                    var key = srcLevel.Name?.Trim();
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
                    _target.SaveChanges();
                }

                // === PROGRAMS ===
                var existingProgramIds = _target.ProgramsIdMap.Select(x => x.OldId).ToHashSet();
                foreach (var srcProgram in source.Programs.AsNoTracking())
                {
                    if (existingProgramIds.Contains(srcProgram.Id)) continue;

                    var key = srcProgram.Name?.Trim();
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
                    _target.SaveChanges();
                }

                // === DEGREE PROGRAMS ===
                var existingDPIds = _target.DegreeprogramsIdMap
                    .Where(x => x.CompanyId == companyId)
                    .Select(x => x.OldId)
                    .ToHashSet();

                foreach (var srcDP in source.degreeprograms.AsNoTracking())
                {
                    if (existingDPIds.Contains(srcDP.Id)) continue;

                    var newLevelId = _target.LevelsIdMap.FirstOrDefault(x => x.OldId == srcDP.Levelid)?.NewId;
                    var newProgramId = _target.ProgramsIdMap.FirstOrDefault(x => x.OldId == srcDP.Programid)?.NewId;

                    if (newLevelId == null || newProgramId == null)
                    {
                        Console.WriteLine($"⚠️ Skipping DegreeProgram ID {srcDP.Id} — LevelId: {srcDP.Levelid} (found: {newLevelId}), ProgramId: {srcDP.Programid} (found: {newProgramId})");
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

                Console.WriteLine($"✅ Completed migration for {dbName}");
                break; // Exit after first
            }
        }
        public void MigrateCampusDegrees()
        {
            var companies = _target.Company.ToList();

            var allConnections = _configuration.GetSection("ConnectionStrings").GetChildren().ToList();
            var sourceConnections = allConnections
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            Console.WriteLine($"🔍 Found {sourceConnections.Count} source databases to process.");

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                Console.WriteLine($"\n🔁 Starting migration for: {dbName}");

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {
                    Console.WriteLine($"⏭️ Skipping: No matching company found for {dbName}");
                    continue;
                }

                var companyId = company.Id;

                try
                {
                    using var source = new SourceDbContext(connectionString);
                    var sourceCampusDegrees = source.campusdegrees.AsNoTracking().ToList();

                    Console.WriteLine($"📦 {sourceCampusDegrees.Count} campusdegree records found in {dbName}");

                    foreach (var sourceCD in sourceCampusDegrees)
                    {
                        // Remap Degreeid
                        var newDegreeProgramId = _target.DegreeprogramsIdMap
                            .FirstOrDefault(x => x.OldId == sourceCD.Degreeid)?.NewId;

                        if (newDegreeProgramId == null)
                        {
                            Console.WriteLine($"⚠️ Skipped: DegreeId={sourceCD.Degreeid} not mapped for CompanyId={companyId}");
                            continue;
                        }

                        // Remap Campusid or fallback
                        var newCampusId = _target.CampusIdMap
                            .FirstOrDefault(x => x.OldId == sourceCD.Campusid && x.CompanyId == companyId)?.NewId
                            ?? sourceCD.Campusid;

                        var newCampusDegree = new Campusdegree
                        {
                            Campusid = newCampusId,
                            Name = sourceCD.Name,
                            Copy = sourceCD.Copy,
                            Active = sourceCD.Active,
                            Clientid = sourceCD.Clientid,
                            Degreeid = newDegreeProgramId.Value,
                            CompanyId = companyId,
                            oldId = sourceCD.Id
                        };

                        try
                        {
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Error saving campusdegree Id={sourceCD.Id}: {ex.Message}");
                            _target.ChangeTracker.Clear();
                            continue;
                        }
                    }

                    Console.WriteLine($"✅ Completed migration for: {dbName}");
                }
                catch (Exception outerEx)
                {
                    Console.WriteLine($"❌ Fatal error with {dbName}: {outerEx.Message}");
                    continue;
                }
            }

            Console.WriteLine("\n🎉 All source databases processed.");
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
        public void MigrateDownSellOffers()
        {
            var companies = _target.Company.AsNoTracking().ToList();
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Build global uniqueness key: key = name|clientid
            var globalKeyToOfferId = new Dictionary<string, int>();
            var globalKeyToOfferEntity = new Dictionary<string, DownSellOffer>();
            var allIdMaps = new List<DownSellOffersIdMap>();

            // STEP 1: Load already existing DownSellOffers from DB to prevent duplicates
            var existingOffers = _target.DownSellOffers
                .AsNoTracking()
                .Select(o => new { o.Id, o.Name, o.Clientid })
                .ToList();

            foreach (var existing in existingOffers)
            {
                var key = $"{existing.Name?.Trim().ToLower()}|{existing.Clientid}";
                globalKeyToOfferId[key] = existing.Id;
            }

            // STEP 2: Collect new unique DownSellOffers from all sources
            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;
                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                using var source = new SourceDbContext(connectionString);

                var clientMap = _target.ClientIdMap
                    .Where(x => x.CompanyId == company.Id)
                    .ToDictionary(x => x.OldId, x => x.NewId);

                var offers = source.DownSellOffers.AsNoTracking().ToList();

                foreach (var src in offers)
                {
                    if (!clientMap.TryGetValue(src.Clientid, out var newClientId))
                        continue;

                    var key = $"{src.Name?.Trim().ToLower()}|{newClientId}";
                    if (globalKeyToOfferId.ContainsKey(key)) continue;

                    var entity = new DownSellOffer
                    {
                        Clientid = newClientId,
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
                        Minage = src.Minage
                    };

                    globalKeyToOfferEntity[key] = entity;
                }
            }

            // STEP 3: Save new entities and capture their IDs
            if (globalKeyToOfferEntity.Any())
            {
                _target.DownSellOffers.AddRange(globalKeyToOfferEntity.Values);
                _target.SaveChanges();

                foreach (var kvp in globalKeyToOfferEntity)
                    globalKeyToOfferId[kvp.Key] = kvp.Value.Id;

                Console.WriteLine($"✅ Inserted {globalKeyToOfferEntity.Count} new DownSellOffers.");
            }

            // STEP 4: Now generate DownSellOffersIdMap from all companies
            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;
                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                using var source = new SourceDbContext(connectionString);

                var clientMap = _target.ClientIdMap
                    .Where(x => x.CompanyId == company.Id)
                    .ToDictionary(x => x.OldId, x => x.NewId);

                var offers = source.DownSellOffers.AsNoTracking().ToList();

                foreach (var src in offers)
                {
                    if (!clientMap.TryGetValue(src.Clientid, out var newClientId))
                        continue;

                    var key = $"{src.Name?.Trim().ToLower()}|{newClientId}";
                    if (globalKeyToOfferId.TryGetValue(key, out var newId))
                    {
                        allIdMaps.Add(new DownSellOffersIdMap
                        {
                            OldId = src.Id,
                            NewId = newId
                        });
                    }
                }

                Console.WriteLine($"📦 Mapped DownSellOffers for company {company.Id}");
            }

            _target.DownSellOffersIdMap.AddRange(allIdMaps);
            _target.SaveChanges();
            Console.WriteLine($"🎯 Inserted {allIdMaps.Count} rows into DownSellOffersIdMap.");
        }
        public void MigrateDownSellOfferPostalCodes()
        {
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Get existing combinations to avoid duplicates
            var existingEntries = _target.DownSellOfferPostalCodes
                .AsNoTracking()
                .Select(e => $"{e.DownSellOfferId}|{e.Postalcodeid}")
                .ToHashSet();

            var allIdMaps = new List<DownSellOfferPostalCodesIdMap>();

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.DownSellOfferPostalCodes.AsNoTracking().ToList();

                // If this source has data, process and exit
                if (sourceRows.Any())
                {
                    var newEntries = new List<DownSellOfferPostalCode>();

                    foreach (var row in sourceRows)
                    {
                        var key = $"{row.DownSellOfferId}|{row.Postalcodeid}";

                        if (existingEntries.Contains(key))
                            continue;

                        var entry = new DownSellOfferPostalCode
                        {
                            DownSellOfferId = row.DownSellOfferId,
                            Postalcodeid = row.Postalcodeid,
                            oldId = row.Id
                        };

                        newEntries.Add(entry);
                        existingEntries.Add(key); // prevent duplication
                    }

                    if (newEntries.Count > 0)
                    {
                        _target.DownSellOfferPostalCodes.AddRange(newEntries);
                        _target.SaveChanges();

                        foreach (var entry in newEntries)
                        {
                            allIdMaps.Add(new DownSellOfferPostalCodesIdMap
                            {
                                OldId = entry.oldId ?? 0,
                                NewId = entry.Id
                            });
                        }

                        _target.DownSellOfferPostalCodesIdMap.AddRange(allIdMaps);
                        _target.SaveChanges();

                        Console.WriteLine($"✅ {dbName}: Migrated {newEntries.Count} DownSellOfferPostalCodes.");
                    }
                    else
                    {
                        Console.WriteLine($"ℹ️ {dbName}: No unique new entries to migrate.");
                    }

                    // 🔁 Done after first non-empty source
                    break;
                }
                else
                {
                    Console.WriteLine($"❌ {dbName}: No DownSellOfferPostalCodes found.");
                }
            }

            if (!allIdMaps.Any())
            {
                Console.WriteLine("⚠️ No DownSellOfferPostalCodes were migrated from any source.");
            }
            else
            {
                Console.WriteLine($"🎯 Inserted {allIdMaps.Count} rows into DownSellOfferPostalCodesIdMap.");
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
                    continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.master_schools.AsNoTracking().ToList();

                var newEntities = new List<MasterSchool>();
                var idMaps = new List<Master_schoolsIdMap>();

                foreach (var sourceItem in sourceRows)
                {
                    var newItem = new MasterSchool
                    {
                        Name = sourceItem.Name,
                        oldId = sourceItem.Id
                    };

                    newEntities.Add(newItem);
                }

                // Insert new records
                _target.MasterSchools.AddRange(newEntities);
                _target.SaveChanges();

                // Map old IDs to new IDs
                idMaps.AddRange(newEntities.Select(x => new Master_schoolsIdMap
                {
                    OldId = x.oldId ?? 0,
                    NewId = x.Id
                }));

                _target.Master_schoolsIdMap.AddRange(idMaps);
                _target.SaveChanges();
                _target.ChangeTracker.Clear();

                Console.WriteLine($"✅ {dbName}: Inserted {newEntities.Count} MasterSchools and {idMaps.Count} ID maps.");

                // 🚪 Exit after first source
                Console.WriteLine($"⏹️ Exiting after first source: {dbName}");
                return;
            }

            Console.WriteLine("⚠️ No valid sources processed.");
        }
        public void MigrateMasterSchoolMappings()
        {
            var companies = _target.Company.AsNoTracking().ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            var existingMappingKeys = _target.MasterSchoolMappings
                .AsNoTracking()
                .Select(x => new { x.MasterSchoolsId, x.Identifier })
                .ToHashSet();

            var existingMappedIds = _target.Master_school_mappingsIdMap
                .AsNoTracking()
                .Select(m => m.OldId)
                .ToHashSet();

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.master_school_mappings.AsNoTracking().ToList();

                var masterSchoolIdMap = _target.Master_schoolsIdMap
                    .AsNoTracking()
                    .GroupBy(x => x.OldId)
                    .ToDictionary(g => g.Key, g => g.First().NewId);

                var newMappings = new List<MasterSchoolMapping>();
                var localIdMap = new List<Master_school_mappingsIdMap>();

                foreach (var row in sourceRows)
                {
                    if (existingMappedIds.Contains(row.Id))
                        continue;

                    if (!masterSchoolIdMap.TryGetValue(row.master_schools_id, out var newMasterSchoolId))
                        continue;

                    if (existingMappingKeys.Contains(new { MasterSchoolsId = newMasterSchoolId, row.Identifier }))
                        continue;

                    var mapping = new MasterSchoolMapping
                    {
                        MasterSchoolsId = newMasterSchoolId,
                        Identifier = row.Identifier,
                        oldId = row.Id
                    };

                    newMappings.Add(mapping);
                    existingMappingKeys.Add(new { MasterSchoolsId = newMasterSchoolId, row.Identifier });
                }

                if (newMappings.Any())
                {
                    _target.MasterSchoolMappings.AddRange(newMappings);
                    _target.SaveChanges();

                    localIdMap.AddRange(newMappings.Select(m => new Master_school_mappingsIdMap
                    {
                        OldId = m.oldId ?? 0,
                        NewId = m.Id
                    }));

                    _target.Master_school_mappingsIdMap.AddRange(localIdMap);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    Console.WriteLine($"✅ {dbName}: Migrated {newMappings.Count} MasterSchoolMappings.");
                }
                else
                {
                    Console.WriteLine($"⚠️ {dbName}: No new mappings to migrate.");
                }

                // Exit the method after processing the first source
                Console.WriteLine($"⏹️ Stopping after processing first source: {dbName}");
                return;
            }

            Console.WriteLine($"🔁 No sources processed (this should not be reached if at least one source exists).");
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
                    continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceAreas = source.Areas.AsNoTracking().ToList();

                if (!sourceAreas.Any())
                {
                    Console.WriteLine($"⚠️ {dbName}: No areas found to migrate.");
                    return;
                }

                var newAreas = new List<Area>();
                var idMaps = new List<AreasIdMap>();

                foreach (var sourceArea in sourceAreas)
                {
                    var newArea = new Area
                    {
                        Name = sourceArea.Name,
                        Copy = sourceArea.Copy,
                        oldId = sourceArea.Id
                    };

                    newAreas.Add(newArea);
                }

                _target.Areas.AddRange(newAreas);
                _target.SaveChanges();

                idMaps.AddRange(newAreas.Select(x => new AreasIdMap
                {
                    OldId = x.oldId ?? 0,
                    NewId = x.Id
                }));

                _target.AreasIdMap.AddRange(idMaps);
                _target.SaveChanges();
                _target.ChangeTracker.Clear();

                Console.WriteLine($"✅ {dbName}: Inserted {newAreas.Count} Areas and {idMaps.Count} ID maps.");

                // 🚪 Exit after first source
                Console.WriteLine($"⏹️ Exiting after first source: {dbName}");
                return;
            }

            Console.WriteLine("⚠️ No valid sources processed.");
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
                    continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.programareas.AsNoTracking().ToList();

                if (!sourceRows.Any())
                {
                    Console.WriteLine($"⚠️ {dbName}: No ProgramAreas to migrate.");
                    return;
                }

                var programIdMap = _target.ProgramsIdMap
                    .AsNoTracking()
                    .GroupBy(x => x.OldId)
                    .ToDictionary(g => g.Key, g => g.First().NewId);

                var areaIdMap = _target.AreasIdMap
                    .AsNoTracking()
                    .GroupBy(x => x.OldId)
                    .ToDictionary(g => g.Key, g => g.First().NewId);

                var newProgramAreas = new List<Programarea>();
                var newIdMaps = new List<ProgramareasIdMap>();

                foreach (var row in sourceRows)
                {
                    if (!programIdMap.TryGetValue(row.Programid, out var newProgramId) ||
                        !areaIdMap.TryGetValue(row.Areaid, out var newAreaId))
                    {
                        continue;
                    }

                    var newEntry = new Programarea
                    {
                        Programid = newProgramId,
                        Areaid = newAreaId,
                        oldId = row.Id
                    };

                    newProgramAreas.Add(newEntry);
                }

                if (newProgramAreas.Any())
                {
                    _target.Programareas.AddRange(newProgramAreas);
                    _target.SaveChanges();

                    newIdMaps.AddRange(newProgramAreas.Select(p => new ProgramareasIdMap
                    {
                        OldId = p.oldId ?? 0,
                        NewId = p.Id
                    }));

                    _target.ProgramareasIdMap.AddRange(newIdMaps);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    Console.WriteLine($"✅ {dbName}: Inserted {newProgramAreas.Count} ProgramAreas and {newIdMaps.Count} ID mappings.");
                }
                else
                {
                    Console.WriteLine($"⚠️ {dbName}: No valid mappings found.");
                }

                // 🚪 Stop after first source is processed
                Console.WriteLine($"⏹️ Exiting after first source: {dbName}");
                return;
            }

            Console.WriteLine("⚠️ No valid sources processed.");
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
                    continue;

                using var source = new SourceDbContext(connectionString);
                var sourceInterests = source.interests.AsNoTracking().ToList();

                if (!sourceInterests.Any())
                {
                    Console.WriteLine($"⚠️ {dbName}: No interests to migrate.");
                    return;
                }

                // Load existing interests in the target to avoid duplicates
                var existingInterestKeys = _target.Interests
                    .AsNoTracking()
                    .Select(i => i.Name.Trim().ToLower()) // or use a composite key if needed
                    .ToHashSet();

                var newInterests = new List<Interest>();
                var idMaps = new List<InterestsIdMap>();

                foreach (var interest in sourceInterests)
                {
                    var normalizedName = interest.Name?.Trim().ToLower();

                    if (string.IsNullOrWhiteSpace(normalizedName) || existingInterestKeys.Contains(normalizedName))
                        continue;

                    var newInterest = new Interest
                    {
                        Name = interest.Name,
                        Copy = interest.Copy,
                        oldId = interest.Id
                    };

                    newInterests.Add(newInterest);
                    existingInterestKeys.Add(normalizedName); // add to set to avoid dupes within the same batch
                }

                if (newInterests.Any())
                {
                    _target.Interests.AddRange(newInterests);
                    _target.SaveChanges();

                    idMaps.AddRange(newInterests.Select(i => new InterestsIdMap
                    {
                        OldId = i.oldId ?? 0,
                        NewId = i.Id
                    }));

                    _target.InterestsIdMap.AddRange(idMaps);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    Console.WriteLine($"✅ {dbName}: Inserted {newInterests.Count} Interests and {idMaps.Count} ID maps.");
                }
                else
                {
                    Console.WriteLine($"⚠️ {dbName}: All interests were duplicates. Nothing inserted.");
                }

                // 🚪 Exit after first source is processed
                Console.WriteLine($"⏹️ Exiting after first source: {dbName}");
                return;
            }

            Console.WriteLine("⚠️ No valid sources processed.");
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
                    continue;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.programinterests.AsNoTracking().ToList();

                if (!sourceRows.Any())
                {
                    Console.WriteLine($"⚠️ {dbName}: No programinterests to migrate.");
                    return;
                }

                var programIdMap = _target.ProgramsIdMap
                    .AsNoTracking()
                    .GroupBy(x => x.OldId)
                    .ToDictionary(g => g.Key, g => g.First().NewId);

                var interestIdMap = _target.InterestsIdMap
                    .AsNoTracking()
                    .GroupBy(x => x.OldId)
                    .ToDictionary(g => g.Key, g => g.First().NewId);

                var existingKeys = _target.Programinterests
                    .AsNoTracking()
                    .Select(p => new { p.Programid, p.Interestid })
                    .ToHashSet();

                var newEntries = new List<Programinterest>();
                var newIdMaps = new List<PrograminterestsIdMap>();

                foreach (var row in sourceRows)
                {
                    if (!programIdMap.TryGetValue(row.Programid, out var newProgramId) ||
                        !interestIdMap.TryGetValue(row.Interestid, out var newInterestId))
                        continue;

                    var key = new { Programid = newProgramId, Interestid = newInterestId };
                    if (existingKeys.Contains(key))
                        continue;

                    var newEntry = new Programinterest
                    {
                        Programid = newProgramId,
                        Interestid = newInterestId,
                        oldId = row.Id
                    };

                    newEntries.Add(newEntry);
                    existingKeys.Add(key);
                }

                if (newEntries.Any())
                {
                    _target.Programinterests.AddRange(newEntries);
                    _target.SaveChanges();

                    newIdMaps.AddRange(newEntries.Select(p => new PrograminterestsIdMap
                    {
                        OldId = p.oldId ?? 0,
                        NewId = p.Id
                    }));

                    _target.PrograminterestsIdMap.AddRange(newIdMaps);
                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    Console.WriteLine($"✅ {dbName}: Inserted {newEntries.Count} ProgramInterests and {newIdMaps.Count} ID maps.");
                }
                else
                {
                    Console.WriteLine($"⚠️ {dbName}: All programinterests were duplicates or unresolvable.");
                }

                // 🚪 Exit after first source
                Console.WriteLine($"⏹️ Exiting after first source: {dbName}");
                return;
            }

            Console.WriteLine("⚠️ No valid sources processed.");
        }
        public void MigrateGroups()
        {
            var companies = _target.Company.ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Load all existing groups globally (by Name, case-insensitive)
            var existingGroups = _target.Groups
                .AsNoTracking()
                .ToList()
                .GroupBy(g => g.Name.Trim().ToLower())
                .ToDictionary(g => g.Key, g => g.First());

            var globalGroupMap = new Dictionary<string, Group>(existingGroups); // memory cache

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceGroups = source.groups.AsNoTracking().ToList();

                var idMaps = new List<GroupIdMap>();
                var newGroups = new List<Group>();

                foreach (var srcGroup in sourceGroups)
                {
                    var key = srcGroup.Name?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(key)) continue;

                    Group matchedGroup;

                    // Check in global in-memory dictionary
                    if (!globalGroupMap.TryGetValue(key, out matchedGroup))
                    {
                        // Not found – insert new group
                        matchedGroup = new Group
                        {
                            Name = srcGroup.Name,
                            Copy = srcGroup.Copy,
                            oldId = srcGroup.Id,
                        };

                        _target.Groups.Add(matchedGroup);
                        newGroups.Add(matchedGroup);
                        globalGroupMap[key] = matchedGroup;
                    }

                    // Always add mapping
                    idMaps.Add(new GroupIdMap
                    {
                        OldId = srcGroup.Id,
                        NewId = matchedGroup.Id,
                        CompanyId = companyId
                    });
                }

                if (newGroups.Any())
                {
                    _target.SaveChanges(); // Save new global groups
                }

                if (idMaps.Any())
                {
                    _target.GroupIdMap.AddRange(idMaps);
                    _target.SaveChanges();
                }

                _target.ChangeTracker.Clear();
                Console.WriteLine($"✅ {dbName}: Added {newGroups.Count} global groups, mapped {idMaps.Count}.");
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
            var companies = _target.Company.AsNoTracking().ToList();

            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            // Use string-based composite key to check duplicates
            var existingKeys = _target.Extrarequirededucations
                .AsNoTracking()
                .Select(e => $"{e.Degreeid}|{e.Campusid}|{e.CompanyId}")
                .ToHashSet();

            foreach (var kvp in sourceConnections) // Only run for one company
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.extrarequirededucation.AsNoTracking().ToList();

                var newRows = new List<Extrarequirededucation>();

                foreach (var row in sourceRows)
                {
                    var key = $"{row.Degreeid}|{row.Campusid}|{companyId}";

                    if (existingKeys.Contains(key))
                        continue;

                    var newEntry = new Extrarequirededucation
                    {
                        Degreeid = row.Degreeid,
                        Campusid = row.Campusid,
                        Value = row.Value,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    newRows.Add(newEntry);
                    existingKeys.Add(key); // Also update the in-memory set
                }

                if (newRows.Any())
                {
                    _target.Extrarequirededucations.AddRange(newRows);
                    _target.SaveChanges();
                    Console.WriteLine($"✅ Inserted {newRows.Count} new Extrarequirededucation rows for company {company.Name}.");
                }
                else
                {
                    Console.WriteLine($"ℹ️ No new rows to insert for company {company.Name}.");
                }

                break; // only for one company
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
                if (company == null) continue;

                var companyId = company.Id;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.leadposts.AsNoTracking().ToList();

                var schoolMap = _target.SchoolIdMap.Where(x => x.CompanyId == companyId).ToList();
                var sourceMap = _target.SourceIdMap.Where(x => x.CompanyId == companyId).ToList();
                var offerMap = _target.OfferIdMap.Where(x => x.CompanyId == companyId).ToList();
                var programMap = _target.ProgramsIdMap.ToList(); // global
                var campusMap = _target.CampusIdMap.Where(x => x.CompanyId == companyId).ToList();

                var leadpostsToInsert = new List<Leadpost>();
                var idMapsToInsert = new List<LeadpostsIdMap>();

                foreach (var row in sourceRows)
                {
                    var newSchoolId = schoolMap.FirstOrDefault(x => x.OldId == row.Schoolid)?.NewId ?? row.Schoolid;
                    var newSourceId = sourceMap.FirstOrDefault(x => x.OldId == row.Sourceid)?.NewId ?? row.Sourceid;
                    var newOfferId = offerMap.FirstOrDefault(x => x.OldId == row.Offerid)?.NewId ?? row.Offerid;
                    var newProgramId = programMap.FirstOrDefault(x => x.OldId == row.Programid)?.NewId ?? row.Programid;
                    var newCampusId = campusMap.FirstOrDefault(x => x.OldId == row.Campusid)?.NewId ?? row.Campusid;

                    var newEntry = new Leadpost
                    {
                        Schoolid = newSchoolId,
                        Sourceid = newSourceId,
                        Offerid = newOfferId,
                        Programid = newProgramId,
                        Campusid = newCampusId,

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

                    leadpostsToInsert.Add(newEntry);
                }

                // Bulk insert leadposts
                if (leadpostsToInsert.Any())
                {
                    _target.Leadposts.AddRange(leadpostsToInsert);
                    _target.SaveChanges();

                    idMapsToInsert.AddRange(leadpostsToInsert.Select(p => new LeadpostsIdMap
                    {
                        OldId = p.oldId ?? 0,
                        NewId = p.Id,
                        CompanyId = companyId
                    }));

                    _target.LeadpostsIdMap.AddRange(idMapsToInsert);
                    _target.SaveChanges();
                }

                _target.ChangeTracker.Clear();
                Console.WriteLine($"✅ Migrated {leadpostsToInsert.Count} leadposts for {dbName}");
            }

            Console.WriteLine("🎉 Leadpost migration completed from all sources.");
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
                    continue;

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.portaltargeting.AsNoTracking().ToList();

                // Get existing target portal IDs to avoid duplicates (based on your logic)
                var existingPortalIds = _target.Portaltargetings
                    .Select(p => p.Portalid)
                    .ToHashSet();

                var newEntries = new List<Portaltargeting>();

                foreach (var row in sourceRows)
                {
                    if (existingPortalIds.Contains(row.Portalid))
                        continue;

                    newEntries.Add(new Portaltargeting
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
                        WednesdayActive = row.Wednesday_Active,
                        WednesdayStart = row.Wednesday_Start,
                        WednesdayEnd = row.Wednesday_End,
                        ThursdayActive = row.Thursday_Active,
                        ThursdayStart = row.Thursday_Start,
                        ThursdayEnd = row.Thursday_End,
                        FridayActive = row.Friday_Active,
                        FridayStart = row.Friday_Start,
                        FridayEnd = row.Friday_End,
                        SaturdayActive = row.Saturday_Active,
                        SaturdayStart = row.Saturday_Start,
                        SaturdayEnd = row.Saturday_End,
                        SundayActive = row.Sunday_Active,
                        SundayStart = row.Sunday_Start,
                        SundayEnd = row.Sunday_End
                    });
                }

                if (newEntries.Any())
                {
                    _target.Portaltargetings.AddRange(newEntries);
                    _target.SaveChanges();
                }

                // Exit after processing the first valid source
                break;
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

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.searchportals.AsNoTracking().ToList();

                // Get existing portal names + URLs to avoid duplicates
                var existing = _target.Searchportals
                    .Select(p => new { p.Name, p.Url })
                    .ToHashSet();

                var newEntries = new List<Searchportal>();

                foreach (var row in sourceRows)
                {
                    // Skip duplicates
                    if (existing.Contains(new { row.Name, row.Url }))
                        continue;

                    newEntries.Add(new Searchportal
                    {
                        Name = row.Name,
                        Url = row.Url,
                        Active = row.Active,
                        Transfers = row.Transfers,
                        Leads = row.Leads,
                        Rank = row.Rank
                    });
                }

                if (newEntries.Any())
                {
                    _target.Searchportals.AddRange(newEntries);
                    _target.SaveChanges();
                }

                // Only process the first valid source
                break;
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

                using var source = new SourceDbContext(connectionString);
                var sourceRows = source.tblConfigEducationLevels.AsNoTracking().ToList();

                // Create a HashSet of existing Identifier+Value pairs to avoid duplicates
                var existingEntries = _target.TblConfigEducationLevels
                    .Select(x => new { x.Identifier, x.Value })
                    .ToHashSet();

                var newEntries = new List<TblConfigEducationLevel>();

                foreach (var row in sourceRows)
                {
                    if (existingEntries.Contains(new { row.Identifier, row.Value }))
                        continue;

                    newEntries.Add(new TblConfigEducationLevel
                    {
                        Identifier = row.Identifier,
                        Value = row.Value,
                        Label = row.Label
                    });
                }

                if (newEntries.Any())
                {
                    _target.TblConfigEducationLevels.AddRange(newEntries);
                    _target.SaveChanges();
                }

                break; // process only the first matching source
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

        //          _target.DownSellOfferPostalCodesIdMap.Add(new DownSellOfferPostalCodesIdMap
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

    }

}


