//using Data;
//using Microsoft.AspNetCore.Mvc;
//using Project.Utilities;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;
//using static Data.CompanyEnums;
//using CommonClasses;

//namespace Web.Controllers
//{
//    [Authorize(Policy = "AdminOrEmail")]
//    public class EmailController : Controller
//    {
        
//        private readonly RazorViewToStringRenderer viewRenderer;
//        private UserDataLibrary UserDL { get; }
//        public CustomerDataLibrary CustomerDL { get; }
//        public CompanyDataLibrary CompanyDL { get; }
//        public SignInManager<User> SignInManager { get; }
//        private CompanyIntegrationDataLibrary CompanyIntegrationDL { get; }
//        Email email { get; }

//        public UserManager<User> UserManager { get; }
//        ExceptionLogger exceptionLogger { get; }
//        public List<Customer> lst { get; set; } = new List<Customer>();
//        public EmailController(
//            CustomerDataLibrary CustomerDataLibrary,
//            RazorViewToStringRenderer RazorViewToStringRenderer,
//            CompanyIntegrationDataLibrary CompanyIntegrationDataLibrary,
//            UserDataLibrary UserDataLibrary,
//            ExceptionLogger ExceptionLogger,
//            UserManager<User> userManager,
//            SignInManager<User> signInManager,
//            CompanyDataLibrary CompanyDataLibrary,
//            Email Email,
//        )
//        {
//            CustomerDL = CustomerDataLibrary;
//            CompanyDL = CompanyDataLibrary;
//            CompanyIntegrationDL = CompanyIntegrationDataLibrary;
//            UserManager = userManager;
//            SignInManager = signInManager;
//            email = Email;
//            viewRenderer = RazorViewToStringRenderer;
//            exceptionLogger = ExceptionLogger;
//        }  

//        [HttpPost]
//        public async Task<UserEmailVM> UserEmail(UserEmailVM ViewModel,string UserName,int CompanyId, string Action)
//        {            
//            if (Action == "Filter Contact")
//            {
               
//                if (ViewModel.ShowEmailPreview)
//                {
//                    ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(ViewModel.EmailMessage.Id);
//                    var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(ViewModel.EmailMessage.Id);
//                    if (EmailMessageView.ContactId != null)
//                    {
//                        ViewModel.EmailMessage.IsContact = true;
//                    }
//                    if (EmailMessageView.CustomerEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsCustomer = true;
//                    }
//                    if (EmailMessageView.VendorEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsVendor = true;
//                    }
//                    if (EmailMessageView.LeadEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsLead = true;
//                    }
//                    if (EmailMessageView.TasksId != null)
//                    {
//                        ViewModel.EmailMessage.IsTask = true;
//                    }
//                    if (EmailMessageView.SendEmailId != null)
//                    {
//                        ViewModel.EmailMessage.HasDraft = true;
//                    }
//                    if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                    {
//                        string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                        //await MicrosoftAPI.MarkEmailsAsReadAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                        ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                    }
//                    else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                    {
//                        string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                        //await GoogleAPI.MarkEmailAsRead(accessToken, ViewModel.EmailMessage.MessageId);
//                        ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                    }

//                    //await UserEmailDL.MarkEmailAsRead(ViewModel.EmailMessage.Id);

//                    ViewModel.ShowEmailPreview = true;
//                }

//            }  
//            if (Action == "View All from Sender")
//            {
//                if (ViewModel.ShowEmailPreview)
//                {
//                    ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(ViewModel.EmailMessage.Id);
//                    var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(ViewModel.EmailMessage.Id);
//                    if (EmailMessageView.ContactId != null)
//                    {
//                        ViewModel.EmailMessage.IsContact = true;
//                    }
//                    if (EmailMessageView.CustomerEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsCustomer = true;
//                    }
//                    if (EmailMessageView.VendorEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsVendor = true;
//                    }
//                    if (EmailMessageView.LeadEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsLead = true;
//                    }
//                    if (EmailMessageView.TasksId != null)
//                    {
//                        ViewModel.EmailMessage.IsTask = true;
//                    }
//                    if (EmailMessageView.SendEmailId != null)
//                    {
//                        ViewModel.EmailMessage.HasDraft = true;
//                    }
//                    if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                    {
//                        string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                        //await MicrosoftAPI.MarkEmailsAsReadAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                        ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                    }
//                    else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                    {
//                        string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                        //await GoogleAPI.MarkEmailAsRead(accessToken, ViewModel.EmailMessage.MessageId);
//                        ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                    }
//                }

