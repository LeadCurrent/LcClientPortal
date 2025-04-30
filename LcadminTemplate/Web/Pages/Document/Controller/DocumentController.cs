using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using Microsoft.AspNetCore.Http;
using static Data.DocumentEnums;
using System.IO;
using CommonClasses;

namespace Web.Controllers
{
    [Authorize(Policy = "AdminOrDocuments")]

    public class DocumentController : Controller
    {
        
        private readonly RazorViewToStringRenderer viewRenderer;
        public DocumentDataLibrary DocumentDL { get; }
        private readonly ExceptionLogger exceptionLogger;
        public CompanyDataLibrary CompanyDL { get; }
        public CompanyRolesDataLibrary CompanyRolesDL { get; }
        public UserDataLibrary UserDL { get; }


        public DocumentController(
            RazorViewToStringRenderer RazorViewToStringRenderer,
            DocumentDataLibrary DocumentDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            ExceptionLogger _exceptionLogger,
            UserDataLibrary userDataLibrary,
            CompanyRolesDataLibrary CompanyRolesDataLibrary

        )
        {
            DocumentDL = DocumentDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            CompanyDL = CompanyDataLibrary;
            CompanyRolesDL = CompanyRolesDataLibrary;
            exceptionLogger = _exceptionLogger;
            UserDL = userDataLibrary;
        }

        public async Task<DocumentVM> getDocumentViewModel(bool MobileApp=false)
        {
            var Model = new DocumentVM();

            if (HttpContext.Session.GetString("DocumentFilterName") != null)
                Model.FilterName = HttpContext.Session.GetString("DocumentFilterName");

            Model.CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            Model.IsAdmin = User.Claims.Any(x => x.Type == "SystemAdmin" || x.Type == "Admin");
            Model.ParentFolders = await DocumentDL.GetParentFolders(User.Identity.Name, Model.FilterName, Model.CompanyId, Model.IsAdmin);
            Model.Documents = await DocumentDL.GetActiveDocuments(Model.CompanyId, Model.FilterName);
            Model.IsViewOnly = User.Claims.Where(x => x.Type == "Documents-ViewOnly").FirstOrDefault() != null;

            if (HttpContext.Session.GetString("SortBy") != null)
            {
                Model.SortBy = HttpContext.Session.GetString("SortBy");
                if (Model.SortBy == "SortNameAscending")
                {
                    Model.ParentFolders = Model.ParentFolders.OrderBy(x => x.Name).ToList();
                    Model.Documents = Model.Documents.OrderBy(x => x.DocumentName).ToList();
                }
                else if (Model.SortBy == "SortNameDescending")
                {
                    Model.ParentFolders = Model.ParentFolders.OrderByDescending(x => x.Name).ToList();
                    Model.Documents = Model.Documents.OrderByDescending(x => x.DocumentName).ToList();
                }

                if (Model.SortBy == "SortModifiedAscending")
                {
                    Model.ParentFolders = Model.ParentFolders.OrderBy(x => x.UpdateDate).ToList();
                    Model.Documents = Model.Documents.OrderBy(x => x.UpdateDate).ToList();
                }
                else if (Model.SortBy == "SortModifiedDescending")
                {
                    Model.ParentFolders = Model.ParentFolders.OrderByDescending(x => x.UpdateDate).ToList();
                    Model.Documents = Model.Documents.OrderByDescending(x => x.UpdateDate).ToList();
                }
            }
            if (HttpContext.Session.GetString("MobileApp") != null)
                Model.MobileApp = true;
            else if (MobileApp == true)
            {
                Model.MobileApp = MobileApp;
                HttpContext.Session.SetString("MobileApp", "True");
            }
            return Model;
        }

        public async Task<IActionResult> Index(bool MobileApp)
        {
            try
            {
                return View("DocumentList", await getDocumentViewModel(MobileApp));
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DocumentList(DocumentVM ViewModel, string Action)
        {
            try
            {
                var Model = new DocumentVM();
                if (Action == "Create Folder")
                {
                    return RedirectToAction("CreateFolder");
                }
                if (Action == "Edit Folder")
                {
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.Param, EditFolder = true });
                }
                if (Action == "View Folder")
                {
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.Param, EditFolder = false });
                }
                if (Action == "View Document")
                {
                    return RedirectToAction("Edit", new { DocumentId = ViewModel.Param });
                }
                if (Action == "Edit Document")
                {
                    return RedirectToAction("Edit", new { DocumentId = ViewModel.Param });
                }
                if (Action == "Create Document")
                {
                    return RedirectToAction("CreateDocument");
                }

