using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class G_SaveToCosmosdb
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("G_SaveToCosmosdb")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "iothub", ConsumerGroup = "cosmostab")]EventData message,
            [CosmosDB(
                databaseName: "IOT20",
                collectionName: "G_Measurements",
                ConnectionStringSetting = "cosmosdb",
                CreateIfNotExists = true
            )]out dynamic cosmos,
            ILogger log)
        {
            log.LogInformation($"Message saved to cosmosdb: {Encoding.UTF8.GetString(message.Body.Array)}");
            cosmos = Encoding.UTF8.GetString(message.Body.Array);
        }
    }
}