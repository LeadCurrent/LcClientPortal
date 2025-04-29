using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class CustomerUpload
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string FullAddress
        {
            get
            {
                return BillingAddress + ", " + BillingCity + ", " + BillingState + ", " + BillingZip;
            }
        }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Phone
        {
            get
            {
                var p = "";
                if (Phone1 != null)
                    if (Phone1 != "")
                        p = Phone1;

                if (Phone2 != null)
                    if (Phone2 != "")
                        p += "-" + Phone2;

                if (Phone3 != null)
                    if (Phone3 != "")
                        p += "-" + Phone3;

                return p;
            }
        }
        public string Email { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}
