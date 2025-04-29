using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using CommonClasses;

namespace Web.Controllers
{


    public class PrivacyController : Controller
    {
        RazorViewToStringRenderer viewRenderer;
        public ExceptionLogger exceptionLogger { get; }

        public Email email { get; }

        public PrivacyController(
            ExceptionLogger ExceptionLogger,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            Email Email
        )
        {
            exceptionLogger = ExceptionLogger;
            email = Email;
            viewRenderer = RazorViewToStringRenderer;
        }



        [HttpGet]
        public async Task<IActionResult> Index(string Section = "")
        {
            try
            {
                return View("Privacy");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }

      

    }
}