//            }
//            if (Action == "Delete Email")
//            {
//                var Email = await UserEmailDL.GetEmailMessageById(ViewModel.Param);
//                var emailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(Email.CompanyEmailAccountId);
//                int emailIndex = ViewModel.EmailMessageView.FindIndex(e => e.EmailMessage.Id == Email.Id);
//                if (emailAccount.EmailType == CompanyEnums.EmailType.Gmail)
//                {
//                    string accessTokenGmail = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(emailAccount.RefreshToken);
//                    await GoogleAPI.DeleteEmail(accessTokenGmail, Email.MessageId);
//                    await UserEmailDL.DeleteMessage(Email.Id);
//                }
//                else if (emailAccount.EmailType == CompanyEnums.EmailType.Office365)
//                {
//                    string accessTokenOffice = await MicrosoftAPI.GetAccessTokenAsync(emailAccount.RefreshToken);
//                    var emails = await MicrosoftAPI.DeleteEmailsAsync(accessTokenOffice, Email.MessageId);
//                    await UserEmailDL.DeleteMessage(Email.Id);

//                }
//                ViewModel.ShowEmailPreview = false;
//                ViewModel.EmailMessageView.RemoveAt(emailIndex);
//                var nextEmail = (emailIndex < ViewModel.EmailMessageView.Count)
//                             ? ViewModel.EmailMessageView[emailIndex]
//                             : null;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);

//                if (nextEmail != null)
//                {
//                    ViewModel.Param = nextEmail.EmailMessage.Id;
//                    ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(nextEmail.EmailMessage.Id);
//                    var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(nextEmail.EmailMessage.Id);
//                    if (EmailMessageView != null)
//                    {
//                        if (EmailMessageView.ContactId != null)
//                        {
//                            ViewModel.EmailMessage.IsContact = true;
//                        }
//                        if (EmailMessageView.CustomerEmail == true)
//                        {
//                            ViewModel.EmailMessage.IsCustomer = true;
//                        }
//                        if (EmailMessageView.VendorEmail == true)
//                        {
//                            ViewModel.EmailMessage.IsVendor = true;
//                        }
//                        if (EmailMessageView.LeadEmail == true)
//                        {
//                            ViewModel.EmailMessage.IsLead = true;
//                        }
//                        if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                        {
//                            string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                            ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                        }
//                        else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                        {
//                            string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                            ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                        }


//                        ViewModel.ShowEmailPreview = true;
//                    }
                   

//                }

//            }
//            if (Action == "Mark Email As Read")
//            {
//                var message = await UserEmailDL.GetEmailMessageById(ViewModel.Param);
//                var emailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(message.CompanyEmailAccountId);
//                int emailIndex = ViewModel.EmailMessageView.FindIndex(e => e.EmailMessage.Id == message.Id);
//                var nextEmail = (emailIndex < ViewModel.EmailMessageView.Count)
//                            ? ViewModel.EmailMessageView[emailIndex + 1]
//                            : null;



//                if (emailAccount.EmailType == CompanyEnums.EmailType.Gmail)
//                {
//                    string accessTokenGmail = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(emailAccount.RefreshToken);
//                    await GoogleAPI.MarkEmailAsRead(accessTokenGmail, message.MessageId);
//                    await UserEmailDL.MarkEmailAsRead(message.Id);
//                }
//                else if (emailAccount.EmailType == CompanyEnums.EmailType.Office365)
//                {
//                    string accessTokenOffice = await MicrosoftAPI.GetAccessTokenAsync(emailAccount.RefreshToken);
//                    var emails = await MicrosoftAPI.MarkEmailsAsReadAsync(accessTokenOffice, message.MessageId);
//                    await UserEmailDL.MarkEmailAsRead(message.Id);
//                }


