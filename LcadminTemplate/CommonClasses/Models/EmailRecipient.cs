using CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonClasses.Enums;

namespace CommonClasses
{ 
    public class CommonEmailRecipientHelper
    {
        public int Id { get; set; }
        public int EmailMessageId { get; set; }
        public CommonEmailMessage? EmailMessage { get; set; }
        public RecipientType RecipientType { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
