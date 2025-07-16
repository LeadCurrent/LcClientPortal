using Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;


namespace Web
{
    [Authorize(Policy = "AdminOrSchools")]
    public class SchoolsController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        public SchoolsDataLibrary SchoolDL { get; set; }
        public CampusDataLibrary CampusDL { get; set; }
        public GroupDataLibrary GroupDL { get; set; }
        public AllocationDataLibrary AllocationDL { get; set; }
        ExceptionLogger exceptionLogger { get; }

        public SchoolsController(SchoolsDataLibrary SchoolDataLibrary, CampusDataLibrary CampusDataLibrary, GroupDataLibrary GroupDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            AllocationDataLibrary AllocationsDataLibrary,
            ExceptionLogger ExceptionLogger)
        {
            SchoolDL = SchoolDataLibrary;
            CampusDL = CampusDataLibrary;
            GroupDL = GroupDataLibrary;
            AllocationDL = AllocationsDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }
        public async Task<SchoolsViewModel> GetSchoolListModel(string selectedSchoolName = null)
        {
            var model = new SchoolsViewModel();

            try
            {
                model.Schools = await SchoolDL.GetSchools();
                model.Groups = await GroupDL.GetGroups();

                if (!string.IsNullOrEmpty(selectedSchoolName))
                {
                    model.SelectedSchoolName = selectedSchoolName;

                    model.Schools = model.Schools?
                        .Where(s => s.Name.Contains(selectedSchoolName, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                model.ShowNoListAvailable = model.Schools == null || model.Schools.Count == 0;
            }
            catch (Exception ex)
            {
                // Optional: Log exception here (e.g., to a logger or DB)
                // _logger.LogError(ex, "Error fetching school list");

                model.ShowNoListAvailable = true;
                //model.ErrorMessage = "An error occurred while loading the school list.";
            }

            return model;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var selectedSchoolName = HttpContext.Session.GetString("FilterSchoolsName");
            var model = await GetSchoolListModel(selectedSchoolName);
            return View("~/Pages/Schools/SchoolsList.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SchoolsList(SchoolsViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Search")
                {
                    if (ViewModel.SelectedSchoolName != null)
                        HttpContext.Session.SetString("FilterSchoolsName", ViewModel.SelectedSchoolName);

                    return RedirectToAction("Index");
                }

                if (Action == "Clear Search")
                {
                    HttpContext.Session.Remove("FilterSchoolsName");
                    return RedirectToAction("Index");
                }

                if (Action == "Create")
                    return RedirectToAction("Create");

                if (Action == "Edit")
                    return RedirectToAction("EditSchools", new { SchoolId = ViewModel.Param });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var Model = new SchoolsViewModel();
            try
            {
                Model.Scholl = new Scholls();
                Model.ShowEditSchools = true;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("~/Pages/Schools/CreateSchools.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SchoolsViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    var SchoolsId = await SchoolDL.CreateSchool(ViewModel.Scholl);
                    return RedirectToAction("EditSchools", new { SchoolId = SchoolsId });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }


        private async Task<SchoolsViewModel> GetEditModel(int SchoolId)
        {
            var Model = new SchoolsViewModel();
            Model.Scholl = await SchoolDL.GetSchool(SchoolId);
            return Model;
        }
        [HttpGet]
        public async Task<IActionResult> EditSchools(int SchoolId)
        {
            var Model = new SchoolsViewModel();
            try
            {
                Model = await GetEditModel(SchoolId);
                Model.Schools = await SchoolDL.GetSchools();
                Model.Groups = await GroupDL.GetGroups();
                foreach (var group in Model.Groups)
                {
                    group.IsChecked = Model.Scholl.Schoolgroups.Any(g => g.Groupid == group.Id);
                }
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("EditSchools", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditSchools(SchoolsViewModel ViewModel, string Action, int[] SelectedGroupIds)
        {
            try
            {

                if (Action == "Edit School")
                {
                    ViewModel.Scholl = await SchoolDL.GetSchool(ViewModel.Scholl.Id);
                    ViewModel.Groups = await GroupDL.GetGroups();
                    foreach (var group in ViewModel.Groups)
                    {
                        group.IsChecked = ViewModel.Scholl.Schoolgroups.Any(g => g.Groupid == group.Id);
                    }
                    ViewModel.ShowEditSchools = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Schools/PartialViews/Sections/SchoolsDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Update School")
                {
                    await SchoolDL.UpdateSchoolGroups(ViewModel.Scholl.Id, ViewModel.Groups, ViewModel.Scholl.oldId, null/*ViewModel.Scholl.CompanyId*/);
                    bool status = await SchoolDL.UpdateSchool(ViewModel.Scholl);

                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Schools/PartialViews/Sections/SchoolsDetails", ViewModel);
                    if (status)
                    {
                        return Json(new { isValid = true, html = HTML1 });
                    }
                    else
                    {
                        return Json(new { isValid = false, html = HTML1 });
                    }
                }
                if (Action == "Cancel Edit")
                {
                    ViewModel.Scholl = await SchoolDL.GetSchool(ViewModel.Scholl.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Schools/PartialViews/Sections/SchoolsDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Cancel")
                    return RedirectToAction("Index");


                ViewModel = await GetEditModel(ViewModel.Scholl.Id);
                var HTML = await viewRenderer.RenderViewToStringAsync("Schools/PartialViews/EditSchools_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CampusesBySchool(int schoolId)
        {
            try
            {
                var campuses = await CampusDL.GetCampusBYSchooldId(schoolId);
                var school = await SchoolDL.GetSchool(schoolId);
                var model = new SchoolsViewModel
                {
                    
                    Campuses = campuses,
                    School = school
                };

                return View("SchoolCampuses", model);
            }
            catch (Exception ex)
            {
                // Optional: log the exception (e.g., using ILogger)
                // _logger.LogError(ex, "Error fetching campuses for school ID: {SchoolId}", schoolId);

                TempData["ErrorMessage"] = "An error occurred while loading campuses for the selected school.";
                return RedirectToAction("Index"); // Redirect to a fallback action (like the school list)
            }
        }

        [HttpPost]
        public async Task<IActionResult> CampusesBySchool(SchoolsViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "EditCampuses")
                {
                    var Model = new SchoolsViewModel();
                    Model.Campus = await CampusDL.GetCampus(ViewModel.Param);
                }

                var HTML = await viewRenderer.RenderViewToStringAsync("Schools/PartialViews/EditCampus_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }
    }
}
