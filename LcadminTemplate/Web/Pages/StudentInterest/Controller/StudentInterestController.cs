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
    public class StudentInterestController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        StudentInterestDataLibrary StudentInterestDL { get; set; }
        public StudentInterestController(
           StudentInterestDataLibrary StudentInterestDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger)
        {
            StudentInterestDL = StudentInterestDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }


        public async Task<StudentInterestVM> getStudentIntereststList()
        {
            var Model = new StudentInterestVM();
            Model.StudentInterests = await StudentInterestDL.GetStudentInterestList();
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await getStudentIntereststList();
            return View("~/Pages/StudentInterest/StudentInterestList.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> StudentInterest(StudentInterestVM ViewModel, string Action)
        {
            try
            {
                var Model = new StudentInterestVM();

                if (Action == "Update")
                {
                    await StudentInterestDL.UpdateStudentInterest(ViewModel.StudentInterest);
                }

                if (Action == "Create")
                {
                    return RedirectToAction("Create");
                }

                if (Action == "Delete")
                {
                    await StudentInterestDL.DeleteStudentInterest(ViewModel.Param);
                }

                if (Action == "Show Update")
                {
                    Model.StudentInterest = await StudentInterestDL.GetStudentInterest(ViewModel.Param);
                    Model.ShowEditStudentInterest = true;

                    var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("StudentInterest/PartialViews/StudentInterestList_Partial", Model)).Result;
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
                var Model = new StudentInterestVM();
                Model.StudentInterest = new Interest();

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/StudentInterest/Create.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentInterestVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    await StudentInterestDL.CreateStudentInterest(ViewModel.StudentInterest);
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
