using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Microsoft.AspNetCore.Components.Web;

namespace Web
{
    public class ErrorVM
    {
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }

        public ExceptionLog ExceptionLog { get; set; }
        public string Environment { get; set; }
        public bool Submitted { get; set; }
        public bool ShowTechnicalDetails { get; set; }

    }
}
