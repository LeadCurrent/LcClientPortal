using Data;
using Data.DataContexts.ModelForSourceDB;
using System.Collections.Generic;

namespace Web
{
    public class VendorDashboardVM
    {
        /* Ajax */
        public string Action { get; set; }
        public string DivToUpdate { get; set; }
        public int Param { get; set; }
        public int Param2 { get; set; }
        public int NewProductCategoryId { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }

        /* Prop */
        public string OfferName { get; set; }
        public string ClientID { get; set; }
        public string OfferType { get; set; }
        public string TransferPhone { get; set; }
        public string TransferHours { get; set; }
        public string MilitaryTargeting { get; set; }
        public string AgeTargeting { get; set; }
        public string HsGradYearTargeting { get; set; }
        public string SchoolName { get; set; }
        public string SchoolLogo { get; set; }
        public string SchoolShortCopy { get; set; }
        public string SchoolCallCenterHighlights { get; set; }
        public string SchoolTargetingNotes { get; set; }
        public string SchoolTCPA { get; set; }
        public string SchoolAccreditationNotes { get; set; }
        public string SchoolDisclosureURL { get; set; }
        public string SchoolGroups { get; set; }
        public string Campuses { get; set; }
        public string TableHeader { get; set; }

        /* Models */
        public User User { get; set; }
        public Offer Offer { get; set; }
        public Scholls School { get; set; }
        public Source Source { get; set; }
        public Company Company { get; set; }
        public List<Source> Sources { get; set; }
        public VwVendorAllocation VendorAllocation { get; set; }
        public List<VwVendorAllocation> VendorAllocations { get; set; }
        public List<Campus> CampusList { get; set; }

    }
}
