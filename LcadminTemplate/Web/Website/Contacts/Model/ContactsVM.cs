using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace Web
{
    public class ContactsVM
    {
        /*Ajax*/
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }

        /*Models*/
        public User User { get; set; }
        public Company Company { get; set; }

        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Total { get; set; }
        public int TotalInput { get; set; }
        /* strings */

        //public string Id { get; set; }
        //public string UserName { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public string CompanyName { get; set; }
        //public string Message { get; set; }


        /*bool*/
        public bool ContactSent { get; set; }
        public bool RobotFailed { get; set; }


        public string SignupName { get; set; }
    
        public string SignupEmail { get; set; }

        public string SignupPhone { get; set; }
        public string Message { get; set; }

    }
}
