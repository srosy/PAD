using Azure.Storage.Blobs;
using BlazorInputFile;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PAD.Data
{
    public static class AzureStorage
    {
        private static string STORAGE_CONN_STRING = Startup.StaticConfiguration.GetConnectionString("AZURE_STORAGE");

        /// <summary>
        /// Publishes an image to the Azure Blob Storage Account from an existing project data url.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <returns>Uploaded File Url</returns>
        public static async Task<string> PublishImage(string fileName, string url)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(fileName)) return null;

            try
            {
                var fileBytes = new System.Net.WebClient().DownloadData(url);
                return await UploadFile(fileName, fileBytes, "published");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Uploads a file to the Azure Blob Storage Account using an account Id (int). Ids must be three digits or higher.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="acctId"></param>
        /// <returns>Uploaded File Url</returns>
        public static async Task<string> UploadFile(string fileName, byte[] fileBytes, int acctId)
        {
            if (acctId < 100) return null;
            return await UploadFile(fileName, fileBytes, acctId.ToString());
        }
        public static async Task<string> UploadFile(string fileName, byte[] fileBytes, string id)
        {
            var container = await GetContainer(id);
            var blob = container.GetBlobClient(fileName.ToLower());
            var uri = blob.Uri.AbsoluteUri;

            using var ms = new MemoryStream(fileBytes, false);
            try
            {
                await blob.UploadAsync(ms, true);
                await SetCache(blob, fileName.ToLower().Contains("-proj"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return uri;
        }

        /// <summary>
        /// Uploads a file to the Azure Blob Storage Account using an account Id and the uri of an existing image for saving a project.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="projectName"></param>
        /// <param name="acctId"></param>
        /// <returns>Uploaded File Url</returns>
        public static async Task<string> UploadFileFromUri(string localUri, string projectName = null, int acctId = 0, bool isProject = false)
        {
            if (string.IsNullOrEmpty(localUri)) return null;
            projectName ??= Guid.NewGuid().ToString();
            var offset = localUri.IndexOf(',') + 1;
            var imageInBytes = Convert.FromBase64String(localUri[offset..^0]);
            var projMeta = projectName.Split(".");
            return await UploadFile($"{projMeta[0]}{(isProject ? "-proj" : string.Empty)}.{projMeta[1]}"
                .Replace("'", "")
                .Replace(",", "")
                .Replace("_", "")
                .Replace("@", ""), imageInBytes, acctId > 99 ? acctId.ToString() : "files");
        }

        /// <summary>
        /// Uploads a file to the Azure Blob Storage Account using a GUID folder Id.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="guid"></param>
        /// <returns>Uploaded File Url</returns>
        public static async Task<string> UploadFile(string fileName, byte[] fileBytes, Guid guid)
        {
            var container = await GetContainer(guid.ToString());
            BlobClient blob = container.GetBlobClient(fileName);
            var uri = blob.Uri.AbsoluteUri;

            using var ms = new MemoryStream(fileBytes, false);
            await blob.UploadAsync(ms, true);

            return uri;
        }

        /// <summary>
        /// Sets the cache for images.
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        private static async Task SetCache(BlobClient blob, bool isProject = false)
        {
            try
            {
                var metadata = new Dictionary<string, string>();
                metadata["CacheControl"] = isProject ? "public, max-age=0" : "public, max-age=300"; // no cache, 5 minutes
                await blob.SetMetadataAsync(metadata);
            }
            catch (Azure.RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Gets the Azure Blob Container by acctId.
        /// </summary>
        /// <param name="acctId"></param>
        /// <returns></returns>
        private static async Task<BlobContainerClient> GetContainer(string directoryId)
        {
            var blobServiceClient = new BlobServiceClient(STORAGE_CONN_STRING);
            var container = blobServiceClient.GetBlobContainerClient(directoryId);
            try
            {
                var exists = container.Exists();
                if (!exists)
                {
                    await container.CreateIfNotExistsAsync();
                    await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
           

            return container;
        }

        /// <summary>
        /// Converts a file into a byte array to upload to Azure blob storage.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<byte[]> ConvertFileToByteArray(IFileListEntry file)
        {
            if (file == null) return null;

            using var ms = new MemoryStream();
            await file.Data.CopyToAsync(ms);
            var imgBytes = ms.ToArray();

            return imgBytes;
        }
    }
}
