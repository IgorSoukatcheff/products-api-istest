using System;
using AdwentureLogs2016Data.Shared.Models;
using AdwentureLogs2016Data.Shared.Models.Dtos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AzureFuncIstest
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class ProductionDocumentUploaderFunc
    {
        [FunctionName("ProductionDocumentUploaderFunc")]
        public static void Run(
            [QueueTrigger("filemetadataproductionistest", Connection = "QueueStorage")]
            CloudQueueMessage myQueueItem, 
            ILogger log)
        {
            //log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var dto = JsonConvert.DeserializeObject<DocumentDto>(myQueueItem.AsString);
            
            var blobName = dto.AzureFileName;
            //todo get blob from storage (by guid from the queue)
            //var attributes = new Attribute[]
            //{
            //   new BlobAttribute("docstoragecontaineristest/"+blobName),
            //   new StorageAccountAttribute("AzureWebJobsStorage")
            //};
            //using (var writer = await binder.BindAsync<TextWriter>(attributes))
            //{
            //    writer.Write("Hello World!");
            //}
            dto.DocumentContent = new byte[] { };

            var conetnt = ReadBlob("docstoragecontaineristest", blobName, dto.DocumentContentLenght);

            var connectionString = Environment.GetEnvironmentVariable("AdventureWorksDatabase");
            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2016Context>();
            optionsBuilder.UseSqlServer(connectionString);
            using (var context = new AdventureWorks2016Context(optionsBuilder.Options))
            {
                // add doc to the db
            }
        }

        private static byte[] ReadBlob(string containerName, string azureFilename, int lenght)
        {
            var creds = new StorageCredentials("adventureworksrgdiag331", "vEhDtYs7NQ+o/KXW1T9WOO9XMpXbl7jBMQiMBfQk043mP0BkGqjEqyRYpwzFgFYgWCDL1ZWwjkypCZeN6xYqqQ==");
            var _account = new CloudStorageAccount(creds, true);

            var client = _account.CreateCloudBlobClient();
            //var containerName = "docstoragecontaineristest";
            var container = client.GetContainerReference(containerName);
           
            var blob = container.GetBlockBlobReference(azureFilename);
            var content = new byte[lenght];
            var result = blob.DownloadToByteArrayAsync(content, 0).Result;
            return content;


        }
    }
}
