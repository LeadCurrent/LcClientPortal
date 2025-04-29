using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Web
{
    public static class DocumentEnumDDL
    {

        public static List<SelectListItem> DocumentFileTypeDDL()
        {
            var ddl = new List<SelectListItem>()
           {
                new SelectListItem() { Text = "Document", Value = "0"},
                new SelectListItem() { Text = "Image", Value = "1"},
                new SelectListItem() { Text = "Video", Value = "2"},
                 new SelectListItem() { Text = "Link", Value = "3"}

          };
            return ddl;
        }

        public static List<SelectListItem> LinkIconDDL()
        {
            var ddl = new List<SelectListItem>()
           {
                new SelectListItem() { Text = "", Value = "0"},
                new SelectListItem() { Text = "txt", Value = "1"},
                new SelectListItem() { Text = "xlsx", Value = "2"},
                new SelectListItem() { Text = "doc", Value = "3"},
                 new SelectListItem() { Text = "ppt", Value = "4"},
                 new SelectListItem() { Text = "pdf", Value = "5"},
                 new SelectListItem() { Text = "png", Value = "6"},
                 new SelectListItem() { Text = "jpg", Value = "7"},
                 new SelectListItem() { Text = "jpeg", Value = "8"},
                 new SelectListItem() { Text = "mp4", Value = "9"},
                 new SelectListItem() { Text = "zip", Value = "10"}

          };
            return ddl;
        }



    }
}

