using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using AzureFunctions.Models;

namespace AzureFunctions
{
    public static class ClassifyMessage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("ClassifyMessage")]
        public static void Run(
            [IoTHubTrigger("messages/events", Connection = "iothub", ConsumerGroup = "iothub-sunday")]EventData message, 
            [CosmosDB(databaseName: "IOT20", collectionName: "Measurements", CreateIfNotExists = true, ConnectionStringSetting = "cosmosdb")]out dynamic cosmos,
            ILogger log)
            {

            var _data = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(message.Body.Array));
            var _deviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
            var _deviceType = message.Properties["type"].ToString();
            var _School = message.Properties["School"].ToString();
            var _Student = message.Properties["Student"].ToString();
            var _Date = message.Properties["Date"].ToString();
            var _Time = message.Properties["Time"].ToString();

            var _cosmos = new CosmosMeasurementModel()
            {
                Deviceid = _deviceId,
                Devicetype = _deviceType,
                School = _School,
                Student = _Student,
                Date = _Date,
                Time = _Time,
                temperature = _data.temperature ?? 0,
                humidity = _data?.humidity ?? 0,
                Hallvalue = _data.HallValue,
                State = _data.State,
                Statecode = _data.Statecode
            };

            var _cosmosjson = JsonConvert.SerializeObject(_cosmos);
                log.LogInformation($"Measurement was saved to Cosmos DB, Message:: {_cosmosjson}");
                cosmos = _cosmosjson;

            switch (message.Properties["type"].ToString())
            {
                case "dht":
                    var dht = JsonConvert.DeserializeObject<DhtMeasurementTableStorage>(Encoding.UTF8.GetString(message.Body.Array));
                    dht.PartitionKey = "dht";
                    dht.RowKey = Guid.NewGuid().ToString();
                    dht.Date = _Date;
                    dht.Time = _Time;
                    dht.School = _School;
                    dht.Student = _Student;
                    dht.Deviceid = _deviceId;
                    client.PostAsJsonAsync(Environment.GetEnvironmentVariable("DhtHttpUrl"), dht);
                    break;

                case "hall":
                    var hall = JsonConvert.DeserializeObject<HallMeasurementsTableStorage>(Encoding.UTF8.GetString(message.Body.Array));
                    hall.PartitionKey = "hall";
                    hall.RowKey = Guid.NewGuid().ToString();
                    hall.Date = _Date;
                    hall.Time = _Time;
                    hall.School = _School;
                    hall.Student = _Student;
                    hall.Deviceid = _deviceId;
                    client.PostAsJsonAsync(Environment.GetEnvironmentVariable("HallHttpUrl"), hall);
                    break;

                default:
                    log.LogInformation($"Message was not processed: {Encoding.UTF8.GetString(message.Body.Array)}");
                    break;
            }
            
            
        }
    }
}