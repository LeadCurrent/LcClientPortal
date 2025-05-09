using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Data.GeneralEnums;
using static Data.CompanyEnums;
using CommonClasses;

namespace Web
{
    [Authorize(Policy = "AdminOrSources")]
    public class SourcesController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        public SourcesDataLibrary SourcesDL { get; set; }
        public AllocationDataLibrary AllocationDL { get; set; }
        ExceptionLogger exceptionLogger { get; }


        public SourcesController(SourcesDataLibrary SourcesDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            AllocationDataLibrary AllocationsDataLibrary,
            ExceptionLogger ExceptionLogger)
        {
            SourcesDL = SourcesDataLibrary;
            AllocationDL = AllocationsDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }

        public async Task<SourcesViewModel> GetSourceListModel()
        {
            var model = new SourcesViewModel();
            model.Company = new Company();
            var companyClaimID = User.Claims.FirstOrDefault(x => x.Type == "CompanyId");
            var companyNameClaim = User.Claims.FirstOrDefault(x => x.Type == "CompanyName");
            if (companyNameClaim != null)
                model.Company.Name = companyNameClaim.Value;
            if (companyClaimID != null && int.TryParse(companyClaimID.Value, out int companyId))
            {
                model.Company.Id = companyId;

                model.Sources = await SourcesDL.GetSourcesByCompanyId(companyId);

                if (model.Sources != null && model.Sources.Count > 0)
                {
                    foreach (var source in model.Sources)
                    {
                        var (active, inactive) = await AllocationDL.GetAllocationCountsBySourceId(source.Id);
                        source.ActiveAllocationsCount = active;
                        source.InactiveAllocationsCount = inactive;
                    }

                }
                else
                    model.ShowNoListAvailable = true;

            }
            else
            {
                model.ShowNoListAvailable = true;
            }
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await GetSourceListModel();
            return View("~/Pages/Sources/SourcesList.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SourcesList(SourcesViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                    return RedirectToAction("Create");

                if (Action == "Edit")
                    return RedirectToAction("EditSources", new { SourceId = ViewModel.Param });

                #region for Apply Filters
                SourcesViewModel SourceVM = new SourcesViewModel();
                if (ViewModel.SelectedSourceName != null)
                    HttpContext.Session.SetString("FilterSourceName", ViewModel.SelectedSourceName);
                else
                    HttpContext.Session.Remove("FilterSourceName");



                if (Action == "Apply Filters")
                {
                    return RedirectToAction("Index");
                }
                #endregion for Apply Filters

                if (Action == "Clear Filters")
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index");
                }
                var VM = await GetSourceListModel();

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Sources/PartialViews/SourcesList_Partial", VM)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var Model = new SourcesViewModel();
            try
            {
                Model.Source = new Source();
                Model.ShowEditSources = true;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("~/Pages/Sources/CreateSource.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SourcesViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    var companyClaimID = User.Claims.FirstOrDefault(x => x.Type == "CompanyId");
                    ViewModel.Source.CompanyId= Convert.ToInt32(companyClaimID.Value);
                    var SourceId = await SourcesDL.CreateSource(ViewModel.Source);
                    return RedirectToAction("EditSources", new { SourceId = SourceId });
                }

                if (Action == "GenerateGUID")
                {
                    ViewModel.Source.Apikey = Guid.NewGuid().ToString();
                    ViewModel.ShowEditSources = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("~/Pages/Sources/PartialViews/Create_Partial.cshtml", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }


        private async Task<SourcesViewModel> GetEditModel(int SourceId)
        {
            var Model = new SourcesViewModel();
            Model.Source = await SourcesDL.GetSource(SourceId);
            return Model;
        }
        [HttpGet]
        public async Task<IActionResult> EditSources(int SourceId)
        {
            var Model = new SourcesViewModel();
            try
            {
                Model = await GetEditModel(SourceId);

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("EditSources", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditSources(SourcesViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Edit Source")
                {
                    ViewModel.Source = await SourcesDL.GetSource(ViewModel.Source.Id);
                    ViewModel.ShowEditSources = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Sources/PartialViews/Sections/SourcesDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Update Source")
                {
                    bool status = await SourcesDL.UpdateSource(ViewModel.Source);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Sources/PartialViews/Sections/SourcesDetails", ViewModel);
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
                    ViewModel.Source = await SourcesDL.GetSource(ViewModel.Source.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Sources/PartialViews/Sections/SourcesDetails", ViewModel);
                        return Json(new { isValid = true, html = HTML1 });
                }
                if(Action == "GenerateGUID")
                {
                    ViewModel.Source.Apikey = Guid.NewGuid().ToString();
                    ViewModel.ShowEditSources = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Sources/PartialViews/EditSources_Partial", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Cancel")
                    return RedirectToAction("Index");


                ViewModel = await GetEditModel(ViewModel.Source.Id);
                var HTML = await viewRenderer.RenderViewToStringAsync("Sources/PartialViews/EditSources_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

    }
}
