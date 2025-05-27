
using DocumentFormat.OpenXml.Bibliography;
using System;

namespace Data
{
    public static class RoleEnums
    {
        
        public enum Access
        {
            NoAccess = 0,
            ViewOnly = 1,
            EditAndView = 2
        }

        public enum JobAccess
        {
            NoAccess = 0,
            JobDetailsQuoteAndInvoice = 1,
            JobDetailsOnly = 2
        }
        public enum Permission
        {
            Customers = 1,
            Email = 2,
            Documents = 3,
            CompanyProfile = 4,
            CompanyRoles = 5,
            Users = 6,
            Integration = 7,
            ContactForm = 8,
            CompanyAccount = 9,
            Sources = 10,
            Schools = 11,
        }

        public static string PermissionDesc(Permission permission)
        {
            if (permission == Permission.Customers) return "Customers";
            else if (permission == Permission.Email) return "Email";
            else if (permission == Permission.Documents) return "Documents";
            else if (permission == Permission.CompanyProfile) return "Company Profile";
            else if (permission == Permission.CompanyRoles) return "Company Roles";
            else if (permission == Permission.Users) return "Users";
            else if (permission == Permission.Integration) return "Integration";
            else if (permission == Permission.ContactForm) return "Contact Form";
            else if (permission == Permission.CompanyAccount) return "Company Account";
            else if (permission == Permission.CompanyAccount) return "Sources";
            else if (permission == Permission.CompanyAccount) return "Schools";
            else return "";
        }

       
        public static int PermissionSort(Permission permission)
        {
            if (permission == Permission.Customers) return 1;
            if (permission == Permission.Email) return 2;
            if (permission == Permission.Documents) return 3;
            if (permission == Permission.CompanyProfile) return 4;
            if (permission == Permission.CompanyRoles) return 5;
            if (permission == Permission.Users) return 6;
            if (permission == Permission.Integration) return 7;
            if (permission == Permission.ContactForm) return 8;
            if (permission == Permission.CompanyAccount) return 9;
            if (permission == Permission.Sources) return 10;
            if (permission == Permission.Schools) return 11;

            else return 0;
        }

        public static bool PermissionIncluded(Permission permission)
        {
            if (permission == Permission.Customers && CommonClasses.Environment.Customers) return true;       
            if (permission == Permission.Email && CommonClasses.Environment.Email) return true;
            if (permission == Permission.Documents && CommonClasses.Environment.Documents) return true;
            if (permission == Permission.CompanyProfile && CommonClasses.Environment.CompanyProfile) return true;
            if (permission == Permission.Integration && CommonClasses.Environment.CompanyProfile) return true;  
            if (permission == Permission.CompanyRoles && CommonClasses.Environment.CompanyProfile) return true;
            if (permission == Permission.CompanyAccount && CommonClasses.Environment.CompanyProfile) return true;
            if (permission == Permission.ContactForm && CommonClasses.Environment.CompanyProfile) return true;
            if (permission == Permission.Users && CommonClasses.Environment.CompanyProfile) return true;
            if (permission == Permission.Sources && CommonClasses.Environment.CompanyProfile) return true;
            if (permission == Permission.Schools && CommonClasses.Environment.CompanyProfile) return true;

            else return false;
        }
    }
}

