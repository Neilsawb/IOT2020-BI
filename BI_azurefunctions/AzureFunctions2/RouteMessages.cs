using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using AzureFunctions.Models;
using Newtonsoft.Json;
using System;

namespace AzureFunctions
{
    public static class RouteMessages
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("RouteMessages")]
        [return: Table("Measurements")]
        public static TableMeasurementModel Run(
            [IoTHubTrigger("messages/events", Connection = "iothub", ConsumerGroup = "storage")] EventData message,
            [CosmosDB(databaseName: "IOT20", collectionName: "Measurements", CreateIfNotExists = true, ConnectionStringSetting = "cosmosdb")] out dynamic cosmos,
            ILogger log)
        {
            try
            {
                switch (message.Properties["type"])
                {
                    case "dht":
                        var _data = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(message.Body.Array));
                        var _deviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
                        var _deviceType = message.Properties["deviceType"].ToString();
                        var _latitude = message.Properties["latitude"].ToString();
                        var _longitude = message.Properties["longitude"].ToString();
                        var _epochTime = message.Properties["epochTime"].ToString();
                        var _school = message.Properties["school"].ToString();
                        var _student = message.Properties["student"].ToString();

                        var _cosmos = new CosmosMeasurementModel()
                        {
                            deviceId = _deviceId,
                            deviceType = _deviceType,
                            epochTime = _epochTime,
                            location = new Location { latitude = _latitude, longitude = _longitude },
                            data = _data
                        };
                        _cosmos.ConvertEpochTime();

                        var _table = new TableMeasurementModel()
                        {
                            PartitionKey = "Measurement",
                            RowKey = Guid.NewGuid().ToString(),
                            deviceId = _deviceId,
                            deviceType = _deviceType,
                            epochTime = _epochTime,
                            latitude = _latitude,
                            longitude = _longitude,
                            school = _school,
                            student = _student,
                            temperature = _data.temperature,
                            humidity = _data.humidity
                        };
                        _table.ConvertEpochTime();

                        var _cosmosjson = JsonConvert.SerializeObject(_cosmos);
                        log.LogInformation($"Measurement was saved to Cosmos DB, Message:: {_cosmosjson}");
                        cosmos = _cosmosjson;

                        var _tablejson = JsonConvert.SerializeObject(_table);
                        log.LogInformation($"Measurement was saved to Table Storage, Message:: {_tablejson}");
                        return _table;
                }
                    catch (Exception e)
                   {
                    log.LogInformation($"Unable to process Request, Error:: {e.Message}");
                    cosmos = null;
                    }
        }
        }
    }
}