using Microsoft.AspNetCore.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using products_api_istest.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace products_api_istest.AzureProviders
{
    public class AzureRepository
    {
        private  CloudStorageAccount _account = null;
        private readonly MessageQueueProvider _message;
        private readonly BlobProvider _blob;
        public AzureRepository(IHostingEnvironment hostingEnv)
        {
            if (hostingEnv.IsDevelopment())
            {
                _account = CloudStorageAccount.DevelopmentStorageAccount;
            }
            else
            {
                var creds = new StorageCredentials("adventureworksrgdiag331", "vEhDtYs7NQ+o/KXW1T9WOO9XMpXbl7jBMQiMBfQk043mP0BkGqjEqyRYpwzFgFYgWCDL1ZWwjkypCZeN6xYqqQ==");
                _account = new CloudStorageAccount(creds, true);
            }
            _message = new MessageQueueProvider(_account);
            _blob = new BlobProvider(_account);
        }

        public async Task<string> SaveDocToAzure(DocumentDto doc)
        {
            //save doc and update dto with azure blob file name
            var etag = await _blob.SaveDoc(doc);
            await _message.SaveDocMetadata(doc);
            return etag;
        }
    }
}
