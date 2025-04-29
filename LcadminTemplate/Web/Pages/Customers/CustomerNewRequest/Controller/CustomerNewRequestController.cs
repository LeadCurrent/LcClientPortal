using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using static Data.CompanyEnums;

namespace Web.Controllers
{
    [Authorize]
    public class CustomerNewRequestController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;

        private CustomerDataLibrary CustomerDL { get; }
        private UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public SignInManager<User> SignInManager { get; }
        public UserManager<User> UserManager { get; }
        ExceptionLogger exceptionLogger { get; }

        public CustomerNewRequestController(
            CustomerDataLibrary CustomerDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            UserDataLibrary UserDataLibrary,
            ExceptionLogger ExceptionLogger,
             UserManager<User> userManager,
             SignInManager<User> signInManager,
            CompanyDataLibrary CompanyDataLibrary
        )
        {
            CustomerDL = CustomerDataLibrary;
            CompanyDL = CompanyDataLibrary;
            UserDL = UserDataLibrary;
            UserManager = userManager;
            SignInManager = signInManager;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }
        public async Task<CustomerRequestVM> GetCustomerListModel()
        {
            var Model = new CustomerRequestVM();
            Model.User = await UserDL.GetUserByUserName(User.Identity.Name);
            Model.Companys = new List<Company>();
            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool MobileApp)
        {
            var Model = new CustomerRequestVM();
            try
            {
                Model = await GetCustomerListModel();
                if (Model.Companys != null && Model.Companys.Count == 1)
                {
                    var companyId = Model.Companys.First().Id;
                    Model.CompanyId = companyId;
                    if(Model.Customers.Count>1)
                    Model.Customers = await CustomerDL.GetActiveCustomers(companyId);
                }
                if (Model.Customers != null && Model.Customers.Count == 1)
                {
                    var customerId = Model.Customers.First().Id;
                    Model.CompanyId = Model.CompanyId;
                    Model.CustomerId= customerId;
                    Model.Customer = await CustomerDL.GetCustomer(Model.CustomerId);
                    Model.ShowCustomerInfo = true;
                }
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;
                else if (MobileApp == true)
                {
                    Model.MobileApp = MobileApp;
                    HttpContext.Session.SetString("MobileApp", "True");
                }
                return View("CreateNewRequest", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false, this.ControllerContext.RouteData.Values["action"].ToString(), System.Text.Json.JsonSerializer.Serialize(Model), this.ControllerContext.RouteData.Values["controller"].ToString(), HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewRequest(CustomerRequestVM ViewModel, string Action)
        {
            try
            {
                var Model = new CustomerRequestVM();
                Model = await GetCustomerListModel();
                if (Action == "select company")
                {
                    Model.CompanyId = ViewModel.CompanyId;
                    Model.Customers = await CustomerDL.GetCustomers(Model.CompanyId);
                }

                if (Action == "Create New Customer")
                {
                    Model.CompanyId=ViewModel.CompanyId;
                    Model.CustomerId = ViewModel.CustomerId;
                    Model.Customer=await CustomerDL.GetCustomer(Model.CustomerId);
                    Model.ShowCustomerInfo = true;

                }

                if (Action == "Select Service")
                {
                    Model.CompanyId = ViewModel.CompanyId;
                    Model.Customer = await CustomerDL.GetCustomer(ViewModel.CustomerId);
                    Model.CustomerId = ViewModel.CustomerId;
                    Model.ShowCustomerInfo = true;
                }

                if (Action == "Create Job")
                {
                    Model.Customer = await CustomerDL.GetCustomer(ViewModel.CustomerId);         
                    Model.UpdateSuccessful = true;
                    Model.CompanyId = ViewModel.CompanyId;
                    Model.Company= await CompanyDL.GetCompany(ViewModel.CompanyId); 
                    Model.Customers = await CustomerDL.GetCustomers(Model.CompanyId);
                }

                var HTML = Task.Run(() => viewRenderer.RenderViewToStringAsync("CustomerNewRequest/PartialViews/CreateNewRequest_Partial", Model)).Result;
                return Json(new { isValid = true, html = HTML });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }

    }
}