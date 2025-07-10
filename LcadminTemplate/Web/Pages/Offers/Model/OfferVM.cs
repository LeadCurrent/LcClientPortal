using Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Web
{
    public class OfferVM
    {
        /* ------------------ Ajax-related Properties ------------------ */
        public string Action { get; set; }
        public bool AjaxUpdate { get; set; }
        public bool MobileApp { get; set; }
        public int Param { get; set; }
        public string StringParam { get; set; }

        /* ------------------ Core Properties ------------------ */
        public int CompanyId { get; set; }
        public int OfferId { get; set; }
        public int RecordPerPage { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalRecords { get; set; }
        public int SelectedClientId { get; set; }
        public int SelectedSchoolId { get; set; }
        public int SelectedSourceId { get; set; }
        public string SelectedOfferType { get; set; }
        public string SelectedDeliveryIdentifier { get; set; }
        public bool ShowEdit { get; set; }
        public string SchoolName { get; set; }
        public string StatusFilter { get; set; }

        /* ------------------ Calculated Property ------------------ */
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / RecordPerPage);

        /* ------------------ TestLead Property ------------------ */

        public bool ShowAdditionalField { get; set; }
        public string TestLeadSwInstance { get; set; }
        public int TestLeadAge { get; set; }
        public int TestLeadZip { get; set; }
        public string TestLeadEmail { get; set; }
        public int TestLeadCampusId { get; set; }
        public string TestLeadSelectedSchoolId { get; set; }
        public string TestLeadProgramId { get; set; }
        public string TestLeadHsGradYr { get; set; }
        public string TestLeadEduLevelId { get; set; }
        public string TestLeadTransferRecipient { get; set; }
        public string TestLeadSourceUrl { get; set; }
        public bool TestLeadIsCallCenter { get; set; }
        public string TestLeadDeliveryIdentifier { get; set; }
        public string SelectedHsGradYear { get; set; } = "2015"; // Default selection
        public string SelectedEduLevelId { get; set; }
        public string AdditionalFieldName { get; set; }
        public string AdditionalFieldValue { get; set; }
        public string ApiResponseHtml { get; set; }
        public bool ResponseSuccess { get; set; }
        public List<KeyValuePair<string, string>> AdditionalFields { get; set; } = new();
        public List<Campusdegree> Campusdegrees { get; set; }

        public List<SelectListItem> HsGradYearDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Year --" }
        };

                int startYear = 1980;
                int currentYear = DateTime.Now.Year;

                for (int year = startYear; year <= currentYear; year++)
                {
                    ddl.Add(new SelectListItem
                    {
                        Value = year.ToString(),
                        Text = year.ToString(),
                        Selected = (year.ToString() == SelectedHsGradYear)
                    });
                }

                return ddl;
            }
        }

        /* ------------------ Main Model ------------------ */
        public Offer Offer { get; set; }
        public Allocation Allocation { get; set; }
        public List<Offer> Offers { get; set; }
        public virtual ICollection<Source> Sources { get; set; } = new List<Source>();
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
        public virtual ICollection<Scholls> Schools { get; set; } = new List<Scholls>();
        public virtual ICollection<Campus> Campuses { get; set; } = new List<Campus>();
        public virtual ICollection<Program> Programs { get; set; } = new List<Program>();
        public virtual ICollection<TblConfigEducationLevel> EducationLevels { get; set; } = new List<TblConfigEducationLevel>();
        public List<SelectListItem> DeliveryDropDownList { get; set; } = new List<SelectListItem>();

        /* ------------------ Dropdown Lists ------------------ */
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

        public List<SelectListItem> OfferTypeList
        {
            get
            {
                return new List<SelectListItem>
        {
            new SelectListItem { Text = "-Select Type-", Value = "", Selected = string.IsNullOrEmpty(SelectedOfferType) },
            new SelectListItem { Text = "API", Value = "AP", Selected = SelectedOfferType == "AP" },
            new SelectListItem { Text = "Data Lead", Value = "DL", Selected = SelectedOfferType == "DL" },
            new SelectListItem { Text = "Exclusive Lead", Value = "XL", Selected = SelectedOfferType == "XL" },
            new SelectListItem { Text = "Warm Transfer", Value = "HT", Selected = SelectedOfferType == "HT" },
            new SelectListItem { Text = "Web", Value = "WB", Selected = SelectedOfferType == "WB" }
        };
            }
        }

        public List<SelectListItem> ClientDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Client --" }
        };

                if (Clients != null)
                {
                    ddl.AddRange(Clients.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = (c.Id == SelectedClientId) // ✅ Pre-select
                    }));
                }

                return ddl;
            }
        }

        public List<SelectListItem> CampusDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Campus --" }
        };

                if (Campuses != null)
                {
                    ddl.AddRange(Campuses.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = (c.Id == TestLeadCampusId) // Optional: auto-select
                    }));
                }

                return ddl;
            }
        }

        public List<SelectListItem> SourceDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Source --" }
        };

                if (Sources != null)
                {
                    ddl.AddRange(Sources.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = (c.Id == SelectedSourceId) // ✅ Pre-select
                    }));
                }

                return ddl;
            }
        }

        public List<SelectListItem> SchoolDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select School --" }
        };

                if (Schools != null)
                {
                    ddl.AddRange(Schools.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name,
                        Selected = (s.Id == SelectedSchoolId)
                    }));
                }

                return ddl;
            }
        }

        public List<SelectListItem> EduLevelDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Education Level --" }
        };

                if (EducationLevels != null)
                {
                    ddl.AddRange(EducationLevels.Select(e => new SelectListItem
                    {
                        Value = e.Value,
                        Text = e.Label,
                        Selected = e.Value == SelectedEduLevelId
                    }));
                }

                return ddl;
            }
        }

        public List<SelectListItem> ProgramDropdown
        {
            get
            {
                var ddl = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Program --" }
        };

                if (Campusdegrees != null)
                {
                    ddl.AddRange(Campusdegrees.Select(p => new SelectListItem
                    {
                        Value = p.Clientid.ToString(), // or p.Id if you have an Id property
                        Text = p.Name,
                        Selected = p.Clientid.ToString() == TestLeadProgramId
                    }));
                }

                return ddl;
            }
        }

    }
}
