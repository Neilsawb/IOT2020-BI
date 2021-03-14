using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Models
{


    public class CosmosMeasurementModel : TableMeasurementModel
    {
        public string Hallvalue { get; set; }
        public string State { get; set; }
        public string Statecode { get; set; }
    }
}
