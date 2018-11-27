using Microsoft.WindowsAzure.Storage;
using products_api_istest.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace products_api_istest.AzureProviders
{
    public class BlobProvider
    {
        private readonly CloudStorageAccount _account = null;
        
        public BlobProvider(CloudStorageAccount account)
        {
            _account = account;
        }
        public async Task<string> SaveDoc(DocumentDto doc)
        {
            var client = _account.CreateCloudBlobClient();
            var containerName = "docstoragecontaineristest";
            var container = client.GetContainerReference(containerName);
            var result = await container.CreateIfNotExistsAsync();
            var azureFilename = Guid.NewGuid().ToString() + ".doc";
            doc.AzureFileName = azureFilename;
            var blob = container.GetBlockBlobReference(azureFilename);
            await blob.UploadFromByteArrayAsync(doc.DocumentContent, 0, doc.DocumentContent.Length);
            await blob.FetchAttributesAsync();
            var etag = blob.Properties.ETag;
            return etag;
        }
    }
}
