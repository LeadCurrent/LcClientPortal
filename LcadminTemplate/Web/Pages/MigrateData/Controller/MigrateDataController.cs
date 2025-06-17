using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using static Data.GeneralEnums;
using CommonClasses;
using Data.DataContexts;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers
{
    [Authorize(Policy = "SystemAdmin")]
    public class MigrateDataController : Controller
    {
        
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        private readonly RazorViewToStringRenderer viewRenderer;
        public MigrationService MigrationService { get; }

        public MigrateDataController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserDataLibrary UserDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            MigrationService MigrationService,
            RazorViewToStringRenderer RazorViewToStringRenderer)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserDL = UserDataLibrary;
            CompanyDL = CompanyDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            this.MigrationService = MigrationService;
        }

        public async Task<UserVM> getUserListModel()
        {
            var UserVM = new UserVM();
            UserVM.Users = await UserDL.GetUsers();
         

            //Filters Start
            if (HttpContext.Session.GetString("FilterName") != null)
            {
                UserVM.FilterName = HttpContext.Session.GetString("FilterName");
                var FilterList = new List<User>();
                foreach (var user in UserVM.Users)
                    if (user.FirstName.ToUpper().Contains(UserVM.FilterName.ToUpper()))
                        FilterList.Add(user);
                UserVM.Users = FilterList;
            }     
            if (HttpContext.Session.GetInt32("UserFilterStatus") != null)
            {
                UserVM.FilterStatus = (Status)HttpContext.Session.GetInt32("UserFilterStatus").Value;
                UserVM.Users = UserVM.Users.Where(x => x.Status == UserVM.FilterStatus).ToList();
            }
            else
                UserVM.Users = UserVM.Users.Where(x => x.Status == Status.Active).ToList();
            //Filters End
            return UserVM;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ViewModel = new MigrateDataVM();
            return View("MigrateDataList", ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(MigrateDataVM ViewModel, string Action)
        {
             ViewModel = new MigrateDataVM();

            //Filters Start
            if (Action == "Clients")
            {
                
                MigrationService.MigrateAll();

            }
            if (Action == "States")
            {
                MigrationService.MigrateStates();
                MigrationService.MigratePostalCodes();
            }
            if (Action == "Campuses")
            {
            }
            if (Action == "Degree")
            {
                MigrationService.MigrateCampusDegrees();
                MigrationService.MigrateSources();
            }
            if (Action == "Allocations")
            {
                MigrationService.MigrateAllocations();
                MigrationService.MigrateCampusPostalCodes();
            }
            if (Action == "Downsell")
            {
                //MigrationService.MigrateDownSellOffers();
                MigrationService.MigrateDownSellOfferPostalCodes();
                MigrationService.MigrateMasterSchools();
                MigrationService.MigrateMasterSchoolMappings();
            }
            if (Action == "Area")
            {
                MigrationService.MigrateAreas();
                //MigrationService.MigrateProgramAreas();
                //MigrationService.MigrateInterests();
                //MigrationService.MigrateProgramInterests();
                //MigrationService.MigrateGroups();
                //MigrationService.MigrateSchoolGroups();
            }
            if (Action == "ExtraRequiredEducation")
            {
                //MigrationService.MigrateExtraRequiredEducation();
                //MigrationService.MigrateLeadPosts();
                //MigrationService.MigrateOfferTargeting();
                //MigrationService.MigratePingCache();
                //MigrationService.MigratePortalTargeting();
                //MigrationService.MigrateSearchPortals();
                //MigrationService.MigrateConfigEducationLevels();
            }



            var HTML = await viewRenderer.RenderViewToStringAsync("MigrateDate/PartialViews/MigrateData_Partial", ViewModel);
            return Json(new { isValid = true, html = HTML });
        }

        
    }
}