                if (Action == "Apply Filters")
                {
                    if (ViewModel.FilterName != null)
                        HttpContext.Session.SetString("DocumentFilterName", ViewModel.FilterName);
                    else
                        HttpContext.Session.Remove("DocumentFilterName");
                }

                if (Action == "Clear Filters")
                {
                    HttpContext.Session.Remove("DocumentFilterName");
                }

                if (Action.Contains("Sort"))
                    HttpContext.Session.SetString("SortBy", Action);

                Model = await getDocumentViewModel();

                if (Action == "Recently Modified Documents")
                {
                    var RoleIds = await CompanyRolesDL.GetUserRoleIdsByUserNameAsync(User.Identity.Name);
                    Model.ParentFolders = new List<CompanyFolder>();
                    Model.Documents = new List<Document>();

                    foreach (var RoleId in RoleIds)
                    {
                        var documents = await DocumentDL.GetRestrictFolderDocumentsByRole(RoleId, Model.CompanyId);
                        Model.Documents.AddRange(documents);
                    }
                    Model.Documents = Model.Documents.OrderByDescending(doc => doc.CreateDate).ToList();
                }


                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Document/PartialViews/DocumentList_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });




            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateFolder(int ParentFolderId = 0)
        {
            try
            {
                var Model = new DocumentVM();
                Model.CompanyFolder = new CompanyFolder();
                Model.CompanyFolder.CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                Model.CompanyFolder.ParentFolderId = ParentFolderId;
                Model.CompanyFolder.AllUserAccess = true;
                Model.ShowCreateOrUpdate = true;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("CreateCompanyFolder", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateFolder(DocumentVM ViewModel, string Action)
        {
            try
            {
                var Model = new DocumentVM();
                if (Action == "Show Role Access")
                {
                    Model.CompanyFolder = new CompanyFolder();
                    ViewModel.CompanyFolder.AllUserAccess = true;
                    var CompanySubFolderId = await DocumentDL.CreateCompanySubFolder(ViewModel.CompanyFolder, User.Identity.Name);
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = CompanySubFolderId });
                }
                if (Action == "Create Folder")
                {
                    Model.CompanyFolder = new CompanyFolder();
                    ViewModel.CompanyFolder.AllUserAccess = true;
                    var CompanySubFolderId = await DocumentDL.CreateCompanySubFolder(ViewModel.CompanyFolder, User.Identity.Name);
                    if (ViewModel.CompanyFolder.ParentFolderId > 0)
                        return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.ParentFolderId });
                    else
                        return RedirectToAction("Index");
                }

                if (Action == "Cancel")
                    return RedirectToAction("Index");

                var HTML = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/CreateCompanyFolder_Partial", Model);
                return Json(new { isValid = true, html = HTML });

            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        private async Task<DocumentVM> GetFolderEditModel(int FolderId)
        {
            var Model = new DocumentVM();
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            Model.CompanyFolder = await DocumentDL.GetCompanySubFolder(FolderId);
            Model.FolderAccessies = await DocumentDL.GetFolderAcccess(FolderId);
            Model.ParentFolders = await DocumentDL.GetSubFolders(FolderId);
            Model.Documents = await DocumentDL.GetFolderDocuments(FolderId);
            Model.Roles = await CompanyRolesDL.GetRoles(CompanyId);
            Model.CompanyUserRoles = await CompanyRolesDL.GetCompanyUserRoles(CompanyId);
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> EditFolder(int CompanySubFolderId, bool EditFolder = false)
        {
            try
            {
                var Model = new DocumentVM();
                Model.Document = new Document();
                Model.CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                Model.GetAllFoldersName = await DocumentDL.GetAllCompanySubFoldersAsync(CompanySubFolderId);
                Model.CompanyFolder = await DocumentDL.GetCompanySubFolder(CompanySubFolderId);
                Model.ParentFolders = await DocumentDL.GetSubFolders(CompanySubFolderId);
                Model.Documents = await DocumentDL.GetFolderDocuments(CompanySubFolderId);
                Model.FolderAccessies = await DocumentDL.GetFolderAcccess(CompanySubFolderId);
                Model.Roles = await CompanyRolesDL.GetRoles(Model.CompanyId);

                if (EditFolder)
                    Model.ShowCreateOrUpdate = true;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("EditCompanyFolder", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFolder(DocumentVM ViewModel, string Action)
        {
            try
            {
                var Model = new DocumentVM();

                #region Document

                if (Action == "View Document")
                {
                    return RedirectToAction("Edit", new { DocumentId = ViewModel.Param });
                }

                if (Action == "show Create Document")
                {
                    return RedirectToAction("CreateDocument", new { CompanySubFolderId = ViewModel.CompanyFolder.Id });
                }

                if (Action == "Create Document")
                {
                    return RedirectToAction("CreateDocument", new { CompanySubFolderId = ViewModel.CompanyFolder.Id });
                }

                if (Action == "Edit Document")
                {
                    return RedirectToAction("Edit", new { DocumentId = ViewModel.Param });
                }

                if (Action == "Go To Document List")
                {
                    return RedirectToAction("Index");
                }

                #endregion

                #region Folder

                if (Action == "View Folder")
                {
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.Param });
                }

                if (Action == "Create Folder")
                {
                    return RedirectToAction("CreateFolder", new { ParentFolderId = ViewModel.CompanyFolder.Id });
                }

                if (Action == "Edit Folder")
                {
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.Param, EditFolder = true });
                }

                if (Action == "Update Folder")
                {
                    await DocumentDL.UpdateFolder(ViewModel.CompanyFolder, User.Identity.Name);
                    return RedirectToAction("Index");
                }

                if (Action == "Show Role Access")
                {
                    await DocumentDL.UpdateCompanySubFolder(ViewModel.CompanyFolder.Id, ViewModel.CompanyFolder.Name, false, true);
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.Id, EditFolder = true });
                }

                if (Action == "Hide Role Access")
                {
                    await DocumentDL.UpdateCompanySubFolder(ViewModel.CompanyFolder.Id, ViewModel.CompanyFolder.Name, true, false);
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.Id, EditFolder = true });
                }

                if (Action == "Cancel")
                {
                    if (ViewModel.CompanyFolder.ParentFolderId > 0)
                        return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.Id });
                    else
                        return RedirectToAction("Index");
                }

                #endregion

                #region Folder Role Access

                if (Action == "ShowAdd Role")
                {
                    ViewModel = await GetFolderEditModel(ViewModel.CompanyFolder.Id);
                    ViewModel.FolderAccess = new FolderAccess();
                    ViewModel.ShowCreateOrUpdate = true;
                    ViewModel.ShowAddRole = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/Sections/FolderRole", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Cancel Add Role")
                {
                    ViewModel.CompanyFolder = await DocumentDL.GetCompanySubFolder(ViewModel.CompanyFolder.Id);
                    ViewModel.ShowCreateOrUpdate = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/Sections/FolderRole", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Add Role")
                {
                    await DocumentDL.CreateFolderAcccess(ViewModel.FolderAccess, User.Identity.Name, ViewModel.CompanyFolder.Id);
                    ViewModel = await GetFolderEditModel(ViewModel.CompanyFolder.Id);
                    ViewModel.ShowCreateOrUpdate = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/Sections/FolderRole", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Remove Folder Access")
                {
                    await DocumentDL.DeleteFolderAccess(ViewModel.Param);
                    ViewModel = await GetFolderEditModel(ViewModel.CompanyFolder.Id);
                    ViewModel.ShowCreateOrUpdate = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/Sections/FolderRole", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                #endregion

                var HTML = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/EditCompanyFolder_Partial", Model);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateDocument(int CompanySubFolderId)
        {
            try
            {
                var Model = new DocumentVM();
                Model.Document = new Document();
                Model.Document.CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                if (CompanySubFolderId > 0)
                {
                    Model.CompanyFolder = await DocumentDL.GetCompanySubFolder(CompanySubFolderId);
                    Model.CompanyFolder.ParentFolderId = CompanySubFolderId;
                }
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("CreateDocument", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 999999999)]
        public async Task<IActionResult> CreateDocument(DocumentVM ViewModel, string Action, List<IFormFile> uploadfile)
        {
            try
            {

                if (Action == "Upload Video")
                {
                    if (uploadfile.Count > 0 && uploadfile.FirstOrDefault().Length > 0)
                    {
                        var formFile = uploadfile.FirstOrDefault();
                        ViewModel.DocumentId = await DocumentDL.CreateDocument(ViewModel.Document, User.Identity.Name);
                        var UploadHTML = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/Sections/CreateVideo", ViewModel);
                        return Json(new { isValid = true, html = UploadHTML });
                    }
                }

                if (Action == "Resume Video Upload")
                {
                    var UploadHTML = await viewRenderer.RenderViewToStringAsync("Document/PartialViews/Sections/CreateVideo", ViewModel);
                    return Json(new { isValid = true, html = UploadHTML });
                }

                if (Action == "Finish Video Upload")
                {
                    return RedirectToAction("Edit", "Document", new { DocumentId = ViewModel.Document.Id });
                }

                if (Action == "Cancel")
                {
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.Id });
                }

                if (Action == "Create")
                {
                    var DocumentVersion = new DocumentVersion();
                    var DocumentId = 0;

                    string BuildDocumentPath(string basePath, string folderName, string subFolderName, string fileName)
                    {
                        var path = basePath;
                        if (!string.IsNullOrEmpty(folderName))
                            path += folderName + "/";

                        if (!string.IsNullOrEmpty(subFolderName))
                            path += subFolderName + "/";

                        return path + Data.DateTimeFunctions.FileTimeStamp() + "/" + fileName;
                    }

                    foreach (var formFile in uploadfile)
                    {
                        if (formFile.Length > 0)
                        {
                            
                            var isVideo = ViewModel.Document.DocumentFileType == DocumentFileType.Video;
                            var isDocumentOrImage = ViewModel.Document.DocumentFileType == DocumentFileType.Document || ViewModel.Document.DocumentFileType == DocumentFileType.Image;

                            string baseFolder = "Company Documents/";

                            if (isVideo)
                            {
                                var filename = "Company Document/" + ViewModel.Document.DocumentName.Replace(" ", "") + "video" + CommonClasses.StringFormating.FileTimeStamp() + ".mp4";
                                await Storage.UploadLargeDocument(formFile, filename);
                                ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id > 0 ? ViewModel.CompanyFolder.Id : (int?)null;
                                ViewModel.Document.FilePath = CommonClasses.Environment.StorageURL() + filename;
                                ViewModel.Document.CurrentVersionNumber = 1;
                                ViewModel.Document.FileName = filename;
                            }
                            else if (isDocumentOrImage)
                            {
                                string folderName = null, subFolderName = null;
                                if (ViewModel.CompanyFolder.Id > 0)
                                {
                                    ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id;
                                    var companySubfolder = await DocumentDL.GetCompanySubFolder(ViewModel.CompanyFolder.Id);
                                    folderName = companySubfolder.Name;
                                    if (companySubfolder.ParentFolderId > 0)
                                    {
                                        var parentFolder = await DocumentDL.GetCompanySubFolder(companySubfolder.ParentFolderId);
                                        folderName = parentFolder.Name;
                                        subFolderName = companySubfolder.Name;
                                    }
                                }
                                else
                                {
                                    folderName = ViewModel.CompanyFolder.Name;
                                }

                                var filename = BuildDocumentPath(baseFolder, folderName, subFolderName, formFile.FileName);
                                await Storage.UploadLargeDocument(formFile, filename);

                                ViewModel.Document.FilePath = CommonClasses.Environment.StorageURL() + filename;
                                ViewModel.Document.FileName = formFile.FileName;
                                ViewModel.Document.CurrentVersionNumber = 1;
                            }
                        }
                    }

                    if (ViewModel.Document.FilePath != null && ViewModel.Document.DocumentFileType == DocumentFileType.Video)
                    {
                        ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id > 0 ? ViewModel.CompanyFolder.Id : (int?)null;
                        ViewModel.Document.FileName = "YouTube Video - " + ViewModel.Document.DocumentName.Replace(" ", "") + CommonClasses.StringFormating.FileTimeStamp();
                    }

                    if (ViewModel.Document.DocumentFileType == DocumentFileType.Link)
                    {
                        ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id > 0 ? ViewModel.CompanyFolder.Id : (int?)null;
                        ViewModel.Document.CurrentVersionNumber = 1;
                        ViewModel.Document.FileName = ViewModel.Document.DocumentName.Replace(" ", "") + "." + ViewModel.Document.IconType;
                        DocumentVersion.LinkURL = ViewModel.Document.LinkURL;
                    }

                    DocumentId = await DocumentDL.CreateDocument(ViewModel.Document, User.Identity.Name);
                    DocumentVersion.DocumentId = DocumentId;
                    DocumentVersion.VersionNumber = 1;
                    DocumentVersion.FileName = ViewModel.Document.FileName;
                    if (ViewModel.Document.DocumentFileType != DocumentFileType.Link)
                    {
                        DocumentVersion.FilePath = ViewModel.Document.FilePath;
                    }
                    await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);


                    //var DocumentVersion = new DocumentVersion();
                    //var DocumentId = 0;
                    //foreach (var formFile in uploadfile)
                    //{
                    //    if (formFile.Length > 0)
                    //    {
                    //        var env = new Environment();

                    //        if (ViewModel.Document.DocumentFileType == DocumentFileType.Video)
                    //        {
                    //            var filename = "Company Document/" + ViewModel.Document.DocumentName.Replace(" ", "") + "video" + CommonClasses.StringFormating.FileTimeStamp() + ".mp4";
                    //            await Storage.UploadLargeDocument(formFile, filename);
                    //            ViewModel.Document.FilePath = CommonClasses.Environment.StorageURL() + filename;
                    //            ViewModel.Document.CurrentVersionNumber = 1;
                    //            ViewModel.Document.FileName = filename;
                    //            ViewModel.DocumentId = await DocumentDL.CreateDocument(ViewModel.Document, User.Identity.Name);
                    //            DocumentVersion.DocumentId = ViewModel.DocumentId;
                    //            DocumentVersion.VersionNumber = 1;
                    //            DocumentVersion.FileName = ViewModel.Document.FileName;
                    //            DocumentVersion.FilePath = CommonClasses.Environment.StorageURL() + filename;
                    //            await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);
                    //        }
                    //        else if (ViewModel.Document.DocumentFileType == DocumentFileType.Document || ViewModel.Document.DocumentFileType == DocumentFileType.Image)
                    //        {
                    //            var filename = "Company Documents/";
                    //            if (ViewModel.CompanyFolder.Id > 0)
                    //            {
                    //                ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id;
                    //                var companySubfolder = await DocumentDL.GetCompanySubFolder(ViewModel.CompanyFolder.Id);
                    //                if (companySubfolder.ParentFolderId > 0)
                    //                {
                    //                    var folder = await DocumentDL.GetCompanySubFolder(companySubfolder.ParentFolderId);
                    //                    filename += folder.Name + "/" + companySubfolder.Name;


                    //                }
                    //                else
                    //                {
                    //                    filename += companySubfolder.Name;
                    //                }
                    //            }

                    //            else
                    //                filename += ViewModel.CompanyFolder.Name + "/";

                    //            filename += Data.DateTimeFunctions.FileTimeStamp() + "/";
                    //            filename += formFile.FileName;
                    //            await Storage.UploadLargeDocument(formFile, filename);

                    //            ViewModel.Document.FileName = formFile.FileName;
                    //            ViewModel.Document.FilePath = CommonClasses.Environment.StorageURL() + filename;
                    //            ViewModel.Document.CurrentVersionNumber = 1;
                    //            DocumentId = await DocumentDL.CreateDocument(ViewModel.Document, User.Identity.Name);
                    //            DocumentVersion.DocumentId = DocumentId;
                    //            DocumentVersion.VersionNumber = 1;
                    //            DocumentVersion.FileName = ViewModel.Document.FileName;
                    //            DocumentVersion.FilePath = ViewModel.Document.FilePath;
                    //            await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);
                    //        }
                    //    }
                    //}

                    //if(ViewModel.Document.FilePath != null)
                    //{
                    //    ViewModel.Document.FileName = "Youtube Video - " + ViewModel.Document.DocumentName.Replace(" ", "") + CommonClasses.StringFormating.FileTimeStamp();
                    //}

                    //if (ViewModel.Document.DocumentFileType == DocumentFileType.Link)
                    //{
                    //    if(ViewModel.CompanyFolder.Id > 0)
                    //        ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id;
                    //    else
                    //        ViewModel.Document.CompanySubFolderId = null;

                    //    ViewModel.Document.CurrentVersionNumber = 1;
                    //    ViewModel.Document.FileName = ViewModel.Document.DocumentName.Replace(" ", "") + "." + ViewModel.Document.IconType;
                    //    DocumentId = await DocumentDL.CreateDocument(ViewModel.Document, User.Identity.Name);
                    //    DocumentVersion.DocumentId = DocumentId;
                    //    DocumentVersion.VersionNumber = 1;
                    //    DocumentVersion.LinkURL = ViewModel.Document.LinkURL;
                    //    DocumentVersion.FileName = ViewModel.Document.FileName;
                    //    await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);
                    //}
                }

                if (Action == "Resume Video Upload")
                {
                    var UploadHTML = await viewRenderer.RenderViewToStringAsync("Video/PartialViews/Sections/Video", ViewModel);
                    return Json(new { isValid = true, html = UploadHTML });
                }

                if (ViewModel.CompanyFolder.ParentFolderId > 0)
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.Id });
                else
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        private async Task<DocumentVM> GetEditModel(int DocumentId)
        {
            var Model = new DocumentVM();
            Model.Document = await DocumentDL.GetDocument(DocumentId);
            Model.DocumentVersion = await DocumentDL.GetDocumentVersion(DocumentId);
            if (Model.Document.CompanySubFolderId != null && Model.Document.CompanySubFolderId > 0)
            {
                Model.GetAllFoldersName = await DocumentDL.GetAllCompanySubFoldersAsync((int)Model.Document.CompanySubFolderId);
            }
            Model.CompanyFolder = Model.Document.CompanyFolder;
            if (Model.Document.CompanyFolder != null && Model.Document.CompanyFolder.ParentFolderId > 0)
            {
                Model.Document.ParentFolder = await DocumentDL.GetCompanySubFolder(Model.Document.CompanyFolder.ParentFolderId);
            }
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int DocumentId)
        {
            try
            {
                var Model = await GetEditModel(DocumentId);
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("EditDocument", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DocumentVM ViewModel, string Action, List<IFormFile> uploadfile)
        {
            try
            {
                if (Action == "Update")
                {
                    var DocumentVersion = new DocumentVersion();

                    foreach (var formFile in uploadfile)
                    {
                        if (formFile.Length > 0)
                        {
                            

                            if (ViewModel.Document.DocumentFileType == DocumentFileType.Video)
                            {
                                var filename = "Company Document/" + ViewModel.Document.DocumentName.Replace(" ", "") + "video" + CommonClasses.StringFormating.FileTimeStamp() + ".mp4";
                                await Storage.UploadLargeDocument(formFile, filename);
                                ViewModel.Document.FilePath = CommonClasses.Environment.StorageURL() + filename;
                                ViewModel.Document.CurrentVersionNumber += 1;
                                ViewModel.Document.FileName = filename;
                                ViewModel.UpdateSuccessful = await DocumentDL.UpdateDocument(ViewModel.Document, User.Identity.Name);
                                DocumentVersion.DocumentId = ViewModel.Document.Id;
                                DocumentVersion.VersionNumber = ViewModel.Document.CurrentVersionNumber;
                                DocumentVersion.FilePath = ViewModel.Document.FilePath;
                                DocumentVersion.FileName = ViewModel.Document.FileName;
                                await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);
                            }
                            else if (ViewModel.Document.DocumentFileType == DocumentFileType.Document || ViewModel.Document.DocumentFileType == DocumentFileType.Image)
                            {
                                var filename = "Company Documents/";
                                if (ViewModel.CompanyFolder.Id > 0)
                                {
                                    ViewModel.Document.CompanySubFolderId = ViewModel.CompanyFolder.Id;
                                    var companySubfolder = await DocumentDL.GetCompanySubFolder(ViewModel.CompanyFolder.Id);
                                    if (companySubfolder.ParentFolderId > 0)
                                    {
                                        var folder = await DocumentDL.GetCompanySubFolder(companySubfolder.ParentFolderId);
                                        filename += folder.Name + "/" + companySubfolder.Name;


                                    }
                                    else
                                    {
                                        filename += companySubfolder.Name;
                                    }
                                }

                                else
                                    filename += ViewModel.CompanyFolder.Name + "/";

                                filename += Data.DateTimeFunctions.FileTimeStamp() + "/";
                                filename += formFile.FileName;
                                await Storage.UploadLargeDocument(formFile, filename);

                                ViewModel.Document.FileName = formFile.FileName;
                                ViewModel.Document.FilePath = CommonClasses.Environment.StorageURL() + filename;
                                DocumentVersion.FileName = formFile.FileName;
                                DocumentVersion.FilePath = CommonClasses.Environment.StorageURL() + filename;

                                ViewModel.Document.CurrentVersionNumber += 1;
                                ViewModel.UpdateSuccessful = await DocumentDL.UpdateDocument(ViewModel.Document, User.Identity.Name);
                                DocumentVersion.DocumentId = ViewModel.Document.Id;
                                DocumentVersion.VersionNumber = ViewModel.Document.CurrentVersionNumber;
                                DocumentVersion.FilePath = ViewModel.Document.FilePath;
                                DocumentVersion.FileName = ViewModel.Document.FileName;
                                await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);
                            }
                        }
                    }
                    if (ViewModel.Document.DocumentFileType == DocumentFileType.Link)
                    {
                        ViewModel.Document.CurrentVersionNumber += 1;
                        ViewModel.UpdateSuccessful = await DocumentDL.UpdateDocument(ViewModel.Document, User.Identity.Name);
                        DocumentVersion.DocumentId = ViewModel.Document.Id;
                        DocumentVersion.VersionNumber = ViewModel.Document.CurrentVersionNumber;
                        DocumentVersion.LinkURL = ViewModel.Document.LinkURL;
                        DocumentVersion.FileName = ViewModel.Document.FileName;
                        await DocumentDL.CreateDocumentVersion(DocumentVersion, User.Identity.Name);
                    }
                }

                if (Action == "Go To Document List")
                {
                    return RedirectToAction("Index");
                }

                if (Action == "View Folder")
                {
                    return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.Param, EditFolder = false });
                }

                if (Action == "Remove")
                {
                    var DocumentVersions = await DocumentDL.GetDocumentVersionsByDocumentId(ViewModel.Document.Id);
                    foreach (var version in DocumentVersions)
                    {
                        Storage.DeleteDocument(version.FileName);
                        await DocumentDL.DeleteDocumentVersion(version.Id);
                    }
                    await DocumentDL.DeleteDocument(ViewModel.Document.Id);
                }

                if (Action == "Cancel" || Action == "Remove")
                {
                    if (ViewModel.CompanyFolder.Id > 0)
                        return RedirectToAction("EditFolder", new { CompanySubFolderId = ViewModel.CompanyFolder.Id });
                    else
                        return RedirectToAction("Index");
                }

                var VM = await GetEditModel(ViewModel.Document.Id);
                VM.UpdateSuccessful = ViewModel.UpdateSuccessful;

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Document/PartialViews/EditDocument_Partial", VM)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

        public async Task<IActionResult> PreviewAttachment(string filename, int DocumentId)
        {
            var document = await DocumentDL.GetDocument(DocumentId);
            string fileUrl = null;

            if (document != null)
            {
                fileUrl = document.FilePath;
            }
            else
            {
                var documentVersion = await DocumentDL.GetDocumentVersion(DocumentId);
                if (documentVersion != null)
                {
                    fileUrl = documentVersion.FilePath;
                }
                else
                {
                    return NotFound(); // Neither document nor document version was found
                }
            }

            string extension = Path.GetExtension(filename).ToLowerInvariant();

            if (extension == ".docx")
            {
                // Convert the DOCX to PDF
                var pdfFileUrl = ConvertDocxToPdf(fileUrl);
                if (pdfFileUrl == null)
                {
                    return BadRequest("Failed to convert DOCX to PDF.");
                }
                return Json(new { fileUrl = pdfFileUrl });
            }

            return Json(new { fileUrl });
        }

        private string ConvertDocxToPdf(string docxFilePath)
        {
            // Implement the conversion logic here
            string pdfFilePath = docxFilePath.Replace(".docx", ".pdf");

            // Simulating conversion result:
            bool conversionSuccessful = true; // Replace with actual conversion check

            return conversionSuccessful ? pdfFilePath : null;
        }
    }
}