//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                ViewModel.Param = nextEmail.EmailMessage.Id;
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(nextEmail.EmailMessage.Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(message.Id);
//                if (EmailMessageView.ContactId != null)
//                {
//                    ViewModel.EmailMessage.IsContact = true;
//                }
//                if (EmailMessageView.CustomerEmail == true)
//                {
//                    ViewModel.EmailMessage.IsCustomer = true;
//                }
//                if (EmailMessageView.VendorEmail == true)
//                {
//                    ViewModel.EmailMessage.IsVendor = true;
//                }
//                if (EmailMessageView.LeadEmail == true)
//                {
//                    ViewModel.EmailMessage.IsLead = true;
//                }
//                if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                {
//                    string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                    ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                }
//                else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                {
//                    string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                    ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                }

//                ViewModel.ShowEmailPreview = true;
                

//            }
//            if (Action == "Change Contact")
//            {
//                var CustomerId = ViewModel.SelectedCustomer;
//                var email = ViewModel.SendEmail;
//                //ViewModel = await getModel("Customer", StringFormating.CurrentTime().AddDays(-7).Date, StringFormating.CurrentTime().Date, CompanyId ,UserName);
//                ViewModel.Customers = await CustomerDL.GetActiveCustomers(CompanyId);

//                ViewModel.Customer = ViewModel.Customers.Where(x => x.Id == Convert.ToInt32(CustomerId)).FirstOrDefault(); 
//                ViewModel.SelectedCustomer = CustomerId;
//                ViewModel.ShowAddCustomerContact = true;
//                ViewModel.ShowNewEmail = true;
//                ViewModel.SendEmail = email;
//            }
//            if (Action == "Change CC Contact")
//            {
//                var CustomerId = ViewModel.SelectedCcCustomer;
//                var email = ViewModel.SendEmail;
//                // ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);                ViewModel.Customers = await CustomerDL.GetActiveCustomers(CompanyId);
//                ViewModel.Customers = await CustomerDL.GetActiveCustomers(CompanyId);

//                ViewModel.Customer = ViewModel.Customers.Where(x => x.Id == Convert.ToInt32(CustomerId)).FirstOrDefault(); ;
//                ViewModel.SelectedCcCustomer = CustomerId;
//                ViewModel.ShowAddCCCustomerContact = true;
//                ViewModel.ShowNewEmail = true;
//                ViewModel.SendEmail = email;
              
//            }
//            if (Action == "Show Email Preview")
//            {
//                var Id = ViewModel.Param;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);



//                if (EmailMessageView.ContactId != null)
//                {
//                    ViewModel.EmailMessage.IsContact = true;
//                }
//                if (EmailMessageView.CustomerEmail == true)
//                {
//                    ViewModel.EmailMessage.IsCustomer = true;
//                }
//                if (EmailMessageView.VendorEmail == true)
//                {
//                    ViewModel.EmailMessage.IsVendor = true;
//                }
//                if (EmailMessageView.LeadEmail == true)
//                {
//                    ViewModel.EmailMessage.IsLead = true;
//                }
//                if (EmailMessageView.SendEmailId != null)
//                {
//                    ViewModel.EmailMessage.HasDraft = true;
//                }
//                if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                {
//                    string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                    //await MicrosoftAPI.MarkEmailsAsReadAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                    ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                }
//                else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                {
//                    string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                    //await GoogleAPI.MarkEmailAsRead(accessToken, ViewModel.EmailMessage.MessageId);
//                    ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                }

//                //await UserEmailDL.MarkEmailAsRead(ViewModel.EmailMessage.Id);

//                ViewModel.ShowEmailPreview = true;
                
//            }
//            if (Action == "Show Email Preview Mobile")
//            {
//                var Id = ViewModel.Param;
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);
//                if (EmailMessageView.ContactId != null)
//                {
//                    ViewModel.EmailMessage.IsContact = true;
//                }
//                if (EmailMessageView.CustomerEmail == true)
//                {
//                    ViewModel.EmailMessage.IsCustomer = true;
//                }
//                if (EmailMessageView.VendorEmail == true)
//                {
//                    ViewModel.EmailMessage.IsVendor = true;
//                }
//                if (EmailMessageView.LeadEmail == true)
//                {
//                    ViewModel.EmailMessage.IsLead = true;
//                }
//                if (EmailMessageView.TasksId != null)
//                {
//                    ViewModel.EmailMessage.IsTask = true;
//                }
//                if (EmailMessageView.SendEmailId != null)
//                {
//                    ViewModel.EmailMessage.HasDraft = true;
//                }
//                if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                {
//                    string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                    ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                }
//                else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                {
//                    string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                    ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                }
//                ViewModel.EmailMessageView = new List<EmailMessageView>();
//                ViewModel.ShowEmailPreview = true;
                
//            }
//            if (Action == "Add Customer Contact")
//            {
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                ViewModel.ShowAddCustomerContact = true;
//                ViewModel.ShowNewEmail = true;
                
//            }
//            if (Action == "Add CC Customer Contact")
//            {
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                ViewModel.ShowAddCCCustomerContact = true;
//                ViewModel.ShowNewEmail = true;
               
//            }
//            if (Action == "Show Fields")
//            {
//                var Id = ViewModel.Param;
//                int companyId = int.Parse(base.User.Claims.Where((Claim x) => x.Type == "CompanyId").FirstOrDefault().Value);

//                if (ViewModel.SelectedContactType == "Customer")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                    ViewModel.SelectedContactType = "Customer";
//                    ViewModel.ContactType = "ExistingCustomer";
//                    ViewModel.ShowIsContact = true;
//                    ViewModel.IsCustomer = true;
//                    ViewModel.IsOtherContact = false;
//                    ViewModel.IsVendor = false;
//                    ViewModel.IsLead = false;
//                }
//                else if (ViewModel.SelectedContactType == "Other")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                    ViewModel.IsOtherContact = true;
//                    ViewModel.SelectedContactType = "Other";
//                    ViewModel.IsVendor = false;
//                    ViewModel.IsLead = false;
//                    ViewModel.IsCustomer = false;
//                    ViewModel.IsOtherContact = true;
//                }
//                ViewModel.ShowModelPopup = true;
//                ViewModel.DivToUpdate = null;
                

