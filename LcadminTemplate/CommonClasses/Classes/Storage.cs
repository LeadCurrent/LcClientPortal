using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace Web
{
    public static class Storage
    {
        public static void uploadimage(string image, string filename)
        {
            var base64Data = Regex.Match(image, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            var stream = new MemoryStream(binData);

            BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());

            containerClient.DeleteBlobIfExists(filename);
            containerClient.UploadBlob(filename, stream);
        }
        public static async Task<string> UploadEmailHtmlContentToBlob(string emailHtmlContent, string relatedContentType, int id)
        {
            string emailHtmlUrl = await UploadBlobContent(emailHtmlContent, $"EmailContent/{relatedContentType}/HTML/{id}", CommonClasses.Environment.StorageContainer());
            return emailHtmlUrl;
        }
        public static async Task<(string emailHtmlContent, string emailJsonContent)> RetrieveEmailContentFromBlob(string relatedcontenttype, int Id)
        {
            string emailHtmlContent = await DownloadBlobContent($"EmailContent/{relatedcontenttype}/HTML/{Id}", CommonClasses.Environment.StorageContainer());

            string emailJsonContent = await DownloadBlobContent($"EmailContent/{relatedcontenttype}/JSON/{Id}", CommonClasses.Environment.StorageContainer());

            return (emailHtmlContent, emailJsonContent);
        }

        public static async Task<string> RetrieveEmailHtmlContentFromBlob(string relatedContentType, int id)
        {
            string emailHtmlContent = await DownloadBlobContent($"EmailContent/{relatedContentType}/HTML/{id}", CommonClasses.Environment.StorageContainer());
            if (emailHtmlContent == null && !CommonClasses.Environment.StorageContainer().Contains("prod"))
            {
                emailHtmlContent = await DownloadBlobContent($"EmailContent/{relatedContentType}/HTML/{id}", "storeprod");
            }
            return emailHtmlContent;
        }



        public static async Task<string> RetrieveEmailJsonContentFromBlob(string relatedContentType, int id)
        {
            string emailJsonContent = await DownloadBlobContent($"EmailContent/{relatedContentType}/JSON/{id}", CommonClasses.Environment.StorageContainer());
            return emailJsonContent;
        }

        public static async Task<string> RetrieveFullEmailContentFromBlob(string relatedcontenttype, int Id)
        {
            string emailFullHtmlContent = await DownloadBlobContent($"EmailContent/{relatedcontenttype}/FullEmail/{Id}", CommonClasses.Environment.StorageContainer());

            return emailFullHtmlContent;
        }

        public static async Task<string> UploadEmailJsonContentToBlob(string emailJsonContent, string relatedContentType, int id)
        {
            string emailJsonUrl = await UploadBlobContent(emailJsonContent, $"EmailContent/{relatedContentType}/JSON/{id}", CommonClasses.Environment.StorageContainer());
            return emailJsonUrl;
        }
        private static async Task<string> UploadBlobContent(string content, string blobName, string containerName)
        {
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (await containerClient.ExistsAsync())
                {
                    containerClient.DeleteBlobIfExists(blobName);
                }
                await containerClient.UploadBlobAsync(blobName, stream);

                return containerClient.GetBlobClient(blobName).Uri.ToString();
            }
        }
        private static async Task<string> DownloadBlobContent(string blobName, string containerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            if (await blobClient.ExistsAsync())
            {
                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                return downloadResult.Content.ToString();
            }
            else return null;

        }
        public static async Task UploadLargeDocument(IFormFile file, string filename)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());

                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);
                var binData = Convert.FromBase64String(s);
                var fileStream = new MemoryStream(binData);
                containerClient.DeleteBlobIfExists(filename);

                if (fileStream.Length < 10000000)
                {
                    containerClient.UploadBlob(filename, fileStream);
                }
                else
                {
                    BlockBlobClient blobClient = containerClient.GetBlockBlobClient(filename);
                    var blockSize = 10000000;
                    ArrayList blockIDArrayList = new ArrayList();
                    byte[] buffer;
                    var bytesLeft = (fileStream.Length - fileStream.Position);

                    while (bytesLeft > 0)
                    {
                        if (bytesLeft >= blockSize)
                        {
                            buffer = new byte[blockSize];
                            await fileStream.ReadAsync(buffer, 0, blockSize);
                        }
                        else
                        {
                            buffer = new byte[bytesLeft];
                            await fileStream.ReadAsync(buffer, 0, Convert.ToInt32(bytesLeft));
                            bytesLeft = (fileStream.Length - fileStream.Position);
                        }

                        using (var stream = new MemoryStream(buffer))
                        {
                            string blockID = Convert.ToBase64String
                                (Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

                            blockIDArrayList.Add(blockID);


                            await blobClient.StageBlockAsync(blockID, stream);
                        }
                        bytesLeft = (fileStream.Length - fileStream.Position);
                    }
                    string[] blockIDArray = (string[])blockIDArrayList.ToArray(typeof(string));
                    await blobClient.CommitBlockListAsync(blockIDArray);
                }

            }
            catch (Exception ex)
            {
                var str = ex.InnerException;
            }
        }
        public static void UploadDocument(IFormFile file, string filename)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    var binData = Convert.FromBase64String(s);
                    var stream = new MemoryStream(binData);
                    containerClient.DeleteBlobIfExists(filename);

                    containerClient.UploadBlob(filename, stream);
                }

            }
            catch (Exception ex)
            {
                var str = ex.InnerException;
            }
        }

        public static void UploadDocument(Stream stream, string filename)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());

                containerClient.DeleteBlobIfExists(filename);

                containerClient.UploadBlob(filename, stream);
            }
            catch (Exception ex)
            {
                var str = ex.InnerException;
            }
        }
        public static async Task UploadDocument(string fileUrl, string filename)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(fileUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var fileStream = await response.Content.ReadAsStreamAsync();
                    await containerClient.DeleteBlobIfExistsAsync(filename);
                    await containerClient.UploadBlobAsync(filename, fileStream);
                }
            }
        }

        public static void DeleteDocument(string filename)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());
                containerClient.DeleteBlobIfExists(filename);
            }
            catch (Exception ex)
            {
                var str = ex.InnerException;
            }
        }

        public static void UploadImages(IFormFile file, string filename)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(CommonClasses.Environment.StorageAccount());
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CommonClasses.Environment.StorageContainer());

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    var binData = Convert.FromBase64String(s);
                    var stream = new MemoryStream(binData);
                    containerClient.DeleteBlobIfExists(filename);

                    containerClient.UploadBlob(filename, stream);
                }

            }
            catch (Exception ex)
            {
                var str = ex.InnerException;
            }
        }
    }
}
