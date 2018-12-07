using System;
using AdwentureLogs2016Data.Shared.Models;
using AdwentureLogs2016Data.Shared.Models.Dtos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings.Runtime;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Linq;

namespace AzureFuncIstest
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class ProductionDocumentUploaderFunc
    {
        [FunctionName("ProductionDocumentUploaderFunc")]
        public static async Task Run(
            [QueueTrigger("filemetadataproductionistest", Connection = "QueueStorage")]
            CloudQueueMessage myQueueItem,
            Binder binder,
            ILogger log)
        {
            //log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var dto = JsonConvert.DeserializeObject<DocumentDto>(myQueueItem.AsString);
            
            var blobName = dto.AzureFileName;
            //todo get blob from storage (by guid from the queue)
            var attributes = new Attribute[]
            {
               new BlobAttribute("docstoragecontaineristest/"+blobName),
               new StorageAccountAttribute("AzureWebJobsStorage")
            };
            CloudBlockBlob blob = await binder.BindAsync<CloudBlockBlob>(attributes);
            dto.DocumentContent = new byte[dto.DocumentContentLenght];
            var result = blob.DownloadToByteArrayAsync(dto.DocumentContent, 0).Result;

            

            //dto.DocumentContent = ReadBlob("docstoragecontaineristest", blobName, dto.DocumentContentLenght);

            var connectionString = Environment.GetEnvironmentVariable("AdventureWorksDatabase");
            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2016Context>();
            optionsBuilder.UseSqlServer(connectionString);
            using (var context = new AdventureWorks2016Context(optionsBuilder.Options))
            {
                // add doc to the db
                //var owner = context.Employee.Where(x => x.BusinessEntityId == 1).FirstOrDefault();
                var docId = context.GetNextDocId();
                context.Document.Add(new Document()
                {
                    ChangeNumber = 1,
                    DocumentContent = dto.DocumentContent,
                    DocumentNode = docId,
                    DocumentSummary = dto.DocumentMetadata,
                    FileExtension = Path.GetExtension(dto.FileName),
                    FileName = dto.FileName,
                    FolderFlag = false,
                    ModifiedDate = DateTime.Now,
                    Owner = 1,
                    Revision = "",
                    Rowguid = Guid.NewGuid(),
                    Status = 1,
                    Title = dto.DocumentName

                });
                context.SaveChanges();
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