//            }
//            if (Action == "Show Edit With PopUp")
//            {
//                var Id = ViewModel.Param;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);
//                if (EmailMessageView.ContactId != null)
//                {
//                    ViewModel.EmailMessage.IsContact = true;
//                }
//                if (EmailMessageView.CustomerEmail == true)
//                {
//                    ViewModel.EmailMessage.IsCustomer = true;
//                }
//                if (EmailMessageView.VendorEmail == true)
//                {
//                    ViewModel.EmailMessage.IsVendor = true;
//                }
//                if (EmailMessageView.LeadEmail == true)
//                {
//                    ViewModel.EmailMessage.IsLead = true;
//                }
//                ViewModel.SelectedContactType = "Customer";
//                ViewModel.ContactType = "ExistingCustomer";
//                ViewModel.IsCustomer = true;
//                ViewModel.IsExistingCustomer = true;
//                ViewModel.ShowModelPopup = true;
//                ViewModel.DivToUpdate = null;
               
//            }
//            if (Action == "Cancel Popup")
//            {
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                //var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/ModalPopup", ViewModel);
//                //return Json(new { isValid = true, html = PopupHTML });
//            }
//            if (Action == "Show Task PopUp")
//            {
//                var Id = ViewModel.Param;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                ViewModel.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                int companyId = CompanyId;
//                ViewModel.CompanyUsers = await UserDL.GetActiveUsersForCompany(companyId);
//                ViewModel.ShowTaskPopup = true;
//                ViewModel.DivToUpdate = null;
//                ViewModel.CompanyUsers = await CompanyDL.GetCompanyUsers(CompanyId);

                
//            }
//            if (Action == "Cancel Task Popup")
//            {
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                //var PopupHTML = await viewRenderer.RenderViewToStringAsync("UserEmail/PartialViews/Sections/TaskPopUp", ViewModel);
//                //return Json(new { isValid = true, html = PopupHTML });
//            }
//            if (Action == "Show Existing Customer")
//            {
//                var Id = ViewModel.Param;
//                if (ViewModel.ContactType == "ExistingCustomer")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsExistingCustomer = true;
//                    ViewModel.ContactType = "ExistingCustomer";

//                }
//                else if (ViewModel.ContactType == "NewCustomer")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsNewCustomer = true;
//                    ViewModel.ContactType = "NewCustomer";
//                }
//                ViewModel.SelectedContactType = "Customer";
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);
//                ViewModel.ShowModelPopup = true;
//                ViewModel.DivToUpdate = null;
//                ViewModel.IsCustomer = true;
                
//            }
//            if (Action == "Show Existing Vendor")
//            {
//                var Id = ViewModel.Param;
//                if (ViewModel.VendorType == "ExistingVendor")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsVendor = true;
//                    ViewModel.IsExistingVendor = true;
//                    ViewModel.VendorType = "ExistingVendor";

//                }
//                else if (ViewModel.VendorType == "NewVendor")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsExistingVendor = true;
//                    ViewModel.IsVendor = true;
//                    ViewModel.VendorType = "NewVendor";
//                }
//                ViewModel.SelectedContactType = "Vendor";
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                int companyId = int.Parse(base.User.Claims.Where((Claim x) => x.Type == "CompanyId").FirstOrDefault().Value);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);
//                ViewModel.ShowModelPopup = true;
//                ViewModel.DivToUpdate = null;
//                ViewModel.IsCustomer = true;
               

//            }
//            if (Action == "Show Existing Lead")
//            {
//                var Id = ViewModel.Param;
//                if (ViewModel.LeadType == "ExistingLead")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsLead = true;
//                    ViewModel.IsExistingLead = true;
//                    ViewModel.LeadType = "ExistingLead";

//                }
//                else if (ViewModel.LeadType == "NewLead")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsLead = true;
//                    ViewModel.IsNewLead = true;
//                    ViewModel.LeadType = "NewLead";
//                }
//                ViewModel.SelectedContactType = "Lead";
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);
//                int companyId = int.Parse(base.User.Claims.Where((Claim x) => x.Type == "CompanyId").FirstOrDefault().Value);
//                ViewModel.ShowModelPopup = true;
//                ViewModel.DivToUpdate = null;
//                ViewModel.IsCustomer = true;
                
//            }
//            if (Action == "Show Existing Lead")
//            {
//                var Id = ViewModel.Param;
//                if (ViewModel.LeadType == "ExistingLead")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsLead = true;
//                    ViewModel.IsExistingLead = true;
//                    ViewModel.LeadType = "ExistingLead";

//                }
//                else if (ViewModel.LeadType == "NewLead")
//                {
//                    //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                    ViewModel.IsLead = true;
//                    ViewModel.IsNewLead = true;
//                    ViewModel.LeadType = "NewLead";
//                }
//                ViewModel.SelectedContactType = "Lead";
//                ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(Id);
//                int companyId = int.Parse(base.User.Claims.Where((Claim x) => x.Type == "CompanyId").FirstOrDefault().Value);
//                ViewModel.ShowModelPopup = true;
//                ViewModel.DivToUpdate = null;
//                ViewModel.IsCustomer = true;
                
