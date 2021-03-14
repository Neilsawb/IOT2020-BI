using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureFunctions
{
    public static class FetchCosmosData
    {
        [FunctionName("FetchCosmosData")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "IOT20",
                collectionName: "Measurements",
                ConnectionStringSetting = "cosmosdb",
                SqlQuery = "SELECT * FROM c"
            )]IEnumerable<dynamic> cosmos,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function executed.");
            if (cosmos == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(cosmos);
        }
    }
}