using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using AzureFunctions.Models;
using System.Linq;

namespace AzureFunctions
{
    public static class GetDataFromTableStorage
    {
        [FunctionName("GetDataFromTableStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("DhtMeasurements")] CloudTable cloudTable,
            ILogger log)
        {
            string limit = "10";
            string orderby = "desc";

            IEnumerable<DhtMessage> results = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<DhtMessage>(), null);

            results = results.OrderBy(ts => ts.Time);

            if (orderby == "desc")
                results = results.OrderByDescending(ts => ts.Time);

            if (limit != null)
                results = results.Take(int.Parse(limit));

            return new OkObjectResult(results);
        }
    }
}