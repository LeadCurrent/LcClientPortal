using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Project.Utilities;
using Twilio.TwiML.Voice;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using CommonClasses;

namespace Web.Controllers
{


    public class ContactsController : Controller
    {
        RazorViewToStringRenderer viewRenderer;
        public ExceptionLogger exceptionLogger { get; }

        public Email email { get; }
        private static IConfiguration _configuration;

        public ContactsController(
            ExceptionLogger ExceptionLogger,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            Email Email,
            IConfiguration configuration
        )
        {
            exceptionLogger = ExceptionLogger;
            email = Email;
            viewRenderer = RazorViewToStringRenderer;
            _configuration = configuration;
        }



        [HttpGet]
        public async Task<IActionResult> Index(bool ContactSent = false, bool Robot = false)
        {
            try
            {
                var contactVM = new ContactsVM();
                contactVM.ContactSent = ContactSent;
                contactVM.RobotFailed = Robot;

                Random rnd = new Random();
                contactVM.Number1 = rnd.Next(1, 10);
                Random rnd2 = new Random();
                contactVM.Number2 = rnd2.Next(1, 10);
                contactVM.Total = contactVM.Number1 + contactVM.Number2;

                return View("Contact", contactVM);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactsVM ViewModel, string Action)
        {
            try
            {           

                if (Action == "Home")
                    return RedirectToAction("Index");



                var recaptchaResponse = Request.Form["g-recaptcha-response"];
                string secretKey = _configuration.GetValue<string>("GoogleRecaptchaSettings:SecretKey");

                var client = new HttpClient();
                var result = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}");
                var captchaResult = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleReCaptchaResponse>(result);

                if (!captchaResult.Success)
                {
                    ModelState.AddModelError("", "Captcha verification failed. Please try again.");
                    return RedirectToAction("Index", "Contacts", new { Robot = true });
                }


                email.EmailTo = "support@LeadCurrent.com";
                email.EmailFrom = "support@LeadCurrent.com";
                email.Subject = "Contact Request from Lead Current Website";
                email.Message = "New Contact Request<br>";
                email.Message += "Business Name:" + ViewModel.Company.Name + "<br>";
                email.Message += "Contact Name:" + ViewModel.SignupName + "<br>";
                email.Message += "Email:" + ViewModel.SignupEmail + "<br>";
                email.Message += "Phone:" + ViewModel.SignupPhone + "<br>";
                email.Message += "Message:" + ViewModel.Message + "<br>";
                await email.SendMail();

                return RedirectToAction("Index", "Contacts", new { ContactSent = true });

            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel));
            }
        }

    }
}
