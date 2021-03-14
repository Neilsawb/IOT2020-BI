using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Models
{
    public class TableMeasurementModel : TableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Deviceid { get; set; }
        public string Devicetype { get; set; }
        public string Epoch { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string School { get; set; }
        public string Student { get; set; }
        public double temperature { get; set; }
        public double humidity { get; set; }
                        
    }
    class HallMeasurementModel : TableEntity
    {
        public string deviceid { get; set; }
        public string school { get; set; }
        public string student { get; set; }
        public long created { get; set; }
        public double Hallvalue { get; set; }
        public string State { get; set; }
        public string Statecode { get; set; }
    }
}
