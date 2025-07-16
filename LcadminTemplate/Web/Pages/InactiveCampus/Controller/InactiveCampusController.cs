using Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Project.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class InactiveCampusController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        CampusDataLibrary CampusDL { get; set; }

        public InactiveCampusController(
            CampusDataLibrary CampusDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger)
        {
            CampusDL = CampusDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }

        public async Task<InactiveCampusVM> getInactiveCampuses(string Search = null)
        {
            var Model = new InactiveCampusVM();
            Model.InactiveCampuses = await CampusDL.GetInactiveCampuses();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                Model.CampusSearch = Search;
                Model.InactiveCampuses = Model.InactiveCampuses
                    .Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Model;
        }

        public async Task<IActionResult> Index(string Search = null)
        {
            var VM = await getInactiveCampuses(Search); // Pass search term to method
            return View("~/Pages/InactiveCampus/InactiveCampuses.cshtml", VM);
        }

        [HttpPost]
        public async Task<IActionResult> InactiveCampuses(InactiveCampusVM ViewModel, string Action)
        {
            try
            {
                var VM = new InactiveCampusVM();

                if (Action == "Search" || Action == "Cancel")
                {
                     VM = await getInactiveCampuses(ViewModel.CampusSearch); 
                }

                //if (Action == "Delete Selected Campuses")
                //{
                //    await CampusDL.DeleteCampuses(ViewModel.InactiveCampuses);
                //     VM = await getInactiveCampuses(ViewModel.CampusSearch);

                //}

                if (Action == "Update InActiveCampuses")
                {
                    await CampusDL.UpdateCampusesStatus(ViewModel.InactiveCampuses);
                     VM = await getInactiveCampuses(ViewModel.CampusSearch);
                }

                if (Action == "Clear Search")
                {
                    VM = await getInactiveCampuses();
                }

                var __html = await viewRenderer.RenderViewToStringAsync("InactiveCampus/PartialViews/InactiveCampuses_Partial", VM);
                return Json(new { isValid = true, html = __html });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(
                    ex,
                    User.Identity.Name,
                    ViewModel.AjaxUpdate,
                    ViewModel.Action,
                    System.Text.Json.JsonSerializer.Serialize(ViewModel),
                    HttpContext.Request.GetDisplayUrl(),
                    HttpContext.Request.GetDisplayUrl());
            }
        }
    }
}
