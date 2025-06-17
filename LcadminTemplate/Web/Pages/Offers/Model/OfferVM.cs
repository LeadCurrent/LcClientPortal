using Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web
{
    public class OfferVM
    {
        /* Ajax-related Properties */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }

        /* Core Properties */
        public int CompanyId { get; set; }
        public int RecordPerPage { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalRecords { get; set; }
        public int SelectedClientId { get; set; }
        public int SelectedSchoolId { get; set; }
        public string SelectedOfferType { get; set; }
        public bool ShowEdit { get; set; }
        public string SchoolName { get; set; }
        public string StatusFilter { get; set; }

        /* Calculated Property */
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / RecordPerPage);

        /* Main Model */
        public Offer Offer { get; set; }
        public List<Offer> Offers { get; set; }
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
        public virtual ICollection<Scholls> Schools { get; set; } = new List<Scholls>();
        public List<SelectListItem> DeliveryDropDownList { get; set; } = new List<SelectListItem>();

        /* Dropdown Lists */
        public List<SelectListItem> PageRecordNumberDropdown { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "25", Value = "25" },
            new SelectListItem { Text = "50", Value = "50" },
            new SelectListItem { Text = "75", Value = "75" },
            new SelectListItem { Text = "100", Value = "100" },
            new SelectListItem { Text = "View All", Value = "-1" }
        };

        public List<SelectListItem> StatusFilterDropdown { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "All Offers", Value = "All Offers" },
            new SelectListItem { Text = "Active Only", Value = "Active Only" },
            new SelectListItem { Text = "Inactive Only", Value = "Inactive Only" }
        };

        public List<SelectListItem> OfferTypeList { get; set; } = new List<SelectListItem>
{
    new SelectListItem { Text = "-Select Type-", Value = "", Selected = true },
    new SelectListItem { Text = "API", Value = "AP" },
    new SelectListItem { Text = "Data Lead", Value = "DL" },
    new SelectListItem { Text = "Exclusive Lead", Value = "XL" },
    new SelectListItem { Text = "Warm Transfer", Value = "HT" },
    new SelectListItem { Text = "Web", Value = "WB" }
};


        public List<SelectListItem> ClientDropdown
        {
            get
            {
                var DDL = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "" }
        };

                if (Clients != null)
                {
                    foreach (var client in Clients)
                    {
                        DDL.Add(new SelectListItem
                        {
                            Value = client.Id.ToString(),
                            Text = client.Name
                        });
                    }
                }

                return DDL;
            }
        }
        public List<SelectListItem> SchoolDropdown
        {
            get
            {
                var DDL = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "" }
        };

                if (Schools != null)
                {
                    foreach (var school in Schools)
                    {
                        DDL.Add(new SelectListItem
                        {
                            Value = school.Id.ToString(),
                            Text = school.Name
                        });
                    }
                }

                return DDL;
            }
        }


    }
}
