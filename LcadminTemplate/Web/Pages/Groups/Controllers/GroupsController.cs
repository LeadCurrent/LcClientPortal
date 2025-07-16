using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Project.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class GroupsController : Controller
    {
        private readonly RazorViewToStringRenderer viewRenderer;
        ExceptionLogger exceptionLogger { get; }
        GroupDataLibrary GroupDL { get; set; }

        public GroupsController(
            GroupDataLibrary GroupDataLibrary,
            RazorViewToStringRenderer RazorViewToStringRenderer,
            ExceptionLogger ExceptionLogger)
        {
            GroupDL = GroupDataLibrary;
            viewRenderer = RazorViewToStringRenderer;
            exceptionLogger = ExceptionLogger;
        }

        public async Task<GroupsVM> getGroupList(string Search = null)
        {
            var Model = new GroupsVM();
            Model.Groups = await GroupDL.GetGroups();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                Model.GroupSearch = Search;
                Model.Groups = Model.Groups
                    .Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Model;
        }

        public async Task<IActionResult> Index(string Search = null)
        {
            var VM = await getGroupList(Search); // Pass search term to method
            return View("~/Pages/Groups/Groups.cshtml", VM);
        }

        [HttpPost]
        public async Task<IActionResult> Groups(GroupsVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create New Group")
                {
                    return RedirectToAction("Create");
                }
                
                if (Action == "Edit")
                {
                    return RedirectToAction("Edit", new { GroupId = ViewModel.Param});
                }

                var VM = new GroupsVM();

                if (Action == "Search")
                {
                     VM = await getGroupList(ViewModel.GroupSearch);
                }

                if (Action == "Clear Search")
                {
                     VM = await getGroupList();
                }

                if (Action == "Update InActiveCampuses")
                {
                    //await GroupDL.UpdateCampusesStatus(ViewModel.InactiveCampuses);
                     VM = await getGroupList(ViewModel.GroupSearch);
                }

                var __html = await viewRenderer.RenderViewToStringAsync("Groups/PartialViews/Groups_Partial", VM);
                return Json(new { isValid = true, html = __html });
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(
                    ex,
                    User.Identity.Name,
                    ViewModel.AjaxUpdate,
                    ViewModel.Action,
                    System.Text.Json.JsonSerializer.Serialize(ViewModel),
                    HttpContext.Request.GetDisplayUrl(),
                    HttpContext.Request.GetDisplayUrl());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            try
            {
                var Model = new GroupsVM();
                Model.Group = new Group();

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Groups/Create.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupsVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Create")
                {
                    await GroupDL.CreateGroups(ViewModel.Group);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int GroupId)
        {
            try
            {
                var Model = new GroupsVM();
                Model.Group = await GroupDL.GetGroupById(GroupId);

                if (HttpContext.Session.GetString("MobileApp") != null)
                    Model.MobileApp = true;

                return View("~/Pages/Groups/Edit.cshtml", Model);
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupsVM ViewModel, string Action)
        {
            try
            {
                if (Action == "Update")
                {
                    await GroupDL.UpdateGroup(ViewModel.Group);
                }

                if (Action == "Delete")
                {
                    await GroupDL.DeleteGroupById(ViewModel.Param);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return await exceptionLogger.LogException(ex, User.Identity.Name, ViewModel.AjaxUpdate, ViewModel.Action, System.Text.Json.JsonSerializer.Serialize(ViewModel), HttpContext.Request.GetDisplayUrl(), HttpContext.Request.GetDisplayUrl());
            }
        }
    }
}
