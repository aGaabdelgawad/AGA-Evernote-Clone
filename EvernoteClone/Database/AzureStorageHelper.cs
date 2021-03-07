using Azure.Storage.Blobs;
using System.Net;
using System.Threading.Tasks;

namespace EvernoteClone.Database
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public static class AzureStorageHelper
    {
        #region Fields
        private static readonly string connectionString = "Your_Azure_Storage_Connection_String";

        private static readonly string containerName = "Your_Azure_Container_Name_For_Notes";
        #endregion

        #region Methods
        public static async Task<string> UpdateBlob(string blobName, string path)
        {
            var container = new BlobContainerClient(connectionString, containerName);

            var blob = container.GetBlobClient(blobName);
            await blob.UploadAsync(path, true);

            return $"Your_Azure_Storage_Blob_Link/{containerName}/{blobName}";
        }

        public static async Task<bool> DeleteBlob(string blobName)
        {
            var container = new BlobContainerClient(connectionString, containerName);

            var blob = container.GetBlobClient(blobName);

            return await blob.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
        }

        public static void DownloadBlob(string blobName, string filePath)
        {
            string remoteUri = $"Your_Azure_Storage_Blob_Link/{containerName}/";
            string webResource = remoteUri + blobName;

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(webResource, filePath);
            }
        }
        #endregion
    }
}
