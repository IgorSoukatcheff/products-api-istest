using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace CosmosDBTestConsole
{
    public class Program
    {
        private static readonly Uri _endpointUri = new Uri("https://cosmosdb-lab-istest.documents.azure.com:443/");
        private static readonly string _primaryKey = "pEG5SU7bToLr5nu7qDuVlhRT9EVUgPdZgW19pN0MkTRkNEEsU7yQfCoZuGtgE3JpQZFKJKo1a1DFVQL0BEPpww==";
        public static async Task Main(string[] args)
        {
            using (DocumentClient client = new DocumentClient(_endpointUri, _primaryKey))
            {
                //Database targetDatabase = new Database { Id = "EntertainmentDatabase" };
                //targetDatabase = await client.CreateDatabaseIfNotExistsAsync(targetDatabase);
                //await Console.Out.WriteLineAsync($"Database Self-Link:\t{targetDatabase.SelfLink}");
                await client.OpenAsync();


                //Uri databaseLink = UriFactory.CreateDatabaseUri("EntertainmentDatabase");
                //DocumentCollection defaultCollection = new DocumentCollection
                //{
                //    Id = "DefaultCollection"
                //};
                //defaultCollection = await client.CreateDocumentCollectionIfNotExistsAsync(databaseLink, defaultCollection);
                //await Console.Out.WriteLineAsync($"Default Collection Self-Link:\t{defaultCollection.SelfLink}");


                //IndexingPolicy indexingPolicy = new IndexingPolicy
                //{
                //    IndexingMode = IndexingMode.Consistent,
                //    Automatic = true,
                //    IncludedPaths = new Collection<IncludedPath>
                // {
                //     new IncludedPath
                //     {
                //         Path = "/*",
                //         Indexes = new Collection<Index>
                //         {
                //             new RangeIndex(DataType.Number, -1),
                //             new RangeIndex(DataType.String, -1)
                //         }
                //     }
                // }
                //};
                //PartitionKeyDefinition partitionKeyDefinition = new PartitionKeyDefinition
                //{
                //    Paths = new Collection<string> { "/type" }
                //};

                //DocumentCollection customCollection = new DocumentCollection
                //{
                //    Id = "CustomCollection",
                //    PartitionKey = partitionKeyDefinition,
                //    IndexingPolicy = indexingPolicy
                //};
                //RequestOptions requestOptions = new RequestOptions
                //{
                //    OfferThroughput = 10000
                //};
                //customCollection = await client.CreateDocumentCollectionIfNotExistsAsync(databaseLink, customCollection, requestOptions);
                //await Console.Out.WriteLineAsync($"Custom Collection Self-Link:\t{customCollection.SelfLink}");

                Uri collectionLink = UriFactory.CreateDocumentCollectionUri("EntertainmentDatabase", "CustomCollection");
                                var foodInteractions = new Bogus.Faker<PurchaseFoodOrBeverage>()
                    .RuleFor(i => i.type, (fake) => nameof(PurchaseFoodOrBeverage))
                    .RuleFor(i => i.unitPrice, (fake) => Math.Round(fake.Random.Decimal(1.99m, 15.99m), 2))
                    .RuleFor(i => i.quantity, (fake) => fake.Random.Number(1, 5))
                    .RuleFor(i => i.totalPrice, (fake, user) => Math.Round(user.unitPrice * user.quantity, 2))
                    .Generate(500);

                foreach (var interaction in foodInteractions)
                {
                    ResourceResponse<Document> result = await client.CreateDocumentAsync(collectionLink, interaction);
                    await Console.Out.WriteLineAsync($"Document #{foodInteractions.IndexOf(interaction):000} Created\t{result.Resource.Id}");
                }
                Console.ReadKey();
            }
        }
    }
}
