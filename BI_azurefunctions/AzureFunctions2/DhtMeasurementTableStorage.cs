using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions
{
    public class DhtMeasurementTableStorage : TableEntity
    {
        public string Deviceid { get; set; }
        public string Devicetype { get; set; }
        public string Student { get; set; }
        public string School { get; set; }
        public double Temperature { get; set; }
        public string Epoch { get; set; }
        public double Humidity { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
