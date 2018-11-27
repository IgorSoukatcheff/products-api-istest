using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using products_api_istest.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace products_api_istest.AzureProviders
{
    public class MessageQueueProvider
    {
        private readonly CloudStorageAccount _account = null;
        public MessageQueueProvider(CloudStorageAccount account)
        {
            _account = account;
        }
        public async Task<bool> SaveDocMetadata(DocumentDto doc)
        {
            var queueName = "filemetadataproductionistest";
            var client = _account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(queueName);
            var result = await queue.CreateIfNotExistsAsync();

            await queue.FetchAttributesAsync();
            var addMetadata = true;
            if (queue.Metadata != null & queue.Metadata.Any(x=>x.Key=="metadataKey"))
            {
                addMetadata = false;
            }
            if (addMetadata)
            {
                queue.Metadata.Add("metadataKey", "metadataValue");
                await queue.SetMetadataAsync();
            }
            doc.DocumentContent = new byte[]{ };
            var data = JsonConvert.SerializeObject(doc);
            var msg = new CloudQueueMessage(data);
            await queue.AddMessageAsync(msg);
            return true;
        }
    }
}