//            }
//            if (Action == "Add Send Recipient")
//            {
//                var SendRecipient = new SendEmailRecipient();
//                SendRecipient.SendEmailId = ViewModel.SendEmail.Id;
//                if (ViewModel.NewEmail != null)
//                {
//                    SendRecipient.Email = ViewModel.NewEmail;
//                }
//                SendRecipient.RecipientType = RecipientType.TO;
//                await UserEmailDL.CreateSendEmailRecipient(SendRecipient);
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                ViewModel.ShowNewEmail = true;
               

//            }
//            if (Action == "Create Contact")
//            {
//                if (ViewModel.SelectedContactType == "Customer")
//                {
//                    var Id = ViewModel.Param;
//                    if (ViewModel.ContactType == "ExistingCustomer")
//                    {
//                        var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                        var Customer = await CustomerDL.GetCustomer(ViewModel.CustomerId);
//                        var Contact = ViewModel.ContactInfo;
//                        Contact.CustomerId = Customer.Id;
//                        Contact.CompanyId = companyId;
//                        await ContactDL.CreateContact(Contact, UserName);
//                        //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);

//                    }
//                    else if (ViewModel.ContactType == "NewCustomer")
//                    {
//                        var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                        var Customer = new Customer();
//                        Customer.CompanyId = companyId;
//                        Customer.Name = ViewModel.CustomerName;
//                        var CustomerId = await CustomerDL.CreateCustomer(Customer);
//                        var Contact = ViewModel.ContactInfo;
//                        Contact.CustomerId = CustomerId;
//                        Contact.CompanyId = companyId;
//                        await ContactDL.CreateContact(Contact, UserName);
//                    }

//                }
//                else if (ViewModel.SelectedContactType == "Vendor")
//                {
//                    if (ViewModel.VendorType == "ExistingVendor")
//                    {
//                        var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                        var Contact = ViewModel.ContactInfo;
//                        Contact.VendorId = ViewModel.VendorId;
//                        Contact.CompanyId = companyId;
//                        await ContactDL.CreateContact(Contact, UserName);
//                        //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);

//                    }
//                    else if (ViewModel.VendorType == "NewVendor")
//                    {
//                        var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                        var Vendor = new Vendor();
//                        Vendor.CompanyId = companyId;
//                        Vendor.Name = ViewModel.VendorName;
//                        var VendorId = await VendorDL.CreateVendor(Vendor, UserName);
//                        var Contact = ViewModel.ContactInfo;
//                        Contact.VendorId = VendorId;
//                        Contact.CompanyId = companyId;
//                        await ContactDL.CreateContact(Contact, UserName);
//                    }
//                }
//                else if (ViewModel.SelectedContactType == "Lead")
//                {
//                    var Id = ViewModel.Param;
//                    if (ViewModel.LeadType == "ExistingLead")
//                    {
//                        var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                        var Contact = ViewModel.ContactInfo;
//                        Contact.VendorId = ViewModel.LeadId;
//                        Contact.CompanyId = companyId;
//                        await ContactDL.CreateContact(Contact, UserName);
//                        //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);

//                    }
//                    else if (ViewModel.LeadType == "NewLead")
//                    {
//                        var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                        var Lead = new Lead();
//                        Lead.CompanyId = companyId;
//                        Lead.CompanyName = ViewModel.VendorName;
//                        var LeadId = await LeadDL.CreateLead(Lead, UserName);
//                        var Contact = ViewModel.ContactInfo;
//                        Contact.LeadId = LeadId;
//                        Contact.CompanyId = companyId;
//                        await ContactDL.CreateContact(Contact, UserName);
//                    }
//                }
//                else if (ViewModel.SelectedContactType == "Other")
//                {
//                    var companyId = Int32.Parse(User.Claims.Where(x => x.Type == "CompanyId").FirstOrDefault().Value);
//                    var Contact = ViewModel.ContactInfo;
//                    Contact.VendorId = ViewModel.LeadId;
//                    Contact.CompanyId = companyId;
//                    await ContactDL.CreateContact(Contact, UserName);
//                   // ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                }
              

