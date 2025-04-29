using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Web.Controllers;
using CommonClasses;
using Data;
using Microsoft.AspNetCore.Identity;
using Project.Utilities;
using static Data.CompanyEnums;
namespace Web
{
    public class CustomerServiceController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        private UserDataLibrary UserDL { get; }
        public CompanyDataLibrary CompanyDL { get; }
        public CustomerDataLibrary CustomerDL { get; }
        ExceptionLogger exceptionLogger { get; }
        public UserManager<User> UserManager { get; }


        public CustomerServiceController(
            CustomerDataLibrary CustomerDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            UserDataLibrary UserDataLibrary,
            ExceptionLogger ExceptionLogger,
            CompanyDataLibrary CompanyDataLibrary
        )
        {
            CustomerDL = CustomerDataLibrary;
            CompanyDL = CompanyDataLibrary;
            UserDL = UserDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
