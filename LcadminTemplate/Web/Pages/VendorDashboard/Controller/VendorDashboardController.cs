using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class VendorDashboardController : Controller
    {

        private readonly RazorViewToStringRenderer viewRenderer;
        public SourcesDataLibrary SourcesDL { get; set; }
        public OffersDataLibrary OffersDL { get; set; }
        public SchoolsDataLibrary SchoolsDL { get; set; }
        public VendorDashboardDataLibrary VendorDashboardDL { get; set; }
        public AllocationDataLibrary AllocationDL { get; set; }
        public CampusDataLibrary CampusDL { get; set; }
        ExceptionLogger exceptionLogger { get; }


        public VendorDashboardController(SourcesDataLibrary SourcesDataLibrary,
      OffersDataLibrary OffersDataLibrary,
      SchoolsDataLibrary SchoolsDataLibrary,
      RazorViewToStringRenderer RazorViewToStringRenderer,
      AllocationDataLibrary AllocationsDataLibrary,
      VendorDashboardDataLibrary VendorDashboardDataLibrary,
      CampusDataLibrary CampusDataLibrary,
      ExceptionLogger ExceptionLogger)
        {
            SourcesDL = SourcesDataLibrary;
            OffersDL = OffersDataLibrary;
            SchoolsDL = SchoolsDataLibrary;
            AllocationDL = AllocationsDataLibrary;
            VendorDashboardDL = VendorDashboardDataLibrary;
            CampusDL = CampusDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string OSAccessKey)
        {
            try
            {
                if (string.IsNullOrEmpty(OSAccessKey))
                {
                    return Content("OSAccessKey is null or empty.");
                }

                var model = new VendorDashboardVM();
                model.VendorAllocations = await VendorDashboardDL.GetVendorAllocationsByAccessKey(OSAccessKey);

                return View("~/Pages/VendorDashboard/VendorDashboardList.cshtml", model);
            }
            catch (Exception ex)
            {
                // Optional: Log the exception using your logger if available
                // _logger.LogError(ex, "Error in VendorDashboard Index");

                return Content($"An error occurred: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> VendorDashboardList(VendorDashboardVM ViewModel, string Action)
        {
            try
            {
                var VM = new VendorDashboardVM();

                if (Action == "View Details")
                {
                    // Fetch allocation, source, offer, school
                    VM.VendorAllocation = await VendorDashboardDL.GetVendorAllocationById(ViewModel.Param);
                    VM.Source = await SourcesDL.GetSource(VM.VendorAllocation.Sourceid);
                    VM.Offer = await OffersDL.GetOfferById(VM.VendorAllocation.Offerid);
                    VM.School = await SchoolsDL.GetSchool(VM.Offer.Schoolid);
                    VM.CampusList = await CampusDL.GetCampusWithZIPCountBySchoolId(VM.School.Id);
                    // Targeting: Assume first (or only) Offertargeting is used
                    var targeting = VM.Offer.Offertargetings.FirstOrDefault();
                    if (targeting != null)
                    {
                        var sb = new System.Text.StringBuilder();
                        string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

                        foreach (var day in days)
                        {
                            bool active = (bool)targeting.GetType().GetProperty(day + "Active")?.GetValue(targeting);
                            DateTime start = (DateTime)targeting.GetType().GetProperty(day + "Start")?.GetValue(targeting);
                            DateTime end = (DateTime)targeting.GetType().GetProperty(day + "End")?.GetValue(targeting);

                            sb.Append($"{day.ToUpper()}: {(active ? "ACTIVE" : "INACTIVE")}, ");
                            sb.Append($"START TIME: {(start != DateTime.MinValue ? start.ToShortTimeString() : "12:00 AM")}, ");
                            sb.Append($"END TIME: {(end != DateTime.MinValue ? end.ToShortTimeString() : "24:59 PM")}<br>");
                        }

                        VM.TransferHours = sb.ToString();
                    }
                    else
                    {
                        VM.TransferHours = "No targeting found.";
                    }

                    // (Optional) Get School Group Names as string
                    VM.SchoolGroups = string.Join(", ", VM.School.Schoolgroups.Select(g => g.Group.Name));
                }

                var html = await viewRenderer.RenderViewToStringAsync("VendorDashboard/PartialViews/AllocationDetail_Partial", VM);
                return Json(new { isValid = true, html = html });
            }
            catch (Exception ex)
            {
                return Content($"An error occurred: {ex.Message}");
            }
        }

    }
}
