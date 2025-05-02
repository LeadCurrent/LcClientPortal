using System;

namespace CommonClasses
{
    public static class Environment
    {
        public static string environment = "Dev";

        public static string TenantId = "";
        public static string SiteEmail = "support@leadcurrent.com";
        public static string SiteName = "Lead Current";
        public static string SupportName = "Lead Current";
        public static string SupportEmail = "support@leadcurrent.com";
        public static string SupportPhone = "";
        public static string NoReplyEmail = "";
        public static string SendGridAPIKey = "";

        public static bool Microsoft = true;
        public static bool Gmail = true;
        public static bool SendTestEmail = false;
        public static string TestEmail = "";
        public static string SystemAPIKey = "jFuek*fsS#f934";
        public static string url()
        {
            if (environment.Contains("Dev"))
                return "https://localhost:44390/";
            else if (environment.Contains("Test"))
                return "https://localhost:44390/";
            else
                return "";
        }

        public static string DBConnection()
        {
            if (environment == "Dev")
                return "Server=(localdb)\\MSSQLLocalDB;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
            else if (environment == "Test")
                return "Server=(localdb)\\MSSQLLocalDB;Database=LcClientPortal;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
            else
                return "";
        }

        public static string MicrosoftClientId()
        {
            return "";
        }

        public static string MicrosoftClientSecret()
        {
            return "";
        }


        public static string GoogleClientId()
        {
            if (environment.Contains("Dev"))
                return "";
            else
                return "";
        }

        public static string GoogleClientSecret()
        {
            return "";
        }

        public static string GoogleApplicationName()
        {
            return "";
        }

        public static bool SendNotifications()
        {
            //if (environment.Contains("Prod"))
            return true;
            //else
            //    return false;
        }

        public static string StorageContainer()
        {
            if (environment.Contains("Dev"))
                return "dev";
            else if (environment.Contains("Test"))
                return "test";
            else if (environment.Contains("Prod"))
                return "prod";
            return "";
        }

        public static string BaseStorageURL()
        {
            if (environment.Contains("Dev"))
                return "";
            else
                return "";
        }

        public static string StorageURLWithoutSlash()
        {
            return "";
        }

        public static string StorageAccount()
        {
            return "";
        }

        public static string StorageAccountName()
        {
            return "leadcurrent";
        }

        public static string StorageAccountKey()
        {
            return "";
        }

        public static string StorageURL()
        {
            return BaseStorageURL() + StorageContainer() + "/";
        }


        public static bool Customers
        {
            get
            {

                return true;
            }
        }
        public static bool CompanyAdmin
        {
            get
            {

                return true;
            }
        }
        public static bool Templates
        {
            get
            {
                return true;
            }
        }
        public static bool Email
        {
            get
            {
                return true;
            }
        }
        public static bool Documents
        {
            get
            {
                return true;
            }
        }
        public static bool CompanyProfile
        {
            get
            {
                return true;
            }
        }
        public static bool Users
        {
            get
            {
                return true;
            }
        }

    }
}
