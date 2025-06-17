using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Project.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web
{
    public class OffersController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        private readonly ExceptionLogger exceptionLogger;
        private readonly OffersDataLibrary OffersDL;
        private readonly SchoolsDataLibrary SchoolsDL;

        public OffersController(
            OffersDataLibrary offersDataLibrary,
            RazorViewToStringRenderer razorViewToStringRenderer,
            SchoolsDataLibrary SchoolsDataLibrary,
            ExceptionLogger exceptionLogger)
        {
            OffersDL = offersDataLibrary;
            SchoolsDL = SchoolsDataLibrary;
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
                    return RedirectToAction("Edit", new{ OfferId = ViewModel.Param});
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

                    if(Action == "Deactivate Selected")
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
                Model.Schools = await SchoolsDL.SchoolsByCompanyId(companyId);
                Model.DeliveryDropDownList =  GetDeliveriesList();
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
                if (Action == "Create")
                {
                    var selectedItem = ViewModel.DeliveryDropDownList.FirstOrDefault(x => x.Value == ViewModel.SelectedOfferType.ToString());
                    ViewModel.Offer.DeliveryName = selectedItem?.Text;
                    ViewModel.Offer.DeliveryIdentifier = ViewModel.SelectedOfferType;
                    await OffersDL.CreateOffer(ViewModel.Offer);
                }

                if (Action == "Create and Create Again")
                {
                    var selectedItem = ViewModel.DeliveryDropDownList.FirstOrDefault(x => x.Value == ViewModel.SelectedOfferType.ToString());
                    ViewModel.Offer.DeliveryName = selectedItem?.Text;
                    ViewModel.Offer.DeliveryIdentifier = ViewModel.SelectedOfferType;
                    await OffersDL.CreateOffer(ViewModel.Offer);
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
                int companyId = int.Parse(base.User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
                var Model = new OfferVM();
                Model.Offer = await OffersDL.GetOfferById(OfferId);
                Model.Offer.Targeting = new Offertargeting();
                Model.Clients = await OffersDL.GetClients();
                Model.Schools = await SchoolsDL.SchoolsByCompanyId(companyId);
                Model.DeliveryDropDownList = GetDeliveriesList();
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


        public static List<SelectListItem> GetDeliveriesList()
        {
            var list = new List<SelectListItem>
        {
            new SelectListItem { Text = "- Select Delivery -", Value = "" }
        };

            using (WebClient client = new WebClient())
            {
                string responseJSON = client.DownloadString("http://os2.leadcurrent.net/SearchService.asmx/GetDeliveries");

                JArray deliveries = JArray.Parse(responseJSON);

                foreach (JObject d in deliveries)
                {
                    list.Add(new SelectListItem
                    {
                        Text = d["name"]?.ToString(),
                        Value = d["identifier"]?.ToString()
                    });
                }
            }

            return list;
        }
    }
}
