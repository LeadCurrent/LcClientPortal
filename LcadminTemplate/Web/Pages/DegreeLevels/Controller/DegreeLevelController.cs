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
    public class DegreeLevelController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        DegreeLevelDataLibrary DegreeLevelDL { get; set; }

        public DegreeLevelController(
            DegreeLevelDataLibrary DegreeLevelDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger)
        {
            DegreeLevelDL = DegreeLevelDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }

        public async Task<DegreeLevelVM> getDegreeLevelList()
        {
            var Model = new DegreeLevelVM();
            Model.DegreeLevels = await DegreeLevelDL.GetDegreeLevels();
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await getDegreeLevelList();
            return View("~/Pages/DegreeLevels/DegreeLevels.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> DegreeLevel(DegreeLevelVM ViewModel, string Action)
        {
            try
            {
                var Model = new DegreeLevelVM();

                if (Action == "Update Degree Level")
                {
                    await DegreeLevelDL.UpdateDegreeLevel(ViewModel.DegreeLevel);
                }

                if (Action == "Create New Degree Level")
                {
                    return RedirectToAction("Create", new { Id = ViewModel.Param });
                }

                if (Action == "Delete Degree Level")
                {
                    await DegreeLevelDL.DeleteDegreeLevel(ViewModel.Param);
                }

                if (Action == "Edit Degree Level")
                {
                    Model.DegreeLevel = await DegreeLevelDL.GetDegreeLevel(ViewModel.Param);
                    Model.ShowEditDegreeLevel = true;

                    var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("DegreeLevels/PartialViews/DegreeLevels_Partial", Model)).Result;
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
                var Model = new DegreeLevelVM();
                Model.DegreeLevel = new Level();

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/DegreeLevels/CreateDegreeLevel.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(DegreeLevelVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create Degree Level")
                {
                    await DegreeLevelDL.CreateDegreeLevel(ViewModel.DegreeLevel);
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