//            }
//            if (Action == "Add CC Send Recipient")
//            {
//                var SendRecipient = new SendEmailRecipient();
//                SendRecipient.SendEmailId = ViewModel.SendEmail.Id;
//                if (ViewModel.CcCustomerContact != null)
//                {
//                    var CustomerContact = await CustomerDL.GetCustomerContact(Convert.ToInt32(ViewModel.CcCustomerContact));
//                    SendRecipient.Email = CustomerContact.Email;
//                }
//                else if (ViewModel.CcContact != null)
//                {
//                    var Contact = await ContactDL.GetContact(Convert.ToInt32(ViewModel.CcContact));
//                    SendRecipient.Email = Contact.Email;
//                }
//                else if (ViewModel.CCNewEmail != null)
//                {
//                    SendRecipient.Email = ViewModel.CCNewEmail;
//                }
//                SendRecipient.RecipientType = RecipientType.CC;
//                await UserEmailDL.CreateSendEmailRecipient(SendRecipient);
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                ViewModel.ShowNewEmail = true;
                

//            }
//            if (Action == "Create Task")
//            {
//                ViewModel.Task.CompanyId = CompanyId;
//                ViewModel.Task.EmailMessageId = ViewModel.Param;
//                await TaskDL.CreateTask(ViewModel.Task, UserName);
//                var message = await UserEmailDL.GetEmailMessageById(ViewModel.Param);
//                await TaskDL.CreateTask(ViewModel.Task, User.Identity.Name);
//                var emailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(message.CompanyEmailAccountId);
//                if (emailAccount.EmailType == CompanyEnums.EmailType.Gmail)
//                {
//                    string accessTokenGmail = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(emailAccount.RefreshToken);
//                    await GoogleAPI.MarkEmailAsRead(accessTokenGmail, message.MessageId);
//                    await UserEmailDL.MarkEmailAsRead(message.Id);
//                }
//                else if (emailAccount.EmailType == CompanyEnums.EmailType.Office365)
//                {
//                    string accessTokenOffice = await MicrosoftAPI.GetAccessTokenAsync(emailAccount.RefreshToken);
//                    var emails = await MicrosoftAPI.MarkEmailsAsReadAsync(accessTokenOffice, message.MessageId);
//                    await UserEmailDL.MarkEmailAsRead(message.Id);
//                }
//                var emailpreview = ViewModel.ShowEmailPreview;
//                var emailMessageId = ViewModel.EmailMessage.Id;
//                //ViewModel = await getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                if (emailpreview)
//                {
//                    ViewModel.EmailMessage = await UserEmailDL.GetEmailMessageById(emailMessageId);
//                    var EmailMessageView = await UserEmailDL.GetEmailMessageViewByEmailMessageId(emailMessageId);
//                    if (EmailMessageView.ContactId != null)
//                    {
//                        ViewModel.EmailMessage.IsContact = true;
//                    }
//                    if (EmailMessageView.CustomerEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsCustomer = true;
//                    }
//                    if (EmailMessageView.VendorEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsVendor = true;
//                    }
//                    if (EmailMessageView.LeadEmail == true)
//                    {
//                        ViewModel.EmailMessage.IsLead = true;
//                    }
//                    if (EmailMessageView.TasksId != null)
//                    {
//                        ViewModel.EmailMessage.IsTask = true;
//                        ViewModel.TaskName = EmailMessageView.Tasks.Name;
//                    }
//                    if (EmailMessageView.SendEmailId != null)
//                    {
//                        ViewModel.EmailMessage.HasDraft = true;
//                    }
//                    if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                    {
//                        string accessToken = await MicrosoftAPI.GetAccessTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                        //await MicrosoftAPI.MarkEmailsAsReadAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                        ViewModel.EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                    }
//                    else if (ViewModel.EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                    {
//                        string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(ViewModel.EmailMessage.CompanyEmailAccount.RefreshToken);
//                        //await GoogleAPI.MarkEmailAsRead(accessToken, ViewModel.EmailMessage.MessageId);
//                        ViewModel.EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, ViewModel.EmailMessage.MessageId);
//                    }

//                    //await UserEmailDL.MarkEmailAsRead(ViewModel.EmailMessage.Id);

//                    ViewModel.ShowEmailPreview = true;
                  
//                }

//            }
//            if (Action == "Remove Recipient")
//            {
//                await UserEmailDL.RemoveSendEmailRecipient(ViewModel.Param);
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                ViewModel.ShowNewEmail = true;
              
//            }
//            if (Action == "Show Add New Email")
//            {
//                var email = ViewModel.SendEmail;
//               // ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                //ViewModel.SelectedAccount = AccountId;
//                ViewModel.ShowAddNewEmail = true;
//                ViewModel.ShowNewEmail = true;
               
//            }
//            if (Action == "Show Add CC New Email")
//            {
//                var email = ViewModel.SendEmail;
//               // ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                //ViewModel.SelectedAccount = AccountId;
//                ViewModel.ShowAddCCNewEmail = true;
//                ViewModel.ShowNewEmail = true;
                
//            }
//            if (Action == "Show Add Other Contact")
//            {
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                //ViewModel.SelectedAccount = AccountId;
//                ViewModel.ShowAddOtherContact = true;
//                ViewModel.ShowNewEmail = true;
               
