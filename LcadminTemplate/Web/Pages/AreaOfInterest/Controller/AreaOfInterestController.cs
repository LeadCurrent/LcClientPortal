using Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class AreaOfInterestController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        AreaOfInterestDataLibrary AreaOfInterestDL { get; set; }
        public AreaOfInterestController(
           AreaOfInterestDataLibrary AreaOfInterestDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger)
        {
            AreaOfInterestDL = AreaOfInterestDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }


        public async Task<AreaOfInterestVM> getAreaOfInterestList(string Search = null)
        {
            var Model = new AreaOfInterestVM();
            Model.AreaOfInterests = await AreaOfInterestDL.GetAreaOfInterestList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                Model.AreaOfInterestSearch = Search;
                Model.AreaOfInterests = Model.AreaOfInterests
                    .Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Model;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await getAreaOfInterestList();
            return View("~/Pages/AreaOfInterest/AreaOfInterestList.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AreaOfInterest(AreaOfInterestVM ViewModel, string Action)
        {
            try
            {
                var Model = new AreaOfInterestVM();

                if (Action == "Search")
                {
                    Model = await getAreaOfInterestList(ViewModel.AreaOfInterestSearch);
                    var _HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("AreaOfInterest/PartialViews/AreaOfInterestList_Partial", Model)).Result;
                    return Json(new { isValid = true, html = _HTML });
                }

                if (Action == "Update")
                {
                    await AreaOfInterestDL.UpdateAreaOfInterest(ViewModel.AreaOfInterest);
                }

                if (Action == "Create")
                {
                    return RedirectToAction("Create");
                }

                if (Action == "Delete")
                {
                    await AreaOfInterestDL.DeleteAreaOfInterest(ViewModel.Param);
                }

                if (Action == "Show Update")
                {
                    Model.AreaOfInterest = await AreaOfInterestDL.GetAreaOfInterest(ViewModel.Param);
                    Model.ShowEditAreaOfInterest = true;

                    var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("AreaOfInterest/PartialViews/AreaOfInterestList_Partial", Model)).Result;
                    return Json(new { isValid = true, html = HTML });
                }

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            try
            {
                var Model = new AreaOfInterestVM();
                Model.AreaOfInterest = new Area();

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/AreaOfInterest/CreateAreaOfInterest.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AreaOfInterestVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    await AreaOfInterestDL.CreateAreaOfInterest(ViewModel.AreaOfInterest);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }
    }
}
