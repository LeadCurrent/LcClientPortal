using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Data.GeneralEnums;

namespace Data
{
    public class Customer : BaseModel
    {
        /* Foreign Keys */
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public int LeadId { get; set; }

        public int ExternalCustomerId { get; set; }


        /* Lists */
        public List<CustomerNote> CustomerNotes { get; set; }
        public List<Document> CustomerDocuments { get; set; }

        /*Strings */
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public string BillingCompanyName { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string BillingPhone { get; set; }
        public string BillingEmail { get; set; }
        public string QuickBooksCustomerId { get; set; }
        public string Guid { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        /* Bool */
        public bool Unsubscribed { get; set; }
        public bool IsDeleted { get; set; }

        /*Decimal*/

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxRate { get; set; }

        /*Calculated */

        public string CityState
        {
            get
            {
                return City + ", " + GeneralEnums.StateShort(State);
            }
        }
        public string FullAddress
        {
            get
            {
                if (Address != null && City != null && State != GeneralEnums.State.None && ZipCode != null)
                    return Address + ", " + City + ", " + GeneralEnums.StateShort(State) + " " + ZipCode;
                else if (Address != null)
                    return Address;
                else
                    return "";
            }
        }

        public string CityStateZip
        {
            get
            {
                if (City != null && State != GeneralEnums.State.None && ZipCode != null)
                    return City + ", " + GeneralEnums.StateShort(State) + ", " + ZipCode;
                else if (City != null && State != GeneralEnums.State.None)
                    return City + ", " + GeneralEnums.StateShort(State);
                else if (City != null && ZipCode != null)
                    return City + ", " + ZipCode;
                else if (State != GeneralEnums.State.None && ZipCode != null)
                    return GeneralEnums.StateShort(State) + ", " + ZipCode;
                else if (City != null)
                    return City;
                else if (State != GeneralEnums.State.None)
                    return GeneralEnums.StateShort(State);
                else if (ZipCode != null)
                    return ZipCode;
                else
                    return "";
            }
        }


        public string googlemaps
        {
            get
            {
                return "https://maps.google.com/?q=" + System.Web.HttpUtility.UrlEncode(Address + " " + City + ", " + GeneralEnums.StateDesc(State) + ", " + ZipCode);
            }
        }
    }
}
