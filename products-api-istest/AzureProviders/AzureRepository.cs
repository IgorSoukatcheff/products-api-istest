using AdwentureLogs2016Data.Shared.Models.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

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
        public AzureRepository(IHostingEnvironment hostingEnv, IConfiguration _configuration)
        {
            if (hostingEnv.IsDevelopment())
            {
                _account = CloudStorageAccount.DevelopmentStorageAccount;
            }
            else
            {
                var name = "adventureworksrgdiag331";
                var key = _configuration[name];
                var creds = new StorageCredentials(name,key);
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
