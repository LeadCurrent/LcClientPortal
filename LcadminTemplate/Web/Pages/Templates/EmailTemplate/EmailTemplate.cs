
using Data;

namespace Web
{
    public class EmailTemplate
    {
        public string EmailBody { get; set; }
        public string EmailBody2 { get; set; }
        public string LogoURL { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string ButtonURL { get; set; }
        public string ButtonText { get; set; }

        public string Button2URL { get; set; }
        public string Button2Text { get; set; }
        public string UnsubscribeLink { get; set; }
        public string Subject { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCityStateZip { get; set; }
       
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCityStateZip { get; set; }

        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorCityStateZip { get; set; }
        public Company Company { get; set; }


    }
}
