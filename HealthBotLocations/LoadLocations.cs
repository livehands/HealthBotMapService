using HealthBotLocations.Helpers;
using HealthBotLocations.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthBotLocations
{
    public static class LoadLocations
    {
        [FunctionName("LoadLocations")]
        public static async Task Run([BlobTrigger("mylocations/{name}", Connection = "BlobConnectionString")]string myBlob,
            [CosmosDB(
                databaseName: Constants.CosmosDbName,
                collectionName: Constants.MyLocationsCollection,
                ConnectionStringSetting = "AzureCosmosDBConnectionString")]IAsyncCollector<Location> document,
            string name, 
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            List<Location> locations = JsonConvert.DeserializeObject<List<Location>>(myBlob);

            foreach (Location l in locations)
            {
                try
                {
                    await document.AddAsync(l);
                }
                catch(Exception ex)
                {
                    log.LogError(ex, "Exception Adding item to collection!");
                }
            }
        }
    }
}
