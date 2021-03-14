using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions
{
    public class HallMeasurementsTableStorage : TableEntity

    {
        public string Deviceid { get; set; }
        public string Student { get; set; }
        public string School { get; set; }
        public int HallValue { get; set; }
        public string State { get; set; }
        public int StateCode { get; set; }
        public string Epoch { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

    }
}
