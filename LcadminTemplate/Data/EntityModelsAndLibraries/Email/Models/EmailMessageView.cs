using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EmailMessageView
    {
        public int Id { get; set; }
        public int CompanyEmailAccountId { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public int EmailMessageId { get; set; }
        public EmailMessage EmailMessage { get; set; }
        public int? ContactId { get; set; }
        public int? TasksId { get; set; }
        public int? SendEmailId { get; set; }
        public SendEmail SendEmail { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public bool ContactEmail { get; set; }
        public bool CustomerEmail { get; set; }
        public bool VendorEmail { get; set; }
        public bool LeadEmail { get; set; }
        public bool SentEmail { get; set; }
        public bool NotificationEmail { get; set; }
    }

    public class EmailMessageSummary
    {
        public int CompanyEmailAccountId { get; set; }
        public CompanyEmailAccount CompanyEmailAccount { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string CustomerVendorLead { get; set; }
        public int Read { get; set; }
        public int Unread { get; set; }
        public int Tasks { get; set; }
        public bool ContactEmail { get; set; }
        public bool CustomerEmail { get; set; }
        public bool VendorEmail { get; set; }
        public bool LeadEmail { get; set; }
        public bool NotificationEmail { get; set; }
        public DateTime LastMessageDate {  get; set; }
        public string LastMessageSubject {  get; set; }
        public int TotalMessages { get; set; }
        public bool IsChecked { get; set; }
    }

}
