using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses
{
    public class CommonTelnyxMessage
    {
        public int Id { get; set; }   
        public string? Type { get; set; } 
        public string? MessageId { get; set; }
        public string? FromCarrier { get; set; }
        public string? FromLineType { get; set; }
        public string? FromPhoneNumber { get; set; }
        public string? FormattedFromNumber { get; set; }
        public string? ToCarrier { get; set; }
        public string? ToLineType { get; set; }
        public string? ToPhoneNumber { get; set; }
        public string? FormattedToNumber { get; set; }
        public string? Status { get; set; } 
        public string? Direction { get; set; }  
        public string? Text { get; set; } 
        public DateTime? ReceivedAt { get; set; } 
        public DateTime? SentAt { get; set; } 
        public string? MessagingProfileId { get; set; }
        public bool IsRead { get; set; }

        public List<CommonTelnyxMessageMedia>? Media { get; set; }  


    }
}
