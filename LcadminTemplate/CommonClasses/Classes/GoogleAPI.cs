using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using MimeKit;
using System.Text;
using System.Globalization;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using static CommonClasses.Enums;

namespace CommonClasses
{
    public static class GoogleAPI
    {

        private static string? ClientId;
        private static string? ClientSecret;
        private static string? ApplicationName;
        private static readonly string[] Scopes = {
            "https://www.googleapis.com/auth/gmail.send",
            "https://www.googleapis.com/auth/userinfo.email",
            "https://www.googleapis.com/auth/gmail.compose",
            "https://www.googleapis.com/auth/gmail.modify",
            "https://www.googleapis.com/auth/gmail.readonly",
            "https://www.googleapis.com/auth/calendar.events"
        };

        public static void Initialize()
        {

            ClientId = Environment.GoogleClientId();
            ApplicationName = Environment.GoogleApplicationName();
            ClientSecret = Environment.GoogleClientSecret();
        }

        public static string GetAuthorizationUrl(string redirectUri, string state)
        {
            // Generate Google OAuth 2.0 authorization URL
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
                Scopes = Scopes,
                DataStore = new FileDataStore("Google.Apis.Auth.OAuth2.Responses.TokenResponse")
            });

            var authorizationUrl = flow.CreateAuthorizationCodeRequest(redirectUri);
            authorizationUrl.State = state;
            // Append prompt=consent parameter manually
            string consentUrl = authorizationUrl.Build().ToString();
            //consentUrl += "&prompt=select_account";
            consentUrl += "&prompt=consent";

