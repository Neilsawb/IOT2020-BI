using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using AzureFunctions.Models;


namespace SaveByMessageType
{
    public static class SaveByMessageType
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveByMessageType")]
        [return: Table("Measurements")]
        public static DynamicTableEntity Run(
            [IoTHubTrigger("messages/events", Connection = "iothub", ConsumerGroup = "cosmostab")] EventData message,
            [CosmosDB(
                databaseName: "IOT20",
                collectionName: "Measurements",
                ConnectionStringSetting = "cosmosdb",
                CreateIfNotExists = true
            )]out dynamic cosmos,
            ILogger log
        )
        {
            // table storage returntype
            //var dte = new DynamicTableEntity();

            object _haltablejson = null;
            // format message into table storage format depending on sensor type
            switch (message.Properties["deviceType"])
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
                        personal_Details = new Personal_details { school = _school, student = _student },
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
                        temperature = _data.temperature,
                        humidity = _data.humidity
                    };
                    _table.ConvertEpochTime();

                    var _cosmosjson = JsonConvert.SerializeObject(_cosmos);
                    cosmos = _cosmosjson;
                    string _tablejson = JsonConvert.SerializeObject(_table);
                    log.LogInformation($"Measurement was saved to Table Storage, Message:: {_tablejson}");
                    return _table;
                case "hall":

                    var _haldata = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(message.Body.Array));
                    var _haldeviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
                    var _haldeviceType = message.Properties["deviceType"].ToString();
                    var _hallatitude = message.Properties["latitude"].ToString();
                    var _hallongitude = message.Properties["longitude"].ToString();
                    var _halepochTime = message.Properties["epochTime"].ToString();
                    var _halschool = message.Properties["school"].ToString();
                    var _halstudent = message.Properties["student"].ToString();

                    var _halcosmos = new CosmosMeasurementModel()
                    {
                        deviceId = _haldeviceId,
                        deviceType = _haldeviceType,
                        epochTime = _halepochTime,
                        location = new Location { latitude = _hallatitude, longitude = _hallongitude },
                        personal_Details = new Personal_details { school = _halschool, student = _halstudent },
                        data = _haldata
                    };

                    var _haltable = new TableMeasurementModel()
                    {
                        PartitionKey = "Measurement",
                        RowKey = Guid.NewGuid().ToString(),
                        deviceId = _haldeviceId,
                        deviceType = _haldeviceType,
                        epochTime = _halepochTime,
                        latitude = _hallatitude,
                        longitude = _hallongitude,
                        temperature = _haldata.temperature,
                        humidity = _haldata.humidity
                    };
                    _halcosmos.ConvertEpochTime();

                    var _halcosmosjson = JsonConvert.SerializeObject(_halcosmos);
                    cosmos = _halcosmosjson;
                    log.LogInformation($"message processed: {Encoding.UTF8.GetString(message.Body.Array)}");
                    log.LogInformation($"Measurement was saved to Table Storage, Message:: {_haltablejson}");
                    return _haltable;
                default:
                    log.LogInformation("message type not defined!");
                    cosmos = null;
                    return null;
            }

            log.LogInformation($"message processed: {Encoding.UTF8.GetString(message.Body)}");
            
        }
    }
}

