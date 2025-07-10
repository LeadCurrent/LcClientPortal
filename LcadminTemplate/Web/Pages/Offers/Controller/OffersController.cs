using Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Project.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace Web
{
    public class OffersController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        private readonly ExceptionLogger exceptionLogger;
        private readonly OffersDataLibrary OffersDL;
        private readonly SchoolsDataLibrary SchoolsDL;
        private readonly SourcesDataLibrary SourcesDL;
        private readonly CompanyDataLibrary CompanyDL;
        private readonly ProgramDataLibrary ProgramDL;

        public OffersController(
            OffersDataLibrary offersDataLibrary,
            RazorViewToStringRenderer razorViewToStringRenderer,
            SchoolsDataLibrary SchoolsDataLibrary,
            SourcesDataLibrary SourcesDataLibrary,
            CompanyDataLibrary CompanyDataLibrary,
            ProgramDataLibrary ProgramDataLibrary,
            ExceptionLogger exceptionLogger)
        {
            OffersDL = offersDataLibrary;
            SchoolsDL = SchoolsDataLibrary;
            SourcesDL = SourcesDataLibrary;
            CompanyDL = CompanyDataLibrary;
            ProgramDL = ProgramDataLibrary;
            viewRenderer = razorViewToStringRenderer;
            this.exceptionLogger = exceptionLogger;
        }

        public async Task<OfferVM> GetOfferList(int Currentpage = 1)
        {
            var Model = new OfferVM();
            int companyId = int.Parse(base.User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);

            Model.SchoolName = HttpContext.Session.GetString("SchoolsNameFilter");
            Model.StatusFilter = HttpContext.Session.GetString("StatusFilter");
            Model.RecordPerPage = HttpContext.Session.GetInt32("RecordPerPage") ?? 25;

            int? status = null;
            if (Model.StatusFilter == "Active Only")
                status = 1;
            else if (Model.StatusFilter == "Inactive Only")
                status = 0;

            Model.CurrentPageNumber = Currentpage;
            Model.TotalRecords = await OffersDL.GetOffersCount(companyId, Model.SchoolName, status);
            Model.Offers = await OffersDL.GetOffers(companyId, Model.RecordPerPage, Model.SchoolName, Currentpage, status);


            return Model;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var model = await GetOfferList(page);
                return View("~/Pages/Offers/OfferList.cshtml", model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity?.Name ?? "Unknown", false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Offers(OfferVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    return RedirectToAction("Create");
                }

                if (Action == "Edit")
                {
                    return RedirectToAction("Edit", new { OfferId = ViewModel.Param });
                }

                if (Action == "Send Test Lead")
                {
                    return RedirectToAction("SendTestLead", new { OfferId = ViewModel.Param });
                }

                var VM = new OfferVM();

                if (Action == "Search")
                {
                    if (ViewModel.SchoolName != null)
                        HttpContext.Session.SetString("SchoolsNameFilter", ViewModel.SchoolName);
                    else
                        HttpContext.Session.Remove("SchoolsNameFilter");

                    VM = await GetOfferList();
                }

                if (Action == "Filter")
                {
                    if (ViewModel.RecordPerPage != 0)
                        HttpContext.Session.SetInt32("RecordPerPage", ViewModel.RecordPerPage);
                    else
                        HttpContext.Session.Remove("RecordPerPage");

                    if (ViewModel.StatusFilter != null)
                        HttpContext.Session.SetString("StatusFilter", ViewModel.StatusFilter ?? "All");
                    else
                        HttpContext.Session.Remove("StatusFilter");

                    VM = await GetOfferList();
                }

                if (Action == "Clear Search")
                {
                    HttpContext.Session.Clear();
                    VM = await GetOfferList();
                }

                if (Action == "Activate Selected" || Action == "Deactivate Selected")
                {
                    bool status = true;

                    if (Action == "Deactivate Selected")
                        status = false;
                    else
                        status = true;

                    foreach (var offer in ViewModel.Offers.Where(x => x.IsChecked))
                    {
                        await OffersDL.ChangeOfferStatus(status, offer.Id);
                    }

                    VM = await GetOfferList();
                }

                if (Action == "Activate Single Offer" || Action == "Deactivate Single Offer")
                {
                    bool status = true;

                    if (Action == "Deactivate Single Offer")
                        status = false;
                    else
                        status = true;

                    await OffersDL.ChangeOfferStatus(status, ViewModel.Param);


                    VM = await GetOfferList();
                }

                if (HttpContext.Session.GetString("MobileApp") != null)
                    ViewModel.MobileApp = true;

                var HTML1 = await viewRenderer.RenderViewToStringAsync("~/Pages/Offers/PartialViews/OfferList_Partial.cshtml", VM);
                return Json(new { isValid = true, html = HTML1 });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                int companyId = int.Parse(base.User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var Model = new OfferVM();
                Model.Offer = new Offer();
                Model.Offer.Targeting = new Offertargeting();
                Model.Clients = await OffersDL.GetClients();
                Model.Schools = await SchoolsDL.GetSchools();
                Model.DeliveryDropDownList = await GetDeliveriesList();
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Offers/Create.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfferVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create" || Action == "Create and Create Again")
                {
                    var selectedDelivery = await GetDeliveryByIdentifier(ViewModel.SelectedDeliveryIdentifier);

                    if (selectedDelivery != null)
                    {
                        ViewModel.Offer.DeliveryIdentifier = selectedDelivery.Value;
                        ViewModel.Offer.DeliveryName = selectedDelivery.Text;
                    }

                    ViewModel.CompanyId = int.Parse(base.User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                    ViewModel.Offer.Schoolid = ViewModel.SelectedSchoolId;
                    ViewModel.Offer.Clientid = ViewModel.SelectedClientId;
                    ViewModel.Offer.Type = ViewModel.SelectedOfferType;

                    await OffersDL.CreateOffer(ViewModel.Offer);
                }

                if (Action == "Create and Create Again")
                {
                    return RedirectToAction("Create");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(
                    ex,
                    User.Identity.Name,
                    ViewModel.AjaxUpdate,
                    ViewModel.Action,
                    System.Text.Json.JsonSerializer.Serialize(ViewModel)
                );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int OfferId)
        {
            try
            {
                var Model = new OfferVM();
                Model.Offer = await OffersDL.GetOfferById(OfferId);
                Model.SelectedClientId = Model.Offer.Clientid;
                Model.SelectedOfferType = Model.Offer.Type;
                Model.SelectedSchoolId = Model.Offer.Schoolid;
                Model.SelectedDeliveryIdentifier = Model.Offer.DeliveryIdentifier;
                Model.Offer.Targeting = new Offertargeting();
                Model.Offer.Allocations = await OffersDL.GetAllocationsForOfferId(OfferId);
                Model.Clients = await OffersDL.GetClients();
                Model.Schools = await SchoolsDL.GetSchools();
                Model.DeliveryDropDownList = await GetDeliveriesList();
                var Targeting = await OffersDL.GetTargetingByOfferId(OfferId);
                if (Targeting != null)
                    Model.Offer.Targeting = Targeting;

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Offers/Edit.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OfferVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Update")
                {
                    var selectedDelivery = await GetDeliveryByIdentifier(ViewModel.SelectedDeliveryIdentifier);

                    if (selectedDelivery != null)
                    {
                        ViewModel.Offer.DeliveryIdentifier = selectedDelivery.Value;
                        ViewModel.Offer.DeliveryName = selectedDelivery.Text;
                    }
                    ViewModel.CompanyId = int.Parse(base.User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                    ViewModel.Offer.Schoolid = ViewModel.SelectedSchoolId;
                    ViewModel.Offer.Clientid = ViewModel.SelectedClientId;
                    ViewModel.Offer.Type = ViewModel.SelectedOfferType;
                    await OffersDL.EditOffer(ViewModel.Offer);
                }

                if (Action == "EditOffersAllocation")
                {
                    return RedirectToAction("EditOffersAllocation", new { OfferId = ViewModel.Param });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(
                    ex,
                    User.Identity.Name,
                    ViewModel.AjaxUpdate,
                    ViewModel.Action,
                    System.Text.Json.JsonSerializer.Serialize(ViewModel)
                );
            }
        }

        public async Task<OfferVM> GetSendTestLeadViewModel(OfferVM ViewModel)
        {
            var model = new OfferVM();

            // Ensure OfferId
            if (ViewModel.OfferId == 0 && ViewModel.Offer != null)
                ViewModel.OfferId = ViewModel.Offer.Id;

            var companyId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value ?? "0");
            var company = await CompanyDL.GetCompany(companyId);

            model.Offer = await OffersDL.GetOfferById(ViewModel.OfferId);
            model.EducationLevels = await OffersDL.GetEducationLevelsAsync();

            var school = await SchoolsDL.GetSchoolById(model.Offer.Schoolid);
            model.Campuses = await SchoolsDL.GetCampusesBySchoolId(model.Offer.Schoolid);
            model.SelectedSchoolId = model.Offer.Schoolid;

            // ❗ Readonly fields → always assign default/computed value
            model.TestLeadSwInstance = company?.Name;
            model.TestLeadSelectedSchoolId = school?.Name;
            model.TestLeadSourceUrl = "http://www.careerboutique.com/?creative=&kw=";
            model.TestLeadDeliveryIdentifier = model.Offer.DeliveryName;

            // ✅ Editable fields → use ViewModel if present
            model.TestLeadAge = ViewModel.TestLeadAge > 0
                ? ViewModel.TestLeadAge
                : 25;

            model.TestLeadEmail = !string.IsNullOrWhiteSpace(ViewModel.TestLeadEmail)
                ? ViewModel.TestLeadEmail
                : "test@gmail.com";

            model.TestLeadZip = ViewModel.TestLeadZip > 0
                ? ViewModel.TestLeadZip
                : 33076;

            model.TestLeadEduLevelId = !string.IsNullOrWhiteSpace(ViewModel.TestLeadEduLevelId)
                ? ViewModel.TestLeadEduLevelId
                : "MA";

            model.TestLeadTransferRecipient = !string.IsNullOrWhiteSpace(ViewModel.TestLeadTransferRecipient)
                ? ViewModel.TestLeadTransferRecipient
                : "";

            model.TestLeadHsGradYr = !string.IsNullOrWhiteSpace(ViewModel.TestLeadHsGradYr)
                ? ViewModel.TestLeadHsGradYr
                : "2015";

            model.TestLeadIsCallCenter = ViewModel.TestLeadIsCallCenter;
            model.TestLeadCampusId = ViewModel.TestLeadCampusId;
            model.TestLeadProgramId = ViewModel.TestLeadProgramId;

            // Additional Fields from session
            model.AdditionalFields = HttpContext.Session.GetObject<List<KeyValuePair<string, string>>>("AdditionalFields") ?? new();

            // Load Campusdegree programs if a valid campus is selected
            if (model.TestLeadCampusId > 0)
            {
                model.Campusdegrees = await ProgramDL.GetProgramsByCampusId(model.TestLeadCampusId);
            }

            // Detect mobile context
            if (User?.Identity?.IsAuthenticated == true)
            {
                var httpContext = new HttpContextAccessor().HttpContext;
                if (httpContext?.Session.GetString("MobileApp") != null)
                    model.MobileApp = true;
            }

            return model;
        }


        [HttpGet]
        public async Task<IActionResult> SendTestLead(int OfferId)
        {
            try
            {
                var model = new OfferVM();
                model.OfferId = OfferId;
                model = await GetSendTestLeadViewModel(model);
                model.TestLeadIsCallCenter = true;
                return View("~/Pages/Offers/SendTestLead.cshtml", model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendTestLead(OfferVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Go back")
                    return RedirectToAction("Index");

                OfferVM model = null;

                if (Action == "Campus Changed")
                {
                    model = await GetSendTestLeadViewModel(ViewModel);
                }

                if (Action == "Show Additional Field")
                {
                    model = await GetSendTestLeadViewModel(ViewModel);
                    model.AdditionalFields = new List<KeyValuePair<string, string>>();
                    model.TestLeadCampusId = ViewModel.TestLeadCampusId;
                    model.ShowAdditionalField = true;
                }

                if (Action == "AddAdditionalField")
                {
                    if (!string.IsNullOrEmpty(ViewModel.AdditionalFieldName) && !string.IsNullOrEmpty(ViewModel.AdditionalFieldValue))
                    {
                        var currentList = HttpContext.Session.GetObject<List<KeyValuePair<string, string>>>("AdditionalFields") ?? new();
                        currentList.Add(new KeyValuePair<string, string>(ViewModel.AdditionalFieldName, ViewModel.AdditionalFieldValue));
                        HttpContext.Session.SetObject("AdditionalFields", currentList);
                    }

                    model = await GetSendTestLeadViewModel(ViewModel);
                    model.Campusdegrees = await ProgramDL.GetProgramsByCampusId(ViewModel.TestLeadCampusId);
                    model.AdditionalFields = HttpContext.Session.GetObject<List<KeyValuePair<string, string>>>("AdditionalFields") ?? new();
                }

                if (Action == "RemoveAdditionalField")
                {
                    var existingList = HttpContext.Session.GetObject<List<KeyValuePair<string, string>>>("AdditionalFields") ?? new();
                    existingList = existingList.Where(x => x.Key != ViewModel.StringParam).ToList();
                    HttpContext.Session.SetObject("AdditionalFields", existingList);

                    model = await GetSendTestLeadViewModel(ViewModel);
                    model.TestLeadCampusId = ViewModel.TestLeadCampusId;
                    model.Campusdegrees = await ProgramDL.GetProgramsByCampusId(ViewModel.TestLeadCampusId);
                    model.AdditionalFields = existingList;
                }

                if (Action == "Send")
                {
                    var formData = new
                    {
                        swInstance = ViewModel.TestLeadSwInstance?.Trim(),
                        age = ViewModel.TestLeadAge.ToString(),
                        zip = ViewModel.TestLeadZip.ToString(),
                        email = ViewModel.TestLeadEmail?.Trim(),
                        campusId = ViewModel.TestLeadCampusId.ToString(),
                        programId = ViewModel.TestLeadProgramId,
                        hsGradYr = ViewModel.TestLeadHsGradYr,
                        eduLevelId = ViewModel.TestLeadEduLevelId,
                        transferRecipient = ViewModel.TestLeadTransferRecipient?.Trim(),
                        sourceUrl = ViewModel.TestLeadSourceUrl?.Trim(),
                        isCallCenter = ViewModel.TestLeadIsCallCenter ? "Yes" : "No",
                        deliveryIdentifier = ViewModel.TestLeadDeliveryIdentifier,
                        additionalFieldsJSON = JsonSerializer.Serialize(
                            ViewModel.AdditionalFields.ToDictionary(x => x.Key, x => x.Value))
                    };

                    var jsonData = JsonSerializer.Serialize(formData);

                    var postUrl = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                        ? "http://os2.leadcurrent.net/SearchService.asmx/SubmitSendTestLead"
                        : "http://os2.leadcurrent.cloud/SearchService.asmx/SubmitSendTestLead";

                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.ExpectContinue = false;

                    // Correct way to send form-urlencoded content
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                      new KeyValuePair<string, string>("json", jsonData)
                    });

                    // Send the POST request
                    var response = await client.PostAsync(postUrl, formContent);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Rebuild view model
                    model = await GetSendTestLeadViewModel(ViewModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        model.ResponseSuccess = false;
                        model.ApiResponseHtml = $@"
        <div class='text-danger'>
            <strong>API Error ({(int)response.StatusCode} {response.StatusCode}):</strong><br/>
            <pre>{WebUtility.HtmlEncode(responseContent)}</pre>
        </div>";
                    }
                    else
                    {
                        model.ResponseSuccess = true;
                        model.ApiResponseHtml = ParseSendTestLeadResponse(responseContent);
                    }
                }

                var html = await viewRenderer.RenderViewToStringAsync("Offers/PartialViews/SendTestLead_Partial", model);
                return Json(new { isValid = true, html });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(
                    ex,
                    User.Identity.Name,
                    ViewModel.AjaxUpdate,
                    ViewModel.Action,
                    System.Text.Json.JsonSerializer.Serialize(ViewModel)
                );
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditOffersAllocation(int OfferId)
        {
            try
            {
                var Model = new OfferVM();

                Model.Allocation = await OffersDL.GetAllocationByOfferId(OfferId);
                Model.Sources = await SourcesDL.GetSourcesByCompanyId(int.Parse(base.User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value));
                Model.SelectedSourceId = Model.Allocation.Sourceid;
                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Offers/EditOffersAllocation.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditOffersAllocation(OfferVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Update")
                {
                    ViewModel.Allocation.Sourceid = ViewModel.SelectedSourceId;
                    bool success = await OffersDL.UpdateAllocation(ViewModel.Allocation);
                }

                return RedirectToAction("Edit", new { OfferId = ViewModel.Allocation.Offerid });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(
                    ex,
                    User.Identity.Name,
                    ViewModel.AjaxUpdate,
                    ViewModel.Action,
                    System.Text.Json.JsonSerializer.Serialize(ViewModel)
                );
            }
        }

        public static async Task<List<SelectListItem>> GetDeliveriesList(string selectedValue = null)
        {
            var list = new List<SelectListItem>
    {
        new SelectListItem { Text = "- Select Delivery -", Value = "", Selected = string.IsNullOrEmpty(selectedValue) }
    };

            using (var httpClient = new HttpClient())
            {
                string json = await httpClient.GetStringAsync("http://os2.leadcurrent.net/SearchService.asmx/GetDeliveries");
                JArray deliveries = JArray.Parse(json);

                foreach (JObject d in deliveries)
                {
                    string value = d["identifier"]?.ToString();
                    list.Add(new SelectListItem
                    {
                        Text = d["name"]?.ToString(),
                        Value = value,
                        Selected = value == selectedValue
                    });
                }
            }

            return list;
        }

        public static async Task<SelectListItem> GetDeliveryByIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return null;

            using (var httpClient = new HttpClient())
            {
                string json = await httpClient.GetStringAsync("http://os2.leadcurrent.net/SearchService.asmx/GetDeliveries");
                JArray deliveries = JArray.Parse(json);

                foreach (JObject d in deliveries)
                {
                    string currentIdentifier = d["identifier"]?.ToString();
                    if (currentIdentifier == identifier)
                    {
                        return new SelectListItem
                        {
                            Text = d["name"]?.ToString(),
                            Value = currentIdentifier
                        };
                    }
                }
            }

            return null;
        }

        private static string ParseSendTestLeadResponse(string rawResponse)
        {
            try
            {
                string jsonResponse = rawResponse.Trim();

                if (jsonResponse.StartsWith("<string"))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(jsonResponse);
                    jsonResponse = xmlDoc.InnerText?.Trim();
                }

                // Defensive: Ensure valid JSON format
                if (!jsonResponse.StartsWith("{"))
                {
                    return $"<div class='text-danger'>Unexpected API response:<br/><pre>{WebUtility.HtmlEncode(jsonResponse)}</pre></div>";
                }

                var responseObj = JsonDocument.Parse(jsonResponse).RootElement;

                string Format(string key) =>
                    responseObj.TryGetProperty(key, out var val) ? val.ToString() : "";

                var tableRows = new List<string>
        {
            TableRow("Submission Date", Format("submission_date")),
            TableRow("Result Date", Format("result_date")),
            TableRow("Success", Format("success")),
            TableRow("Error", Format("error")),
            TableRow("HTTP Post String", Format("http_post_string")),
            TableRow("HTTP Post URL", Format("http_post_url")),
            TableRow("HTTP Post Response", Format("http_post_response")),
            TableRow("Raw Response", Format("raw_response"))
        };

                return $@"
            <table class='table table-bordered'>
                <thead><tr><th>Field</th><th>Value</th></tr></thead>
                <tbody>
                    {string.Join("\n", tableRows)}
                </tbody>
            </table>";
            }
            catch (Exception ex)
            {
                return $"<div class='text-danger'>Error parsing API response: {WebUtility.HtmlEncode(ex.Message)}<br/><pre>{WebUtility.HtmlEncode(rawResponse)}</pre></div>";
            }

            string TableRow(string key, string value)
            {
                string safeValue = WebUtility.HtmlEncode(value ?? "");
                return $"<tr><td><strong>{key}</strong></td><td>{safeValue}</td></tr>";
            }
        }



    }

    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
            => session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
