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
    public static class GetAllFromCosmosDb
    {
        [FunctionName("GetAllFromCosmosDb")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "IOT20",
                collectionName: "Measurements",
                ConnectionStringSetting = "cosmosdb",
                SqlQuery = "SELECT * FROM c ORDER BY c.Time DESC OFFSET 0 LIMIT 10"
            )]IEnumerable<dynamic> cosmos,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function executed.");
            return new OkObjectResult(cosmos);
        }
    }
}
