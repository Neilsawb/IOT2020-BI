using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Models
{
    public class DhtMessage : TableEntity
    {
        public string Deviceid { get; set; }
        public string Type { get; set; }
        public long EpochTime { get; set; }
        
        public string School { get; set; }
        public string Student { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Temperature { get; set; }
        public string Humidity { get; set; }
    }
}
