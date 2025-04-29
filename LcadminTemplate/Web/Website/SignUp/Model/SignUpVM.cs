using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace Web
{
    public class SignUpVM
    {
        /*Ajax*/
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }

        /*Models*/
        public User User { get; set; }
        public ContactUs ContactUs { get; set; }
        public List<User> Users { get; set; }
        public Company Company { get; set; }
        public List<Company> Companys { get; set; }
        public List<CompanyUser> CompanyUsers { get; set; }

        /* strings */
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Message { get; set; }
        public string SignupName { get; set; }
        public string SignupEmail { get; set; }
        public string SignupPhone { get; set; }
        public string InvalidUser { get; set; }


        /*bool*/
        public bool PasswordInvalid { get; set; }
        public bool InvalidLogin { get; set; }
        public bool PasswordDoesNotMatch { get; set; }
        public bool UpdateSuccessful { get; set; }
        public bool passwordsent { get; set; }
        public bool nomatch { get; set; }

    }
}
