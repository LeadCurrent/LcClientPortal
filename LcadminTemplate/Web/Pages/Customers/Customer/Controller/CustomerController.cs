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
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Crypto.Agreement;
using DinkToPdf;
using DinkToPdf.Contracts;
namespace Web.Controllers
{
    [Authorize(Policy = "AdminOrCustomers")]
    public class CustomerController : Controller
    {

        private readonly IConverter PDFConverter;

        private readonly RazorViewToStringRenderer viewRenderer;
        private UserDataLibrary UserDL { get; }
        public CustomerDataLibrary CustomerDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public SignInManager<User> SignInManager { get; }

        Email email { get; }
        public UserManager<User> UserManager { get; }
        ExceptionLogger exceptionLogger { get; }

        public List<Customer> lst { get; set; } = new List<Customer>();
        public CompanyIntegrationDataLibrary CompanyIntegrationDL { get; set; }
        

        public CustomerController(
            CustomerDataLibrary CustomerDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            UserDataLibrary UserDataLibrary,
            ExceptionLogger ExceptionLogger,
             UserManager<User> userManager,
             SignInManager<User> signInManager,
            CompanyDataLibrary CompanyDataLibrary,
            Email Email,
            IConverter converter,
            CompanyIntegrationDataLibrary CompanyIntegrationDataLibrary

        )
        {
            CustomerDL = CustomerDataLibrary;
            CompanyDL = CompanyDataLibrary;
            UserDL = UserDataLibrary;
            UserManager = userManager;
            SignInManager = signInManager;
            email = Email;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
            PDFConverter = converter;
            CompanyIntegrationDL = CompanyIntegrationDataLibrary;

        }
        public async Task<CustomersViewModel> GetCustomerListModel()
        {

            var Model = new CustomersViewModel();
            Model.Customers = await CustomerDL.GetCustomers(Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value));
            Model.Customers2 = await CustomerDL.GetCustomers(Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value));
            return Model;
        }
        public async Task<CustomersViewModel> GetCustomerFilterdListModel(string companyName, string city)
        {
            var Model = new CustomersViewModel();
            Model.Customers = await CustomerDL.GetFilterdCustomers(companyName, city);
            Model.Customers2 = await CustomerDL.GetCustomers(Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value));
            return Model;
        }
        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var Model = new CustomersViewModel();
            try
            {

                Model.Customer = new Customer();
                Model.CurrentTab = "Customer Profile";
                if (HttpContext.Session.GetString("FilterCompanyName") != null)
                    Model.SelectedCompanyName = HttpContext.Session.GetString("FilterCompanyName");
                if (Model.SelectedCompanyName != null || Model.SelectedCityState != null || Model.SelectedContacted != null)
                {
                    return await CustomerList(Model, "");
                }

                Model = await GetCustomerListModel();
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                else if (MobileApp == true)
                {
                    Model.MobileApp = MobileApp;
                    HttpContext.Session.SetString("MobileApp", "True");
                }
                Model.DivToUpdate = null;
                return View("~/Pages/Customers/Customer/CustomerList.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }
        [HttpPost]
        public async Task<IActionResult> CustomerList(CustomersViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                    return RedirectToAction("Create");

                #region for Aplly Filters
                CustomersViewModel CustomersVM = new CustomersViewModel();
                if (ViewModel.SelectedCompanyName != null)
                    HttpContext.Session.SetString("FilterCompanyName", ViewModel.SelectedCompanyName);
                else
                    HttpContext.Session.Remove("FilterCompanyName");



                if (Action == "Apply Filters")
                {
                    return RedirectToAction("Index");
                }
                #endregion for Aplly Filters
                if (Action == "Clear Filters")
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index");
                }
                var VM = await GetCustomerListModel();
                var CompanyId = Int32.Parse(User.Claims.Where(c => c.Type == "CompanyId").FirstOrDefault().Value);
                if (ViewModel.SelectedCompanyName != null || ViewModel.SelectedCityState != null || ViewModel.SelectedContacted != null)
                {
                    if (ViewModel.SelectedCompanyName != null || ViewModel.SelectedCityState != null)
                    {
                        VM = await GetCustomerFilterdListModel(ViewModel.SelectedCompanyName, ViewModel.SelectedCityState);
                        VM.SelectedCompanyName = ViewModel.SelectedCompanyName != null ? ViewModel.SelectedCompanyName : null;
                        VM.SelectedCityState = ViewModel.SelectedCityState != null ? ViewModel.SelectedCityState.ToString() : null;
                    }
                    return View("CustomerList", VM);
                }
                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("Customer/PartialViews/CustomerList_Partial", VM)).Result;
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
            var Model = new CustomersViewModel();
            try
            {
                Model.Customer = new Customer();
                Model.ShowEditCustomer = true;
                Model.Customers = await CustomerDL.GetCustomers(Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value));
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                //return View("CreateCustomer", Model);
                return View("~/Pages/Customers/Customer/CreateCustomer.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomersViewModel ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    var CompanyId = Int32.Parse(User.Claims.Where(c => c.Type == "CompanyId").FirstOrDefault().Value);
                    ViewModel.Customer.CompanyId = CompanyId;
                    var CustomerId = await CustomerDL.CreateCustomer(ViewModel.Customer, User.Identity.Name);
                    return RedirectToAction("Edit", new { CustomerId = CustomerId });
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }
        private async Task<CustomersViewModel> GetEditModel(int CustomerId)
        {
            var Model = new CustomersViewModel();
            Model.CurrentTab = "Customer Profile";
            Model.Customer = await CustomerDL.GetCustomer(CustomerId);
            var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
            Model.Company = await CompanyDL.GetCompany(CompanyId);
            Model.Customers = await CustomerDL.GetCustomers(Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value));
            return Model;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int CustomerId, int EmailMessageId = 0)
        {
            var Model = new CustomersViewModel();
            try
            {
                Model = await GetEditModel(CustomerId);
                Model.EmailMessageId = EmailMessageId;

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                return View("EditCustomer", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomersViewModel ViewModel, string Action, List<IFormFile> uploadfile)
        {
            try
            {
                /* Customer Details */

                if (Action == "Edit Customer")
                {
                    ViewModel.Customer = await CustomerDL.GetSimpleCustomer(ViewModel.Customer.Id);
                    ViewModel.ShowEditCustomer = true;
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/CustomerDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Cancel Edit")
                {
                    await CustomerDL.UpdateCustomer(ViewModel.Customer, User.Identity.Name);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/CustomerDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Update Customer")
                {
                    await CustomerDL.UpdateCustomer(ViewModel.Customer, User.Identity.Name);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/CustomerDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Delete Contacts")
                {
                    await CustomerDL.DeleteCustomer(ViewModel.Customer, User.Identity.Name);
                    return RedirectToAction("Index");
                }

                if (Action == "View Lead")
                {
                    return RedirectToAction("Edit", "Leads", new { LeadId = ViewModel.Param });
                }
                if (Action == "Cancel Customer Details")
                {
                    ViewModel = await GetEditModel(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/CustomerDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Cancel Edit Customer")
                {
                    await CustomerDL.UpdateCustomer(ViewModel.Customer, User.Identity.Name);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/CustomerDetails", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Remove")
                {
                    await CustomerDL.DeleteCustomer(ViewModel.Customer.Id);
                    return RedirectToAction("Index");
                }
                if (Action == "Cancel")
                    return RedirectToAction("Index");

                if (Action == "Go To Emails")
                {
                    return RedirectToAction("Customers", "UserEmail", new { EmailMessageId = ViewModel.Param, CustomerId = ViewModel.Customer.Id });
                }


                /* Notes & Documents */


                if (Action == "Show Add Note")
                {
                    ViewModel.ShowNewNote = true;
                    ViewModel.CustomerNote = new CustomerNote();
                    ViewModel.CustomerDocument = await CustomerDL.GetCustomerDocument(ViewModel.Param);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Show Edit Note")
                {
                    ViewModel.ShowEditNote = true;
                    ViewModel.CustomerNote = await CustomerDL.GetCustomerNote(ViewModel.Param);
                    ViewModel.CustomerDocument = await CustomerDL.GetCustomerDocument(ViewModel.Param);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Add Note")
                {
                    ViewModel.CustomerNote.CustomerId = ViewModel.Customer.Id;
                    ViewModel.CurrentNotesTab = "Notes";
                    await CustomerDL.CreateCustomerNote(ViewModel.CustomerNote, User.Identity.Name);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Cancel Note")
                {
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    ViewModel.CurrentNotesTab = "Notes";
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Update Note")
                {
                    await CustomerDL.UpdateCustomerNote(ViewModel.CustomerNote, User.Identity.Name);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Remove Note")
                {
                    await CustomerDL.DeleteCustomerNote(ViewModel.Param);
                    ViewModel.CurrentNotesTab = "Notes";
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Show Add Document")
                {
                    ViewModel.ShowNewDocument = true;
                    ViewModel.CustomerDocument = new Document();
                    ViewModel.CustomerNote = await CustomerDL.GetCustomerNote(ViewModel.Param);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Show Edit Document")
                {
                    ViewModel.ShowEditDocument = true;
                    ViewModel.CustomerDocument = await CustomerDL.GetCustomerDocument(ViewModel.Param);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Add Document")
                {
                    foreach (var formFile in uploadfile)
                    {
                        if (formFile.Length > 0)
                        {
                            var date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time"); ;
                            var filename = ViewModel.Customer.Id.ToString() + "/" + formFile.FileName;
                            Storage.UploadDocument(formFile, filename);
                            var CustomerDocument = new Document();
                            CustomerDocument.DocumentName = formFile.FileName;
                            CustomerDocument.FilePath = CommonClasses.Environment.StorageURL() + filename;
                            CustomerDocument.CustomerId = ViewModel.Customer.Id;
                            await CustomerDL.CreateCustomerDocument(CustomerDocument, User.Identity.Name);
                        }
                    }
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    ViewModel.CurrentNotesTab = "Document";
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                if (Action == "Cancel Document")
                {
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);

                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Update Document")
                {
                    await CustomerDL.UpdateCustomerDocument(ViewModel.CustomerDocument, User.Identity.Name);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }
                if (Action == "Remove Document")
                {
                    ViewModel.CustomerDocument = await CustomerDL.GetCustomerDocument(ViewModel.Param);
                    Storage.DeleteDocument(ViewModel.CustomerDocument.DocumentName);
                    await CustomerDL.DeleteCustomerDocument(ViewModel.Param);
                    ViewModel.Customer.CustomerDocuments = await CustomerDL.GetCustomerDocuments(ViewModel.Customer.Id);
                    ViewModel.Customer.CustomerNotes = await CustomerDL.GetCustomerNotes(ViewModel.Customer.Id);
                    var HTML1 = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Sections/Customer_Notes", ViewModel);
                    return Json(new { isValid = true, html = HTML1 });
                }

                ViewModel = await GetEditModel(ViewModel.Customer.Id);
                var HTML = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/EditCustomer_Partial", ViewModel);
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Email(int CustomerId)
        //{
        //    try
        //    {
        //        var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
        //        var Model = new CustomersViewModel();
        //        Model = await GetEditModel(CustomerId);
        //        Model.UserEmailVM.DateTo = StringFormating.CurrentTime().Date;
        //        Model.UserEmailVM.DateFrom = DateTime.MinValue;
        //        Model.UserEmailVM.Company = await CompanyDL.GetCompanyProfile(CompanyId);
        //        Model.UserEmailVM.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);
        //        //Model.UserEmailVM.UserProfile = await UserDL.GetCurrentUser(User.Identity.Name);
        //        Model.UserEmailVM.CompanyId = Model.UserEmailVM.Company.Id;
        //        var CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(CompanyId);
        //        var user = await UserDL.GetCurrentUser(User.Identity.Name);
        //        var CompanyUser = await UserDL.GetCompanyUserByUserId(user.Id);
        //        var CompanyUserEmails = await UserDL.GetCompanyUserEmails(CompanyUser.Id);
        //        Model.UserEmailVM.CompanyEmailAccounts = new List<CompanyEmailAccount>();
        //        Model.UserEmailVM.EmailMessages = new List<EmailMessage>();
        //        Model.UserEmailVM.EmailMessageView = new List<EmailMessageView>();
        //        Model.UserEmailVM.Drafts = new List<SendEmail>();
        //        Model.UserEmailVM.Customers = await CustomerDL.GetActiveCustomers(CompanyId);

        //        //Model.UserEmailVM.Vendors = await VendorDL.GetVendors(CompanyId);
        //        //Model.UserEmailVM.Leads = await LeadDL.GetLeads(CompanyId);
        //        Model.UserEmailVM.Customer = await CustomerDL.GetCustomer(CustomerId);
        //        Model.UserEmailVM.FilterCustomer = CustomerId;
        //        Model.UserEmailVM.CurrentTab = "Customers";
        //        foreach (var CompanyEmail in CompanyEmailAccounts)
        //        {
        //            if (CompanyUser != null)
        //            {
        //                if (CompanyEmail.AllStaffAccess)
        //                {
        //                    if (CompanyUser.CompanyId == CompanyEmail.CompanyId)
        //                    {

        //                        List<EmailMessageView> messages = await UserEmailDL.GetEmailMessagesByCompanyEmailAccountIdAsync(CompanyEmail.Id, "Customers", Model.UserEmailVM.DateFrom, Model.UserEmailVM.DateTo, User.Identity.Name);
        //                        Model.UserEmailVM.EmailMessageView.AddRange(messages);
        //                        List<UserEmailAutoDelete> userEmailAutoDeletes = await UserEmailDL.GetUserEmailAutoDeletes(CompanyEmail.Id);
        //                        if (Model.UserEmailVM.UserEmailAutoDelete != null && userEmailAutoDeletes != null)
        //                            Model.UserEmailVM.UserEmailAutoDelete.AddRange(userEmailAutoDeletes);

        //                    }

        //                }
        //                else
        //                {
        //                    if (CompanyUserEmails.Any(x => x.CompanyEmailAccountId == CompanyEmail.Id && x.CompanyUserId == CompanyUser.Id))
        //                    {

        //                        List<EmailMessageView> messages = await UserEmailDL.GetEmailMessagesByCompanyEmailAccountIdAsync(CompanyEmail.Id, "Customers", Model.UserEmailVM.DateFrom, Model.UserEmailVM.DateTo, User.Identity.Name);
        //                        Model.UserEmailVM.EmailMessageView.AddRange(messages);
        //                        List<UserEmailAutoDelete> userEmailAutoDeletes = await UserEmailDL.GetUserEmailAutoDeletes(CompanyEmail.Id);
        //                        if (Model.UserEmailVM.UserEmailAutoDelete != null && userEmailAutoDeletes != null)
        //                            Model.UserEmailVM.UserEmailAutoDelete.AddRange(userEmailAutoDeletes);
        //                    }
        //                }
        //            }
        //        }
        //        //foreach (var emailAccount in CompanyUserEmails)
        //        //{
        //        //        List<EmailMessageView> messages = await UserEmailDL.GetEmailMessagesByCompanyEmailAccountIdAsync(emailAccount.CompanyEmailAccountId, "Customers", Model.UserEmailVM.DateFrom, Model.UserEmailVM.DateTo);
        //        //        Model.UserEmailVM.EmailMessageView.AddRange(messages);
        //        //        Model.UserEmailVM.CompanyEmailAccounts.Add(emailAccount.CompanyEmailAccount);
        //        //}             
        //        Model.UserEmailVM.EmailMessageView = Model.UserEmailVM.EmailMessageView.OrderByDescending(x => x.ReceivedDateTime).ToList();
        //        Model.UserEmailVM.OtherController = true;
        //        Model.UserEmailVM.CustId = CustomerId;
        //        Model.CurrentTab = "Email";
        //        Model.UserEmailVM.CustomerController = true;
        //        if (HttpContext.Session.GetString("MobileApp") != null)
        //            Model.MobileApp = true;
        //        return View("Email", Model);

        //    }
        //    catch (Exception ex)
        //    {
        //        return await exceptionLogger.LogException(ex, User.Identity.Name);
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> Email(UserEmailVM ViewModel, string Action)
        //{
        //    var CompanyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
        //    var Model = new CustomersViewModel();
        //    Model.UserEmailVM = new UserEmailVM();
        //    Model = await GetEditModel(ViewModel.CustId);
        //    Model.UserEmailVM = await emailController.UserEmail(ViewModel, User.Identity.Name, CompanyId, Action);
        //    Model.UserEmailVM.DateTo = StringFormating.CurrentTime().Date;
        //    Model.UserEmailVM.DateFrom = DateTime.MinValue;
        //    Model.UserEmailVM.Company = await CompanyDL.GetCompanyProfile(CompanyId);
        //    Model.UserEmailVM.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);
        //    //Model.UserEmailVM.UserProfile = await UserDL.GetCurrentUser(User.Identity.Name);
        //    Model.UserEmailVM.CompanyId = Model.UserEmailVM.Company.Id;
        //    var CompanyEmailAccounts = await CompanyIntegrationDL.GetCompanyEmailAccounts(CompanyId);
        //    var user = await UserDL.GetCurrentUser(User.Identity.Name);
        //    var CompanyUser = await UserDL.GetCompanyUserByUserId(user.Id);
        //    var CompanyUserEmails = await UserDL.GetCompanyUserEmails(CompanyUser.Id);
        //    Model.UserEmailVM.CompanyEmailAccounts = new List<CompanyEmailAccount>();
        //    Model.UserEmailVM.EmailMessages = new List<EmailMessage>();
        //    Model.UserEmailVM.EmailMessageView = new List<EmailMessageView>();
        //    Model.UserEmailVM.Drafts = new List<SendEmail>();
        //    Model.UserEmailVM.Customers = await CustomerDL.GetActiveCustomers(CompanyId);
        //    Model.UserEmailVM.Customer = await CustomerDL.GetCustomer(ViewModel.CustId);



        //    Model.UserEmailVM.CurrentTab = "Customers";
        //    foreach (var emailAccount in CompanyUserEmails)
        //    {
        //        List<EmailMessageView> messages = await UserEmailDL.GetEmailMessagesByCompanyEmailAccountIdAsync(emailAccount.CompanyEmailAccountId, "Customers", Model.UserEmailVM.DateFrom, Model.UserEmailVM.DateTo, User.Identity.Name);
        //        Model.UserEmailVM.EmailMessageView.AddRange(messages);
        //        Model.UserEmailVM.CompanyEmailAccounts.Add(emailAccount.CompanyEmailAccount);
        //    }
        //    Model.UserEmailVM.EmailMessageView = Model.UserEmailVM.EmailMessageView.OrderByDescending(x => x.ReceivedDateTime).ToList();
        //    Model.UserEmailVM.OtherController = true;
        //    if (Action == "View All from Sender")
        //    {
        //    }
        //    if (Action == "Go To Task")
        //    {
        //        var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(ViewModel.Param);
        //        return RedirectToAction("Edit", "Tasks", new { TaskId = EmailMessageView.TasksId });

        //    }
        //    if (Action == "Change Contact")
        //    {
        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Change CC Contact")
        //    {
        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Add Customer Contact")
        //    {
        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Add CC Customer Contact")
        //    {
        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Show Fields")
        //    {
        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Show Edit With PopUp")
        //    {

        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Cancel Popup")
        //    {
        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Show Task PopUp")
        //    {
        //        ViewModel.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);

        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/TaskPopUp", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Show Existing Customer")
        //    {
        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Show Existing Vendor")
        //    {

        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });

        //    }
        //    if (Action == "Show Existing Lead")
        //    {

        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Show Existing Lead")
        //    {

        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });
        //    }
        //    if (Action == "Add Send Recipient")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });

        //    }
        //    if (Action == "Create Contact")
        //    {

        //        var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
        //        return Json(new { isValid = true, html = PopupHTML });

        //    }
        //    if (Action == "Add CC Send Recipient")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });

        //    }
        //    if (Action == "Go To Task")
        //    {
        //        var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(ViewModel.Param);
        //        return RedirectToAction("Edit", "Tasks", new { TaskId = EmailMessageView.TasksId });

        //    }

        //    if (Action == "Remove Recipient")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Show Add New Email")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Show Add CC New Email")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Show Add Other Contact")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Show Add CC Other Contact")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }
        //    if (Action == "Cancel")
        //    {

        //        var HTML1 = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/NewEmail_Actions", ViewModel);
        //        return Json(new { isValid = true, html = HTML1 });
        //    }

        //    if (Action == "Go To Drafts")
        //    {
        //        return RedirectToAction("Email", new { CustomerId = ViewModel.CustId });
        //    }

        //    if (Action == "Select All")
        //    {
        //        foreach (var mail in ViewModel.EmailMessageView)
        //            mail.EmailMessage.IsChecked = true;
        //    }
        //    Model.CurrentTab = "Email";
        //    Model.UserEmailVM.CustomerController = true;
        //    var HTML = await viewRenderer.RenderViewToStringAsync("Customer/PartialViews/Email_Partial", Model);
        //    return Json(new { isValid = true, html = HTML });
        //}
    }
}