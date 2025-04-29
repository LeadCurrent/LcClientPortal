using Azure.Core;
using CommonClasses;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CommonClasses
{
    public class Email
    {

        SendGridClient? client;
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string? EmailTo { get; set; }
        public string? EmailToName { get; set; }
        public string? EmailFrom { get; set; }
        public string? EmailFromName { get; set; }
        public string? GmailRefreshToken { get; set; }
        public string? MicrosoftRefreshToken { get; set; }
        public List<string>? ToEmailList { get; set; }
        public List<string>? CCEmailList { get; set; }
        //public Email()
        //{
        //    client = new SendGridClient(Environment.SendGridAPIKey);
        //}
        public async Task<bool> SendMail()
        {
            string testEmail = Environment.TestEmail;
            bool sendTestEmail = Environment.SendTestEmail;

            if (!Environment.environment.Contains("Prod") && Environment.SendTestEmail)
            {
                EmailTo = testEmail;
            }
            if (GmailRefreshToken != null)
            {
                if (EmailFrom != null && Subject != null && Message != null && EmailFromName != null)
                {
                    var accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(GmailRefreshToken);

                    if (ToEmailList != null && ToEmailList.Any())
                        return GoogleAPI.SendEmail(EmailFromName, EmailFrom, ToEmailList, Subject, Message, accessToken);
                    else if (EmailTo != null)
                    {
                        ToEmailList = new List<string> { (string)EmailTo };
                        return GoogleAPI.SendEmail(EmailFromName, EmailFrom, ToEmailList, Subject, Message, accessToken);
                    }
                }
                return false;
            }
            else if (MicrosoftRefreshToken != null)
            {
                if (Subject != null && Message != null)
                {
                    var accessToken = await MicrosoftAPI.GetAccessTokenAsync(MicrosoftRefreshToken);
                    if (ToEmailList != null && ToEmailList.Any())
                        return await MicrosoftAPI.SendEmail(ToEmailList, Subject, Message, accessToken);
                    else if (EmailTo != null)
                    {
                        ToEmailList = new List<string> { EmailTo };
                        return await MicrosoftAPI.SendEmail(ToEmailList, Subject, Message, accessToken);
                    }
                }
                return false;
            }
            else
            {
                try
                {
                    var HTML = "<div>" + Message + "</div>";
                    var client = new SendGridClient(Environment.SendGridAPIKey);
                    var from = new EmailAddress(EmailFrom, EmailFromName);
                    var subject = Subject;
                    var plainTextContent = "";

                    if (ToEmailList != null && ToEmailList.Any())
                    {
                        foreach (var email in ToEmailList)
                        {
                            var to = new EmailAddress(email, email);
                            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, HTML);
                            if (Environment.SendNotifications())
                                await client.SendEmailAsync(msg);
                        }
                    }
                    else
                    {
                        var to = new EmailAddress(EmailTo, EmailToName);
                        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, HTML);
                        if (Environment.SendNotifications())
                            await client.SendEmailAsync(msg);
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }
        }
        public async Task<bool> SendMailList(List<string> toEmails, List<string> ccEmails, List<string> bccEmails)
        {

            if (!Environment.environment.Contains("Prod") && Environment.SendTestEmail)
            {
                toEmails = new List<string> { Environment.TestEmail };
                ccEmails = new List<string>();
                bccEmails = new List<string>();
            }
            if (GmailRefreshToken != null)
            {
                if (EmailTo != null && EmailFrom != null && Subject != null && Message != null && EmailFromName != null)
                {
                    var accessToken = await GoogleAPI.GetAccessTokenFromRefreshTokenAsync(GmailRefreshToken);
                    return GoogleAPI.SendEmailList(EmailFromName, EmailFrom, toEmails, ccEmails, bccEmails, Subject, Message, accessToken);
                }
                else return false;
            }
            else if (MicrosoftRefreshToken != null)
            {
                if (EmailTo != null && EmailFrom != null && Subject != null && Message != null && EmailFromName != null)
                {
                    var accessToken = await MicrosoftAPI.GetAccessTokenAsync(MicrosoftRefreshToken);
                    return await MicrosoftAPI.SendEmailList(EmailFromName, EmailFrom, toEmails, ccEmails, bccEmails, Subject, Message, accessToken);
                }
                else return false;
            }
            else
            {
                try
                {
                    var client = new SendGridClient(Environment.SendGridAPIKey);
                    var from = new EmailAddress(EmailFrom, EmailFromName);
                    var htmlContent = "<div>" + Message + "</div>";

                    var tasks = new List<Task<Response>>();

                    foreach (var toEmail in toEmails)
                    {
                        var to = new EmailAddress(toEmail, toEmail);
                        var msg = MailHelper.CreateSingleEmail(from, to, Subject, "", htmlContent);
                        tasks.Add(client.SendEmailAsync(msg));
                    }

                    await Task.WhenAll(tasks);

                    return true;


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }

        }

    }
}