//            }
//            if (Action == "Show Add CC Other Contact")
//            {
//                var email = ViewModel.SendEmail;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                //ViewModel.SelectedAccount = AccountId;
//                ViewModel.ShowAddCCOtherContact = true;
//                ViewModel.ShowNewEmail = true;
              
//            }
//            if (Action == "Cancel")
//            {
//                var email = ViewModel.SendEmail;
//               // ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                email.SendEmailRecipients = draft.SendEmailRecipients;
//                ViewModel.SendEmail = email;
//                ViewModel.ShowAddOtherContact = false;
//                ViewModel.ShowAddNewEmail = false;
//                ViewModel.ShowAddCustomerContact = false;
//                ViewModel.ShowNewEmail = true;
               
//            }
//            if (Action == "Show Draft From Email")
//            {
//                var draft = await UserEmailDL.GetDraftByOriginalMessageId(ViewModel.Param);
//                ViewModel.Param = draft.Id;
//                Action = "Show Draft";
//                ViewModel.CurrentTab = "Drafts";

//            }
//            if (Action == "Show Draft")
//            {
//                var draft = await UserEmailDL.GetDraftById(ViewModel.Param);
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                ViewModel.ShowNewEmail = true;
//                ViewModel.SendEmail = draft;              

//            }
//            if (Action == "Save Changes")
//            {
//                var email = ViewModel.SendEmail;
//                email.CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(email.CompanyEmailAccount.Id);
//                await UserEmailDL.UpdateSendEmail(email);
//                var draft = await UserEmailDL.GetDraftById(email.Id);
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                ViewModel.ShowNewEmail = true;
//                ViewModel.SendEmail = draft;
              

//            }        
//            if (Action == "Delete Draft")
//            {
//                var DraftId = ViewModel.SendEmail.Id;
//                await UserEmailDL.DeleteDraft(DraftId);
//            }
//            if (Action == "Send Email")
//            {
//                var CompanyEmailAccount = await CompanyIntegrationDL.GetCompanyEmailAccount(ViewModel.SendEmail.CompanyEmailAccountId);
//                var draft = await UserEmailDL.GetDraftById(ViewModel.SendEmail.Id);
//                var Company = await CompanyDL.GetCompany(CompanyId);
//                var ReplyDateTime = StringFormating.CurrentTime();

//                ReplyDateTime = ReplyDateTime.AddHours((int)Company.CompanyTimeZone);

//                //if (Company.CompanyTimeZone == CompanyTimeZone.Eastern)
//                //{
//                //    ReplyDateTime = ReplyDateTime.AddHours(-1);
//                //}
//                //if (Company.CompanyTimeZone == CompanyTimeZone.Mountain)
//                //{
//                //    ReplyDateTime = ReplyDateTime.AddHours(1);
//                //}
//                //if (Company.CompanyTimeZone == CompanyTimeZone.Pacific)
//                //{
//                //    ReplyDateTime = ReplyDateTime.AddHours(2);

//                //}
//                if (draft.OriginalMessageId != 0)
//                {
//                    var EmailMessage = await UserEmailDL.GetEmailMessageById(draft.OriginalMessageId);
//                    EmailMessage.ReplySent = true;
//                    var user = await UserDL.GetCurrentUser(User.Identity.Name);
//                    EmailMessage.ReplySentBy = user.FullName;
//                    EmailMessage.ReplySentOn = ReplyDateTime;
//                    await UserEmailDL.UpdateEmailMessage(EmailMessage);

//                }
//                // Fetch the recipients
//                var toRecipients = draft.SendEmailRecipients
//                    .Where(r => r.RecipientType == RecipientType.TO)
//                    .Select(r => r.Email)
//                    .ToList();

//                var ccRecipients = draft.SendEmailRecipients
//                    .Where(r => r.RecipientType == RecipientType.CC)
//                    .Select(r => r.Email)
//                    .ToList();

//                var bccRecipients = draft.SendEmailRecipients
//                    .Where(r => r.RecipientType == RecipientType.BCC)
//                    .Select(r => r.Email)
//                    .ToList();

//                email.MicrosoftRefreshToken = CompanyEmailAccount.EmailType == CompanyEnums.EmailType.Office365 ? CompanyEmailAccount.RefreshToken : null;
//                email.GmailRefreshToken = CompanyEmailAccount.EmailType == CompanyEnums.EmailType.Gmail ? CompanyEmailAccount.RefreshToken : null;
//                email.EmailFrom = CompanyEmailAccount.Email;
//                email.EmailFromName = CompanyEmailAccount.Name;        
//                email.Message= ViewModel.SendEmail.Body;
//                email.Subject = ViewModel.SendEmail.Subject;
//                await email.SendMailList(toRecipients, ccRecipients, bccRecipients);
//                await UserEmailDL.DeleteDraft(ViewModel.SendEmail.Id);
//            }

