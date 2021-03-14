using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace azurefuncstorage
{
    public static class Savetotablestorage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Savetotablestorage")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "DefaultEndpointsProtocol=https;AccountName=neilsiot2020bi;AccountKey=9WkpJol8Spnxj27kTd+upG84aLUB8ztLrXc8cPBQgsOhQc2qCvqngP1hnrfOEO/RhZ2gJqD5mPgXkqsC0HeaXQ==;EndpointSuffix=core.windows.net")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}