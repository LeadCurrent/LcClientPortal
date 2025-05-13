using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using static Data.DocumentEnums;

namespace Web
{
    public class DocumentVM
    {
        /* Ajax */
        public string Action { get; set; }
        public int Param { get; set; }
        public bool AjaxUpdate { get; set; }
        public string DivToUpdate { get; set; }
        public bool BlockPostBack { get; set; }
        public bool MobileApp { get; set; }

        /* Models */
        public List<CompanyFolder> ParentFolders { get; set; }
        public List<CompanyFolder> ParentFolderss { get; set; }
        public List<CompanyFolder> GetAllFoldersName { get; set; }
        public List<Document> Documents { get; set; }
        public List<FolderAccess> FolderAccessies { get; set; }
        public List<CompayUserRole> CompanyUserRoles { get; set; }
        public List<Role> Roles { get; set; }
        public Document Document { get; set; }
        public DocumentVersion DocumentVersion { get; set; }
        public CompanyFolder CompanyFolder { get; set; }
        public FolderAccess FolderAccess { get; set; }

        /* bool Params */
        public bool UpdateSuccessful { get; set; }
        public bool ShowAddNewSubFolder { get; set; }
        public bool BackToEdit { get; set; }
        public bool ShowAddRole { get; set; }
        public bool ShowCreateOrUpdate { get; set; }
        public bool IsViewOnly { get; set; }
        public bool IsAdmin { get; set; }


        /* enums */



        /* ints */
        public int EditSubFolderId { get; set; }
        public int VideoId { get; set; }
        public int CompanyId { get; set; }
        public int DocumentId { get; set; }

        /* strings */
        public string FilterName { get; set; }
        public string NewSubFolder { get; set; }
        public string SortBy { get; set; }
        public string SelectedUploadType { get; set; }
        public string ErrorMsg { get; set; }


        /* dates */
        public DateTime SelectedDate { get; set; }

        /* dropdowns */

        public List<SelectListItem> RoleDDL
        {
            get
            {
                var DDL = new List<SelectListItem>();
                var item1 = new SelectListItem();
                item1.Value = "0";
                item1.Text = "";
                DDL.Add(item1);

                if (Roles != null)
                    foreach (var i in Roles)
                    {
                        var item = new SelectListItem();
                        item.Value = i.Id.ToString();
                        item.Text = i.RoleName;
                        DDL.Add(item);
                    }
                return DDL;
            }
        }

    }
}