//            if (Action == "Reply" || Action == "Reply All" || Action == "Forward")
//            {
//                var Id = ViewModel.Param;
//                //ViewModel = await userEmailController.getModel(ViewModel.CurrentTab, ViewModel.DateFrom, ViewModel.DateTo);
//                var EmailMessage = await UserEmailDL.GetEmailMessageById(Id);
//                var user = await UserDL.GetCurrentUser(UserName);


//                var EmailSignature = await UserEmailDL.GetUserSignatureByUserIdandEmailAccountId(user.Id, EmailMessage.CompanyEmailAccountId);
//                var Signature = "";
//                if (EmailSignature != null)
//                {
//                    Signature = EmailSignature.Signature;
//                }

//                var email = new SendEmail();
//                var from = EmailMessage.CompanyEmailAccount.Email;
//                var sent = EmailMessage.ReceivedDateTime.ToString("f");

//                var toRecipients = EmailMessage.EmailRecipients.Where(r => r.RecipientType == RecipientType.TO).Select(r => r.Email).ToList();
//                var ccRecipients = EmailMessage.EmailRecipients.Where(r => r.RecipientType == RecipientType.CC).Select(r => r.Email).ToList();
//                var bccRecipients = EmailMessage.EmailRecipients.Where(r => r.RecipientType == RecipientType.BCC).Select(r => r.Email).ToList();


//                var to = string.Join(", ", toRecipients);
//                var cc = string.Join(", ", ccRecipients);
//                var bcc = string.Join(", ", bccRecipients);

//                var subject = EmailMessage.Subject;

//                var replyBody = $@"
//                <br>
//                {Signature}
//                <p>From: {from}<br>
//                Sent: {sent}<br>
//                To: {to}<br>";
//                if (ccRecipients.Any())
//                {
//                    replyBody += $"CC: {cc}<br>";
//                }

//                if (bccRecipients.Any())
//                {
//                    replyBody += $"BCC: {bcc}<br>";
//                }

//                replyBody += $"Subject: {subject}</p><p></p><br>";

//                if (EmailMessage.CompanyEmailAccount.EmailType == EmailType.Office365)
//                {
//                    string accessToken = await MicrosoftAPI.GetAccessTokenAsync(EmailMessage.CompanyEmailAccount.RefreshToken);
//                    //await MicrosoftAPI.MarkEmailsAsReadAsync(accessToken, ViewModel.EmailMessage.MessageId);
//                    EmailMessage.Body = await MicrosoftAPI.FetchEmailBodyByIdAsync(accessToken, EmailMessage.MessageId);
//                }
//                else if (EmailMessage.CompanyEmailAccount.EmailType == EmailType.Gmail)
//                {
//                    string accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(EmailMessage.CompanyEmailAccount.RefreshToken);
//                    //await GoogleAPI.MarkEmailAsRead(accessToken, ViewModel.EmailMessage.MessageId);
//                    EmailMessage.Body = await GoogleAPI.GetEmailBody(accessToken, EmailMessage.MessageId);
//                }

//                replyBody += EmailMessage.Body;
//                email.CompanyEmailAccount = EmailMessage.CompanyEmailAccount;
//                email.CompanyEmailAccountId = EmailMessage.CompanyEmailAccount.Id;
//                email.Body = replyBody;

//                if (Action == "Forward")
//                {
//                    email.Subject = "FW <" + subject + ">";
//                }
//                else
//                {
//                    email.Subject = "RE <" + subject + ">";
//                }

//                email.SendEmailRecipients = new List<SendEmailRecipient>();
//                if (Action != "Forward")
//                {
//                    var SendEmailRecipient = new SendEmailRecipient();
//                    SendEmailRecipient.Email = EmailMessage.SenderEmail;
//                    SendEmailRecipient.RecipientType = RecipientType.TO;
//                    email.SendEmailRecipients.Add(SendEmailRecipient);


//                    if (Action == "Reply All")
//                    {
//                        email.SendEmailRecipients.AddRange(
//                            EmailMessage.EmailRecipients
//                                .Select(r => new SendEmailRecipient { Email = r.Email, RecipientType = r.RecipientType })
//                        );
//                        var recipientsToRemove = email.SendEmailRecipients.Where(r => r.Email == EmailMessage.CompanyEmailAccount.Email).FirstOrDefault();
//                        email.SendEmailRecipients.Remove(recipientsToRemove);
//                    }

//                }
//                email.OriginalMessageId = EmailMessage.Id;
//                var SendEmail = await UserEmailDL.CreateNewSendEmail(email);
//                ViewModel.ShowNewEmail = true;
//                ViewModel.SendEmail = SendEmail;
            

//            }
           

//            if (Action == "Select All")
//            {
//                foreach (var mail in ViewModel.EmailMessageView)
//                    mail.EmailMessage.IsChecked = true;
//            }

          
//            return ViewModel;

          
//        }
//    }
//}