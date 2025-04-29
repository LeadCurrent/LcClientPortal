using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
//using OfficeOpenXml;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using static Data.GeneralEnums;
using CommonClasses;

namespace Web.Controllers
{
    [Authorize]

    public class TemplateInlineEditController : Controller
    {
        
        RazorViewToStringRenderer viewRenderer;
        public TemplateDataLibrary TemplateDL { get; }

        public TemplateInlineEditController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            TemplateDataLibrary TemplateDataLibrary
        )
        {
            TemplateDL = TemplateDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
        }

        public async Task<Template2VM> getTemplateListModel()
        {
            var Model = new Template2VM();
            Model.Templates = await TemplateDL.GetTemplates();

            if (HttpContext.Session.GetInt32("TemplateStatus") != null)
                Model.TemplateStatus = (Status)HttpContext.Session.GetInt32("TemplateStatus");

            Model.Templates = Model.Templates.Where(x => x.Status == Model.TemplateStatus).ToList();

            if (HttpContext.Session.GetInt32("FilterDropdown") != null)
            {
                Model.FilterDropdown = (TemplateEnums.SampleDropdown)HttpContext.Session.GetInt32("FilterDropdown");
                Model.Templates = Model.Templates.Where(x => x.Dropdown == Model.FilterDropdown).ToList();
            }

            if (HttpContext.Session.GetString("FilterName") != null)
            {
                Model.FilterName = HttpContext.Session.GetString("FilterName");
                var FilterList = new List<Template>();
                foreach (var template in Model.Templates)
                    if (template.Name.ToUpper().Contains(Model.FilterName.ToUpper()))
                        FilterList.Add(template);
                Model.Templates = FilterList;
            }
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var Model = await getTemplateListModel();
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            else if (MobileApp == true)
            {
                Model.MobileApp = MobileApp;
                HttpContext.Session.SetString("MobileApp", "True");
            }
            return View("TemplateList", Model);
        }

        [HttpPost]
        public async Task<IActionResult> TemplateList(Template2VM ViewModel, string Action)
        {


            if (Action == "Apply Filters")
            {
                HttpContext.Session.SetInt32("TemplateStatus", (int)ViewModel.TemplateStatus);

                if (ViewModel.FilterDropdown > 0)
                    HttpContext.Session.SetInt32("FilterDropdown", (int)ViewModel.FilterDropdown);
                else
                    HttpContext.Session.Remove("FilterDropdown");

                if (ViewModel.FilterName != null)
                    HttpContext.Session.SetString("FilterName", ViewModel.FilterName);
                else
                    HttpContext.Session.Remove("FilterName");
            }


            if (Action == "Clear Filters")
            {
                HttpContext.Session.SetInt32("TemplateStatus", 0);
                HttpContext.Session.Remove("FilterDropdown");
                HttpContext.Session.Remove("FilterName");
            }

            if (Action == "Save Changes")
                await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);

            if (Action == "Delete")
                await TemplateDL.DeleteTemplate(ViewModel.Param);

            if (Action == "Create")
                await TemplateDL.CreateTemplate(ViewModel.Template, User.Identity.Name);

            var VM = new Template2VM();
            if (Action == "Create New") //if we're creating new don't need to refresh the model
            {
                VM = await getTemplateListModel();
                VM.ShowCreateNew = true;
                VM.Template = new Template();
            }
            else //refresh the model
                VM = await getTemplateListModel();

            if (Action == "Edit")
            {
                VM.EditTemplateId = ViewModel.Param;
                VM.Template = VM.Templates.Where(x => x.Id == ViewModel.Param).FirstOrDefault();
            }

            var HTML = await viewRenderer.RenderViewToStringAsync("TemplateInlineEdit/PartialViews/TemplateList_Partial", VM);
            return Json(new { isValid = true, html = HTML });
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var Model = new Template2VM();
        //    Model.Template = new Template();
        //    Model.SelectedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
        //    return View("CreateTemplate", Model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Template2VM ViewModel, string Action)
        //{
           

        //    return RedirectToAction("Index");
        //}

        private async Task<Template2VM> GetEditModel(int TemplateId)
        {
            var Model = new Template2VM();
            Model.Template = await TemplateDL.GetTemplate(TemplateId);
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int TemplateId)
        {
            var Model = await GetEditModel(TemplateId);
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            return View("EditTemplate", Model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Template2VM ViewModel, string Action)
        {
            if (Action == "Update")
            {
                await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                ViewModel.UpdateSuccessful = true;
            }

            if (Action == "Remove")
            {
                await TemplateDL.DeleteTemplate(ViewModel.Template.Id);
                return RedirectToAction("Index");
            }

            if (Action == "Cancel")
                return RedirectToAction("Index");

            var VM = await GetEditModel(ViewModel.Template.Id);
            VM.UpdateSuccessful = ViewModel.UpdateSuccessful;

            var HTML = await viewRenderer.RenderViewToStringAsync("Template2/PartialViews/EditTemplate_Partial", VM);
            return Json(new { isValid = true, html = HTML });

        }

    }
}