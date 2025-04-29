
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Globalization;
using static CommonClasses.Enums;
using System.Runtime.CompilerServices;


namespace CommonClasses
{
    public static class MicrosoftAPI
    {

        private static string? ClientId;
        private static string? ClientSecret;
        private static string? Authority;
        private static string? TenantId;
        private static string[] Scopes = new string[] { "User.Read", "Mail.Send", "Mail.Read", "Mail.Read.Shared", "Mail.ReadBasic.Shared", "Mail.ReadWrite", "Mail.ReadWrite.Shared", "Mail.ReadBasic", "offline_access", "email", "openid", "profile", "Calendars.Read", "Calendars.Read.Shared", "Calendars.ReadWrite", "Calendars.ReadWrite.Shared" };
        private const string TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

        public static void Initialize()
        {
            ClientId = Environment.MicrosoftClientId();
            TenantId = Environment.TenantId;
            ClientSecret = Environment.MicrosoftClientSecret();
            Authority = $"https://login.microsoftonline.com/common/v2.0";
        }
        public static async Task<string> GetAuthorizationUrl(string redirectUri)
        {
            var pca = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithClientSecret(ClientSecret)
                .WithAuthority(new Uri(Authority ?? string.Empty))
                .Build();

            var authUrlBuilder = pca.GetAuthorizationRequestUrl(Scopes)
                .WithRedirectUri(redirectUri);

            var result = await authUrlBuilder.ExecuteAsync();
            return result.AbsoluteUri;
        }
        public static async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            var pca = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithClientSecret(ClientSecret)
                .WithAuthority(new Uri(Authority ?? string.Empty))
                .WithRedirectUri(redirectUri) // Set the redirect URI here
                .Build();

            var result = await pca.AcquireTokenByAuthorizationCode(Scopes, code)
                .ExecuteAsync();

            return result.AccessToken;
        }