            return consentUrl;
        }
        public async static Task<string?> GetAccessToken(string code, string redirectUri)
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
                    Scopes = Scopes,
                    DataStore = new FileDataStore("Google.Apis.Auth.OAuth2.Responses.TokenResponse")
                });

                string accessToken = "";
                try
                {
                    // Construct token endpoint URL
                    string tokenEndpoint = "https://oauth2.googleapis.com/token";

                    // Prepare request parameters
                    var parameters = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id",ClientId ?? String.Empty }, // Replace with your client ID
                { "client_secret",ClientSecret ?? String.Empty }, // Replace with your client secret
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };

                    // Send POST request to token endpoint
                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(parameters));
                        response.EnsureSuccessStatusCode();
                        var tokenResponseContent = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(tokenResponseContent))
                        {
                            var tokenResponseObject = JObject.Parse(tokenResponseContent);
                            accessToken = tokenResponseObject["access_token"]?.ToString() ?? string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately, e.g., log it
                    Console.WriteLine($"An error occurred while getting access token: {ex.Message}");
                    return null;
                }

                return accessToken;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it
                Console.WriteLine($"An error occurred while initializing GoogleAuthorizationCodeFlow: {ex.Message}");
                return null;
            }
        }

        public static async Task<string?> GetRefreshToken(string code, string redirectUri)
        {
            try
            {
                var clientSecrets = new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                };

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets
                });

                var tokenResponse = await flow.ExchangeCodeForTokenAsync("user", code, redirectUri, CancellationToken.None);

                string refreshToken = tokenResponse.RefreshToken;

                if (string.IsNullOrEmpty(refreshToken))
                {
                    Console.WriteLine("No refresh token received. Check if the necessary scopes are requested and the consent screen was shown.");
                    return null;
                }

                return refreshToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting the refresh token: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetAccessTokenFromRefreshTokenAsync(string refreshToken)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            };

            var token = new TokenResponse
            {
                RefreshToken = refreshToken
            };

            var credential = new UserCredential(new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets
                }),
                "user",
                token);

            await credential.RefreshTokenAsync(CancellationToken.None);

            return credential.Token.AccessToken;
        }
        public static UserInfo GetUserInfo(string accessToken)
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            var initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            };

            var service = new Oauth2Service(initializer);
            var userInfoRequest = service.Userinfo.Get();
            var userInfo = userInfoRequest.Execute();

            return new UserInfo
            {
                Email = userInfo.Email,
                Name = userInfo.Name
            };

        }
        public static bool SendEmail(string fromName, string fromEmail, List<string> toList, string subject, string body, string accessToken)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(fromName, fromEmail)); // Use the sender's name and email address
                foreach (var to in toList)
                {
                    email.To.Add(new MailboxAddress("", to)); // Add each to cc recipient to the email
                }

                email.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                email.Body = bodyBuilder.ToMessageBody();
                var rawMessage = Base64UrlEncode(email.ToString());
                var message = new Google.Apis.Gmail.v1.Data.Message { Raw = rawMessage };
                service.Users.Messages.Send(message, "me").Execute();
                service.Dispose();
                // Email sent successfully
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                return false;
            }
        }
        public static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }



        #region Fetch Emails

        public static async Task<List<CommonEmailMessage>> GetAllEmailsWithBodies(string accessToken, DateTime afterTimestamp, CompanyTimeZone CompanyTimeZone)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                List<CommonEmailMessage> allEmails = new List<CommonEmailMessage>();
                string? nextPageToken = null;
                string formattedTimestamp = ((DateTimeOffset)afterTimestamp).ToUnixTimeSeconds().ToString();

                do
                {
                    var request = service.Users.Messages.List("me");
                    request.LabelIds = "INBOX";
                    request.MaxResults = 50;
                    request.Q = $"after:{formattedTimestamp}";

                    request.PageToken = nextPageToken;
                    var response = await request.ExecuteAsync();
                    var messages = response.Messages;

                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            var emailInfoRequest = service.Users.Messages.Get("me", message.Id);
                            emailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

                            var emailInfoResponse = await emailInfoRequest.ExecuteAsync();
                            if (emailInfoResponse != null)
                            {
                                var createdDateTime = ParseDateTime(emailInfoResponse?.Payload?.Headers?.FirstOrDefault(h => h.Name == "Date")?.Value ?? string.Empty);
                                var receivedDateTime = ParseReceivedDateTime(emailInfoResponse?.Payload?.Headers?.FirstOrDefault(h => h.Name == "X-Received")?.Value ?? string.Empty);
                                var downloadDateTime = DateTime.UtcNow;

                                createdDateTime = createdDateTime?.AddHours((int)CompanyTimeZone);
                                receivedDateTime = receivedDateTime?.AddHours((int)CompanyTimeZone);
                                downloadDateTime = downloadDateTime.AddHours((int)CompanyTimeZone);


                                var attachmentHeader = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "X-Attachment-Id");
                                var from = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
                                string senderName = string.Empty;
                                string senderEmail = string.Empty;

                                if (!string.IsNullOrEmpty(from))
                                {
                                    if (from.Contains('<') && from.Contains('>'))
                                    {
                                        // If the format is "Name <email@example.com>"
                                        string[] parts = from.Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (parts.Length == 2)
                                        {
                                            senderName = parts[0].Trim();
                                            senderEmail = parts[1].Trim();
                                        }
                                        else if (parts.Length == 1)
                                        {
                                            senderEmail = parts[0].Trim();
                                        }
                                    }
                                    else
                                    {
                                        // If the format is "email@example.com"
                                        senderEmail = from.Trim();
                                    }
                                }
                                var emailModel = new CommonEmailMessage
                                {
                                    MessageId = emailInfoResponse?.Id,
                                    ReceivedDateTime = receivedDateTime ?? DateTime.MinValue, // Default value if null
                                    Subject = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value ?? string.Empty,
                                    SenderName = senderName ?? string.Empty,
                                    SenderEmail = senderEmail ?? string.Empty,
                                    HasAttachments = attachmentHeader != null && !string.IsNullOrEmpty(attachmentHeader.Value),
                                    Importance = emailInfoResponse?.LabelIds?.Contains("IMPORTANT") == true ? "high" : "normal",
                                    IsRead = emailInfoResponse?.LabelIds.Contains("UNREAD") == true ? false : true,
                                    IsDraft = emailInfoResponse?.LabelIds.Contains("DRAFT") == true ? false : true,
                                    InternetMessageId = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "Message-ID")?.Value ?? string.Empty,
                                    DownloadDateTime = downloadDateTime,
                                    EmailRecipients = new List<CommonEmailRecipientHelper>()
                                };


                                var toRecipients = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value ?? string.Empty;
                                await AddRecipientsAsync(emailModel.EmailRecipients, toRecipients, RecipientType.TO, emailModel.Id);

                                var ccRecipients = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "Cc")?.Value ?? string.Empty;
                                await AddRecipientsAsync(emailModel.EmailRecipients, ccRecipients, RecipientType.CC, emailModel.Id);

                                var bccRecipients = emailInfoResponse?.Payload.Headers.FirstOrDefault(h => h.Name == "Bcc")?.Value ?? string.Empty;
                                await AddRecipientsAsync(emailModel.EmailRecipients, bccRecipients, RecipientType.BCC, emailModel.Id);

                                allEmails.Add(emailModel);
                            }
                        }

                        nextPageToken = response.NextPageToken;
                    }
                    else
                    {
                        nextPageToken = null;
                    }
                } while (nextPageToken != null);

                return allEmails;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching emails: {ex.Message}");
                return new List<CommonEmailMessage>();
            }
        }
        public static async Task<(string body, List<Attachment> attachments)> GetEmailBodyAndAttachments(string accessToken, string messageId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var emailInfoRequest = service.Users.Messages.Get("me", messageId);
                emailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

                var emailInfoResponse = await emailInfoRequest.ExecuteAsync();

                string body = GetBody(emailInfoResponse.Payload);
                var attachments = GetAttachments(emailInfoResponse.Payload, service, messageId);

                return (body, attachments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the email: {ex.Message}");
                return (string.Empty, new List<Attachment>());
            }
        }

        private static List<Attachment> GetAttachments(Google.Apis.Gmail.v1.Data.MessagePart part, GmailService service, string messageId)
        {
            var attachments = new List<Attachment>();

            if (part == null)
            {
                return attachments;
            }

            if (part.Parts == null && part.Body?.AttachmentId != null)
            {
                var attachmentId = part.Body.AttachmentId;
                if (!string.IsNullOrEmpty(attachmentId))
                {
                    try
                    {
                        // Fetch the attachment data from Gmail API
                        var attachmentRequest = service.Users.Messages.Attachments.Get("me", messageId, attachmentId);
                        var attachmentResponse = attachmentRequest.Execute();

                        // Decode base64 data
                        var attachmentData = attachmentResponse.Data != null
                            ? Convert.FromBase64String(attachmentResponse.Data.Replace('-', '+').Replace('_', '/'))
                            : Array.Empty<byte>();

                        attachments.Add(new Attachment
                        {
                            FileName = part.Filename,
                            MimeType = part.MimeType,
                            Data = attachmentData,
                            AttachmentId = attachmentId // Include the AttachmentId if needed for future reference
                        });



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while fetching attachment {attachmentId}: {ex.Message}");
                    }
                }
            }
            else if (part.Parts != null)
            {
                foreach (var subPart in part.Parts)
                {
                    attachments.AddRange(GetAttachments(subPart, service, messageId));
                }
            }

            return attachments;
        }

        public static List<CommonClasses.Attachment> ConvertToCustomAttachments(List<GoogleAPI.Attachment> attachments)
        {
            return attachments.Select(a => new CommonClasses.Attachment
            {
                FileName = a.FileName,
                MimeType = a.MimeType,
                Data = a.Data,
                AttachmentId = a.AttachmentId,
                IsInline = a.IsInline
            }).ToList();
        }

        public static CommonClasses.Attachment ConvertToAttachments(GoogleAPI.Attachment attachments)
        {
            return new CommonClasses.Attachment
            {
                FileName = attachments.FileName,
                MimeType = attachments.MimeType,
                Data = attachments.Data,
                AttachmentId = attachments.AttachmentId
            };

        }

        public static async Task<(string body, Dictionary<string, string> imageData, List<Attachment> attachments)> GetEmailBodyAndAttachmentsAsync(string accessToken, string messageId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var emailInfoRequest = service.Users.Messages.Get("me", messageId);
                emailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

                var emailInfoResponse = await emailInfoRequest.ExecuteAsync();

                var bodyAndImages = GetBodyAndImages(emailInfoResponse.Payload, service, messageId, accessToken);
                var attachments = GetAttachments(emailInfoResponse.Payload, service, messageId);

                return (bodyAndImages.body, bodyAndImages.imageData, attachments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the email: {ex.Message}");
                return (string.Empty, new Dictionary<string, string>(), new List<Attachment>());
            }
        }
        private static (string body, Dictionary<string, string> imageData) GetBodyAndImages(Google.Apis.Gmail.v1.Data.MessagePart part, GmailService service, string messageId, string accessToken)
        {
            var bodyBuilder = new StringBuilder();
            var imageData = new Dictionary<string, string>();

            if (part == null)
            {
                return (string.Empty, imageData);
            }

            if (part.Parts == null && part.Body != null)
            {
                // Process the text parts (plain or HTML)
                if (part.MimeType == "text/html")
                {
                    bodyBuilder.Append(DecodeBase64String(part.Body.Data));
                }
            }
            else if (part.Parts != null)
            {
                foreach (var subPart in part.Parts)
                {
                    if (subPart.MimeType == "text/html")
                    {
                        // Recursively process text parts
                        var (subBody, subImages) = GetBodyAndImages(subPart, service, messageId, accessToken);
                        bodyBuilder.Append(subBody);

                        // Merge sub-images into the main imageData dictionary
                        foreach (var img in subImages)
                        {
                            imageData[img.Key] = img.Value;
                        }
                    }
                    else if (subPart.MimeType.StartsWith("image/"))
                    {
                        var contentId = GetContentId(subPart);
                        if (contentId != null)
                        {
                            // Fetch the image data
                            var base64Data = GetAttachmentDataAsync(accessToken, messageId, subPart.Body.AttachmentId).Result ?? Array.Empty<byte>();
                            var imageHtml = GetImageHtml(base64Data, subPart.MimeType);

                            // Store the image HTML using the contentId as the key
                            imageData[contentId.Trim('<', '>')] = imageHtml; // Trim <> from the contentId
                        }
                    }
                    else
                    {
                        // Recursively process other types of nested parts
                        var (subBody, subImages) = GetBodyAndImages(subPart, service, messageId, accessToken);
                        bodyBuilder.Append(subBody);

                        foreach (var img in subImages)
                        {
                            imageData[img.Key] = img.Value;
                        }
                    }
                }
            }

            return (bodyBuilder.ToString(), imageData);
        }

        //private static (string body, Dictionary<string, string> imageData) GetBodyAndImages(Google.Apis.Gmail.v1.Data.MessagePart part, GmailService service, string messageId, string accessToken)
        //{
        //    var bodyBuilder = new StringBuilder();
        //    var imageData = new Dictionary<string, string>();

        //    if (part == null)
        //    {
        //        return (string.Empty, imageData);
        //    }

        //    if (part.Parts == null && part.Body != null)
        //    {
        //        // Process the text parts (plain or HTML)
        //        if (part.MimeType == "text/plain" || part.MimeType == "text/html")
        //        {
        //            bodyBuilder.Append(DecodeBase64String(part.Body.Data));
        //        }
        //    }
        //    else if (part.Parts != null)
        //    {
        //        foreach (var subPart in part.Parts)
        //        {
        //            if (subPart.MimeType == "text/plain" || subPart.MimeType == "text/html")
        //            {
        //                // Recursively process text parts
        //                var (subBody, subImages) = GetBodyAndImages(subPart, service, messageId, accessToken);
        //                bodyBuilder.Append(subBody);

        //                // Merge sub-images into the main imageData dictionary
        //                foreach (var img in subImages)
        //                {
        //                    imageData[img.Key] = img.Value;
        //                }
        //            }
        //            else if (subPart.MimeType.StartsWith("image/"))
        //            {
        //                var contentId = GetContentId(subPart);
        //                if (contentId != null)
        //                {
        //                    // Fetch the image data
        //                    var base64Data = GetAttachmentDataAsync(accessToken, messageId, subPart.Body.AttachmentId).Result;
        //                    var imageHtml = $"<img src=\"data:{subPart.MimeType};base64,{base64Data}\" alt=\"{contentId}\">";

        //                    // Store the image HTML using the contentId as the key
        //                    imageData[contentId.Trim('<', '>')] = imageHtml; // Trim <> from the contentId
        //                }
        //            }
        //            else
        //            {
        //                // Recursively process other types of nested parts
        //                var (subBody, subImages) = GetBodyAndImages(subPart, service, messageId, accessToken);
        //                bodyBuilder.Append(subBody);

        //                foreach (var img in subImages)
        //                {
        //                    imageData[img.Key] = img.Value;
        //                }
        //            }
        //        }
        //    }

        //    // Replace placeholders with actual image HTML
        //    foreach (var img in imageData)
        //    {
        //        var placeholder = $"<img src=\"cid:{img.Key}\">";
        //        bodyBuilder.Replace(placeholder, img.Value);
        //    }

        //    return (bodyBuilder.ToString(), imageData);
        //}

        private static string GetContentId(Google.Apis.Gmail.v1.Data.MessagePart part)
        {
            var contentIdHeader = part.Headers.FirstOrDefault(h => h.Name.Equals("Content-ID", StringComparison.OrdinalIgnoreCase));
            return contentIdHeader?.Value.Trim('<', '>') ?? string.Empty;
        }

        private static string GetImageHtml(byte[] imageData, string mimeType)
        {
            var base64Image = Convert.ToBase64String(imageData);
            return $"<img src=\"data:{mimeType};base64,{base64Image}\" />";
        }

        public static async Task<byte[]?> GetAttachmentDataAsync(string accessToken, string messageId, string attachmentId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Fetch the full email message to get attachment metadata
                var request = service.Users.Messages.Attachments.Get("me", messageId, attachmentId);
                var attachment = await request.ExecuteAsync();
                string base64Data = attachment.Data;
                // Find the specific attachment part
                //var attachmentPart = FindAttachmentPart(emailInfoResponse.Payload, attachmentId);

                //if (attachmentPart == null)
                //{
                //    throw new Exception("Attachment not found.");
                //}

                //// Fetch the attachment data from Gmail API
                //var request = service.Users.Messages.Attachments.Get("me", messageId, attachmentId);
                //var response = await request.ExecuteAsync();
                //var data = response.Data != null ? Convert.FromBase64String(response.Data.Replace('-', '+').Replace('_', '/')) : null;

                // Create and return the Attachment object
                return DecodeBase64Url(base64Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the attachment: {ex.Message}");
                return null;
            }
        }
        private static byte[] DecodeBase64Url(string base64Url)
        {
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        private static Google.Apis.Gmail.v1.Data.MessagePart? FindAttachmentPart(Google.Apis.Gmail.v1.Data.MessagePart part, string attachmentId)
        {
            if (part == null)
            {
                return null;
            }

            if (part.Body != null && part.Body.AttachmentId == attachmentId)
            {
                return part;
            }

            if (part.Parts != null)
            {
                foreach (var subPart in part.Parts)
                {
                    var foundPart = FindAttachmentPart(subPart, attachmentId);
                    if (foundPart != null)
                    {
                        return foundPart;
                    }
                }
            }

            return null;
        }


        private static DateTime? ParseDateTime(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
                return null;

            var dateStr = dateTimeString.Trim();

            // Remove the optional timezone abbreviation in parentheses, if present
            int openParenIndex = dateStr.IndexOf("(");
            if (openParenIndex != -1)
            {
                dateStr = dateStr.Substring(0, openParenIndex).Trim();
            }

            // Define possible date formats to try
            string[] dateFormats = new string[]
            {
             "ddd, dd MMM yyyy HH:mm:ss zzz",
             "ddd, dd MMM yyyy HH:mm:ss K",
             "ddd, dd MMM yyyy HH:mm:ss",
             "ddd, d MMM yyyy HH:mm:ss zzz",
             "ddd, d MMM yyyy HH:mm:ss K",
             "ddd, d MMM yyyy HH:mm:ss"
            };

            // Attempt to parse the date using each format
            foreach (var dateFormat in dateFormats)
            {
                if (DateTime.TryParseExact(dateStr, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
                {
                    // Convert to UTC time
                    return TimeZoneInfo.ConvertTimeToUtc(parsedDateTime);
                }
            }

            return null; // Return null if parsing fails
        }

        private static DateTime? ParseReceivedDateTime(string xReceivedHeader)
        {
            if (string.IsNullOrEmpty(xReceivedHeader))
                return null;

            var dateStr = xReceivedHeader.Split(';').Last().Trim();
            dateStr = dateStr.Substring(0, dateStr.LastIndexOf('(')).Trim();
            var dateFormat = "ddd, dd MMM yyyy HH:mm:ss zzz";
            var receivedDateTime = DateTime.ParseExact(dateStr, dateFormat, CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeToUtc(receivedDateTime);
        }

        private static async Task AddRecipientsAsync(List<CommonEmailRecipientHelper> emailRecipients, string recipients, RecipientType recipientType, int emailMessageId)
        {
            if (!string.IsNullOrEmpty(recipients))
            {
                // Simulate async operation (e.g., fetching additional info, etc.)
                await Task.Yield();

                emailRecipients.AddRange(recipients.Split(',')
                    .Select(r => new CommonEmailRecipientHelper
                    {
                        EmailMessageId = emailMessageId,
                        RecipientType = recipientType,
                        Name = r.Contains('<') ? r.Split('<')[0].Trim() : string.Empty,
                        Email = r.Contains('<') ? r.Split('<')[1].Trim('>').Trim() : r.Trim()
                    }));
            }
        }

        public static async Task<string?> GetEmailBody(string accessToken, string messageId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var emailInfoRequest = service.Users.Messages.Get("me", messageId);
                emailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

                var emailInfoResponse = await emailInfoRequest.ExecuteAsync();

                if (emailInfoResponse != null)
                {
                    string body = GetBody(emailInfoResponse.Payload);
                    return body;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the email body: {ex.Message}");
                return null;
            }
        }

        private static string GetBody(Google.Apis.Gmail.v1.Data.MessagePart part)
        {
            if (part == null)
            {
                return string.Empty;
            }

            if (part.Parts == null && part.Body != null)
            {
                return DecodeBase64String(part.Body.Data);
            }

            if (part.Parts != null)
            {
                foreach (var subPart in part.Parts)
                {
                    var result = GetBody(subPart);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }

            return string.Empty;
        }

        private static string DecodeBase64String(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var ts = input.Replace('-', '+').Replace('_', '/');
            var bytes = Convert.FromBase64String(ts);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static bool SendEmailList(string fromName, string fromEmail, List<string> toList, List<string> ccList, List<string> bccList, string subject, string body, string accessToken)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(fromName, fromEmail));

                // Add To recipients
                foreach (var to in toList)
                {
                    email.To.Add(new MailboxAddress("", to));
                }

                // Add CC recipients
                if (ccList != null)
                {
                    foreach (var cc in ccList)
                    {
                        email.Cc.Add(new MailboxAddress("", cc));
                    }
                }

                // Add BCC recipients
                if (bccList != null)
                {
                    foreach (var bcc in bccList)
                    {
                        email.Bcc.Add(new MailboxAddress("", bcc));
                    }
                }

                email.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                email.Body = bodyBuilder.ToMessageBody();

                var rawMessage = Base64UrlEncode(email.ToString());
                var message = new Google.Apis.Gmail.v1.Data.Message { Raw = rawMessage };

                service.Users.Messages.Send(message, "me").Execute();
                service.Dispose();

                // Email sent successfully
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                return false;
            }
        }

        #endregion


        #region Modify Email

        public static async Task<bool> MarkEmailAsRead(string accessToken, string emailId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Modify the labels to remove 'UNREAD' label
                var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                {
                    RemoveLabelIds = new List<string> { "UNREAD" }
                };

                var response = await service.Users.Messages.Modify(modifyRequest, "me", emailId).ExecuteAsync();

                // Check if the response is not null and if the 'UNREAD' label was successfully removed
                if (response != null && response.LabelIds != null && !response.LabelIds.Contains("UNREAD"))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while marking the email as read: {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> MarkEmailAsUnread(string accessToken, string emailId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Modify the labels to add the 'UNREAD' label
                var modifyRequest = new Google.Apis.Gmail.v1.Data.ModifyMessageRequest
                {
                    AddLabelIds = new List<string> { "UNREAD" }
                };

                var response = await service.Users.Messages.Modify(modifyRequest, "me", emailId).ExecuteAsync();

                // Check if the response is not null and if the 'UNREAD' label was successfully added
                if (response != null && response.LabelIds != null && response.LabelIds.Contains("UNREAD"))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while marking the email as unread: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> DeleteEmail(string accessToken, string emailId)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var request = service.Users.Messages.Delete("me", emailId);
                await request.ExecuteAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting email: {ex.Message}");
                return false;
            }
        }

        #endregion
        #region Calendar
        public static async Task<CalendarEvent?> FetchGoogleCalendarEventByIdAsync(string accessToken, string eventId, CompanyTimeZone CompanyTImeZone)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var request = calendarService.Events.Get("primary", eventId);
                var googleEvent = await request.ExecuteAsync();

                if (googleEvent != null)
                {
                    DateTime startTimeUtc = googleEvent.Start?.DateTimeDateTimeOffset?.UtcDateTime
                    ?? (googleEvent.Start?.Date != null
                    ? DateTime.SpecifyKind(DateTime.ParseExact(googleEvent.Start.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTimeKind.Utc)
                    : DateTime.MinValue);

                    DateTime endTimeUtc = googleEvent.End?.DateTimeDateTimeOffset?.UtcDateTime
                        ?? (googleEvent.End?.Date != null
                            ? DateTime.SpecifyKind(DateTime.ParseExact(googleEvent.End.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTimeKind.Utc)
                            : DateTime.MinValue);

                    // Now convert UTC → Company time zone
                    DateTime startTime = StringFormating.ConvertUTCToTimeZone(startTimeUtc, CompanyTImeZone);
                    DateTime endTime = StringFormating.ConvertUTCToTimeZone(endTimeUtc, CompanyTImeZone);

                    var calendarEvent = new CalendarEvent
                    {
                        Id = googleEvent.Id,
                        Title = googleEvent.Summary ?? "No Title",
                        StartTime = startTime,
                        StartTime_Hour = startTime.Hour > 12 ? startTime.Hour - 12 : startTime.Hour,
                        StartTime_Minute = startTime.Minute.ToString("D2"),
                        StartTime_AMPM = startTime.Hour >= 12 ? "PM" : "AM",
                        EndTime = endTime,
                        EndTime_Hour = endTime.Hour > 12 ? endTime.Hour - 12 : endTime.Hour,
                        EndTime_Minute = endTime.Minute.ToString("D2"),
                        EndTime_AMPM = endTime.Hour >= 12 ? "PM" : "AM",
                        Description = googleEvent.Description,
                        Location = googleEvent.Location,
                        Organizer = googleEvent.Organizer?.Email,
                        Attendees = googleEvent.Attendees?.Select(a => a.Email).ToList() ?? new List<string>(),
                        HtmlLink = googleEvent.HtmlLink
                    };

                    return calendarEvent;
                }
            }
            catch (Google.GoogleApiException googleApiEx)
            {
                Console.WriteLine($"Google API Error: {googleApiEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Google Calendar event: {ex.Message}");
            }
            return null;
        }

        public static async Task<List<CalendarEvent>> FetchGoogleCalendarEventsAsync(string accessToken, DateTime startDate, DateTime endDate, int EmailAccountId, CompanyTimeZone CompanyTimeZone,string CalendarColor)
        {
            var calendarEventList = new List<CalendarEvent>();

            try
            {
                // Initialize Google Calendar API client
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Set the parameters for the events list request
                var request = calendarService.Events.List("primary");
                request.TimeMinDateTimeOffset = new DateTimeOffset(startDate); // Fetch events starting from startDate
                request.TimeMaxDateTimeOffset = new DateTimeOffset(endDate);   // Fetch events up to endDate


                request.ShowDeleted = false; // Exclude deleted events
                request.SingleEvents = true; // Expand recurring events into individual instances
                request.MaxResults = 50; // Fetch up to 50 events
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime; // Order by start time

                // Execute the request
                var events = await request.ExecuteAsync();

                // Map Google Calendar events to CalendarEvent
                if (events?.Items != null && events.Items.Any())
                {
                    foreach (var googleEvent in events.Items)
                    {
                        //DateTime startTime = googleEvent.Start?.DateTimeDateTimeOffset?.DateTime ?? (googleEvent.Start?.Date != null ? DateTime.ParseExact(googleEvent.Start.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) : DateTime.MinValue);
                        //DateTime endTime = googleEvent.End?.DateTimeDateTimeOffset?.DateTime ?? (googleEvent.End?.Date != null ? DateTime.ParseExact(googleEvent.End.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) : DateTime.MinValue);



                        DateTime startTimeUtc = googleEvent.Start?.DateTimeDateTimeOffset?.UtcDateTime
                        ?? (googleEvent.Start?.Date != null
                        ? DateTime.SpecifyKind(DateTime.ParseExact(googleEvent.Start.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTimeKind.Utc)
                        : DateTime.MinValue);

                        DateTime endTimeUtc = googleEvent.End?.DateTimeDateTimeOffset?.UtcDateTime
                            ?? (googleEvent.End?.Date != null
                                ? DateTime.SpecifyKind(DateTime.ParseExact(googleEvent.End.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTimeKind.Utc)
                                : DateTime.MinValue);

                        // Now convert UTC → Company time zone
                        DateTime startTime = StringFormating.ConvertUTCToTimeZone(startTimeUtc, CompanyTimeZone);
                        DateTime endTime = StringFormating.ConvertUTCToTimeZone(endTimeUtc, CompanyTimeZone);







                        var calendarEvent = new CalendarEvent
                        {
                            Id = googleEvent.Id,
                            Title = googleEvent.Summary ?? "No Title",
                            StartTime = startTime,
                            StartTime_Hour = startTime.Hour > 12 ? startTime.Hour - 12 : startTime.Hour,
                            StartTime_Minute = startTime.Minute.ToString("D2"),
                            StartTime_AMPM = startTime.Hour >= 12 ? "PM" : "AM",
                            EndTime = endTime,
                            EndTime_Hour = endTime.Hour > 12 ? endTime.Hour - 12 : endTime.Hour,
                            EndTime_Minute = endTime.Minute.ToString("D2"),
                            EndTime_AMPM = endTime.Hour >= 12 ? "PM" : "AM",
                            Description = googleEvent.Description,
                            Location = googleEvent.Location,
                            Organizer = googleEvent.Organizer?.Email,
                            Attendees = googleEvent.Attendees?.Select(a => a.Email).ToList() ?? new List<string>(),
                            HtmlLink = googleEvent.HtmlLink,
                            EmailAccountId = EmailAccountId
                        };
                        if (CalendarColor != null)
                            calendarEvent.CalendarColor = CalendarColor;

                        calendarEventList.Add(calendarEvent);
                    }
                }
                else
                {
                    Console.WriteLine("No events found for the specified date range.");
                }
            }
            catch (Google.GoogleApiException googleApiEx)
            {
                Console.WriteLine($"Google API Error: {googleApiEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Google Calendar events: {ex.Message}");
            }

            return calendarEventList;
        }
        public static async Task<Google.Apis.Calendar.v3.Data.Event?> CreateCalendarEvent(string accessToken, string Subject, string Description, DateTime? StartTime, DateTime? EndTime, List<string> Attendees, CompanyTimeZone CompanyTimeZone)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName // Use your application name
                });

                // Build event attendees
                var EventAttendee = new List<Google.Apis.Calendar.v3.Data.EventAttendee>();
                foreach (var item in Attendees)
                {
                    EventAttendee.Add(new Google.Apis.Calendar.v3.Data.EventAttendee { Email = item });
                }



                string ianaTimeZone = GetIanaTimeZone(CompanyTimeZone); // e.g., "Asia/Kolkata"


                var newEvent = new Google.Apis.Calendar.v3.Data.Event
                {
                    Summary = Subject,
                    Description = Description,
                    Start = new EventDateTime
                    {
                        DateTimeDateTimeOffset = StartTime.HasValue ? new DateTimeOffset(StartTime.Value) : (DateTimeOffset?)null,
                        TimeZone = ianaTimeZone // Adjust the time zone as needed
                    },


                    End = new EventDateTime
                    {
                        DateTimeDateTimeOffset = EndTime.HasValue ? new DateTimeOffset(EndTime.Value) : (DateTimeOffset?)null,
                        TimeZone = ianaTimeZone // Adjust the time zone as needed
                    },

                    Attendees = EventAttendee,
                    Reminders = new Google.Apis.Calendar.v3.Data.Event.RemindersData
                    {
                        UseDefault = false,
                        Overrides = new List<EventReminder>
                {
                    new EventReminder { Method = "email", Minutes = 24 * 60 }, // Reminder 1 day before
                    new EventReminder { Method = "popup", Minutes = 10 } // Reminder 10 minutes before
                }
                    }
                };

                // Add the event to the user's primary calendar
                var calendarId = "primary";
                var request = calendarService.Events.Insert(newEvent, calendarId);

                // Specify that invites should be sent to attendees
                request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All;

                var createdEvent = await request.ExecuteAsync();

                Console.WriteLine($"Event created: {createdEvent.HtmlLink}");

                return createdEvent; // Event created successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the calendar event: {ex.Message}");
                return null;
            }
        }
        public static async Task<Google.Apis.Calendar.v3.Data.Event?> UpdateGoogleCalendarEvent(string accessToken, string calendarId, string eventId, string summary, string description, string location, DateTime startTime, DateTime endTime, List<string> attendeesEmails, CompanyTimeZone CompanyTimeZone)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
                string ianaTimeZone = GetIanaTimeZone(CompanyTimeZone); // e.g., "Asia/Kolkata"

                var existingEvent = await calendarService.Events.Get(calendarId, eventId).ExecuteAsync();

                existingEvent.Summary = summary;
                existingEvent.Location = location;
                existingEvent.Description = description;

                existingEvent.Start = new EventDateTime
                {
                    DateTimeDateTimeOffset = new DateTimeOffset(startTime),
                    TimeZone = ianaTimeZone // Set your timezone here
                };

                existingEvent.End = new EventDateTime
                {
                    DateTimeDateTimeOffset = new DateTimeOffset(endTime),
                    TimeZone = ianaTimeZone
                };



                // Update the attendees
                existingEvent.Attendees = attendeesEmails.Select(email => new Google.Apis.Calendar.v3.Data.EventAttendee
                {
                    Email = email
                }).ToList();

                // Add reminders if necessary
                existingEvent.Reminders = new Google.Apis.Calendar.v3.Data.Event.RemindersData
                {
                    UseDefault = false,
                    Overrides = new List<EventReminder>
            {
                new EventReminder { Method = "email", Minutes = 24 * 60 },  // Reminder email 24 hours before event
                new EventReminder { Method = "popup", Minutes = 10 }        // Popup reminder 10 minutes before event
            }
                };

                var updateRequest = calendarService.Events.Update(existingEvent, calendarId, eventId);
                updateRequest.SendUpdates = EventsResource.UpdateRequest.SendUpdatesEnum.All; // Send updated invites to all attendees

                // Execute the request to update the event
                var updatedEvent = await updateRequest.ExecuteAsync();

                // Check if the event update was successful
                return updatedEvent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the calendar event: {ex.Message}");
                return null;
            }
        }
        public static async Task<string> DeleteGoogleCalendarEventAsync(string accessToken, string eventId)
        {
            try
            {
                // Initialize Google Calendar API client
                var credential = GoogleCredential.FromAccessToken(accessToken);
                var calendarService = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Delete the event
                var deleteRequest = calendarService.Events.Delete("primary", eventId);
                await deleteRequest.ExecuteAsync();

                return $"Google Calendar event with ID {eventId} deleted successfully.";
            }
            catch (Google.GoogleApiException googleApiEx)
            {
                return $"Google API Error: {googleApiEx.Message}";
            }
            catch (Exception ex)
            {
                return $"Error deleting Google Calendar event: {ex.Message}";
            }
        }

        #endregion    

        private static byte[] DecodeBase64StringForAttachment(string base64String)
        {
            return Convert.FromBase64String(base64String);
        }

        public class UserInfo
        {
            public string? Email { get; set; }
            public string? Name { get; set; }
        }
        public class Attachment
        {
            public string? FileName { get; set; }     // Name of the file
            public string? MimeType { get; set; }     // MIME type of the file (e.g., "application/pdf", "image/jpeg")
            public byte[]? Data { get; set; }
            public string? AttachmentId { get; set; }  // The content of the file
            public bool IsInline { get; set; }

        }


    }
    public class CalendarEvent
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public int StartTime_Hour { get; set; }
        public string? StartTime_Minute { get; set; }
        public string? StartTime_AMPM { get; set; }
        public DateTime EndTime { get; set; }
        public int EndTime_Hour { get; set; }
        public string? EndTime_Minute { get; set; }
        public string? EndTime_AMPM { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Organizer { get; set; }
        public List<string>? Attendees { get; set; }
        public string? HtmlLink { get; set; }
        public int EmailAccountId { get; set; }
        public string? YOffset
        {
            get
            {
                return StartTime_Minute + "px";
            }
        }
        public Time CalendarSlot
        {
            get
            {
                if (StartTime_Hour == 7 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.SevenThirtyAM;
                if (StartTime_Hour == 7 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.SevenThirty;
                if (StartTime_Hour == 7 && StartTime_AMPM == "AM") return Time.SevenAM;
                if (StartTime_Hour == 7 && StartTime_AMPM == "PM") return Time.Seven;

                if (StartTime_Hour == 8 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.EightThirtyAM;
                if (StartTime_Hour == 8 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.Eight;
                if (StartTime_Hour == 8 && StartTime_AMPM == "AM") return Time.EightAM;
                if (StartTime_Hour == 8 && StartTime_AMPM == "PM") return Time.Eight;

                if (StartTime_Hour == 9 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.NineThirtyAM;
                if (StartTime_Hour == 9 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.NineThirty;
                if (StartTime_Hour == 9 && StartTime_AMPM == "AM") return Time.NineAM;
                if (StartTime_Hour == 9 && StartTime_AMPM == "PM") return Time.Nine;

                if (StartTime_Hour == 10 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.TenThirtyAM;
                if (StartTime_Hour == 10 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.TenThirty;
                if (StartTime_Hour == 10 && StartTime_AMPM == "AM") return Time.TenAM;
                if (StartTime_Hour == 10 && StartTime_AMPM == "PM") return Time.Ten;

                if (StartTime_Hour == 11 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.ElevenThirtyAM;
                if (StartTime_Hour == 11 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.ElevenThirty;
                if (StartTime_Hour == 11 && StartTime_AMPM == "AM") return Time.ElevenAM;
                if (StartTime_Hour == 11 && StartTime_AMPM == "PM") return Time.Eleven;

                if (StartTime_Hour == 12 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.TwelveThirtyAM;
                if (StartTime_Hour == 12 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.TwelveThirty;
                if (StartTime_Hour == 12 && StartTime_AMPM == "AM") return Time.TwelveAM;
                if (StartTime_Hour == 12 && StartTime_AMPM == "PM") return Time.Twelve;

                if (StartTime_Hour == 1 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.OneThirtyAM;
                if (StartTime_Hour == 1 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.OneThirty;
                if (StartTime_Hour == 1 && StartTime_AMPM == "AM") return Time.OneAM;
                if (StartTime_Hour == 1 && StartTime_AMPM == "PM") return Time.One;

                if (StartTime_Hour == 2 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.TwoThirtyAM;
                if (StartTime_Hour == 2 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.TwoThirty;
                if (StartTime_Hour == 2 && StartTime_AMPM == "AM") return Time.TwoAM;
                if (StartTime_Hour == 2 && StartTime_AMPM == "PM") return Time.Two;

                if (StartTime_Hour == 3 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.ThreeThirtyAM;
                if (StartTime_Hour == 3 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.ThreeThirty;
                if (StartTime_Hour == 3 && StartTime_AMPM == "AM") return Time.ThreeAM;
                if (StartTime_Hour == 3 && StartTime_AMPM == "PM") return Time.Three;

                if (StartTime_Hour == 4 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.FourThirtyAM;
                if (StartTime_Hour == 4 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.FourThirty;
                if (StartTime_Hour == 4 && StartTime_AMPM == "AM") return Time.FourAM;
                if (StartTime_Hour == 4 && StartTime_AMPM == "PM") return Time.Four;

                if (StartTime_Hour == 5 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.FiveThirtyAM;
                if (StartTime_Hour == 5 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.FiveThirty;
                if (StartTime_Hour == 5 && StartTime_AMPM == "AM") return Time.FiveAM;
                if (StartTime_Hour == 5 && StartTime_AMPM == "PM") return Time.Five;

                if (StartTime_Hour == 6 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.SixThirtyAM;
                if (StartTime_Hour == 6 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.SixThirty;
                if (StartTime_Hour == 6 && StartTime_AMPM == "AM") return Time.SixAM;
                if (StartTime_Hour == 6 && StartTime_AMPM == "PM") return Time.Six;


                //if (StartTime_Hour == 6 && StartTime_AMPM == "AM") return Time.Six;
                //if (StartTime_Hour == 6 && StartTime_AMPM == "PM") return Time.Six;
                //if (StartTime_Hour == 6 && StartTime_AMPM == "AM" && StartTime_Minute == "30") return Time.SixThirtyAM;
                //if (StartTime_Hour == 6 && StartTime_AMPM == "PM" && StartTime_Minute == "30") return Time.SixThirty;

                else return Time.SevenAM;
            }
        }
        public string CalendarColor { get; set; }

        public string? Height
        {
            get
            {
                var start = StartTime_Hour;
                if (StartTime_AMPM == "pm" && StartTime_Hour != 12)
                    start += 12;
                start = start * 60;

                if (StartTime_Minute != null)
                    start += int.Parse(StartTime_Minute);

                var end = EndTime_Hour;
                if (EndTime_AMPM == "pm" && EndTime_Hour != 12)
                    end += 12;
                end = end * 60;
                if (EndTime_Minute != null)
                    end += int.Parse(EndTime_Minute);

                var duration = end - start;
                duration = duration > 0 ? duration + 30 : duration;
                return duration.ToString() + "px";
            }
        }

    }
    public class EmailDetailsViewModel
    {
        public string? Body { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }

    public class Attachment
    {
        public string? FileName { get; set; }     // Name of the file
        public string? MimeType { get; set; }     // MIME type of the file (e.g., "application/pdf", "image/jpeg")
        public byte[]? Data { get; set; }
        public string? AttachmentId { get; set; }
        public bool IsInline { get; set; }
        public string? ContentId { get; set; }
        public object? ContentType { get; set; }
        public string? ContentBytes { get; set; }
    }
}
