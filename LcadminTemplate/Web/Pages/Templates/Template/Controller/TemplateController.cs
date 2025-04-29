using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using static Data.GeneralEnums;
using Microsoft.AspNetCore.Http.Extensions;
using System.IO;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using CommonClasses;

namespace Web.Controllers
{
    [Authorize]

    public class TemplateController : Controller
    {
        
        RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        Email email { get; }
        TemplateDataLibrary TemplateDL { get; }
        public TemplateController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            TemplateDataLibrary TemplateDataLibrary,
            ExceptionLogger ExceptionLogger,
            Email Email
            
        )
        {
            email = Email;
            exceptionLogger = ExceptionLogger;
            TemplateDL = TemplateDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            
        }

        public async Task<TemplateVM> getTemplateListModel()
        {
            var Model = new TemplateVM();
            Model.Templates = await TemplateDL.GetTemplates();

            //Filters Start
            if (HttpContext.Session.GetInt32("TemplateStatus") != null)
                Model.TemplateStatus = (Status)HttpContext.Session.GetInt32("TemplateStatus");

            Model.Templates = Model.Templates.Where(x => x.Status == Model.TemplateStatus).ToList();

            if (HttpContext.Session.GetString("FilterName") != null)
            {
                Model.FilterName = HttpContext.Session.GetString("FilterName");
                var FilterList = new List<Template>();
                foreach (var template in Model.Templates)
                    if (template.Name.ToUpper().Contains(Model.FilterName.ToUpper()))
                        FilterList.Add(template);
                Model.Templates = FilterList;
            }
            //Filters End

            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var Model = new TemplateVM();
            try
            {
                Model = await getTemplateListModel();
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                else if (MobileApp == true)
                {
                    Model.MobileApp = MobileApp;
                    HttpContext.Session.SetString("MobileApp", "True");
                }
                return View("TemplateList", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> TemplateList(TemplateVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create New")
                    return RedirectToAction("Create");

                if (Action == "Edit")
                    return RedirectToAction("Edit", new { Id = ViewModel.Param });

                if (Action == "Show Edit With PopUp")
                {
                    var Model = new TemplateVM();
                    Model.Template = await TemplateDL.GetTemplate(ViewModel.Param);
                    Model.AllSelectedTemplates = await TemplateDL.GetMutiSelectedTemplates(ViewModel.Param);
                    Model.ShowModelPopup = true;
                    Model.DivToUpdate = null;
                    var PopupHTML = await viewRenderer.RenderViewToStringAsync("Template/PartialViews/Sections/ModalPopup", Model);
                    return Json(new { isValid = true, html = PopupHTML });
                }

                if (Action == "Cancel Popup")
                {
                    var Model = new TemplateVM();
                    var PopupHTML = await viewRenderer.RenderViewToStringAsync("Template/PartialViews/Sections/ModalPopup", Model);
                    return Json(new { isValid = true, html = PopupHTML });
                }

                if (Action == "Save Changes")
                {
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    var selectedCheckboxValues = Request.Form["SelectedTemplate"];
                    await TemplateDL.DeleteTemplateMultiSelect(ViewModel.Template.Id);
                    foreach (var value in selectedCheckboxValues)
                    {
                        await TemplateDL.CreateOrUpdateTemplateMultiSelect(ViewModel.Template.Id, (TemplateEnums.SampleDropdown)Convert.ToInt32(value));
                    }
                    //ViewModel = await GetEditModel(ViewModel.Template.Id);
                    //ViewModel.UpdateSuccessful = true;
                    //return RedirectToAction("Index");
                }

                //Filters Start
                if (Action == "Apply Filters")
                {
                    HttpContext.Session.SetInt32("TemplateStatus", (int)ViewModel.TemplateStatus);

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
                //Filters End

                var VM = await getTemplateListModel();

                var HTML = await viewRenderer.RenderViewToStringAsync("Template/PartialViews/TemplateList_Partial", VM);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var Model = new TemplateVM();
            try
            {
                Model.Template = new Template();
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("CreateTemplate", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(TemplateVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    var template = await TemplateDL.GetTemplateByTemplateName(ViewModel.Template.Name);
                    if (template == null)
                        await TemplateDL.CreateTemplate(ViewModel.Template, User.Identity.Name);
                    else
                    {
                        ViewModel.ShowErrorMsg = true;
                        if (HttpContext.Session.GetString("MobileApp") != null)
                            ViewModel.MobileApp = true;
                        return View("CreateTemplate", ViewModel);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        private async Task<TemplateVM> GetEditModel(int Id)
        {
            var Model = new TemplateVM();
            Model.Template = await TemplateDL.GetTemplate(Id);
            Model.AllSelectedTemplates = await TemplateDL.GetMutiSelectedTemplates(Id);
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var Model = new TemplateVM();
            try
            {
                Model = await GetEditModel(Id);
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("EditTemplate", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TemplateVM ViewModel, string Action, List<IFormFile> uploadfile)
        {
            try
            {
                if (Action == "Remove Photo")
                {
                    Storage.DeleteDocument(ViewModel.Template.Image);
                    ViewModel.Template.Image = null;
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Update Photo")
                {
                    string filename = "TemplateImage" + ViewModel.Template.Id.ToString() + CommonClasses.StringFormating.FileTimeStamp();
                    if (uploadfile.Count > 0 && uploadfile.FirstOrDefault().Length > 0)
                    {
                        var formFile = uploadfile.FirstOrDefault();
                        var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
                        if (formFile.Length > 1024 * 1024)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                memoryStream.Position = 0;

                                using (var image = Image.Load(memoryStream))
                                {
                                    var compressedStream = new MemoryStream();
                                    var encoder = new JpegEncoder
                                    {
                                        Quality = 10
                                    };
                                    image.Save(compressedStream, encoder);
                                    compressedStream.Position = 0;
                                    Storage.UploadDocument(compressedStream, filename);
                                }
                            }
                        }
                        else
                        {
                            Storage.UploadDocument(formFile, filename);
                        }
                        ViewModel.Template.Image = CommonClasses.Environment.StorageURL() + filename;
                        await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    }
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Add Photo")
                {
                    string filename = "TemplateImage" + ViewModel.Template.Id.ToString() + CommonClasses.StringFormating.FileTimeStamp();
                    if (uploadfile.Count > 0 && uploadfile.FirstOrDefault().Length > 0)
                    {
                        var formFile = uploadfile.FirstOrDefault();
                        var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
                        if (formFile.Length > 1024 * 1024)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                memoryStream.Position = 0;

                                using (var image = Image.Load(memoryStream))
                                {
                                    var compressedStream = new MemoryStream();
                                    var encoder = new JpegEncoder
                                    {
                                        Quality = 10
                                    };
                                    image.Save(compressedStream, encoder);
                                    compressedStream.Position = 0;
                                    Storage.UploadDocument(compressedStream, filename);
                                }
                            }
                        }
                        else
                        {
                            Storage.UploadDocument(formFile, filename);
                        }
                        ViewModel.Template.Image = CommonClasses.Environment.StorageURL() + filename;
                        await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    }
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Update Note")
                {
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Remove Photo2")
                {
                    Storage.DeleteDocument(ViewModel.Template.Image2);
                    ViewModel.Template.Note = null;
                    ViewModel.Template.Image2 = null;
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Update Photo2")
                {
                    string filename = "TemplateImage" + ViewModel.Template.Id.ToString() + CommonClasses.StringFormating.FileTimeStamp();
                    if (uploadfile.Count > 0 && uploadfile.FirstOrDefault().Length > 0)
                    {
                        var formFile = uploadfile.FirstOrDefault();
                        var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
                        if (formFile.Length > 1024 * 1024)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                memoryStream.Position = 0;

                                using (var image = Image.Load(memoryStream))
                                {
                                    var compressedStream = new MemoryStream();
                                    var encoder = new JpegEncoder
                                    {
                                        Quality = 10
                                    };
                                    image.Save(compressedStream, encoder);
                                    compressedStream.Position = 0;
                                    Storage.UploadDocument(compressedStream, filename);
                                }
                            }
                        }
                        else
                        {
                            Storage.UploadDocument(formFile, filename);
                        }
                        ViewModel.Template.Image = CommonClasses.Environment.StorageURL() + filename;
                        await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    }
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Add Photo2")
                {
                    string filename = "TemplateImage-2" + ViewModel.Template.Id.ToString() + CommonClasses.StringFormating.FileTimeStamp();
                    if (uploadfile.Count > 0 && uploadfile.FirstOrDefault().Length > 0)
                    {
                        var formFile = uploadfile.FirstOrDefault();
                        var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
                        if (formFile.Length > 1024 * 1024)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                memoryStream.Position = 0;

                                using (var image = Image.Load(memoryStream))
                                {
                                    var compressedStream = new MemoryStream();
                                    var encoder = new JpegEncoder
                                    {
                                        Quality = 10
                                    };
                                    image.Save(compressedStream, encoder);
                                    compressedStream.Position = 0;
                                    Storage.UploadDocument(compressedStream, filename);
                                }
                            }
                        }
                        else
                        {
                            Storage.UploadDocument(formFile, filename);
                        }
                        ViewModel.Template.Image2 = CommonClasses.Environment.StorageURL() + filename;
                        await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    }
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                }

                if (Action == "Cancel")
                    return RedirectToAction("Index");

                else if (Action == "Remove")
                {
                    await TemplateDL.DeleteTemplate(ViewModel.Template.Id);
                    return RedirectToAction("Index");
                }

                else if (Action == "Update")
                {
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                    var selectedCheckboxValues = Request.Form["SelectedTemplate"];
                    await TemplateDL.DeleteTemplateMultiSelect(ViewModel.Template.Id);
                    foreach (var value in selectedCheckboxValues)
                    {
                        await TemplateDL.CreateOrUpdateTemplateMultiSelect(ViewModel.Template.Id, (TemplateEnums.SampleDropdown)Convert.ToInt32(value));
                    }
                    ViewModel = await GetEditModel(ViewModel.Template.Id);
                    ViewModel.UpdateSuccessful = true;
                }
                else if (Action == "Submit Signature" || Action == "Submit Text Signature")
                {
                    if (ViewModel.Signature != null)
                    {
                        string filename = "Signatures/Signature_" + ViewModel.Template.Id.ToString() + CommonClasses.StringFormating.FileTimeStamp();
                        Storage.uploadimage(ViewModel.Signature, filename);
                        ViewModel.Template.Signature = CommonClasses.Environment.StorageURL() + filename;
                    }
                    await TemplateDL.UpdateTemplate(ViewModel.Template, User.Identity.Name);
                }

                var HTML = await viewRenderer.RenderViewToStringAsync("Template/PartialViews/EditTemplate_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }


    }
}