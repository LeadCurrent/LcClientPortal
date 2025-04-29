using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Data.UserEnums;
namespace Data
{
    public class UserLoginHistory 
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime LoginDateTime { get; set; }
        public Device Device { get; set; }
    
    }
}
