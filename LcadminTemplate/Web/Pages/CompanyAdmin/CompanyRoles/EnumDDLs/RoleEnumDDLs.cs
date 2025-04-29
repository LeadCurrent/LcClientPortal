using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Web
{
    public static class RoleEnumDDLs
    {
        public static List<SelectListItem> AccessDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "No Access", Value = "0"},
                    new SelectListItem() { Text = "View Only", Value = "1"},
                    new SelectListItem() { Text = "Edit and View", Value = "2"}
                 };
            return ddl;
        }

        public static List<SelectListItem> JobAccessDDL()
        {
            var ddl = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "No Access", Value = "0"},
                    new SelectListItem() { Text = "Job Details, Quote and Invoice", Value = "1"},
                    new SelectListItem() { Text = "Job Details Only", Value = "2"}
                 };
            return ddl;
        }

        public static List<SelectListItem> RoleDDL()
        {
            var ddl = new List<SelectListItem>()
                  {
                       new SelectListItem() { Text = "Admin", Value = "1"},
                       new SelectListItem() { Text = "SystemAdmin", Value = "2"},
                 };
            return ddl;
        }
    }
}
