using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class ProgramController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        ProgramDataLibrary ProgramDL { get; set; }
        DegreeLevelDataLibrary DegreeLevelDL { get; set; }
        AreaOfInterestDataLibrary AreaOfInterestDL { get; set; }
        StudentInterestDataLibrary StudentInterestDL { get; set; }
        public ProgramController(
           ProgramDataLibrary ProgramDataLibrary,
           DegreeLevelDataLibrary DegreeLevelDataLibrary,
           AreaOfInterestDataLibrary AreaOfInterestDataLibrary,
            StudentInterestDataLibrary StudentInterestDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger)
        {
            ProgramDL = ProgramDataLibrary;
            DegreeLevelDL = DegreeLevelDataLibrary;
            AreaOfInterestDL = AreaOfInterestDataLibrary;
            StudentInterestDL = StudentInterestDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }

        public async Task<ProgramVM> getProgramList()
        {
            var Model = new ProgramVM();
            Model.Programs = await ProgramDL.GetPrograms();
            return Model;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await getProgramList();
            return View("~/Pages/Programs/Programs.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Programs(ProgramVM ViewModel, string Action)
        {
            try
            {
                var Model = new ProgramVM();

                if (Action == "Create")
                {
                    return RedirectToAction("Create");
                }

                if (Action == "Delete")
                {
                    await ProgramDL.DeleteProgram(ViewModel.Param);
                }

                if (Action == "Show Update")
                {
                    return RedirectToAction("Edit",new{ProgramId = ViewModel.Param });
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
                var Model = new ProgramVM();
                Model.Program = new Program();

                Model.Program.Areas = await AreaOfInterestDL.GetAreaOfIntrestList();
                Model.Program.Levels = await DegreeLevelDL.GetDegreeLevels();
                Model.Program.Interests = await StudentInterestDL.GetStudentIntrestList();

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Programs/CreateProgram.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProgramVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    await ProgramDL.CreateProgram(ViewModel.Program);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int ProgramId)
        {

            try
            {
                var Model = new ProgramVM();
                Model.Program = await ProgramDL.GetProgramById(ProgramId);
                Model.Program.Areas = await AreaOfInterestDL.GetAreaOfIntrestList();
                Model.Program.Levels = await DegreeLevelDL.GetDegreeLevels();
                Model.Program.Interests = await StudentInterestDL.GetStudentIntrestList();

                var selectedAreaIds = Model.Program.ProgramAreas.Select(x => x.Areaid).ToHashSet();
                var selectedLevelIds = Model.Program.Degreeprograms.Select(x => x.Levelid).ToHashSet();
                var selectedInterestIds = Model.Program.Programinterests.Select(x => x.Interestid).ToHashSet();

                // Mark as checked
                foreach (var area in Model.Program.Areas)
                {
                    area.IsChecked = selectedAreaIds.Contains(area.Id);
                }
                foreach (var level in Model.Program.Levels)
                {
                    level.IsChecked = selectedLevelIds.Contains(level.Id);
                }
                foreach (var interest in Model.Program.Interests)
                {
                    interest.IsChecked = selectedInterestIds.Contains(interest.Id);
                }


                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Programs/EditProgram.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProgramVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Update")
                {
                    await ProgramDL.UpdateProgram(ViewModel.Program);
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
