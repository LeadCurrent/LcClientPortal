using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using Data;
using Microsoft.AspNetCore.Http;
using CommonClasses;

namespace Web.Controllers
{
    [Authorize]

    public class TemplateWithTabsController : Controller
    {
        
        RazorViewToStringRenderer viewRenderer;
        public TemplateDataLibrary TemplateDL { get; }

        public TemplateWithTabsController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            TemplateDataLibrary TemplateDataLibrary
        )
        {
            TemplateDL = TemplateDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var Model = await getTab1Model();
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            else if (MobileApp == true)
            {
                Model.MobileApp = MobileApp;
                HttpContext.Session.SetString("MobileApp", "True");
            }
            return View("Tab1", Model);
        }

        [HttpGet]
        public async Task<IActionResult> Tab1()
        {
            var Model = await getTab1Model();
            return View("Tab1", Model);
        }

        public async Task<TemplateWithTabsVM> getTab1Model()
        {
            var Model = new TemplateWithTabsVM();
            Model.Templates = await TemplateDL.GetTemplates();
            Model.CurrentTab = "Tab 1";
            return Model;
        }

        [HttpPost]
        public async Task<IActionResult> Tab1(TemplateWithTabsVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Edit")
                {
                    ViewModel.Template = await TemplateDL.GetTemplate(ViewModel.Param);
                    ViewModel.Edit = true;
                }

                if (Action == "Save Changes")
                {
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    ViewModel = await getTab1Model();
                }

                if (Action == null)
                {
                    ViewModel = await getTab1Model();
                }

                var HTML = await viewRenderer.RenderViewToStringAsync("TemplateWithTabs/PartialViews/Tab1_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Tab2()
        {
            var Model = await getTab2Model();
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            return View("Tab2", Model);
        }
        public async Task<TemplateWithTabsVM> getTab2Model()
        {
            var Model = new TemplateWithTabsVM();
            Model.Templates = await TemplateDL.GetTemplates();
            Model.CurrentTab = "Tab 2";
            return Model;
        }

        [HttpPost]
        public async Task<IActionResult> Tab2(TemplateWithTabsVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Edit")
                {
                    ViewModel.Template = await TemplateDL.GetTemplate(ViewModel.Param);
                    ViewModel.Edit = true;
                }

                if (Action == "Save Changes")
                {
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    ViewModel = await getTab2Model();
                }

                if (Action == null)
                {
                    ViewModel = await getTab2Model();
                }

                var HTML = await viewRenderer.RenderViewToStringAsync("TemplateWithTabs/PartialViews/Tab2_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}