        public static async Task<string> GetAccessTokenAndRefreshFromCodeTokenAsync(string code, string redirectUri)
        {
            try
            {
                string refreshtoken = "";

                var parameters = new Dictionary<string, string>
                {
                    { "client_id", ClientId ?? String.Empty },
                    { "client_secret", ClientSecret ?? String.Empty },
                    { "code", code },
                    { "redirect_uri", redirectUri },
                    { "grant_type", "authorization_code" }
                };

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(TokenEndpoint, new FormUrlEncodedContent(parameters));
                    response.EnsureSuccessStatusCode();
                    var tokenResponseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponseObject = JObject.Parse(tokenResponseContent);
                    string? refreshtokentemp = tokenResponseObject?["refresh_token"]?.ToString();
                    if (refreshtokentemp != null)
                        return refreshtokentemp;
                }
                return refreshtoken;
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception here
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                throw; // Re-throw the exception to propagate it further
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception to propagate it further
            }

        }

        public static async Task<string> GetAccessTokenAsync(string _refreshToken)
        {
            try
            {
                string accessToken = "";

                var parameters = new Dictionary<string, string>
                {
                    { "client_id", ClientId ?? String.Empty },
                    { "client_secret", ClientSecret ?? String.Empty },
                    { "refresh_token", _refreshToken },
                    { "grant_type", "refresh_token" },
                    { "scope", string.Join(" ", Scopes) }
                };
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(TokenEndpoint, new FormUrlEncodedContent(parameters));
                    response.EnsureSuccessStatusCode();
                    var tokenResponseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponseObject = new JObject();
                    tokenResponseObject = JObject.Parse(tokenResponseContent);
                    string? Token = tokenResponseObject["access_token"]?.ToString();
                    if (Token != null)
                        accessToken = Token;

                }
                return accessToken;
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception here
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                throw; // Re-throw the exception to propagate it further
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception to propagate it further
            }
        }

        public static async Task<UserInfo> GetUserEmailAsync(string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userJson = JObject.Parse(content);

                    string userEmail = "";
                    string userName = "";


                    string? userEmailTemp = userJson["mail"]?.ToString();
                    string? userNameTemp = userJson["displayName"]?.ToString();

                    if (userEmailTemp != null)
                        userEmail = userEmailTemp;


                    if (userNameTemp != null)
                        userName = userNameTemp;

                    return new UserInfo
                    {
                        Email = userEmail,
                        Name = userName
                    };
                }
                else
                {
                    // Handle error response
                    throw new HttpRequestException($"Failed to retrieve user's email address. StatusCode: {response.StatusCode}");
                }
            }
        }

        public static async Task<bool> SendEmail(List<string> toList, string Subject, string Message, string accessToken)
        {
            try
            {
                

                // Create email message object
                var emailMessage = new
                {
                    message = new
                    {
                        subject = Subject,
                        body = new
                        {
                            contentType = "HTML", // Set the content type to HTML
                            content = Message // Set the HTML content
                        },
                        toRecipients = toList.Select(toEmail => new { emailAddress = new { address = toEmail } }).ToArray()
                    }
                };

                // Serialize email message to JSON
                var jsonContent = JsonConvert.SerializeObject(emailMessage);

                // Send email using Microsoft Graph API
                using (var httpClient = new HttpClient())
                {
                    // Add access token to request headers
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Define API endpoint
                    string apiUrl = "https://graph.microsoft.com/v1.0/me/sendMail";

                    // Create HTTP content from JSON
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send POST request to sendMail endpoint
                    var response = await httpClient.PostAsync(apiUrl, httpContent);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Email sent successfully
                        return true;
                    }
                    else
                    {
                        // Log error or handle failure appropriately
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> SendEmailList(string fromEmailDisplay, string fromEmail, List<string> toList, List<string> ccList, List<string> bccList, string subject, string message, string accessToken)
        {
            try
            {
                // Create email message object
                var emailMessage = new
                {
                    message = new
                    {
                        subject = subject,
                        body = new
                        {
                            contentType = "HTML", // Set the content type to HTML
                            content = message // Set the HTML content
                        },
                        toRecipients = toList.Select(toEmail => new { emailAddress = new { address = toEmail } }).ToArray(),
                        ccRecipients = ccList?.Select(ccEmail => new { emailAddress = new { address = ccEmail } }).ToArray(),
                        bccRecipients = bccList?.Select(bccEmail => new { emailAddress = new { address = bccEmail } }).ToArray()
                    }
                };

                // Serialize email message to JSON
                var jsonContent = JsonConvert.SerializeObject(emailMessage);

                // Send email using Microsoft Graph API
                using (var httpClient = new HttpClient())
                {
                    // Add access token to request headers
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Define API endpoint
                    string apiUrl = "https://graph.microsoft.com/v1.0/me/sendMail";

                    // Create HTTP content from JSON
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send POST request to sendMail endpoint
                    var response = await httpClient.PostAsync(apiUrl, httpContent);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Email sent successfully
                        return true;
                    }
                    else
                    {
                        // Log error or handle failure appropriately
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        //private async Task<string> GetUserPrincipleAsync(string accessToken)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me");
        //        response.EnsureSuccessStatusCode();

        //        var content = await response.Content.ReadAsStringAsync();
        //        var user = JsonConvert.DeserializeObject<User>(content);

        //        return user.Id;
        //    }
        //}

        // Define classes for deserialization

        #region Fetch Email

        //public static async Task<List<EmailModel>> FetchEmailsAsync(string accessToken)
        //{
        //    List<EmailModel> emails = new List<EmailModel>();

        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //        var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me/messages");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            var json = JObject.Parse(content);

        //            foreach (var item in json["value"])
        //            {
        //                var email = new EmailModel
        //                {
        //                    Id = item["id"].ToString(),
        //                    //CompanyEmailAccountId = companyEmailAccountId,
        //                    CreatedDateTime = item["createdDateTime"].ToObject<DateTime?>(),
        //                    ReceivedDateTime = item["receivedDateTime"].ToObject<DateTime?>(),
        //                    Subject = item["subject"].ToString(),
        //                   // Body = item["body"]["content"].ToString(),
        //                    SenderName = item["sender"]["emailAddress"]["name"].ToString(),
        //                    SenderEmail = item["sender"]["emailAddress"]["address"].ToString(),
        //                    //ToRecipients = string.Join(", ", item["toRecipients"].Select(r => r["emailAddress"]["address"].ToString())),
        //                    //CcRecipients = string.Join(", ", item["ccRecipients"].Select(r => r["emailAddress"]["address"].ToString())),
        //                    //BccRecipients = string.Join(", ", item["bccRecipients"].Select(r => r["emailAddress"]["address"].ToString())),
        //                    HasAttachments = item["hasAttachments"].ToObject<bool>(),
        //                    Importance = item["importance"].ToString(),
        //                    IsRead = item["isRead"].ToObject<bool>(),
        //                    IsDraft = item["isDraft"].ToObject<bool>(),
        //                };

        //                emails.Add(email);
        //            }
        //        }
        //        else
        //        {
        //            throw new HttpRequestException($"Failed to retrieve emails. StatusCode: {response.StatusCode}");
        //        }
        //    }

        //    return emails;
        //}

        public static async Task<List<CommonEmailMessage>> FetchEmailsAsync(string accessToken, DateTime afterTimestamp, CompanyTimeZone CompanyTimeZone)
        {
            var emails = new List<CommonEmailMessage>();
            string apiUrl = $"https://graph.microsoft.com/v1.0/me/messages?$filter=receivedDateTime ge {afterTimestamp.ToString("yyyy-MM-ddTHH:mm:ssZ")}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                while (!string.IsNullOrEmpty(apiUrl))
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to fetch emails. StatusCode: {response.StatusCode}");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var messages = JObject.Parse(content);

                    foreach (var item in messages["value"] ?? new JArray())
                    {
                        var createdDateTime = ParseDateTime(item["createdDateTime"]?.ToString() ?? String.Empty);
                        var receivedDateTime = ParseDateTime(item["receivedDateTime"]?.ToString() ?? String.Empty);
                        var downloadDateTime = DateTime.UtcNow;

                        createdDateTime = createdDateTime?.AddHours((int)CompanyTimeZone);
                        receivedDateTime = receivedDateTime?.AddHours((int)CompanyTimeZone);
                        downloadDateTime = downloadDateTime.AddHours((int)CompanyTimeZone);

                        var email = new CommonEmailMessage
                        {
                            MessageId = item["id"]?.ToString() ?? string.Empty,
                            //CompanyEmailAccountId = CompanyEmailAccountId,
                            ReceivedDateTime = receivedDateTime ?? DateTime.MinValue,
                            Subject = item["subject"]?.ToString() ?? string.Empty,
                            SenderName = item["sender"]?["emailAddress"]?["name"]?.ToString() ?? string.Empty,
                            SenderEmail = item["sender"]?["emailAddress"]?["address"]?.ToString() ?? string.Empty,
                            HasAttachments = item["hasAttachments"]?.ToObject<bool>() ?? false,
                            Importance = item["importance"]?.ToString() ?? "normal",
                            IsRead = item["isRead"]?.ToObject<bool>() ?? false,
                            IsDraft = item["isDraft"]?.ToObject<bool>() ?? false,
                            InternetMessageId = item["internetMessageId"]?.ToString() ?? string.Empty,
                            DownloadDateTime = downloadDateTime,
                            EmailRecipients = ParseRecipients(item)
                        };


                        emails.Add(email);
                    }

                    apiUrl = messages["@odata.nextLink"]?.ToString() ?? String.Empty;
                }
            }

            return emails;
        }
        private static void AddRecipients(List<CommonEmailRecipientHelper> recipients, JToken recipientItems, RecipientType recipientType)
        {
            if (recipientItems == null) return;

            foreach (var recipient in recipientItems)
            {
                var emailAddress = recipient?["emailAddress"];
                if (emailAddress != null)
                {
                    recipients.Add(new CommonEmailRecipientHelper
                    {
                        RecipientType = recipientType,
                        Name = emailAddress["name"]?.ToString(),
                        Email = emailAddress["address"]?.ToString()
                    });
                }
            }
        }
        private static List<CommonEmailRecipientHelper> ParseRecipients(JToken item)
        {
            var recipients = new List<CommonEmailRecipientHelper>();

            if (item["toRecipients"] != null)
                AddRecipients(recipients, item["toRecipients"] ?? string.Empty, RecipientType.TO);
            if (item["ccRecipients"] != null)
                AddRecipients(recipients, item["ccRecipients"] ?? string.Empty, RecipientType.CC);
            if (item["bccRecipients"] != null)
                AddRecipients(recipients, item["bccRecipients"] ?? string.Empty, RecipientType.BCC);

            return recipients;
        }

        private static DateTime? ParseDateTime(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
                return null;

            DateTime dateTime;
            if (DateTime.TryParse(dateTimeString, null, DateTimeStyles.AdjustToUniversal, out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        public static async Task<string> FetchEmailBodyByIdAsync(string accessToken, string messageId)
        {
            try
            {
                string apiUrl = $"https://graph.microsoft.com/v1.0/me/messages/{messageId}";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var response = await httpClient.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to fetch email body. StatusCode: {response.StatusCode}");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var message = JObject.Parse(content);

                    var body = message["body"]?["content"]?.ToString();

                    return body ?? String.Empty;
                }
            }
            catch
            {
                return "Email Message No Longer Available";
            }
        }

        public static async Task<string> GetEmailBodyWithEmbeddedImagesAsync(string messageId, string accessToken)
        {
            string requestUrl = $"https://graph.microsoft.com/v1.0/me/messages/{messageId}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var email = JsonConvert.DeserializeObject<dynamic>(jsonResponse) ?? new object();

                    string emailBody = email.body.content;
                    string contentType = email.body.contentType;

                    var attachments = await GetEmailAttachmentsAsync(messageId, accessToken);

                    foreach (var attachment in attachments)
                    {
                        if (attachment.IsInline && !string.IsNullOrEmpty(attachment.ContentId) && attachment.ContentBytes != null)
                        {
                            string base64Content = Convert.ToBase64String(Convert.FromBase64String(attachment.ContentBytes));
                            emailBody = emailBody.Replace($"cid:{attachment.ContentId}", $"data:{attachment.ContentType};base64,{base64Content}");
                        }
                    }

                    return emailBody;
                }
                else
                {
                    throw new Exception($"Failed to fetch email with ID {messageId}: {response.ReasonPhrase}");
                }
            }
        }
        public static async Task<List<CommonClasses.Attachment>> GetEmailAttachmentsAsync(string messageId, string accessToken)
        {
            List<CommonClasses.Attachment> attachments = new List<CommonClasses.Attachment>();
            string requestUrl = $"https://graph.microsoft.com/v1.0/me/messages/{messageId}/attachments";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse) ?? new object();

                    var attachmentResponse = JsonConvert.DeserializeObject<AttachmentResponse>(jsonResponse) ?? new AttachmentResponse();

                    // Manually map each MicrosoftAPI.Attachment to Web.Attachment
                    if (attachmentResponse != null)
                        if (attachmentResponse.Value != null)
                            attachments = attachmentResponse.Value.Select(a => new CommonClasses.Attachment
                        {
                            AttachmentId = a.Id,
                            FileName = a.Name,
                            MimeType = a.ContentType,
                            ContentBytes = a.ContentBytes,
                            Data = Convert.FromBase64String(a.ContentBytes ?? String.Empty),// or another property if needed
                            IsInline = a.IsInline,
                            ContentId = a.ContentId
                        }).ToList();
                }
            }

            return attachments;
        }



        #endregion

        #region Modify Email

        public static async Task<bool> MarkEmailsAsReadAsync(string accessToken, string emailId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                    string apiUrl = $"https://graph.microsoft.com/v1.0/me/messages/{emailId}";
                    var updateData = new { isRead = true };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

                    var response = await httpClient.PatchAsync(apiUrl, jsonContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to mark email as read. StatusCode: {response.StatusCode}");
                    }


                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> MarkEmailsAsUnreadAsync(string accessToken, string emailId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    string apiUrl = $"https://graph.microsoft.com/v1.0/me/messages/{emailId}";
                    var updateData = new { isRead = false };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

                    var response = await httpClient.PatchAsync(apiUrl, jsonContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to mark email as unread. StatusCode: {response.StatusCode}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> DeleteEmailsAsync(string accessToken, string emailId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    string apiUrl = $"https://graph.microsoft.com/v1.0/me/messages/{emailId}";

                    var response = await httpClient.DeleteAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to delete email. StatusCode: {response.StatusCode}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        #endregion
        #region Calendar        
        public static async Task<List<CalendarEvent>> FetchMicrosoftCalendarEventsAsync(string accessToken, DateTime startDate, DateTime endDate, int EmailAccountId,CompanyTimeZone CompanyTimeZone,string CalendarColor)
        {
            var calendarEventList = new List<CalendarEvent>();

            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Initial Microsoft Graph Calendar API endpoint
                var url = $"https://graph.microsoft.com/v1.0/me/calendar/events?" +
                          $"$filter=start/dateTime ge '{startDate:O}' and end/dateTime le '{endDate:O}'" +
                          $"&$orderby=start/dateTime";

                while (!string.IsNullOrEmpty(url))
                {
                    // Make the HTTP GET request
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Parse the response content
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);

                    // Extract events
                    var events = json["value"]?.ToObject<List<JObject>>();
                    if (events != null)
                    {
                        foreach (var microsoftEvent in events)
                        {
                           
                            // Map the event data to a CalendarEvent object
                            var startTimeUtc = DateTime.Parse(microsoftEvent["start"]?["dateTime"]?.ToString() ?? DateTime.MinValue.ToString());
                            var endTimeUtc = DateTime.Parse(microsoftEvent["end"]?["dateTime"]?.ToString() ?? DateTime.MinValue.ToString());

                            var startTime = StringFormating.ConvertUTCToTimeZone(startTimeUtc, CompanyTimeZone);
                            var endTime = StringFormating.ConvertUTCToTimeZone(endTimeUtc, CompanyTimeZone);
                            var calendarEvent = new CalendarEvent
                            {
                                Id = microsoftEvent["id"]?.ToString(),
                                Title = microsoftEvent["subject"]?.ToString() ?? "No Title",
                                StartTime = startTime,
                                StartTime_Hour = startTime.Hour > 12 ? startTime.Hour - 12 : startTime.Hour,
                                StartTime_Minute = startTime.Minute.ToString("D2"),
                                StartTime_AMPM = startTime.Hour >= 12 ? "PM" : "AM",
                                EndTime = endTime,
                                EndTime_Hour = endTime.Hour > 12 ? endTime.Hour - 12 : endTime.Hour,
                                EndTime_Minute = endTime.Minute.ToString("D2"),
                                EndTime_AMPM = endTime.Hour >= 12 ? "PM" : "AM",
                                Description = microsoftEvent["bodyPreview"]?.ToString(),
                                Location = microsoftEvent["location"]?["displayName"]?.ToString(),
                                Organizer = microsoftEvent["organizer"]?["emailAddress"]?["address"]?.ToString(),
                                Attendees = microsoftEvent["attendees"]?.Select(a => a["emailAddress"]?["address"]?.ToString()).OfType<string>().ToList() ?? new List<string>(),
                                HtmlLink = microsoftEvent["webLink"]?.ToString(),
                                EmailAccountId = EmailAccountId,
                            };
                            if (CalendarColor != null)
                                calendarEvent.CalendarColor = CalendarColor;
                            calendarEventList.Add(calendarEvent);
                        }
                    }

                    // Check for the @odata.nextLink property
                    url = json["@odata.nextLink"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Microsoft Calendar events: {ex.Message}");
            }

            return calendarEventList;
        }
        public static async Task<string> CreateOutlookCalendarEventAsync(string accessToken, string subject, DateTime startDateTime, DateTime endDateTime, string timeZone, string location, string bodyContent, List<string> attendeesEmails)
        {
            string url = "https://graph.microsoft.com/v1.0/me/events";

            using (var client = new HttpClient())
            {
                // Set the authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Create the request body
                var eventData = new
                {
                    subject = subject,
                    start = new
                    {
                        dateTime = startDateTime.ToString("o"), // ISO 8601 format
                        timeZone = timeZone
                    },
                    end = new
                    {
                        dateTime = endDateTime.ToString("o"), // ISO 8601 format
                        timeZone = timeZone
                    },
                    location = new
                    {
                        displayName = location
                    },
                    body = new
                    {
                        contentType = "HTML",
                        content = bodyContent
                    },
                    attendees = attendeesEmails?.ConvertAll(email => new
                    {
                        emailAddress = new { address = email },
                        type = "required"
                    })
                };

                // Serialize the data to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(eventData), Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync(url, jsonContent);

                // Process the response
                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    return $"Event created successfully: {resultContent}";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Failed to create event. Status code: {response.StatusCode}. Error: {errorContent}";
                }
            }
        }
        public static async Task<CalendarEvent?> FetchMicrosoftCalendarEventByIdAsync(string accessToken, string eventId,CompanyTimeZone CompanyTimeZone)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Microsoft Graph API endpoint for a specific calendar event
                var url = $"https://graph.microsoft.com/v1.0/me/calendar/events/{eventId}";

                // Make the HTTP GET request
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Parse the response content
                var content = await response.Content.ReadAsStringAsync();
                var microsoftEvent = JObject.Parse(content);

                // Map the event data to a CalendarEvent object
                var startTimeUtc = DateTime.Parse(microsoftEvent["start"]?["dateTime"]?.ToString() ?? DateTime.MinValue.ToString());
                var endTimeUtc = DateTime.Parse(microsoftEvent["end"]?["dateTime"]?.ToString() ?? DateTime.MinValue.ToString());

                var startTime = StringFormating.ConvertUTCToTimeZone(startTimeUtc, CompanyTimeZone);
                var endTime = StringFormating.ConvertUTCToTimeZone(endTimeUtc, CompanyTimeZone);
                var calendarEvent = new CalendarEvent
                {
                    Id = microsoftEvent["id"]?.ToString(),
                    Title = microsoftEvent["subject"]?.ToString() ?? "No Title",
                    StartTime = startTime,
                    StartTime_Hour = startTime.Hour > 12 ? startTime.Hour - 12 : startTime.Hour,
                    StartTime_Minute = startTime.Minute.ToString("D2"),
                    StartTime_AMPM = startTime.Hour >= 12 ? "PM" : "AM",
                    EndTime = endTime,
                    EndTime_Hour = endTime.Hour > 12 ? endTime.Hour - 12 : endTime.Hour,
                    EndTime_Minute = endTime.Minute.ToString("D2"),
                    EndTime_AMPM = endTime.Hour >= 12 ? "PM" : "AM",
                    Description = microsoftEvent["bodyPreview"]?.ToString(),
                    Location = microsoftEvent["location"]?["displayName"]?.ToString(),
                    Organizer = microsoftEvent["organizer"]?["emailAddress"]?["address"]?.ToString(),
                    Attendees = microsoftEvent["attendees"]?.Select(a => a["emailAddress"]?["address"]?.ToString()).OfType<string>().ToList() ?? new List<string>(),
                    HtmlLink = microsoftEvent["webLink"]?.ToString()
                };

                return calendarEvent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Microsoft Calendar event: {ex.Message}");
                return null; // Return null or handle errors as appropriate for your application
            }
        }
        public static async Task<string> UpdateOutlookCalendarEventAsync(string accessToken, string eventId, string subject, DateTime startDateTime, DateTime endDateTime, string timeZone, string location,
          string bodyContent, List<string> attendeesEmails)
        {
            string url = $"https://graph.microsoft.com/v1.0/me/events/{eventId}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var eventData = new
                {
                    subject = subject,
                    start = new
                    {
                        dateTime = startDateTime.ToString("o"), // ISO 8601 format
                        timeZone = timeZone
                    },
                    end = new
                    {
                        dateTime = endDateTime.ToString("o"), // ISO 8601 format
                        timeZone = timeZone
                    },
                    location = new
                    {
                        displayName = location
                    },
                    body = new
                    {
                        contentType = "HTML",
                        content = bodyContent
                    },
                    attendees = attendeesEmails?.ConvertAll(email => new
                    {
                        emailAddress = new { address = email },
                        type = "required"
                    })
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(eventData), Encoding.UTF8, "application/json");

                var response = await client.PatchAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    return $"Event updated successfully: {resultContent}";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Failed to update event. Status code: {response.StatusCode}. Error: {errorContent}";
                }
            }
        }
        public static async Task<string> DeleteOutlookCalendarEventAsync(string accessToken, string eventId)
        {
            string url = $"https://graph.microsoft.com/v1.0/me/events/{eventId}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return $"Outlook Calendar event with ID {eventId} deleted successfully.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Failed to delete Outlook Calendar event. Status code: {response.StatusCode}. Error: {errorContent}";
                }
            }
        }

        #endregion
        //public class EmailRecipient
        //{
        //    public int Id { get; set; }
        //    public int EmailMessageId { get; set; }
        //    public CommonEmailMessage EmailMessage { get; set; }
        //    public RecipientType RecipientType { get; set; }
        //    public string Name { get; set; }
        //    public string Email { get; set; }
        //}
        public class AttachmentResponse
        {
            [JsonProperty("value")]
            public List<Attachment>? Value { get; set; }
        }
        //public class CommonEmailMessage
        //{
        //    public int Id { get; set; }
        //    public int CompanyEmailAccountId { get; set; }
        //    //public CompanyEmailAccount CompanyEmailAccount { get; set; }
        //    public DateTime ReceivedDateTime { get; set; }
        //    public DateTime DownloadDateTime { get; set; }
        //    public string MessageId { get; set; }
        //    public string Subject { get; set; }
        //    public string ShortSubject
        //    {
        //        get
        //        {
        //            if (Subject != null)
        //            {
        //                if (Subject.Length > 60)
        //                    return Subject.Substring(0, 60) + "...";
        //            }
        //            return Subject;
        //        }
        //    }
        //    public string SenderName { get; set; }
        //    public string SenderEmail { get; set; }
        //    public bool HasAttachments { get; set; }
        //    public string Importance { get; set; }
        //    public bool IsRead { get; set; }
        //    public bool IsDraft { get; set; }
        //    public string InternetMessageId { get; set; }

        //    public bool ReplySent { get; set; }
        //    public string ReplySentBy { get; set; }
        //    public DateTime? ReplySentOn { get; set; }

        //    [NotMapped]
        //    public bool IsChecked { get; set; }
        //    [NotMapped]
        //    public bool IsCustomer { get; set; }
        //    [NotMapped]
        //    public bool IsContact { get; set; }
        //    [NotMapped]
        //    public bool IsVendor { get; set; }
        //    [NotMapped]
        //    public bool IsLead { get; set; }
        //    [NotMapped]
        //    public bool IsTask { get; set; }
        //    [NotMapped]
        //    public bool HasDraft { get; set; }
        //    [NotMapped]
        //    public string Body { get; set; }
        //    public List<EmailRecipient> EmailRecipients { get; set; }

        //}
        public class Attachment
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public string? ContentType { get; set; }
            public long Size { get; set; }
            public string? ContentId { get; set; }
            public bool IsInline { get; set; }
            public string? ContentBytes { get; set; }
            public string? FileName { get; set; }     // Name of the file
            public string? MimeType { get; set; }     // MIME type of the file (e.g., "application/pdf", "image/jpeg")
            public byte[]? Data { get; set; }

        }
        public class MessageCollection
        {
            public List<Message>? Value { get; set; }
        }
        public class Message
        {
            public string? Id { get; set; }
            public DateTime? CreatedDateTime { get; set; }
            public DateTime? ReceivedDateTime { get; set; }
            public string? Subject { get; set; }
            public string? SenderName { get; set; }
            public string? SenderEmail { get; set; }
            public List<MessageRecipient>? ToRecipients { get; set; }
            public bool HasAttachments { get; set; }
            public string? Importance { get; set; }
            public bool IsRead { get; set; }
            public bool IsDraft { get; set; }
            public string? InternetMessageId { get; set; }
        }
        public class MessageRecipient
        {
            public string? EmailAddress { get; set; }
        }
        public class TokenResult
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
            public DateTimeOffset Expiration { get; set; }
        }
        private class TokenResponse
        {
            public string? FinalAccessToken { get; set; }
            public string? TokenType { get; set; }
            public int ExpiresIn { get; set; }
        }
        public class UserInfo
        {
            public string? Email { get; set; }
            public string? Name { get; set; }
        }

    }
}