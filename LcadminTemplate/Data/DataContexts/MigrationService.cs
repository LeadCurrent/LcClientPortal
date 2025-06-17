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
using Data.EntityModelsAndLibraries.TblConfigEducationLevel.Models;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public void MigrateClients()
        {
            // Get all company IDs from target
            var companies = _target.Company.ToList();

            // Step 1: Extract all source DB connection strings (except TargetDb)
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var companyId = _target.Company.Where(c => c.Name == dbName).Select(c => c.Id).FirstOrDefault();

                using var source = new SourceDbContext(connectionString);
                var sourceClients = source.Clients.ToList();

                foreach (var sourceClient in sourceClients)
                {
                    // Insert client into target
                    var newClient = new Client
                    {
                        Name = sourceClient.Name,
                        Active = sourceClient.Active,
                        Direct = sourceClient.Direct,
                        oldId = sourceClient.Id
                    };

                    _target.Clients.Add(newClient);
                    _target.SaveChanges();


                    _target.ClientIdMap.Add(new ClientIdMap
                    {
                        OldId = sourceClient.Id,
                        NewId = newClient.Id,
                        CompanyId = companyId
                    });


                    _target.SaveChanges();
                }
            }


        }

        public void MigrateSchools()
        {
            // Get all company IDs from target
            var companies = _target.Company.ToList();

            // Step 1: Extract all source DB connection strings (except TargetDb)
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                var companyId = _target.Company.Where(c => c.Name == dbName).Select(c => c.Id).FirstOrDefault();

                using var source = new SourceDbContext(connectionString);
                var sourceScholls = source.Schools.ToList();

                foreach (var sourceClient in sourceScholls)
                {
                    // Insert client into target
                    var newClient = new Scholls
                    {
                        Name = sourceClient.Name,
                        Abbr = sourceClient.Abbr,
                        oldId = sourceClient.Id,
                        Website = sourceClient.Website,
                        Logo100 = sourceClient.Logo100,
                        Maxage = sourceClient.Maxage,
                        Minage = sourceClient.Minage,
                        Minhs = sourceClient.Minhs,
                        Maxhs = sourceClient.Maxhs,
                        Notes = sourceClient.Notes,
                        Shortcopy = sourceClient.Shortcopy,
                        Targeting = sourceClient.Targeting,
                        Accreditation = sourceClient.Accreditation,
                        Highlights = sourceClient.Highlights,
                        Alert = sourceClient.Alert,
                        Startdate = sourceClient.Startdate,
                        Scoreadjustment = sourceClient.Scoreadjustment,
                        Militaryfriendly = sourceClient.Militaryfriendly,
                        Disclosure = sourceClient.Disclosure,
                        Schoolgroup = sourceClient.Schoolgroup,
                        TcpaText = sourceClient.Tcpa_Text
                       

                    };

                    _target.Schools.Add(newClient);
                    _target.SaveChanges();


                    _target.SchoolIdMap.Add(new SchoolIdMap
                    {
                        OldId = sourceClient.Id,
                        NewId = newClient.Id,
                        CompanyId = companyId
                    });


                    _target.SaveChanges();
                }
            }


        }

        public void MigrateOffers()
        {
            // Step 1: Get all companies from target
            var companies = _target.Company.ToList();

            // Step 2: Get all source DB connections
            var sourceConnections = _configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .Where(cs => cs.Key != "TargetDb")
                .ToDictionary(cs => cs.Key, cs => cs.Value);

            foreach (var kvp in sourceConnections)
            {
                var dbName = kvp.Key;
                var connectionString = kvp.Value;

                // Step 3: Match the connection to a company
                var company = companies.FirstOrDefault(c => c.Name == dbName);
                if (company == null)
                {

                    continue;
                }

                var companyId = company.Id;



                // Step 4: Create the source context dynamically
                using var source = new SourceDbContext(connectionString);

                // Step 5: Read source offers
                var sourceOffers = source.Offers.ToList();

                foreach (var sourceOffer in sourceOffers)
                {
                    // Optional: map SchoolId and ClientId using your SchoolIdMap and ClientIdMap
                    var newSchoolId = _target.SchoolIdMap.FirstOrDefault(m => m.OldId == sourceOffer.Schoolid && m.CompanyId == companyId)?.NewId;
                    var newClientId = _target.ClientIdMap.FirstOrDefault(m => m.OldId == sourceOffer.Clientid && m.CompanyId == companyId)?.NewId;

                    // Skip if FK mapping is missing
                    if (newSchoolId == null || newClientId == null)
                    {

                        continue;
                    }

                    var newOffer = new Offer
                    {
                        Schoolid = newSchoolId.Value,
                        Clientid = newClientId.Value,
                        Url = sourceOffer.Url,
                        Active = sourceOffer.Active,
                        Rpl = sourceOffer.Rpl,
                        Dcap = sourceOffer.Dcap,
                        Dcapamt = sourceOffer.Dcapamt,
                        Mcap = sourceOffer.Mcap,
                        Mcapamt = sourceOffer.Mcapamt,
                        Wcap = sourceOffer.Wcap,
                        Wcapamt = sourceOffer.Wcapamt,
                        Type = sourceOffer.Type,
                        Militaryonly = sourceOffer.Militaryonly,
                        Nomilitary = sourceOffer.Nomilitary,
                        Transferphone = sourceOffer.Transferphone,
                        Lccampaignid = sourceOffer.Lccampaignid,
                        Archive = sourceOffer.Archive,
                        EndClient = sourceOffer.End_Client,
                        CecRplA = sourceOffer.cec_rplA,
                        CecRplB = sourceOffer.cec_rplB,
                        CecRplC = sourceOffer.cec_rplC,
                        CecRplD = sourceOffer.cec_rplD,
                        CecRplE = sourceOffer.cec_rplE,
                        CecRplF = sourceOffer.cec_rplF,
                        CecRplG = sourceOffer.cec_rplG,
                        DeliveryIdentifier = sourceOffer.Delivery_Identifier,
                        DeliveryName = sourceOffer.Delivery_Name,
                        CompanyId = companyId,
                        oldId = sourceOffer.Id
                    };

                    _target.Offers.Add(newOffer);
                    _target.SaveChanges();

                    // Step 6: Add OfferIdMap
                    _target.OfferIdMap.Add(new OfferIdMap
                    {
                        OldId = sourceOffer.Id,
                        NewId = newOffer.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                }


            }


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

        public void MigrateCampuses()
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
                var sourceCampuses = source.Campuses.AsNoTracking().ToList();

                int count = 0;

                foreach (var sourceCampus in sourceCampuses)
                {
                    try
                    {

                        var newSchoolId = _target.SchoolIdMap.FirstOrDefault(x => x.OldId == sourceCampus.Schoolid && x.CompanyId == companyId)?.NewId;

                        var newStateId = _target.StateIdMap.FirstOrDefault(x => x.OldId == sourceCampus.Stateid && x.CompanyId == companyId)?.NewId;

                        var newPostalcodeId = _target.PostalCodeIdMap.FirstOrDefault(x => x.OldId == sourceCampus.Postalcodeid && x.CompanyId == companyId)?.NewId;

                        if (newSchoolId == null || newPostalcodeId == null)
                        {

                            continue;
                        }

                        var newCampus = new Campus
                        {
                            Schoolid = newSchoolId.Value,
                            Name = sourceCampus.Name,
                            Address = sourceCampus.Address,
                            City = sourceCampus.City,
                            PortalStatesid = newStateId, // can be null
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


                        _target.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }



            }


        }
        public void MigrateLevels()
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
                var sourceLevels = source.Levels.AsNoTracking().ToList();

                foreach (var sourceLevel in sourceLevels)
                {
                    var newLevel = new Level
                    {
                        Name = sourceLevel.Name,
                        Copy = sourceLevel.Copy,
                        CompanyId = companyId,
                        oldId = sourceLevel.Id
                    };

                    _target.Levels.Add(newLevel);
                    _target.SaveChanges();

                    _target.LevelsIdMap.Add(new LevelsIdMap
                    {
                        OldId = sourceLevel.Id,
                        NewId = newLevel.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }


            }


        }

        public void MigratePrograms()
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
                var sourcePrograms = source.Programs.AsNoTracking().ToList();

                foreach (var sourceProgram in sourcePrograms)
                {


                    var newProgram = new Program
                    {
                        Name = sourceProgram.Name,
                        Copy = sourceProgram.Copy,
                        oldId = sourceProgram.Id,
                        CompanyId = companyId
                    };

                    _target.Programs.Add(newProgram);
                    _target.SaveChanges();

                    _target.ProgramsIdMap.Add(new ProgramsIdMap
                    {
                        OldId = sourceProgram.Id,
                        NewId = newProgram.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }


            }

        }
        public void MigrateDegreePrograms()
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
                var sourceDegreePrograms = source.degreeprograms.AsNoTracking().ToList();

                foreach (var sourceDP in sourceDegreePrograms)
                {
                    var newLevelId = _target.LevelsIdMap.FirstOrDefault(x => x.OldId == sourceDP.Levelid && x.CompanyId == companyId)?.NewId;
                    var newProgramid = _target.ProgramsIdMap.FirstOrDefault(x => x.OldId == sourceDP.Programid && x.CompanyId == companyId)?.NewId;

                    if (newLevelId == null)
                    {
                        continue;
                    }
                    var newDP = new Degreeprogram
                    {
                        Levelid = newLevelId.Value,
                        Copy = sourceDP.Copy,
                        Programid = newProgramid.Value,
                        oldId = sourceDP.Id,
                        CompanyId = companyId
                    };

                    _target.Degreeprograms.Add(newDP);
                    _target.SaveChanges();

                    _target.DegreeprogramsIdMap.Add(new DegreeprogramsIdMap
                    {
                        OldId = sourceDP.Id,
                        NewId = newDP.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

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

                var sourceCPs = source.campuspostalcodes.AsNoTracking().ToList();

                foreach (var sourceCP in sourceCPs)
                {
                    var newCampusId = _target.CampusIdMap
                        .FirstOrDefault(x => x.OldId == sourceCP.Campusid && x.CompanyId == companyId)
                        ?.NewId;

                    var newPostalCodeId = _target.PostalCodeIdMap
                        .FirstOrDefault(x => x.OldId == sourceCP.Postalcodeid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newCampusId == null || newPostalCodeId == null)
                    {
                        continue;
                    }

                    var newCP = new Campuspostalcode
                    {
                        Campusid = newCampusId.Value,
                        Postalcodeid = newPostalCodeId.Value,
                        CompanyId = companyId,
                        oldId = sourceCP.Id
                    };

                    _target.Campuspostalcodes.Add(newCP);
                    _target.SaveChanges();

                    _target.CampuspostalcodesIdMap.Add(new CampuspostalcodesIdMap
                    {
                        OldId = sourceCP.Id,
                        NewId = newCP.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateDownSellOffers()
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
                var sourceDownSells = source.DownSellOffers.AsNoTracking().ToList();

                foreach (var sourceDownSell in sourceDownSells)
                {
                    var newClientid = _target.ClientIdMap
                        .FirstOrDefault(x => x.OldId == sourceDownSell.Clientid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newClientid == null)
                    {
                        continue;
                    }

                    var newDownSell = new DownSellOffer
                    {
                        Clientid = newClientid.Value,
                        Formurl = sourceDownSell.Formurl,
                        Priority = sourceDownSell.Priority,
                        Dcap = sourceDownSell.Dcap,
                        Active = sourceDownSell.Active,
                        Dcapamt = sourceDownSell.Dcapamt,
                        Mcap = sourceDownSell.Mcap,
                        Mcapamt = sourceDownSell.Mcapamt,
                        Wcap = sourceDownSell.Wcap,
                        Wcapamt = sourceDownSell.Wcapamt,
                        Type = sourceDownSell.Type,
                        Transferphone = sourceDownSell.Transferphone,
                        IncludeUscitizens = sourceDownSell.IncludeUscitizens,
                        IncludePermanentResidents = sourceDownSell.IncludePermanentResidents,
                        IncludeGreenCardHolders = sourceDownSell.IncludeGreenCardHolders,
                        IncludeNonCitizens = sourceDownSell.IncludeNonCitizens,
                        IncludeInternet = sourceDownSell.IncludeInternet,
                        IncludeNoInternet = sourceDownSell.IncludeNoInternet,
                        IncludeMilitary = sourceDownSell.IncludeMilitary,
                        IncludeNonMilitary = sourceDownSell.IncludeNonMilitary,
                        Name = sourceDownSell.Name,
                        Description = sourceDownSell.Description,
                        MondayActive = sourceDownSell.MondayActive,
                        MondayStartTime = sourceDownSell.MondayStartTime,
                        MondayEndTime = sourceDownSell.MondayEndTime,
                        TuesdayActive = sourceDownSell.TuesdayActive,
                        TuesdayStartTime = sourceDownSell.TuesdayStartTime,
                        TuesdayEndTime = sourceDownSell.TuesdayEndTime,
                        WednesdayActive = sourceDownSell.WednesdayActive,
                        WednesdayStartTime = sourceDownSell.WednesdayStartTime,
                        WednesdayEndTime = sourceDownSell.WednesdayEndTime,
                        ThursdayActive = sourceDownSell.ThursdayActive,
                        ThursdayStartTime = sourceDownSell.ThursdayEndTime,
                        FridayActive = sourceDownSell.FridayActive,
                        FridayStartTime = sourceDownSell.FridayStartTime,
                        FridayEndTime = sourceDownSell.FridayEndTime,
                        SaturdayActive = sourceDownSell.SaturdayActive,
                        SaturdayStartTime = sourceDownSell.SaturdayStartTime,
                        SaturdayEndTime = sourceDownSell.SaturdayEndTime,
                        SundayActive = sourceDownSell.SundayActive,
                        SundayStartTime = sourceDownSell.SundayStartTime,
                        SundayEndTime = sourceDownSell.SundayEndTime,
                        Identifier = sourceDownSell.Identifier,
                        Maxage = sourceDownSell.Maxage,
                        Minage = sourceDownSell.Minage,

                        CompanyId = companyId,
                        oldId = sourceDownSell.Id
                    };

                    _target.DownSellOffers.Add(newDownSell);
                    _target.SaveChanges();

                    _target.DownSellOffersIdMap.Add(new DownSellOffersIdMap
                    {
                        OldId = sourceDownSell.Id,
                        NewId = newDownSell.Id,
                        CompanyId = companyId
                    });

                    _target.SaveChanges();
                    _target.ChangeTracker.Clear();

                    _target.SaveChanges();
                }

            }

        }

        public void MigrateDownSellOfferPostalCodes()
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
                var sourceRows = source.DownSellOfferPostalCodes.AsNoTracking().ToList();

                foreach (var row in sourceRows)
                {
                    var newDownSellOfferId = _target.DownSellOffersIdMap.FirstOrDefault(x => x.OldId == row.DownSellOfferId && x.CompanyId == companyId)
                        ?.NewId;

                    var newPostalCodeId = _target.PostalCodeIdMap.FirstOrDefault(x => x.OldId == row.Postalcodeid && x.CompanyId == companyId)
                        ?.NewId;

                    if (newDownSellOfferId == null || newPostalCodeId == null)
                    {
                        continue;
                    }

                    var newEntry = new DownSellOfferPostalCode
                    {
                        DownSellOfferId = newDownSellOfferId.Value,
                        Postalcodeid = newPostalCodeId.Value,
                        CompanyId = companyId,
                        oldId = row.Id
                    };

                    _target.DownSellOfferPostalCodes.Add(newEntry);
                    _target.SaveChanges();

                    _target.DownSellOfferPostalCodesIdMap.Add(new DownSellOfferPostalCodesIdMap
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
                        .FirstOrDefault(x => x.OldId == row.Programid && x.CompanyId == companyId)
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
                        .FirstOrDefault(x => x.OldId == row.Programid && x.CompanyId == companyId)
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
                       .FirstOrDefault(x => x.OldId == row.Programid && x.CompanyId == companyId)
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
